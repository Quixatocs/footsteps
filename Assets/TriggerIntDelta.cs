using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class TriggerIntDelta : MonoBehaviour
{
    [Header("Asset References")]
    [SerializeField]
    private AssetReference intDeltaReference;

    private IntDelta intDelta;
    
    private void Awake()
    {
        Addressables.LoadAssetAsync<IntDelta>(intDeltaReference).Completed += OnIntDeltaAssetLoaded;
    }

    private void OnIntDeltaAssetLoaded(AsyncOperationHandle<IntDelta> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            intDelta = obj.Result;
            Debug.Log($"Successfully loaded asset <{intDelta.name}>");
        }
    }

    public void ApplyDelta()
    {
        intDelta.ApplyDelta();
    }
}
