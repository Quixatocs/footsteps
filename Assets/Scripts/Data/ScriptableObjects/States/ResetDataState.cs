using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/States/ResetDataState", order = 1)]
[Serializable]
public class ResetDataState : State
{
    [Header("Asset References")]
    [SerializeField]
    private AssetReference intVariableReference;

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
        intVariable.Value = intVariable.DefaultValue;
        IsComplete = true;
    }

    public override State GetNextState()
    {
        return nextState;
    }
}