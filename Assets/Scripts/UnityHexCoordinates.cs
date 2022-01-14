using System;

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
}
