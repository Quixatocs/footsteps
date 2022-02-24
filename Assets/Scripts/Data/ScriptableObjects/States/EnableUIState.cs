using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class EnableUIState : State
{
    [Header("Asset References")]
    [SerializeField]
    private AssetReference UIActivationEventReference;
    
    private BoolEvent uiActivationEvent;

    public override void OnEnter()
    {
        IsComplete = false;
        Addressables.LoadAssetAsync<BoolEvent>(UIActivationEventReference).Completed += OnUIActivationEventAssetLoaded;
    }
    
    private void OnUIActivationEventAssetLoaded(AsyncOperationHandle<BoolEvent> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            uiActivationEvent = obj.Result;
            Debug.Log($"Successfully loaded asset <{uiActivationEvent.name}>");

            IsInitialised = true;
            uiActivationEvent.Raise(true);
        }
    }
    
    public override void OnExit()
    {
        uiActivationEvent.Raise(false);
    }

    public override void OnUpdate()
    {
    }

    public void ReturnFromUI()
    {
        IsComplete = true;
    }
    
}
