using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ArgEventListener<T> : MonoBehaviour
{
    [Header("Asset References")]
    [SerializeField]
    private AssetReference EventReference;
    
    private ArgEvent<T> @event;
    
    public UnityEvent<T> Response;

    private void OnEnable()
    {
        Addressables.LoadAssetAsync<ArgEvent<T>>(EventReference).Completed += OnEventAssetLoaded;
        
    }

    private void OnEventAssetLoaded(AsyncOperationHandle<ArgEvent<T>> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            @event = obj.Result;
            Debug.Log($"Successfully loaded asset <{@event.name}>");

            @event.RegisterListener(this);
        }
    }
    
    private void OnDisable()
    {
        @event.UnregisterListener(this);
    }

    public void OnEventRaised(T arg)
    {
        Response.Invoke(arg);
    }
}
