using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/New WorldTile", order = 1)]
[Serializable]
public class WorldTile : Tile
{
    public string tileName;
    public Color visibleTint;
    public Color fogTint;
    public TileNeighbourWeight[] tileNeighbourWeight;
}
