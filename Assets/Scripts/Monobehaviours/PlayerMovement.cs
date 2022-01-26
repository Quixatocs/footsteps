using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField]
    private Grid grid;
    
    [Header("Variables")]
    public HexVariable playerCurrentHex;
    
    [Header("Events")]
    public HexEvent PlayerMoved;

    public void InitialisePlayerPosition()
    {
        Move(playerCurrentHex.Value);
    }
    
    public void Move(Hex hex)
    {
        playerCurrentHex.Value = hex;
        transform.position = grid.HexToWorld(hex);
        PlayerMoved.Raise(hex);
    }

}
