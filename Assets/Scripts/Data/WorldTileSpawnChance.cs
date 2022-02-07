using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Map/SpawnChance", order = 1)]
[Serializable]
public class WorldTileSpawnChance : ScriptableObject
{
    public Interactable Interactable;
    public int Chance;
}
