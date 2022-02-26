using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class CostsUIController : MonoBehaviour
{
    [Header("Asset References")]
    [SerializeField]
    private AssetReference worldObjectManagerReference;
    [SerializeField]
    private AssetReference playerCurrentHexReference;
    [SerializeField]
    private AssetReference costsClickedEventReference;
    
    [Header("Scene References")]
    [SerializeField]
    private GameObject continueButtonPrefab;
    [SerializeField]
    private GameObject canvas;

    private WorldObjectManager worldObjectManager;
    private HexVariable playerCurrentHex;
    private VoidEvent costsClickedEvent;
    private Tilemap tileMap;
    private GameObject buttonHolder;
    private int assetLoadCount;
    
    private void OnEnable()
    {
        LoadAssets();
    }
    
    private void LoadAssets()
    {
        ++assetLoadCount;
        Addressables.LoadAssetAsync<WorldObjectManager>(worldObjectManagerReference).Completed += OnWorldObjectManagerAssetLoaded;
        ++assetLoadCount;
        Addressables.LoadAssetAsync<HexVariable>(playerCurrentHexReference).Completed += OnPlayerCurrentHexAssetLoaded;
        ++assetLoadCount;
        Addressables.LoadAssetAsync<VoidEvent>(costsClickedEventReference).Completed += OnCostsClickedEventAssetLoaded;
    }
    
    private void OnWorldObjectManagerAssetLoaded(AsyncOperationHandle<WorldObjectManager> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            worldObjectManager = obj.Result;
            Debug.Log($"Successfully loaded asset <{worldObjectManager.name}>");

            ContinueOnAllAssetsLoaded();
        }
    }
    
    private void OnPlayerCurrentHexAssetLoaded(AsyncOperationHandle<HexVariable> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            playerCurrentHex = obj.Result;
            Debug.Log($"Successfully loaded asset <{playerCurrentHex.name}>");

            ContinueOnAllAssetsLoaded();
        }
    }
    
    private void OnCostsClickedEventAssetLoaded(AsyncOperationHandle<VoidEvent> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            costsClickedEvent = obj.Result;
            Debug.Log($"Successfully loaded asset <{costsClickedEvent.name}>");

            ContinueOnAllAssetsLoaded();
        }
    }
    
    private void ContinueOnAllAssetsLoaded()
    {
        if (--assetLoadCount == 0)
        {
            Initialise();
        }
    }

    private void Initialise()
    {
        buttonHolder = Instantiate(continueButtonPrefab, canvas.transform);
        Button button = buttonHolder.GetComponent<Button>();
        if (tileMap == null)
        {
            tileMap = worldObjectManager.GetComponent<Tilemap>();
        }
        
        WorldTile currentTile = (WorldTile)tileMap.GetTile(playerCurrentHex.Value);
        IntDelta[] currentTileCosts = currentTile.costs;
        
        foreach (IntDelta cost in currentTileCosts)
        {
            button.onClick.AddListener(() =>
            {
                cost.ApplyDelta();
                costsClickedEvent.Raise();
            });
        }
    }

    private void OnDisable()
    {
        Destroy(buttonHolder);
    }
    
}
