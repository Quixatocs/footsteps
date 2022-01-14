using System;

[Serializable]
public class AxialHexCoordinates
{
    public int Q { get; private set; }

    public int R { get; private set; }

    public AxialHexCoordinates (int q, int r) {
        Q = q;
        R = r;
    }

    public override string ToString() => $"({Q}, {R})";
}
