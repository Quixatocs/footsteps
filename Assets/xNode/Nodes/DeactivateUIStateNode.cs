using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class DeactivateUIStateNode : StateNode
{
    [Header("Asset References")]
    [SerializeField]
    private AssetReference UIDeactivationEventReference;
    
    private BoolEvent uiDeactivationEvent;

    public override void OnEnter()
    {
        base.OnEnter();

        if (IsInitialised)
        {
            Continue();
            return;
        }
        
        ++assetLoadCount;
        Addressables.LoadAssetAsync<BoolEvent>(UIDeactivationEventReference).Completed += OnUIDeactivationEventAssetLoaded;
    }
    
    private void OnUIDeactivationEventAssetLoaded(AsyncOperationHandle<BoolEvent> obj)
    {
        if (obj.Status != AsyncOperationStatus.Succeeded) return;
        
        uiDeactivationEvent = obj.Result;
        Debug.Log($"Successfully loaded asset <{uiDeactivationEvent.name}>");

        ContinueOnAllAssetsLoaded();
    }
    
    protected override void Continue()
    {
        uiDeactivationEvent.Raise(false);
        IsComplete = true;
    }
}
