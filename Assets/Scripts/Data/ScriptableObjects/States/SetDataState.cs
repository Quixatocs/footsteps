using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SetDataState : State
{
    [Header("Asset References")]
    [SerializeField]
    private AssetReference intVariableReference;

    public int ValueToSet;

    private IntVariable intVariable;
    
    public override void OnEnter()
    {
        base.OnEnter();
        
        if (IsInitialised) return;
        
        stateHandleOperation.Completed += OnNextStateAssetLoaded;
        
        ++assetLoadCount;
        Addressables.LoadAssetAsync<IntVariable>(intVariableReference).Completed += OnIntVariableAssetLoaded;
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
    
    private void OnIntVariableAssetLoaded(AsyncOperationHandle<IntVariable> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            intVariable = obj.Result;
            Debug.Log($"Successfully loaded asset <{intVariable.name}>");

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
            SetData();
        }
    }
    
    private void SetData()
    {
        intVariable.Value = ValueToSet;
        IsComplete = true;
    }

    public override State GetNextState()
    {
        return nextState;
    }
}