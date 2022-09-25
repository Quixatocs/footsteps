using System.Collections.Generic;
using UnityEngine;

public abstract class WorldGenerationAlgorithm : ScriptableObject
{
    public abstract void GenerateTile(WorldTileSet worldTileSet, List<WorldTile> neighbours);
}
