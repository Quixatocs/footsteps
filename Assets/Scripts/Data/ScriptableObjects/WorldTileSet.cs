using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Map/WorldTileSet", order = 1)]
[Serializable]
public class WorldTileSet : ScriptableObject
{
    public AssetReference[] WorldTiles;

    //TODO Change this to a list
    private Dictionary<string, WorldTile> worldTiles = new();
    
    public void AddTile(WorldTile worldTile)
    {
        if (worldTiles.ContainsKey(worldTile.tileName))
        {
            Debug.LogError($"WorldTileSet already contains an entry for <{worldTile.tileName}>.");
            return;
        }
        worldTiles.Add(worldTile.tileName, worldTile);
    }

    public WorldTile GetWorldTile(string tileName)
    {
        return worldTiles[tileName];
    }
}
