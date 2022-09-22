using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Tilemaps;

public class ResetMapStateNode : StateNode
{
    
    [Header("Asset References")]
    [SerializeField]
    private AssetReference worldObjectManagerReference;
    [SerializeField]
    private AssetReference lastInRangeWorldTilesReference;
    
    private WorldObjectManager worldObjectManager;
    private WorldTileList lastInRangeWorldTiles;
    private Tilemap tileMap;
    
    public override void OnEnter()
    {
        base.OnEnter();

        if (IsInitialised)
        {
            Continue();
            return;
        }

        ++assetLoadCount;
        Addressables.LoadAssetAsync<WorldObjectManager>(worldObjectManagerReference).Completed += OnWorldObjectManagerAssetLoaded;
        ++assetLoadCount;
        Addressables.LoadAssetAsync<WorldTileList>(lastInRangeWorldTilesReference).Completed += OnLastInRangeWorldTilesAssetLoaded;

    }
    
    private void OnWorldObjectManagerAssetLoaded(AsyncOperationHandle<WorldObjectManager> obj)
    {
        if (obj.Status != AsyncOperationStatus.Succeeded) return;
        
        worldObjectManager = obj.Result;
        Debug.Log($"Successfully loaded asset <{worldObjectManager.name}>");
        
        if (tileMap == null)
        {
            tileMap = worldObjectManager.GetComponent<Tilemap>();
        }
            
        ContinueOnAllAssetsLoaded();
    }
    
    private void OnLastInRangeWorldTilesAssetLoaded(AsyncOperationHandle<WorldTileList> obj)
    {
        if (obj.Status != AsyncOperationStatus.Succeeded) return;
        
        lastInRangeWorldTiles = obj.Result;
        Debug.Log($"Successfully loaded asset <{lastInRangeWorldTiles.name}>");
            
        ContinueOnAllAssetsLoaded();
    }

    protected override void Continue()
    {
        lastInRangeWorldTiles.Clear();
        tileMap.ClearAllTiles();
        IsComplete = true;
    }
}
