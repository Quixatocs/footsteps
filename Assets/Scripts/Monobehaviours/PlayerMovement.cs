using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PlayerMovement : MonoBehaviour
{
    [Header("Asset References")]
    [SerializeField]
    private AssetReference worldObjectManagerReference;
    [SerializeField]
    private AssetReference playerCurrentHexReference;
    [SerializeField]
    private AssetReference playerMovedEventReference;
    
    private WorldObjectManager worldObjectManager;
    private HexVariable playerCurrentHex;
    private HexEvent PlayerMovedHexEvent;
    private Grid grid;
    
    private int count;
    
    private void Start()
    {
        LoadAssets();
    }

    private void LoadAssets()
    {
        ++count;
        Addressables.LoadAssetAsync<WorldObjectManager>(worldObjectManagerReference).Completed += OnWorldObjectManagerAssetLoaded;
        ++count;
        Addressables.LoadAssetAsync<HexVariable>(playerCurrentHexReference).Completed += OnPlayerCurrentHexAssetLoaded;
        ++count;
        Addressables.LoadAssetAsync<HexEvent>(playerMovedEventReference).Completed += OnPlayerMovedEventAssetLoaded;
    }

    private void OnWorldObjectManagerAssetLoaded(AsyncOperationHandle<WorldObjectManager> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            --count;
            worldObjectManager = obj.Result;
            Debug.Log($"Successfully loaded asset <{worldObjectManager.name}>");

            if (grid == null)
            {
                grid = worldObjectManager.GetComponent<Grid>();
            }
        }
    }
    
    private void OnPlayerCurrentHexAssetLoaded(AsyncOperationHandle<HexVariable> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            --count;
            playerCurrentHex = obj.Result;
            Debug.Log($"Successfully loaded asset <{playerCurrentHex.name}>");
        }
    }
    
    private void OnPlayerMovedEventAssetLoaded(AsyncOperationHandle<HexEvent> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            --count;
            PlayerMovedHexEvent = obj.Result;
            Debug.Log($"Successfully loaded asset <{PlayerMovedHexEvent.name}>");
        }
    }
    
    public void Move(Hex hex)
    {
        playerCurrentHex.Value = hex;
        transform.position = grid.HexToWorld(hex);
        PlayerMovedHexEvent.Raise(hex);
    }

}
