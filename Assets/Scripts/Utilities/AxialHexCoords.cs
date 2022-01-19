using System;

[Serializable]
public class AxialHexCoords
{
    public int q { get; private set; }

    public int r { get; private set; }

    public AxialHexCoords (int q, int r) {
        this.q = q;
        this.r = r;
    }

    public override string ToString() => $"({q}, {r})";
}
