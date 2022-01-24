using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Debug = UnityEngine.Debug;


public class Map : MonoBehaviour
{
    
    public IntVariable playerVisionRange;

    [SerializeField]
    private Grid grid;
    [SerializeField]
    private Tilemap tileMap;
    
    private List<WorldTile> worldTiles;
    private List<WorldTile> lastInRangeWorldTiles;

    public AssetReference worldTileSetReference;

    public VoidEvent mapAssetsLoadingCompleteVoidEvent;
    public HexEvent hexClickedEvent;
    
    void Awake()
    {
        Addressables.LoadAssetAsync<WorldTileSet>(worldTileSetReference).Completed += OnWorldTileSetLoadDone;
    }

    private void OnWorldTileSetLoadDone(AsyncOperationHandle<WorldTileSet> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            WorldTileSet worldTileSet = obj.Result;
            Debug.Log("Successfully loaded WorldTileSet.");
            
            worldTiles = new List<WorldTile>();
            int counter = worldTileSet.WorldTiles.Length;
            foreach (AssetReference worldTile in worldTileSet.WorldTiles)
            {
                Addressables.LoadAssetAsync<WorldTile>(worldTile).Completed += operation =>
                {
                    if (operation.Status == AsyncOperationStatus.Succeeded)
                    {
                        counter--;
                        worldTiles.Add(operation.Result);
                        Debug.Log($"Successfully loaded and instantiated WorldTile <{operation.Result.tileName}>.");

                        if (counter == 0)
                        {
                            mapAssetsLoadingCompleteVoidEvent.Raise();
                        }
                    }
                };
            }
        }
        else
        {
            Debug.LogError($"Something went wrong loading the WorldTileSet");
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);

            Hex clickedHex = new Hex(grid.WorldToCell(worldPoint), worldPoint, false);
            hexClickedEvent.Raise(clickedHex);
            
            //transform.position = map.grid.CellToWorld(cell);
            
            //PlayerMoved.Raise(cell);
            
            //CubeHexCoords position = CoordUtils.UnityHexToCubeHex(cell.ToUnityHexCoordinates());

