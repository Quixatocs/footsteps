using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/WorldGenerationAlgorithm/ExistingNeighbourWeight", order = 1)]
public class ExistingNeighbourWeightWorldGenerationAlgorithm : WorldGenerationAlgorithm
{
    private const int TESTED_PERCENTAGE = 96;
    private const int CUMULATIVE_PERCENTAGE_START = 4;
    public override WorldTile GenerateTile(WorldTile self, ReadOnlyCollection<WorldTile> allWorldTiles, List<WorldTile> currentTileExistingNeighbours = null)
    {
        //TODO deal with self is null (pick a random tile)
        //TODO decide whether we want to use self instead of random picking
        
        Dictionary<WorldTile, int> neighbourWeights = new Dictionary<WorldTile, int>();
        
        foreach (WorldTile neighbour in currentTileExistingNeighbours)
        {
            if (neighbour != null)
            {
                if (neighbourWeights.ContainsKey(neighbour))
                {
                    neighbourWeights[neighbour] += 1;
                }
                else
                {
                    neighbourWeights.Add(neighbour, 1);
                }
            }
        }
        int rngRandomWorldTile = Random.Range(0, allWorldTiles.Count);

        if (neighbourWeights.Count == 0)
        {
            return allWorldTiles[rngRandomWorldTile].Copy();
        }
        
        WorldTile nextTile = null;
        int totalValues = 0;
        
        foreach (int value in neighbourWeights.Values)
        {
            totalValues += value;
        }
        
        int percentWeightUnit = Mathf.FloorToInt(TESTED_PERCENTAGE / totalValues);
        
        //Build the Table
        Dictionary<int, WorldTile> percentageTileWeights = new Dictionary<int, WorldTile>();

        int cumulativePercentage = CUMULATIVE_PERCENTAGE_START;
        
        percentageTileWeights.Add(cumulativePercentage, allWorldTiles[rngRandomWorldTile].Copy());

        foreach (KeyValuePair<WorldTile, int> keyValuePair in neighbourWeights)
        {
            cumulativePercentage += keyValuePair.Value * percentWeightUnit;
            percentageTileWeights.Add(cumulativePercentage, keyValuePair.Key);
        }

        //Query the table
        int rng = Random.Range(0, 100);

        foreach (KeyValuePair<int, WorldTile> keyValuePair in percentageTileWeights)
        {
            if (keyValuePair.Key > rng)
            {
                nextTile = keyValuePair.Value.Copy();
                break;
            }
        }

        return nextTile;
    }
}
