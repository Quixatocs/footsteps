using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChoicesUIController : MonoBehaviour
{
    public WorldObjectManager WorldObjectManager;
    [SerializeField]
    private GameObject choicesButtonPrefab;

    private Tilemap tileMap;

    private GameObject buttonHolder;
    
    private void OnEnable()
    {
        if (tileMap == null)
        {
            tileMap = WorldObjectManager.GetComponent<Tilemap>();
        }
        //TODO get the tiledelta from the tilemap and then make a lambda and plonk it on the buttonHolder.onClick
        WorldTileDelta currentDelta = 
        buttonHolder
    }
}
