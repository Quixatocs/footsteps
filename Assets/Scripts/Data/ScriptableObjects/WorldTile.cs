using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Map/WorldTile", order = 1)]
[Serializable]
public class WorldTile : Tile
{
    public Hex coords;
    public string tileName;
    public Color visibleTint;
    public Color fogTint;
    public IntDelta[] costs;
    public IntDelta[] harvestables;
    public InteractableSpawnChance[] interactableSpawnChances;
    public TileNeighbourWeight[] tileNeighbourWeight;
    
    [NonSerialized]
    public List<Interactable> runtimeInteractables;

    public WorldTile Copy()
    {
        WorldTile copiedTile = CreateInstance<WorldTile>();
        
        copiedTile.sprite = sprite;
        copiedTile.color = color;
        copiedTile.coords = coords;
        copiedTile.tileName = tileName;
        copiedTile.visibleTint = visibleTint;
        copiedTile.fogTint = fogTint;
        copiedTile.costs = costs;
        copiedTile.harvestables = harvestables;
        copiedTile.interactableSpawnChances = interactableSpawnChances;
        copiedTile.tileNeighbourWeight = tileNeighbourWeight;

        copiedTile.runtimeInteractables = GenerateRuntimeInteractables();

        return copiedTile;
    }

    public List<Interactable> GenerateRuntimeInteractables()
    {
        List<Interactable> runtimeInteractables = new List<Interactable>();

        foreach (InteractableSpawnChance interactableSpawnChance in interactableSpawnChances)
        {
            int rng = Random.Range(0, 1000);
            if (rng < interactableSpawnChance.Chance)
            {
                runtimeInteractables.Add(interactableSpawnChance.Interactable.Copy());
            }
        }

        return runtimeInteractables;
    }
}
