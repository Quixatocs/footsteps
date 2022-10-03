using System.Collections.Generic;
using UnityEngine;

public abstract class WorldGenerationAlgorithm : ScriptableObject
{
    public abstract WorldTile GenerateTile(List<WorldTile> allWorldTiles, List<WorldTile> currentTileExistingNeighbours = null);
}
