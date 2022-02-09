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
    
    [Header("Variables")]
    public IntVariable Food;
    public IntVariable Water;
    
    [Header("Events")]
    public BoolEvent DeathUIActivationEvent;

    private void Awake()
    {
        Addressables.LoadAssetAsync<IntVariable>(FoodReference).Completed += OnFoodAssetLoaded;
        Addressables.LoadAssetAsync<IntVariable>(WaterReference).Completed += OnWaterAssetLoaded;
    }

    private void OnFoodAssetLoaded(AsyncOperationHandle<IntVariable> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            Food = obj.Result;
            Debug.Log($"Successfully loaded asset <{Food.name}>");
        }
    }
    
    private void OnWaterAssetLoaded(AsyncOperationHandle<IntVariable> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            Water = obj.Result;
            Debug.Log($"Successfully loaded asset <{Water.name}>");
        }
    }
    
    public void CheckFoodAndWater()
    {
        if (Food.Value < 0 || Water.Value < 0)
        {
            DeathUIActivationEvent.Raise(true);
        }
    }
}
