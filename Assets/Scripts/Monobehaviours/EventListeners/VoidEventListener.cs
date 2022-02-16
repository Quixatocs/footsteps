using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

public class VoidEventListener : MonoBehaviour
{
    [Header("Asset References")]
    [SerializeField]
    private AssetReference EventReference;
    
    private VoidEvent voidEvent;
    public UnityEvent Response;

    private void OnEnable()
    {
        Addressables.LoadAssetAsync<VoidEvent>(EventReference).Completed += OnEventAssetLoaded;
    }
    
    private void OnEventAssetLoaded(AsyncOperationHandle<VoidEvent> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            voidEvent = obj.Result;
            Debug.Log($"Successfully loaded asset <{voidEvent.name}>");

            voidEvent.RegisterListener(this);
        }
    }

    private void OnDisable()
    {
        voidEvent.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        Response.Invoke();
    }
}
