using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/New WorldTile", order = 1)]
[Serializable]
public class WorldTile : Tile
{
    public string name;
    public TileNeighbourWeight[] tileNeighbourWeight;
}
