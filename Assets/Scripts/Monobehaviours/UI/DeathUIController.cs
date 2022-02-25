using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class DeathUIController : MonoBehaviour
{
    [Header("Asset References")]
    [SerializeField]
    private AssetReference retryClickedEventReference;
    
    [Header("Scene References")]
    [SerializeField]
    private Button retryButton;
    
    private VoidEvent retryClickedEvent;

    private void Awake()
    {
        Addressables.LoadAssetAsync<VoidEvent>(retryClickedEventReference).Completed += OnRetryClickedEventAssetLoaded;
    }
    
    private void OnRetryClickedEventAssetLoaded(AsyncOperationHandle<VoidEvent> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            retryClickedEvent = obj.Result;
            Debug.Log($"Successfully loaded asset <{retryClickedEvent.name}>");
            
            retryButton.onClick.AddListener(retryClickedEvent.Raise);
        }
    }

}
