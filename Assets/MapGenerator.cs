using System;
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
        }
    }

    void Start()
    {
        tileMap = GetComponent<Tilemap>();

        int rows = 10;
        int columns = 10;

        GenerateStartingMap();
        
        tileMap.RefreshAllTiles();
        Debug.Log("---");
               
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
}
