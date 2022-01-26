﻿using System;
using UnityEngine;

[Serializable]
public class Hex
{
    private Vector3Int coords;
    
    public int q => coords.x;
    public int r => coords.y;
    public int s => coords.z;

    public Hex(Vector3Int coords, bool isCube = true)
    {
        this.coords = coords;

        if (isCube) return;
        
        var newQ = coords.x + (coords.y + (coords.y & 1)) / 2;
        var newR = -coords.y;
        this.coords = new Vector3Int(newQ, newR, -newQ - newR);
    }
    
    public Hex(int q, int r, int s)
    {
        coords = new Vector3Int(q, r, s);
    }

    public Vector3Int ToVector3Int()
    {
        return coords;
    }

    public Vector3Int ToUnityCoords()
    {
        var newX = coords.x - (-coords.y + (-coords.y & 1)) / 2;
        var newY = -coords.y;
        return new Vector3Int(newX, newY, 0);
    }

    public Hex Subtract(Hex other)
    {
        return new Hex(new Vector3Int(coords.x - other.coords.x, coords.y - other.coords.y, coords.z - other.coords.z));
    }

    public int Distance(Hex other)
    {
        Hex vector = Subtract(other);
        return Mathf.Max(Mathf.Abs(vector.coords.x), Math.Abs(vector.coords.y), Math.Abs(vector.coords.z));
    }
}
