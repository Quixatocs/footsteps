using System;
using UnityEngine;

[Serializable]
public class Hex
{
    private Vector3Int unity;
    public Vector3Int Unity => unity;

    private Vector3 hexCenterWorldPoint;
    public Vector3 HexCenterWorldPoint => hexCenterWorldPoint;

    public Vector3Int Cube
    {
        get
        {
            var newCubeX = unity.x + (unity.y + (unity.y & 1)) / 2;
            var newCubeY = -unity.y;
            return new Vector3Int(newCubeX, newCubeY, -newCubeX - newCubeY);
        }
        set
        {
            var newUnityX = value.x - (-value.y + (-value.y & 1)) / 2;
            var newUnityY = -value.y;
            unity = new Vector3Int(newUnityX, newUnityY, 0);
        }
    }

    public Hex(Vector3Int unity)
    {
        this.unity = unity;
    }
    
    public Hex(Vector3Int unity, Vector3 hexCenterWorldPoint)
    {
        this.unity = unity;
        this.hexCenterWorldPoint = hexCenterWorldPoint;
    }

    public Hex Subtract(Hex other)
    {
        return new Hex(new Vector3Int(Cube.x - other.Cube.x, Cube.y - other.Cube.y, Cube.z - other.Cube.z));
    }

    public int Distance(Hex other)
    {
        Hex vector = Subtract(other);
        return Mathf.Max(Mathf.Abs(vector.Cube.x), Math.Abs(vector.Cube.y), Math.Abs(vector.Cube.z));
    }
}
