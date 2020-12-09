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
        
        //work out the weights of the new tile generation from the neighbours
        
        tileMap.SetTile(new Vector3Int(0, 0, 0), sandTile);
        tileMap.RefreshAllTiles();
    }

    private List<TileBase> GetNeighbours(Vector3Int newPosition)
    {
        List<TileBase> neighbours = new List<TileBase>();
        TileBase leftNeighbour = tileMap.GetTile(new Vector3Int(newPosition.x - 1, newPosition.y, newPosition.z));
        TileBase leftUpNeighbour = tileMap.GetTile(new Vector3Int(newPosition.x - 1, newPosition.y + 1, newPosition.z));
        TileBase rightUpNeighbour = tileMap.GetTile(new Vector3Int(newPosition.x, newPosition.y + 1, newPosition.z));
        TileBase rightNeighbour = tileMap.GetTile(new Vector3Int(newPosition.x + 1, newPosition.y, newPosition.z));
        TileBase rightDownNeighbour = tileMap.GetTile(new Vector3Int(newPosition.x, newPosition.y - 1, newPosition.z));
        TileBase leftDownNeighbour = tileMap.GetTile(new Vector3Int(newPosition.x - 1, newPosition.y - 1, newPosition.z));
        
        neighbours.Add(leftNeighbour);
        neighbours.Add(leftUpNeighbour);
        neighbours.Add(rightUpNeighbour);
        neighbours.Add(rightNeighbour);
        neighbours.Add(rightDownNeighbour);
        neighbours.Add(leftDownNeighbour);

        return neighbours;
    }
}
