using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Grid grid;
    public HexEvent PlayerMoved;
    
    public void Move(Hex hex)
    {
        transform.position = grid.CellToWorld(hex);
        PlayerMoved.Raise(hex);
    }

}
