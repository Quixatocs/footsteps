using UnityEngine;

public static class Vector3IntExtension
{
    public static UnityHexCoords ToUnityHexCoordinates(this Vector3Int vector3Int)
    {
        return new UnityHexCoords(vector3Int.x, vector3Int.y);
    }
}
