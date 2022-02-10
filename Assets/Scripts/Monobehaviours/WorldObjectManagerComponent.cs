using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class WorldObjectManagerComponent : MonoBehaviour
{
    [Header("Asset References")]
    [SerializeField]
    private AssetReference worldObjectManagerReference;

    private WorldObjectManager worldObjectManager;
    
    public WorldObjectManager WorldObjectManager => worldObjectManager;

    private void Start()
    {
        Addressables.LoadAssetAsync<WorldObjectManager>(worldObjectManagerReference).Completed += OnWorldTileSetLoadDone;
    }

    private void OnWorldTileSetLoadDone(AsyncOperationHandle<WorldObjectManager> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            worldObjectManager = obj.Result;
            Debug.Log($"Successfully loaded asset <{worldObjectManager.name}>");
            WorldObjectManager.SetWorldObjectManager(gameObject);
        }
    }
    
}
