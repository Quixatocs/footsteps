using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class WorldTileLoadStateNode : StateNode
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
        
        ++assetLoadCount;
        Addressables.LoadAssetAsync<WorldObjectManager>(worldObjectManagerReference).Completed += OnWorldObjectManagerAssetLoaded;
        ++assetLoadCount;
        Addressables.LoadAssetAsync<WorldTileSet>(worldTileSetReference).Completed += OnWorldTileSetAssetLoaded;
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
    
    private void ContinueOnAllAssetsLoaded()
    {
        if (--assetLoadCount != 0) return;
        
        worldObjectManager.SetWorldTiles(worldTiles);
        Continue();
    }

    private void Continue()
    {
        IsComplete = true;
    }
}
