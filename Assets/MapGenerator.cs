using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour {
    [SerializeField] private Tile waterTile;
    [SerializeField] private Tile sandTile;
    [SerializeField] private Tile grassTile;
    
    void Start()
    {
        Tilemap tileMap = this.GetComponent<Tilemap>();
        
        Tile newTile = ScriptableObject.CreateInstance<Tile>();
        newTile = waterTile;
        
        Tile newTile2 = ScriptableObject.CreateInstance<Tile>();
        newTile2 = sandTile;
        
        tileMap.SetTile(new Vector3Int(0, 0, 0), newTile);
        tileMap.SetTile(new Vector3Int(1, 0, 0), newTile2);
        tileMap.RefreshAllTiles();
        Debug.Log("---");
               
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
