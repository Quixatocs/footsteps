using UnityEngine;

public static class GridExtension
{
    public static Vector3 CellToWorld(this Grid grid, Hex hex)
    {
        return grid.CellToWorld(hex.ToUnityCoords());
    }
}
