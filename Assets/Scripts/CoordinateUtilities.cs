public static class CoordinateUtilities
{
    public static AxialHexCoordinates UnityHexToAxialHex(UnityHexCoordinates unityHexCoordinates)
    {
        var newQ = unityHexCoordinates.X + (unityHexCoordinates.Z + (unityHexCoordinates.Z & 1)) / 2;
        var newR = -unityHexCoordinates.Z;
        return new AxialHexCoordinates(newQ, newR);
    }
    
    public static UnityHexCoordinates AxialHexToUnityHex(AxialHexCoordinates axialHexCoordinates)
    {
        return new UnityHexCoordinates(2 * axialHexCoordinates.R + axialHexCoordinates.Q, axialHexCoordinates.R);
    }

    public static CubeHexCoordinates AxialHexToCubeHex(AxialHexCoordinates axialHexCoordinates)
    {
        return new CubeHexCoordinates(axialHexCoordinates.Q, axialHexCoordinates.R, -axialHexCoordinates.Q - axialHexCoordinates.R);
    }

    public static AxialHexCoordinates CubeHexToAxialHex(CubeHexCoordinates cubeHexCoordinates)
    {
        return new AxialHexCoordinates(cubeHexCoordinates.Q, cubeHexCoordinates.R);
    }

    public static CubeHexCoordinates UnityHexToCubeHex(UnityHexCoordinates unityHexCoordinates)
    {
        var newQ = unityHexCoordinates.X + (unityHexCoordinates.Z + (unityHexCoordinates.Z & 1)) / 2;
        var newR = -unityHexCoordinates.Z;
        return new CubeHexCoordinates(newQ, newR, -newQ - newR);
    }

    public static UnityHexCoordinates CubeHexToUnityHex(CubeHexCoordinates cubeHexCoordinates)
    {
        var newX = cubeHexCoordinates.Q - (-cubeHexCoordinates.R + (-cubeHexCoordinates.R & 1)) / 2;
        var newY = -cubeHexCoordinates.R;
        return new UnityHexCoordinates(newX, newY);
    }
}