            //UpdateMap(position);
            
        }
    }

    public void GenerateTilesAroundPlayer(Hex playerPositionHex)
    {
        if (lastInRangeWorldTiles != null)
        {
            ApplyFogToTiles(playerPositionHex, playerVisionRange.Value);
        }
        
        lastInRangeWorldTiles = SpawnHexesInRange(playerPositionHex, playerVisionRange.Value);

        tileMap.RefreshAllTiles();
    }

    private WorldTile GenerateTile(Hex hex)
    {
        WorldTile newTile = GenerateTileFromNeighbourWeights(hex);

        if (newTile == null)
        {
            newTile = GenerateRandomTile();
        }

        newTile.coords = hex;
        
        tileMap.SetTile(hex, newTile);

        return newTile;
    }

    private WorldTile GenerateRandomTile()
    {
        int rng = Random.Range(0, worldTiles.Count);
        return worldTiles[rng].Copy();
    }

    private WorldTile GenerateTileFromNeighbourWeights(Hex hex)
    {
        
        // Get the six neighbours
        Dictionary<WorldTile, int> neighbourWeights = GetNeighbourWeights(hex);

        if (neighbourWeights.Count == 0) return null;
        
        WorldTile nextTile = null;
        int totalValues = 0;
        
        foreach (int value in neighbourWeights.Values)
        {
            totalValues += value;
        }
        
        int percentWeightUnit = Mathf.FloorToInt(96 / totalValues);
        
        //Build the Table
        Dictionary<int, WorldTile> percentageTileWeights = new Dictionary<int, WorldTile>();

        int cumulativePercentage = 4;
        percentageTileWeights.Add(cumulativePercentage, GenerateRandomTile());

        foreach (KeyValuePair<WorldTile, int> keyValuePair in neighbourWeights)
        {
            cumulativePercentage += keyValuePair.Value * percentWeightUnit;
            percentageTileWeights.Add(cumulativePercentage, keyValuePair.Key);
        }

        //Query the table
        int rng = Random.Range(0, 100);

        foreach (KeyValuePair<int, WorldTile> keyValuePair in percentageTileWeights)
        {
            if (keyValuePair.Key > rng)
            {
                nextTile = keyValuePair.Value.Copy();
                break;
            }
        }

        return nextTile;
    }

    private Dictionary<WorldTile, int> GetNeighbourWeights(Hex hex)
    {
        
        List<WorldTile> neighbours = new List<WorldTile>();

        Vector3Int newPosition = hex.ToVector3Int();
        WorldTile left = (WorldTile)tileMap.GetTile(new Vector3Int(newPosition.x - 1, newPosition.y, newPosition.z));
        
        WorldTile leftUp = newPosition.y % 2 == 0 
            ? (WorldTile)tileMap.GetTile(new Vector3Int(newPosition.x - 1, newPosition.y + 1, newPosition.z))
            : (WorldTile)tileMap.GetTile(new Vector3Int(newPosition.x, newPosition.y + 1, newPosition.z));
        
        WorldTile rightUp = newPosition.y % 2 == 1 
            ? (WorldTile)tileMap.GetTile(new Vector3Int(newPosition.x + 1, newPosition.y + 1, newPosition.z))
            : (WorldTile)tileMap.GetTile(new Vector3Int(newPosition.x, newPosition.y + 1, newPosition.z));
        
        WorldTile right = (WorldTile)tileMap.GetTile(new Vector3Int(newPosition.x + 1, newPosition.y, newPosition.z));
        
        WorldTile rightDown = newPosition.y % 2 == 1 
            ? (WorldTile)tileMap.GetTile(new Vector3Int(newPosition.x + 1, newPosition.y - 1, newPosition.z))
            : (WorldTile)tileMap.GetTile(new Vector3Int(newPosition.x, newPosition.y - 1, newPosition.z));
        
        WorldTile leftDown = newPosition.y % 2 == 0 
            ? (WorldTile)tileMap.GetTile(new Vector3Int(newPosition.x - 1, newPosition.y - 1, newPosition.z))
            : (WorldTile)tileMap.GetTile(new Vector3Int(newPosition.x, newPosition.y - 1, newPosition.z));
        
        Dictionary<WorldTile, int> neighbourWeights = new Dictionary<WorldTile, int>();
        
        neighbours.Add(left);
        neighbours.Add(leftUp);
        neighbours.Add(rightUp);
        neighbours.Add(right);
        neighbours.Add(rightDown);
        neighbours.Add(leftDown);

        foreach (WorldTile neighbour in neighbours)
        {
            if (neighbour != null)
            {
                if (neighbourWeights.ContainsKey(neighbour))
                {
                    neighbourWeights[neighbour] += 1;
                }
                else
                {
                    neighbourWeights.Add(neighbour, 1);
                }
            }
        }

        return neighbourWeights;
    }
    
    private List<WorldTile> SpawnHexesInRange(Hex centerHex, int range)
    {
        List<WorldTile> tilesInRange = new List<WorldTile>();
        
        for (int q = centerHex.q - range; q <= centerHex.q + range; q++)
        {
            for (int r = centerHex.r - range; r <= centerHex.r + range; r++)
            {
                for (int s = centerHex.s - range; s <= centerHex.s + range; s++)
                {
                    if (q + r + s != 0) continue;
                    
                    Hex newHexPosition = new Hex(q, r, s);

                    WorldTile inRangeTile = (WorldTile)tileMap.GetTile(newHexPosition);

                    if (inRangeTile == null)
                    {
                        inRangeTile = GenerateTile(newHexPosition);
                    }

                    inRangeTile.color = inRangeTile.visibleTint;
                    tilesInRange.Add(inRangeTile);
                }
            }
        }

        return tilesInRange;
    }

    private void ApplyFogToTiles(Hex centerHex, int range)
    {
        foreach (WorldTile worldTile in lastInRangeWorldTiles)
        {
            if (centerHex.Distance(worldTile.coords) < range) continue;

            worldTile.color = worldTile.fogTint;
            tileMap.SetTile(worldTile.coords, worldTile);
        }
    }
    
}
