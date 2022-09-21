using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ActivateUIStateNode : StateNode
{
    [Header("Asset References")]
    [SerializeField]
    private AssetReference uiActivationEventReference;
    
    private BoolEvent uiActivationEvent;

    public override void OnEnter()
    {
        base.OnEnter();

        if (IsInitialised)
        {
            Continue();
            return;
        }
        
        ++assetLoadCount;
        Addressables.LoadAssetAsync<BoolEvent>(uiActivationEventReference).Completed += OnUIActivationEventAssetLoaded;
    }
    
    private void OnUIActivationEventAssetLoaded(AsyncOperationHandle<BoolEvent> obj)
    {
        if (obj.Status != AsyncOperationStatus.Succeeded) return;
        
        uiActivationEvent = obj.Result;
        Debug.Log($"Successfully loaded asset <{uiActivationEvent.name}>");

        ContinueOnAllAssetsLoaded();
    }

    protected override void Continue()
    {
        uiActivationEvent.Raise(true);
        IsComplete = true; 
    }
}
