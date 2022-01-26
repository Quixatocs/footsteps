using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Grid grid;
    public HexEvent PlayerMoved;
    
    public void Move(Hex hex)
    {
        transform.position = grid.HexToWorld(hex);
        PlayerMoved.Raise(hex);
    }

}
