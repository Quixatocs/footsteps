﻿using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Map map;

    public static Action<Vector3> OnPlayerMoved;

    private void OnEnable()
    {
        map.OnMapAssetsLoadingComplete += OnMapInitialised;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // get the collision point of the ray with the z = 0 plane
            Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
            // get current grid location
            Vector3Int cell = map.grid.WorldToCell(worldPoint);

            transform.position = map.grid.CellToWorld(cell);
            
            OnPlayerMoved?.Invoke(transform.position);
            
            CubeHexCoords position = CoordUtils.UnityHexToCubeHex(cell.ToUnityHexCoordinates());

            UpdateMap(position);
        }
    }

    private void OnMapInitialised()
    {
        map.OnMapAssetsLoadingComplete -= OnMapInitialised;
        UpdateMap(new CubeHexCoords(0, 0, 0));
    }

    private void Move()
    {
        
    }

    private void UpdateMap(CubeHexCoords position)
    {
        map.GenerateTilesAroundPlayer(position);
    }
}
