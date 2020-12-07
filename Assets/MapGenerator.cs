using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour {
    [SerializeField] private Tile waterTile;
    [SerializeField] private Tile sandTile;
    [SerializeField] private Tile grassTile;
    
    void Start()
    {
        Tilemap tileMap = GetComponent<Tilemap>();

        int rows = 10;
        int columns = 10;

        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < columns; j++) {
                Tile newTile = ScriptableObject.CreateInstance<Tile>();
                newTile = sandTile;
                tileMap.SetTile(new Vector3Int(i, j, 0), newTile);
            }
        }

        tileMap.RefreshAllTiles();
        Debug.Log("---");
               
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
