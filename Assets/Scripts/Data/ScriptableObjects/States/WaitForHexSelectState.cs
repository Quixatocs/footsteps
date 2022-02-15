using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/States/WaitForHexSelectState", order = 1)]
[Serializable]
public class WaitForHexSelectState : State
{
    [Header("Asset References")]
    [SerializeField]
    private AssetReference worldObjectManagerReference;
    [SerializeField]
    private AssetReference playerCurrentHexEventReference;
    
    private WorldObjectManager worldObjectManager;
    private HexEvent hexClickedEvent;
    private Grid grid;

    public override void OnEnter()
    {
        IsComplete = false;
        Addressables.LoadAssetAsync<WorldObjectManager>(worldObjectManagerReference).Completed += OnWorldObjectManagerAssetLoaded;
        Addressables.LoadAssetAsync<HexEvent>(playerCurrentHexEventReference).Completed += OnPlayerCurrentHexAssetLoaded;
        Addressables.LoadAssetAsync<State>(NextStateReference).Completed += OnNextStateAssetLoaded;
    }

    private void OnNextStateAssetLoaded(AsyncOperationHandle<State> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            nextState = obj.Result;
            Debug.Log($"Successfully loaded asset <{nextState.name}>");

            if (worldObjectManager != null && hexClickedEvent != null)
            {
                IsInitialised = true;
            }
        }
    }
    
    private void OnWorldObjectManagerAssetLoaded(AsyncOperationHandle<WorldObjectManager> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            worldObjectManager = obj.Result;
            Debug.Log($"Successfully loaded asset <{worldObjectManager.name}>");

            if (grid == null)
            {
                grid = worldObjectManager.GetComponent<Grid>();
            }

            if (hexClickedEvent != null && nextState != null)
            {
                IsInitialised = true;
            }
        }
    }
    
    private void OnPlayerCurrentHexAssetLoaded(AsyncOperationHandle<HexEvent> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            hexClickedEvent = obj.Result;
            Debug.Log($"Successfully loaded asset <{hexClickedEvent.name}>");

            if (worldObjectManager != null && nextState != null)
            {
                IsInitialised = true;
            }
        }
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        if (!IsInitialised) return;

        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
            Hex clickedHex = grid.WorldToHex(worldPoint); 
            hexClickedEvent.Raise(clickedHex);
            IsComplete = true;
        }
    }
}
