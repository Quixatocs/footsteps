using System;
using UnityEngine;

public static class CoordUtils
{
    public static AxialHexCoords UnityHexToAxialHex(UnityHexCoords unityHexCoords)
    {
        var newQ = unityHexCoords.x + (unityHexCoords.z + (unityHexCoords.z & 1)) / 2;
        var newR = -unityHexCoords.z;
        return new AxialHexCoords(newQ, newR);
    }
    
    public static UnityHexCoords AxialHexToUnityHex(AxialHexCoords axialHexCoords)
    {
        return new UnityHexCoords(2 * axialHexCoords.r + axialHexCoords.q, axialHexCoords.r);
    }

    public static CubeHexCoords AxialHexToCubeHex(AxialHexCoords axialHexCoords)
    {
        return new CubeHexCoords(axialHexCoords.q, axialHexCoords.r, -axialHexCoords.q - axialHexCoords.r);
    }

    public static AxialHexCoords CubeHexToAxialHex(CubeHexCoords cubeHexCoords)
    {
        return new AxialHexCoords(cubeHexCoords.q, cubeHexCoords.r);
    }

    public static CubeHexCoords UnityHexToCubeHex(UnityHexCoords unityHexCoords)
    {
        var newQ = unityHexCoords.x + (unityHexCoords.z + (unityHexCoords.z & 1)) / 2;
        var newR = -unityHexCoords.z;
        return new CubeHexCoords(newQ, newR, -newQ - newR);
    }

    public static UnityHexCoords CubeHexToUnityHex(CubeHexCoords cubeHexCoords)
    {
        var newX = cubeHexCoords.q - (-cubeHexCoords.r + (-cubeHexCoords.r & 1)) / 2;
        var newY = -cubeHexCoords.r;
        return new UnityHexCoords(newX, newY);
    }

    public static CubeHexCoords CubeSubtract(CubeHexCoords cubeA, CubeHexCoords cubeB)
    {
        return new CubeHexCoords(cubeA.q - cubeB.q, cubeA.r - cubeB.r, cubeA.s - cubeB.s);
    }

    public static int CubeDistance(CubeHexCoords cubeA, CubeHexCoords cubeB)
    {
        CubeHexCoords vector = CubeSubtract(cubeA, cubeB);
        return Mathf.Max(Mathf.Abs(vector.q), Math.Abs(vector.r), Math.Abs(vector.s));
    }
}
