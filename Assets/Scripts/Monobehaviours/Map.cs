﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Debug = UnityEngine.Debug;


public class Map : MonoBehaviour 
{
    [SerializeField] public Grid grid;

    private Tilemap tileMap;
    private List<WorldTile> worldTiles;
    private List<WorldTile> lastInRangeWorldTiles;

    public AssetReference worldTileSetReference;

    public Action OnMapAssetsLoadingComplete;
    
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
                            OnMapAssetsLoadingComplete?.Invoke();
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
    
    public void GenerateTilesAroundPlayer(CubeHexCoordinates playerPosition)
    {
        tileMap = GetComponent<Tilemap>();
        
        lastInRangeWorldTiles = SpawnHexesInRange(playerPosition, 2);

        ApplyFogToTiles();

        tileMap.RefreshAllTiles();
    }

    private WorldTile GenerateTile(Vector3Int newPosition)
    {
        WorldTile newTile = GenerateTileFromNeighbourWeights(newPosition);

        if (newTile == null)
        {
            newTile = GenerateRandomTile();
        }
        
        tileMap.SetTile(newPosition, newTile);

        return newTile;
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
    
    private List<WorldTile> SpawnHexesInRange(CubeHexCoordinates centerHex, int range)
    {
        List<WorldTile> tilesInRange = new List<WorldTile>();
        
        for (int q = centerHex.Q - range; q <= centerHex.Q + range; q++)
        {
            for (int r = centerHex.R - range; r <= centerHex.R + range; r++)
            {
                for (int s = centerHex.S - range; s <= centerHex.S + range; s++)
                {
                    if (q + r + s != 0) continue;
                    
                    Vector3Int newPosition = CoordinateUtilities.CubeHexToUnityHex(new CubeHexCoordinates(q, r, s)).ToVector3Int();

                    WorldTile inRangeTile = (WorldTile)tileMap.GetTile(newPosition);

                    if (inRangeTile == null)
                    {
                        inRangeTile = GenerateTile(newPosition);
                    }

                    inRangeTile.color = inRangeTile.visibleTint;
                    tilesInRange.Add(inRangeTile);
                }
            }
        }

        return tilesInRange;
    }

    private void ApplyFogToTiles()
    {
        foreach (WorldTile worldTile in lastInRangeWorldTiles)
        {
            
        }
    }
    
    
}
