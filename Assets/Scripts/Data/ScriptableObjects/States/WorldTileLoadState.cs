using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/States/WorldTileLoadState", order = 1)]
[Serializable]
public class WorldTileLoadState : State
{
    [Header("Asset References")]
    [SerializeField]
    private AssetReference worldObjectManagerReference;
    [SerializeField]
    private AssetReference worldTileSetReference;
    
    private WorldObjectManager worldObjectManager;
    private List<WorldTile> worldTiles;
    
    public override void OnEnter()
    {
        base.OnEnter();

        if (IsInitialised)
        {
            Continue();
            return;
        }

        if (stateHandleOperation.HasValue) stateHandleOperation.Value.Completed += OnNextStateAssetLoaded;
        
        ++assetLoadCount;
        Addressables.LoadAssetAsync<WorldObjectManager>(worldObjectManagerReference).Completed += OnWorldObjectManagerAssetLoaded;
        ++assetLoadCount;
        Addressables.LoadAssetAsync<WorldTileSet>(worldTileSetReference).Completed += OnWorldTileSetAssetLoaded;
    }
    
    private void OnNextStateAssetLoaded(AsyncOperationHandle<State> obj)
    {
        if (obj.Status != AsyncOperationStatus.Succeeded) return;
        
        nextState = obj.Result;
        Debug.Log($"Successfully loaded asset <{nextState.name}>");

        ContinueOnAllAssetsLoaded();
    }
    
    private void OnWorldObjectManagerAssetLoaded(AsyncOperationHandle<WorldObjectManager> obj)
    {
        if (obj.Status != AsyncOperationStatus.Succeeded) return;
        
        worldObjectManager = obj.Result;
        Debug.Log($"Successfully loaded asset <{worldObjectManager.name}>");
            
        ContinueOnAllAssetsLoaded();
    }
    
    private void OnWorldTileSetAssetLoaded(AsyncOperationHandle<WorldTileSet> obj)
    {
        if (obj.Status != AsyncOperationStatus.Succeeded) return;
        
        WorldTileSet worldTileSet = obj.Result;
        Debug.Log($"Successfully loaded asset <{worldTileSet.name}>");
            
        worldTiles = new List<WorldTile>();
        int counter = worldTileSet.WorldTiles.Length;
        foreach (AssetReference worldTile in worldTileSet.WorldTiles)
        {
            Addressables.LoadAssetAsync<WorldTile>(worldTile).Completed += operation =>
            {
                if (operation.Status != AsyncOperationStatus.Succeeded) return;
                
                worldTiles.Add(operation.Result);
                Debug.Log($"Successfully loaded and instantiated WorldTile <{operation.Result.tileName}>.");

                if (--counter == 0)
                {
                    ContinueOnAllAssetsLoaded();
                }
            };
        }
    }
    
    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
    }

    protected override void ContinueOnAllAssetsLoaded()
    {
        if (--assetLoadCount != 0) return;
        
        worldObjectManager.SetWorldTiles(worldTiles);
        IsInitialised = true;
        Continue();
    }

    protected override void Continue()
    {
        IsComplete = true;
    }

    public override State GetNextState()
    {
        return nextState;
    }
}
