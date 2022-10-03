using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Map/WorldTileSet", order = 1)]
[Serializable]
public class WorldTileSet : ScriptableObject
{
    public AssetReference[] WorldTiles;

    private List<WorldTile> worldTiles = new();
    private List<string> allTileNames = new();

    public void AddTile(WorldTile worldTile)
    {
        worldTiles.Add(worldTile);
    }

    public WorldTile GetWorldTile(string tileName)
    {
        foreach (WorldTile worldTile in worldTiles)
        {
            if (worldTile.tileName != tileName) continue;
            
            return worldTile;
        }
        
        Debug.Log($"No WorldTile with name <{tileName}> exists in the WorldTileSet");
        return null;
    }

    public List<string> GetNames()
    {
        if (allTileNames.Count != 0) return allTileNames;

        for (int i = 0; i < worldTiles.Count; i++)
        {
            allTileNames.Add(worldTiles[i].tileName);
        }

        return allTileNames;
    }
    
}
