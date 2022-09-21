using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class WaitForVoidEventStateNode : StateNode
{
    [Header("Asset References")]
    [SerializeField]
    private AssetReference voidEventReference;
    
    private VoidEvent voidEvent;
    private bool isTriggered;
    private VoidEventListener voidEventListener;

    public override void OnEnter()
    {
        base.OnEnter();
        
        isTriggered = false;
        
        voidEventListener ??= new VoidEventListener(OnEventTriggered);

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

    public override void OnExit()
    {
        voidEvent.UnregisterListener(voidEventListener);
    }

    public override void OnUpdate()
    {
        if (!IsInitialised) return;
        if (!isTriggered) return;
        IsComplete = true;
    }

    protected override void Continue()
    {
        voidEvent.RegisterListener(voidEventListener);
    }

    private void OnEventTriggered()
    {
        isTriggered = true;
    }
}
