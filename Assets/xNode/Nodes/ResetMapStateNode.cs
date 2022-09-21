using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Tilemaps;

public class ResetMapStateNode : StateNode
{
    
    [Header("Asset References")]
    [SerializeField]
    private AssetReference worldObjectManagerReference;
    
    private WorldObjectManager worldObjectManager;
    private Tilemap tileMap;
    private Grid grid;
    
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
        
        if (grid == null)
        {
            grid = worldObjectManager.GetComponent<Grid>();
        }
            
        ContinueOnAllAssetsLoaded();
    }

    protected override void Continue()
    {
        tileMap.ClearAllTiles();
        tileMap.RefreshAllTiles();
        IsComplete = true;
    }
}
