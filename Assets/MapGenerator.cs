using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;


public class MapGenerator : MonoBehaviour 
{
    [SerializeField] public Grid grid;
    [SerializeField] private Tile waterTile;
    [SerializeField] private Tile sandTile;
    [SerializeField] private Tile grassTile;

    private Tilemap tileMap;

    private Tile[] allTiles;
    

    private void Update() {

        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // get the collision point of the ray with the z = 0 plane
            Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
            // get current grid location
            Vector3Int position = grid.WorldToCell(worldPoint);
            Debug.Log(position);
            //Debug.Log(highlightMap.size);

            GenerateTile(position);
        }
    }

    void Start()
    {
        tileMap = GetComponent<Tilemap>();

        int rows = 10;
        int columns = 10;

        allTiles = new[] {waterTile, sandTile, grassTile};

        GenerateStartingMap();
        
        tileMap.RefreshAllTiles();
        
        InvokeRepeating("GenGen", 1f, 0.03f);
    }

    private void GenGen()
    {
        GenerateTile(new Vector3Int(Random.Range(-10, 10), Random.Range(-10, 10), 0));
    }

    private void GenerateStartingMap() 
    {
        //First set the starting tile to sand
        tileMap.SetTile(new Vector3Int(0, 0, 0), sandTile);
        tileMap.SetTile(new Vector3Int(-1, 0, 0), waterTile);
        tileMap.SetTile(new Vector3Int(-1, 1, 0), waterTile);
        tileMap.SetTile(new Vector3Int(0, 1, 0), grassTile);
        tileMap.SetTile(new Vector3Int(1, 0, 0), sandTile);
        tileMap.SetTile(new Vector3Int(0, -1, 0), sandTile);
        tileMap.SetTile(new Vector3Int(-1, -1, 0), waterTile);
    }

    private void GenerateTile(Vector3Int newPosition)
    {
        Tile newTile = GenerateTileFromNeighbourWeights(newPosition);

        if (newTile == null)
        {
            newTile = GenerateRandomTile();
        }
        
        tileMap.SetTile(newPosition, newTile);
        tileMap.RefreshAllTiles();
    }

    private Tile GenerateRandomTile()
    {
        int rng = Random.Range(0, allTiles.Length);

        return allTiles[rng];
    }

    private Tile GenerateTileFromNeighbourTypes(Vector3Int newPosition)
    {
        // Get the six neighbours
        TileBase[] tiles = GetNeighbourWeights(newPosition).Keys.ToArray();

        if (tiles.Length == 0) return null;
        
        int rng = Random.Range(0, tiles.Length);

        return tiles[rng] as Tile;
    }
    
    private Tile GenerateTileFromNeighbourWeights(Vector3Int newPosition)
    {
        
        // Get the six neighbours
        Dictionary<TileBase, int> neighbourWeights = GetNeighbourWeights(newPosition);

        if (neighbourWeights.Count == 0) return null;
        
        Tile nextTile = null;
        int totalValues = 0;
        
        foreach (int value in neighbourWeights.Values)
        {
            totalValues += value;
        }
        
        int percentWeightUnit = Mathf.FloorToInt(96 / totalValues);
        
        //Build the Table
        Dictionary<int, Tile> percentageTileWeights = new Dictionary<int, Tile>();

        int randomTileIndex = Random.Range(0, allTiles.Length);
        int cumulativePercentage = 4;
        percentageTileWeights.Add(cumulativePercentage, allTiles[randomTileIndex]);

        foreach (KeyValuePair<TileBase, int> keyValuePair in neighbourWeights)
        {
            cumulativePercentage += keyValuePair.Value * percentWeightUnit;
            percentageTileWeights.Add(cumulativePercentage, keyValuePair.Key as Tile);
        }

        //TODO: check this out
        //Query the table
        int rng = Random.Range(0, 100);

        foreach (KeyValuePair<int, Tile> keyValuePair in percentageTileWeights)
        {
            if (keyValuePair.Key > rng)
            {
                nextTile = keyValuePair.Value;
                break;
            }
        }

        return nextTile;
    }

    private Dictionary<TileBase, int> GetNeighbourWeights(Vector3Int newPosition)
    {
        
        List<TileBase> neighbours = new List<TileBase>();

        TileBase left = tileMap.GetTile(new Vector3Int(newPosition.x - 1, newPosition.y, newPosition.z));
        
        TileBase leftUp = newPosition.y % 2 == 0 
            ? tileMap.GetTile(new Vector3Int(newPosition.x - 1, newPosition.y + 1, newPosition.z))
            : tileMap.GetTile(new Vector3Int(newPosition.x, newPosition.y + 1, newPosition.z));
        
        TileBase rightUp = newPosition.y % 2 == 1 
            ? tileMap.GetTile(new Vector3Int(newPosition.x + 1, newPosition.y + 1, newPosition.z))
            : tileMap.GetTile(new Vector3Int(newPosition.x, newPosition.y + 1, newPosition.z));
        
        TileBase right = tileMap.GetTile(new Vector3Int(newPosition.x + 1, newPosition.y, newPosition.z));
        
        TileBase rightDown = newPosition.y % 2 == 1 
            ? tileMap.GetTile(new Vector3Int(newPosition.x + 1, newPosition.y - 1, newPosition.z))
            : tileMap.GetTile(new Vector3Int(newPosition.x, newPosition.y - 1, newPosition.z));
        
        TileBase leftDown = newPosition.y % 2 == 0 
            ? tileMap.GetTile(new Vector3Int(newPosition.x - 1, newPosition.y - 1, newPosition.z))
            : tileMap.GetTile(new Vector3Int(newPosition.x, newPosition.y - 1, newPosition.z));
        
        Dictionary<TileBase, int> neighbourWeights = new Dictionary<TileBase, int>();
        
        neighbours.Add(left);
        neighbours.Add(leftUp);
        neighbours.Add(rightUp);
        neighbours.Add(right);
        neighbours.Add(rightDown);
        neighbours.Add(leftDown);

        foreach (TileBase neighbour in neighbours)
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
        
        Debug.Log(neighbourWeights.Count);
        foreach (KeyValuePair<TileBase, int> neighbourWeight in neighbourWeights)
        {
            Debug.Log($"Key: {neighbourWeight.Key}, Val: {neighbourWeight.Value}");
        }
        
        return neighbourWeights;
    }
    
    
}
