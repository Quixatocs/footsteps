using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
        
        //work out the weights of the new tile generation from the neighbours
        
        tileMap.SetTile(newPosition, sandTile);
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
        foreach (var neighbourWeight in neighbourWeights)
        {
            Debug.Log($"Key: {neighbourWeight.Key}, Val: {neighbourWeight.Value}");
        }
        

        return neighbourWeights;
    }
}
