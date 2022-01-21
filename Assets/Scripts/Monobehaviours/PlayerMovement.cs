using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public static Action<Vector3> OnPlayerMoved;
    public Vector3IntEvent PlayerMoved;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // get the collision point of the ray with the z = 0 plane
            Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
            // get current grid location
            //Vector3Int cell = map.grid.WorldToCell(worldPoint);

            //transform.position = map.grid.CellToWorld(cell);
            
            //PlayerMoved.Raise(cell);
            
            //CubeHexCoords position = CoordUtils.UnityHexToCubeHex(cell.ToUnityHexCoordinates());

            //UpdateMap(position);
        }
    }

    public void OnMapClickedEventReceived()
    {
        
    }

    public void MapInitialised()
    {
        UpdateMap(new CubeHexCoords(0, 0, 0));
    }

    private void Move()
    {
        
    }

    private void UpdateMap(CubeHexCoords position)
    {
        //map.GenerateTilesAroundPlayer(position);
    }
}
