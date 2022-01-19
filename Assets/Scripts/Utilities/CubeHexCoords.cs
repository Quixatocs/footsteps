using System;

[Serializable]
public struct CubeHexCoords
{
    public int q { get; private set; }

    public int r { get; private set; }
    
    public int s { get; private set; }

    public CubeHexCoords (int q, int r, int s) {
        this.q = q;
        this.r = r;
        this.s = s;
    }
    
    public override string ToString() => $"({q}, {r}, {s})";
}
