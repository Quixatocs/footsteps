using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;


public class ResetDataState<T> : State
{
    [FormerlySerializedAs("intVariableReference")]
    [Header("Asset References")]
    [SerializeField]
    private AssetReference variableReference;

    protected T variable;
    
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
            Debug.Log($"Successfully loaded asset <{variable}>");

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
    }

    public override State GetNextState()
    {
        return nextState;
    }
}