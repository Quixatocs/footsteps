using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChoicesUIController : MonoBehaviour
{
    public WorldObjectManager WorldObjectManager;
    [SerializeField]
    private GameObject choicesButtonPrefab;
    [SerializeField]
    private GameObject canvas;

    public HexVariable playerCurrentHex; 

    private Tilemap tileMap;

    private GameObject buttonHolder;
    
    private void OnEnable()
    {
        //TODO add choices based on Tile and also other things at the tile location and build buttons
    }

    private void OnDisable()
    {
        //TODO destroy all buttons and listeners etc
    }
}
