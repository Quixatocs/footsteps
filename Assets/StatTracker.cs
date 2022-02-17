using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class StatTracker : MonoBehaviour
{
    [Header("Asset References")]
    [SerializeField]
    private AssetReference FoodReference;
    [SerializeField]
    private AssetReference WaterReference;
    [SerializeField]
    private AssetReference DeathUIActivationEventReference;
    
    private IntVariable food;
    private IntVariable water;
    private BoolEvent deathUIActivationEvent;

    private void Awake()
    {
        Addressables.LoadAssetAsync<IntVariable>(FoodReference).Completed += OnFoodAssetLoaded;
        Addressables.LoadAssetAsync<IntVariable>(WaterReference).Completed += OnWaterAssetLoaded;
        Addressables.LoadAssetAsync<BoolEvent>(DeathUIActivationEventReference).Completed += OnDeathUIActivationEventAssetLoaded;
    }

    private void OnFoodAssetLoaded(AsyncOperationHandle<IntVariable> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            food = obj.Result;
            Debug.Log($"Successfully loaded asset <{food.name}>");
        }
    }
    
    private void OnWaterAssetLoaded(AsyncOperationHandle<IntVariable> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            water = obj.Result;
            Debug.Log($"Successfully loaded asset <{water.name}>");
        }
    }
    
    private void OnDeathUIActivationEventAssetLoaded(AsyncOperationHandle<BoolEvent> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            deathUIActivationEvent = obj.Result;
            Debug.Log($"Successfully loaded asset <{deathUIActivationEvent.name}>");
        }
    }
    
    public void CheckFoodAndWater()
    {
        if (food.Value < 0 || water.Value < 0)
        {
            deathUIActivationEvent.Raise(true);
        }
    }
}
