using System;
using System.Collections.Generic;
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

        GenerateStartingMap();
        
        
        tileMap.RefreshAllTiles();
               
    }

    private void GenerateStartingMap() 
    {
        //First set the starting tile to sand
        tileMap.SetTile(new Vector3Int(0, 0, 0), sandTile);
        tileMap.SetTile(new Vector3Int(-1, 0, 0), waterTile);
        tileMap.SetTile(new Vector3Int(-1, 1, 0), waterTile);
        tileMap.SetTile(new Vector3Int(0, 1, 0), sandTile);
        tileMap.SetTile(new Vector3Int(1, 0, 0), sandTile);
        tileMap.SetTile(new Vector3Int(0, -1, 0), sandTile);
        tileMap.SetTile(new Vector3Int(-1, -1, 0), waterTile);
    }

    private void GenerateTile(Vector3Int newPosition)
    {
        // Get the six neighbours
        Dictionary<TileBase, int> neighbourWeights = GetNeighbourWeights(newPosition);

        Tile nextTile = null;
        int totalValues = 0;
        
        foreach (int value in neighbourWeights.Values)
        {
            totalValues += value;
        }

        int percentWeightUnit = Mathf.FloorToInt(95 / totalValues);
        
        Dictionary<int, TileBase> weightTable = new Dictionary<int, TileBase>();
        
        foreach (KeyValuePair<TileBase, int> neighbourWeight in neighbourWeights)
        {
            int newWeight = neighbourWeight.Value * percentWeightUnit;
            if (weightTable.ContainsKey(newWeight))
            {
                weightTable.Add(newWeight + 5, neighbourWeight.Key);
            }
            else
            {
                weightTable.Add(newWeight, neighbourWeight.Key);  
            }
            
        }

        int rng = Mathf.FloorToInt(Random.Range(0f, 100f));

        foreach (var weight in weightTable)
        {
            if (rng < weight.Key)
            {
                nextTile = weight.Value as Tile;
                break;
            }
            
            //Nothing in the weight table so lets pick something else
            nextTile = grassTile;

        }
        tileMap.SetTile(newPosition, nextTile);
        tileMap.RefreshAllTiles();
    }

    private Dictionary<TileBase, int> GetNeighbourWeights(Vector3Int newPosition)
    {
        Dictionary<TileBase, int> neighbourWeights = new Dictionary<TileBase, int>();
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
