using UnityEngine;
using UnityEngine.Tilemaps;

public static class TileMapExtension
{
    public static TileBase GetTile(this Tilemap tilemap, Hex hex)
    {
        var newUnityX = hex.q - (-hex.r + (-hex.r & 1)) / 2;
        var newUnityY = -hex.r;
        Vector3Int newUnityPosition = new Vector3Int(newUnityX, newUnityY, 0);
        return tilemap.GetTile(newUnityPosition);
    }
    
    public static void SetTile(this Tilemap tilemap, Hex hex, TileBase tile)
    {
        var newUnityX = hex.q - (-hex.r + (-hex.r & 1)) / 2;
        var newUnityY = -hex.r;
        Vector3Int newUnityPosition = new Vector3Int(newUnityX, newUnityY, 0);
        return tilemap.SetTile(newUnityPosition);
    }
}
