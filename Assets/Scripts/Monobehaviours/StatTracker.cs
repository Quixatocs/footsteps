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
}
