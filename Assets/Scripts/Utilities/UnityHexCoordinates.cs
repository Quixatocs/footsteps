using System;
using UnityEngine;

[Serializable]
public struct UnityHexCoordinates
{
    public int X { get; private set; }

    public int Z { get; private set; }

    public UnityHexCoordinates (int x, int z) {
        X = x;
        Z = z;
    }
    
    public override string ToString() => $"({X}, {Z})";

    public Vector3Int ToVector3Int()
    {
        return new Vector3Int(X, Z, 0);
    }
}
