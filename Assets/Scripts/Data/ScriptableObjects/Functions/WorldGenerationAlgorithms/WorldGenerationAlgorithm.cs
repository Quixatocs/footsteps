using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public abstract class WorldGenerationAlgorithm : ScriptableObject
{
    public abstract WorldTile GenerateTile(ReadOnlyCollection<WorldTile> allWorldTiles, List<WorldTile> currentTileExistingNeighbours = null);
}
