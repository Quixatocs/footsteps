using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Event/BoolEvent", order = 1)]
public class RandomWorldGenerationAlgorithm : WorldGenerationAlgorithm
{
    public override void GenerateTile(WorldTileSet worldTileSet, List<WorldTile> neighbours)
    {
        //int rng = Random.Range(0, worldObjectManager.WorldTilesReadOnly.Count);
       // return worldObjectManager.WorldTilesReadOnly[rng].Copy();
    }
}
