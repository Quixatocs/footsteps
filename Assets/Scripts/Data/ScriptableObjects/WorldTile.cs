﻿using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Map/WorldTile", order = 1)]
[Serializable]
public class WorldTile : Tile
{
    public Hex coords;
    public string tileName;
    public Color visibleTint;
    public Color fogTint;
    public WorldTileDelta[] worldTileDeltas;
    public TileNeighbourWeight[] tileNeighbourWeight;

    public WorldTile Copy()
    {
        WorldTile copiedTile = CreateInstance<WorldTile>();
        copiedTile.sprite = sprite;
        copiedTile.color = color;
        copiedTile.coords = coords;
        copiedTile.tileName = tileName;
        copiedTile.visibleTint = visibleTint;
        copiedTile.fogTint = fogTint;
        copiedTile.worldTileDeltas = worldTileDeltas;
        copiedTile.tileNeighbourWeight = tileNeighbourWeight;

        return copiedTile;
    }
}
