using UnityEngine;

public static class GridExtension
{
    public static Vector3 HexToWorld(this Grid grid, Hex hex)
    {
        return grid.CellToWorld(hex.ToUnityCoords());
    }
    
    public static Hex WorldToHex(this Grid grid, Vector3 worldPoint)
    {
        Vector3Int unityCell = grid.WorldToCell(worldPoint);
        return new Hex(unityCell, false);
    }
}
