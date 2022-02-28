using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/States/VariableChangedState", order = 1)]
[Serializable]
public class VariableChangedState : State
{
    [Header("Asset References")]
    [SerializeField]
    private AssetReference intVariableReference;
    [SerializeField]
    private AssetReference voidEventReference;
    
    private IntVariable intVariable;
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
        Addressables.LoadAssetAsync<IntVariable>(intVariableReference).Completed += OnIntVariableAssetLoaded;
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
    
    private void OnIntVariableAssetLoaded(AsyncOperationHandle<IntVariable> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            intVariable = obj.Result;
            Debug.Log($"Successfully loaded asset <{intVariable.name}>");

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
