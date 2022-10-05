using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/WorldGenerationAlgorithm/WaveFunctionCollapse", order = 1)]
public class WaveFunctionCollapseWorldGenerationAlgorithm : WorldGenerationAlgorithm
{
    public override WorldTile GenerateTile(ReadOnlyCollection<WorldTile> allWorldTiles, List<WorldTile> currentTileExistingNeighbours = null)
    {

        List<string> allTileNeighbourConnections = new List<string>();

        foreach (WorldTile neighbourWorldTile in currentTileExistingNeighbours)
        {
            foreach (TileNeighbourInfo tileNeighbourInfo in neighbourWorldTile.tileNeighbourInfos)
            {
                allTileNeighbourConnections.Add(tileNeighbourInfo.tileName);
            }
        }
        
        int rngRandomWorldTile = Random.Range(0, allWorldTiles.Count);
        
        if (allTileNeighbourConnections.Count == 0) return allWorldTiles[rngRandomWorldTile].Copy();
        
        int rng = Random.Range(0, allTileNeighbourConnections.Count);
        
        foreach (WorldTile foundWorldTile in allWorldTiles)
        {
            if (foundWorldTile.tileName == allTileNeighbourConnections[rng])
            {
                return foundWorldTile.Copy();
            }
        }
            
        return allWorldTiles[rngRandomWorldTile].Copy();
    }
}
