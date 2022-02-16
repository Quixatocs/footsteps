using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class ContinueUIController : MonoBehaviour
{
    [Header("Asset References")]
    [SerializeField]
    private AssetReference worldObjectManagerReference;
    [SerializeField]
    private AssetReference playerCurrentHexReference;
    
    [Header("Scene References")]
    [SerializeField]
    private GameObject continueButtonPrefab;
    [SerializeField]
    private GameObject canvas;

    private WorldObjectManager worldObjectManager;
    private HexVariable playerCurrentHex; 
    private Tilemap tileMap;
    private GameObject buttonHolder;
    private int count;
    
    private void OnEnable()
    {
        LoadAssets();
    }
    
    private void LoadAssets()
    {
        ++count;
        Addressables.LoadAssetAsync<WorldObjectManager>(worldObjectManagerReference).Completed += OnWorldObjectManagerAssetLoaded;
        ++count;
        Addressables.LoadAssetAsync<HexVariable>(playerCurrentHexReference).Completed += OnPlayerCurrentHexAssetLoaded;
    }
    
    private void OnWorldObjectManagerAssetLoaded(AsyncOperationHandle<WorldObjectManager> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            --count;
            worldObjectManager = obj.Result;
            Debug.Log($"Successfully loaded asset <{worldObjectManager.name}>");

            if (count <= 0)
            {
                Initialise();
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

            if (count <= 0)
            {
                Initialise();
            }
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
            button.onClick.AddListener(() => cost.ApplyDelta());
        }
    }

    private void OnDisable()
    {
        Destroy(buttonHolder);
    }
    
}
