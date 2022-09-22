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
    private Tilemap tilemap;
    
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
        
        if (tilemap == null)
        {
            tilemap = worldObjectManager.GetComponent<Tilemap>();
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
        
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
        
        foreach (WorldTile worldTile in allTiles)
        {
            if (worldTile == null) continue;

            foreach (Interactable interactable in worldTile.runtimeInteractables)
            {
                interactable.DestroyIcon();
            }
        }
        
        tilemap.ClearAllTiles();
        IsComplete = true;
    }
}
