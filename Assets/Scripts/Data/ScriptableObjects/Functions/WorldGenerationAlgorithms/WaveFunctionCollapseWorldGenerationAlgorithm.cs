using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/WorldGenerationAlgorithm/WaveFunctionCollapse", order = 1)]
public class WaveFunctionCollapseWorldGenerationAlgorithm : WorldGenerationAlgorithm
{
    public override WorldTile GenerateTile(ReadOnlyCollection<WorldTile> allWorldTiles, List<WorldTile> currentTileExistingNeighbours = null)
    {
        throw new System.NotImplementedException();
    }
}
