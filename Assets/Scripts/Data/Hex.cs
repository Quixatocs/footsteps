using UnityEngine;

public class Hex
{
    private Vector3Int unity;
    public Vector3Int Unity => unity;
    
    private Vector3Int cube;
    public Vector3Int Cube => cube;

    public void SetFromUnity(Vector3Int unityCoords)
    {
        unity = unityCoords;
        
        var newCubeX = unityCoords.x + (unityCoords.y + (unityCoords.y & 1)) / 2;
        var newCubeY = -unityCoords.y;
        cube = new Vector3Int(newCubeX, newCubeY, -newCubeX - newCubeY);
    }

    public void SetFromCube(Vector3Int cubeCoords)
    {
        cube = cubeCoords;
        
        var newUnityX = cubeCoords.x - (-cubeCoords.y + (-cubeCoords.y & 1)) / 2;
        var newUnityY = -cubeCoords.y;
        unity = new Vector3Int(newUnityX, newUnityY, 0);
    }
    
}
