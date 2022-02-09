using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class TriggerUIState : MonoBehaviour
{
    [Header("Asset References")]
    [SerializeField]
    private AssetReference enableUIStateReference;

    private EnableUIState enableUIState;
    
    private void Awake()
    {
        Addressables.LoadAssetAsync<EnableUIState>(enableUIStateReference).Completed += OnEnableUIStateAssetLoaded;
    }

    private void OnEnableUIStateAssetLoaded(AsyncOperationHandle<EnableUIState> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            enableUIState = obj.Result;
            Debug.Log($"Successfully loaded asset <{enableUIState.name}>");
        }
    }

    public void ReturnFromUI()
    {
        enableUIState.ReturnFromUI();
    }
}
