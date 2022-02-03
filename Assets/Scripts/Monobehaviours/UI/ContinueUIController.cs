﻿using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class ContinueUIController : MonoBehaviour
{
    public WorldObjectManager WorldObjectManager;
    [SerializeField]
    private GameObject continueButtonPrefab;
    [SerializeField]
    private GameObject canvas;

    public HexVariable playerCurrentHex; 

    private Tilemap tileMap;

    private GameObject buttonHolder;
    
    private void OnEnable()
    {
        buttonHolder = Instantiate(continueButtonPrefab, canvas.transform);
        Button button = buttonHolder.GetComponent<Button>();
        if (tileMap == null)
        {
            tileMap = WorldObjectManager.GetComponent<Tilemap>();
        }
        
        WorldTile currentTile = (WorldTile)tileMap.GetTile(playerCurrentHex.Value);
        WorldTileDelta[] currentDeltas = currentTile.worldTileDeltas;
        
        foreach (WorldTileDelta currentDelta in currentDeltas)
        {
            button.onClick.AddListener(() => currentDelta.ApplyDelta());
        }
    }

    private void OnDisable()
    {
        Destroy(buttonHolder);
    }
    
}
