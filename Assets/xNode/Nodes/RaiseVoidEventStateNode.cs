using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class RaiseVoidEventStateNode : StateNode
{
    [Header("Asset References")]
    [SerializeField]
    private AssetReference voidEventReference;
    
    private VoidEvent voidEvent;
    
    public override void OnEnter()
    {
        base.OnEnter();

        if (IsInitialised)
        {
            Continue();
            return;
        }

        ++assetLoadCount;
        Addressables.LoadAssetAsync<VoidEvent>(voidEventReference).Completed += OnVoidEventAssetLoaded;
    }
    
    private void OnVoidEventAssetLoaded(AsyncOperationHandle<VoidEvent> obj)
    {
        if (obj.Status != AsyncOperationStatus.Succeeded) return;
        
        voidEvent = obj.Result;
        Debug.Log($"Successfully loaded asset <{voidEvent.name}>");

        ContinueOnAllAssetsLoaded();
    }

    protected override void Continue()
    {
        voidEvent.Raise();
        IsComplete = true;
    }
}
