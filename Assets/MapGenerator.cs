﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


public class MapGenerator : MonoBehaviour 
{
    [SerializeField] public Grid grid;

    private Tilemap tileMap;
    private List<WorldTile> worldTiles;
    
    public AssetReference worldTileSetReference;
    

    private void Update() {

        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // get the collision point of the ray with the z = 0 plane
            Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
            // get current grid location
            Vector3Int position = grid.WorldToCell(worldPoint);
            

            GenerateTile(position);
        }
    }

    void Start()
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
                            GenerateStartingMap();
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
    
    private void GenerateStartingMap() 
    {
        tileMap = GetComponent<Tilemap>();
        
        //First set the starting tile to sand
        tileMap.SetTile(new Vector3Int(0, 0, 0), worldTiles[1]);
        tileMap.SetTile(new Vector3Int(-1, 0, 0), worldTiles[0]);
        tileMap.SetTile(new Vector3Int(-1, 1, 0), worldTiles[0]);
        tileMap.SetTile(new Vector3Int(0, 1, 0), worldTiles[2]);
        tileMap.SetTile(new Vector3Int(1, 0, 0), worldTiles[1]);
        tileMap.SetTile(new Vector3Int(0, -1, 0), worldTiles[1]);
        tileMap.SetTile(new Vector3Int(-1, -1, 0), worldTiles[0]);
        
        tileMap.RefreshAllTiles();
    }

    private void GenerateTile(Vector3Int newPosition)
    {
        WorldTile newTile = GenerateTileFromNeighbourWeights(newPosition);

        if (newTile == null)
        {
            newTile = GenerateRandomTile();
        }
        
        tileMap.SetTile(newPosition, newTile);
        UnityHexCoordinates newUnityCoordinate = new UnityHexCoordinates(newPosition.x, newPosition.y);
        CubeHexCoordinates newCubeCoordinate = CoordinateUtilities.UnityHexToCubeHex(newUnityCoordinate);
        UnityHexCoordinates reNewUnityCoordinate = CoordinateUtilities.CubeHexToUnityHex(newCubeCoordinate);
        Debug.Log($"<{newTile.tileName}>, U<{newPosition}>, C<{newCubeCoordinate}>, U<{reNewUnityCoordinate}>");
        tileMap.RefreshAllTiles();
    }

    private WorldTile GenerateRandomTile()
    {
        int rng = Random.Range(0, worldTiles.Count);
        return worldTiles[rng];
    }

    private WorldTile GenerateTileFromNeighbourWeights(Vector3Int newPosition)
    {
        
        // Get the six neighbours
        Dictionary<WorldTile, int> neighbourWeights = GetNeighbourWeights(newPosition);

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

        //TODO: check this out
        //Query the table
        int rng = Random.Range(0, 100);

        foreach (KeyValuePair<int, WorldTile> keyValuePair in percentageTileWeights)
        {
            if (keyValuePair.Key > rng)
            {
                nextTile = keyValuePair.Value;
                break;
            }
        }

        return nextTile;
    }

    private Dictionary<WorldTile, int> GetNeighbourWeights(Vector3Int newPosition)
    {
        
        List<WorldTile> neighbours = new List<WorldTile>();

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
    
    //TODO FIX THISS
    
    private List<Vector3Int> GetNeighbors(Vector3Int unityCell, int range)
    {
        //var centerCubePos = UnityCellToAxial(unityCell);

        var result = new List<Vector3Int>();
    
        int min = -range, max = range;

        for (int x = min; x <= max; x++)
        {
            for (int y = min; y <= max; y++)
            {
                var z = -x - y;
                if (z < min || z > max)
                {
                    continue;
                }

                var cubePosOffset = new Vector3Int(x, y, z);
                //result.Add(CubeToUnityCell(centerCubePos + cubePosOffset));
            }

        }

        return result;
    }
    
    
}
