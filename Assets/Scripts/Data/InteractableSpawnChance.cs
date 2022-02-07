using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Map/SpawnChance", order = 1)]
[Serializable]
public class InteractableSpawnChance : ScriptableObject
{
    public Interactable Interactable;
    public int Chance;
}
