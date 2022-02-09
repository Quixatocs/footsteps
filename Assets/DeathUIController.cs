using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class DeathUIController : MonoBehaviour
{
    [Header("Asset References")]
    [SerializeField]
    private AssetReference FoodReference;
    [SerializeField]
    private AssetReference WaterReference;
    [SerializeField]
    private AssetReference DaysReference;
    
    //TODO Remove nonSerialized
    [NonSerialized] private IntVariable Food;
    [NonSerialized] private  IntVariable Water;
    [NonSerialized] private  IntVariable Days;

    private void Awake()
    {
        Addressables.LoadAssetAsync<IntVariable>(FoodReference).Completed += OnFoodAssetLoaded;
        Addressables.LoadAssetAsync<IntVariable>(WaterReference).Completed += OnWaterAssetLoaded;
        Addressables.LoadAssetAsync<IntVariable>(DaysReference).Completed += OnDaysAssetLoaded;
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
    
    private void OnDaysAssetLoaded(AsyncOperationHandle<IntVariable> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            Days = obj.Result;
            Debug.Log($"Successfully loaded asset <{Days.name}>");
        }
    }
    
    public void ResetScene()
    {
        SceneManager.LoadScene(0);

        Food.Value = Food.DefaultValue;
        Water.Value = Water.DefaultValue;
        Days.Value = Days.DefaultValue;
    }
}
