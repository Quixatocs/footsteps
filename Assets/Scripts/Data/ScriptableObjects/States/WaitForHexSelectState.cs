using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

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
        base.OnEnter();
        
        if (IsInitialised) return;
        
        stateHandleOperation.Completed += OnNextStateAssetLoaded;
        
        ++assetLoadCount;
        Addressables.LoadAssetAsync<WorldObjectManager>(worldObjectManagerReference).Completed += OnWorldObjectManagerAssetLoaded;
        ++assetLoadCount;
        Addressables.LoadAssetAsync<HexEvent>(playerCurrentHexEventReference).Completed += OnPlayerCurrentHexAssetLoaded;
    }
    
    private void OnNextStateAssetLoaded(AsyncOperationHandle<State> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            nextState = obj.Result;
            Debug.Log($"Successfully loaded asset <{nextState.name}>");

            ContinueOnAllAssetsLoaded();
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

            ContinueOnAllAssetsLoaded();
        }
    }
    
    private void OnPlayerCurrentHexAssetLoaded(AsyncOperationHandle<HexEvent> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            hexClickedEvent = obj.Result;
            Debug.Log($"Successfully loaded asset <{hexClickedEvent.name}>");

            ContinueOnAllAssetsLoaded();
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
            
            if (grid == null && worldObjectManager != null)
            {
                grid = worldObjectManager.GetComponent<Grid>();
            }
            
            Hex clickedHex = grid.WorldToHex(worldPoint); 
            hexClickedEvent.Raise(clickedHex);
            IsComplete = true;
        }
    }

    protected override void ContinueOnAllAssetsLoaded()
    {
        if (--assetLoadCount == 0)
        {
            IsInitialised = true;
        }
    }

    public override State GetNextState()
    {
        return nextState;
    }
}
