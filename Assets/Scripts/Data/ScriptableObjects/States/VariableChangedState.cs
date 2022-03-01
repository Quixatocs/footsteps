using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;

public class VariableChangedState<T> : State
{
    [FormerlySerializedAs("intVariableReference")]
    [Header("Asset References")]
    [SerializeField]
    private AssetReference variableReference;
    [SerializeField]
    private AssetReference voidEventReference;
    
    private T variable;
    private VoidEvent voidEvent;
    
    public override void OnEnter()
    {
        base.OnEnter();

        if (IsInitialised)
        {
            Continue();
            return;
        }
        
        if (stateHandleOperation.HasValue) stateHandleOperation.Value.Completed += OnNextStateAssetLoaded;
        
        ++assetLoadCount;
        Addressables.LoadAssetAsync<T>(variableReference).Completed += OnVariableAssetLoaded;
        ++assetLoadCount;
        Addressables.LoadAssetAsync<VoidEvent>(voidEventReference).Completed += OnVoidEventAssetLoaded;
    }
    
    private void OnNextStateAssetLoaded(AsyncOperationHandle<State> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            nextState = obj.Result;
            Debug.Log($"Successfully loaded asset <{nextState.name}>");

            ContinueOnAllAssetsLoaded();
        }
    }
    
    private void OnVariableAssetLoaded(AsyncOperationHandle<T> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            variable = obj.Result;
            Debug.Log($"Successfully loaded asset <{variable}> TEST TEST TEST");

            ContinueOnAllAssetsLoaded();
        }
    }
    
    private void OnVoidEventAssetLoaded(AsyncOperationHandle<VoidEvent> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            voidEvent = obj.Result;
            Debug.Log($"Successfully loaded asset <{voidEvent.name}>");

            ContinueOnAllAssetsLoaded();
        }
    }
    
    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
    }

    protected override void ContinueOnAllAssetsLoaded()
    {
        if (--assetLoadCount == 0)
        {
            IsInitialised = true;
            Continue();
        }
    }

    protected override void Continue()
    {
        voidEvent.Raise();
        IsComplete = true;
    }

    public override State GetNextState()
    {
        return nextState;
    }
}
