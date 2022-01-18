using UnityEngine;

public static class Vector3IntExtension
{
    public static UnityHexCoordinates ToUnityHexCoordinates(this Vector3Int vector3Int)
    {
        return new UnityHexCoordinates(vector3Int.x, vector3Int.y);
    }
}
