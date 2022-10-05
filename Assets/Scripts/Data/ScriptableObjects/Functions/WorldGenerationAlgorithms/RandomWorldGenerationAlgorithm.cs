using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/WorldGenerationAlgorithm/Random", order = 1)]
public class RandomWorldGenerationAlgorithm : WorldGenerationAlgorithm
{
    public override WorldTile GenerateTile(WorldTile self, ReadOnlyCollection<WorldTile> allWorldTiles, List<WorldTile> currentTileExistingNeighbours = null)
    {
        int rng = Random.Range(0, allWorldTiles.Count);
        return allWorldTiles[rng].Copy();
    }
}
