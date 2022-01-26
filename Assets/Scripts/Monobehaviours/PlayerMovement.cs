using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Scene Settings")]
    private Grid grid;
    
    [Header("Variables")]
    public HexVariable playerCurrentHex;
    
    [Header("Events")]
    public HexEvent PlayerMoved;
    
    public void Move(Hex hex)
    {
        playerCurrentHex.Value = hex;
        transform.position = grid.HexToWorld(hex);
        PlayerMoved.Raise(hex);
    }

}
