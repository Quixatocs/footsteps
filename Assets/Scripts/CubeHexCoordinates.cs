using System;

[Serializable]
public struct CubeHexCoordinates
{
    public int Q { get; private set; }

    public int R { get; private set; }
    
    public int S { get; private set; }

    public CubeHexCoordinates (int q, int r, int s) {
        Q = q;
        R = r;
        S = s;
    }
    
    public override string ToString() => $"({Q}, {R}, {S})";
}
