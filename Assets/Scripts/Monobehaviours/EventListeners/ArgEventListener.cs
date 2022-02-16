using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ArgEventListener<T> : MonoBehaviour
{
    [Header("Asset References")]
    [SerializeField]
    private AssetReference EventReference;
    
    private ArgEvent<T> argEvent;
    
    public UnityEvent<T> Response;

    private void OnEnable()
    {
        Addressables.LoadAssetAsync<ArgEvent<T>>(EventReference).Completed += OnEventAssetLoaded;
    }

    private void OnEventAssetLoaded(AsyncOperationHandle<ArgEvent<T>> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            argEvent = obj.Result;
            Debug.Log($"Successfully loaded asset <{argEvent.name}>");

            argEvent.RegisterListener(this);
        }
    }
    
    private void OnDisable()
    {
        argEvent.UnregisterListener(this);
    }

    public void OnEventRaised(T arg)
    {
        Response.Invoke(arg);
    }
}
