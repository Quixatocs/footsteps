using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Map/WorldTileList", order = 1)]
public class WorldTileList : ScriptableObject
{
    private List<WorldTile> worldTiles = new();

    public void SetWorldTiles(List<WorldTile> worldTiles)
    {
        this.worldTiles = worldTiles;
    }

    public List<WorldTile> GetWorldTiles()
    {
        return worldTiles;
    }

    public void Clear()
    {
        worldTiles.Clear();
    }
    
    private void OnEnable()
    {
        worldTiles = new List<WorldTile>();
    }
    
}
