using System;
using UnityEngine;

[Serializable]
public struct UnityHexCoords
{
    public int x { get; private set; }

    public int z { get; private set; }

    public UnityHexCoords (int x, int z) {
        this.x = x;
        this.z = z;
    }
    
    public override string ToString() => $"({x}, {z})";

    public Vector3Int ToVector3Int()
    {
        return new Vector3Int(x, z, 0);
    }
}
