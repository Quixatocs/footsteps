using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Tilemaps;

public class GenerateTilesAtDataHexState : State
{
    [Header("Asset References")]
    [SerializeField]
    private AssetReference worldObjectManagerReference;
    [SerializeField]
    private AssetReference playerVisionRangeReference;
    [SerializeField]
    private AssetReference playerCurrentHexReference;
    
    [Header("Prefab References")]
    [SerializeField] private GameObject interactablePrefab;
    
    private WorldObjectManager worldObjectManager;
    private List<WorldTile> lastInRangeWorldTiles;
    private IntVariable visionRange;
    private HexVariable playerCurrentHex;
    private Tilemap tileMap;
    private Grid grid;
    
    public override void OnEnter()
    {
        base.OnEnter();

        if (IsInitialised)
        {
            Continue();
            return;
        }

        if (stateHandleOperation.HasValue) stateHandleOperation.Value.Completed += OnNextStateAssetLoaded;
        
        ++assetLoadCount;
        Addressables.LoadAssetAsync<WorldObjectManager>(worldObjectManagerReference).Completed += OnWorldObjectManagerAssetLoaded;
        ++assetLoadCount;
        Addressables.LoadAssetAsync<IntVariable>(playerVisionRangeReference).Completed += OnPlayerVisionRangeAssetLoaded;
        ++assetLoadCount;
        Addressables.LoadAssetAsync<HexVariable>(playerCurrentHexReference).Completed += OnPlayerCurrentHexAssetLoaded;
    }
    
    private void OnNextStateAssetLoaded(AsyncOperationHandle<State> obj)
    {
        if (obj.Status != AsyncOperationStatus.Succeeded) return;
        
        nextState = obj.Result;
        Debug.Log($"Successfully loaded asset <{nextState.name}>");

        ContinueOnAllAssetsLoaded();
    }
    
    private void OnWorldObjectManagerAssetLoaded(AsyncOperationHandle<WorldObjectManager> obj)
    {
        if (obj.Status != AsyncOperationStatus.Succeeded) return;
        
        worldObjectManager = obj.Result;
        Debug.Log($"Successfully loaded asset <{worldObjectManager.name}>");
        
        if (tileMap == null)
        {
            tileMap = worldObjectManager.GetComponent<Tilemap>();
        }
        
        if (grid == null)
        {
            grid = worldObjectManager.GetComponent<Grid>();
        }
            
        ContinueOnAllAssetsLoaded();
    }
    
    private void OnPlayerVisionRangeAssetLoaded(AsyncOperationHandle<IntVariable> obj)
    {
        if (obj.Status != AsyncOperationStatus.Succeeded) return;
        
        visionRange = obj.Result;
        Debug.Log($"Successfully loaded asset <{visionRange.name}>");
            
        ContinueOnAllAssetsLoaded();
    }
    
    private void OnPlayerCurrentHexAssetLoaded(AsyncOperationHandle<HexVariable> obj)
    {
        if (obj.Status != AsyncOperationStatus.Succeeded) return;
        
        playerCurrentHex = obj.Result;
        Debug.Log($"Successfully loaded asset <{playerCurrentHex.name}>");
            
        ContinueOnAllAssetsLoaded();
    }
    
    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
    }

    protected override void ContinueOnAllAssetsLoaded()
    {
        if (--assetLoadCount != 0) return;
        
        IsInitialised = true;
        Continue();
    }

    protected override void Continue()
    {
        GenerateTilesAroundPlayer(playerCurrentHex.Value);
    }
    
    public void GenerateTilesAroundPlayer(Hex hex)
    {
        if (lastInRangeWorldTiles != null)
        {
            ApplyFogToTiles(hex, visionRange.Value);
        }
        
        lastInRangeWorldTiles = GetTilesInRange(hex, visionRange.Value, true);

        tileMap.RefreshAllTiles();
    }
    
    private void ApplyFogToTiles(Hex centerHex, int range)
    {
        foreach (WorldTile worldTile in lastInRangeWorldTiles)
        {
            if (centerHex.Distance(worldTile.coords) < range) continue;

            worldTile.color = worldTile.fogTint;
            tileMap.SetTile(worldTile.coords, worldTile);
        }
        tileMap.RefreshAllTiles();
    }
    
    private List<WorldTile> GetTilesInRange(Hex centerHex, int range, bool canSpawn = false)
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

                    if (inRangeTile == null && canSpawn)
                    {
                        inRangeTile = GenerateTile(newHexPosition);
                    }

                    if (inRangeTile != null)
                    {
                        inRangeTile.color = inRangeTile.visibleTint;
                        tilesInRange.Add(inRangeTile); 
                    }
                }
            }
        }

        return tilesInRange;
    }
    
    private WorldTile GenerateTile(Hex hex)
    {
        WorldTile newTile = GenerateTileFromNeighbourWeights(hex);

        if (newTile == null)
        {
            newTile = GenerateRandomTile();
        }

        newTile.coords = hex;
        
        DrawTileInteractables(hex, newTile);
        tileMap.SetTile(hex, newTile);

        return newTile;
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
        List<WorldTile> neighbours = GetTilesInRange(hex, 1);
        
        Dictionary<WorldTile, int> neighbourWeights = new Dictionary<WorldTile, int>();
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
    
    private WorldTile GenerateRandomTile()
    {
        int rng = Random.Range(0, worldObjectManager.WorldTilesReadOnly.Count);
        return worldObjectManager.WorldTilesReadOnly[rng].Copy();
    }
    
    private void DrawTileInteractables(Hex hex, WorldTile tile)
    {
        if (tile.runtimeInteractables == null || tile.runtimeInteractables.Count == 0) return;

        foreach (Interactable interactable in tile.runtimeInteractables)
        {
            Vector3 worldPosition = grid.HexToWorld(hex);
            GameObject imageHolder = Instantiate(interactablePrefab, worldPosition + grid.cellSize * 0.15f, Quaternion.identity, tileMap.gameObject.transform);
            imageHolder.name = hex.ToVector3Int().ToString();
            imageHolder.GetComponent<SpriteRenderer>().sprite = interactable.sprite;
            interactable.MapIcon = imageHolder;
        }
    }

    public override State GetNextState()
    {
        return nextState;
    }
}
