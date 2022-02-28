using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/States/DeactivateUIState", order = 1)]
[Serializable]
public class DeactivateUIState : State
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
        
        if (stateHandleOperation.HasValue) stateHandleOperation.Value.Completed += OnNextStateAssetLoaded;
        
        ++assetLoadCount;
        Addressables.LoadAssetAsync<BoolEvent>(UIDeactivationEventReference).Completed += OnUIDeactivationEventAssetLoaded;
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
    
    private void OnUIDeactivationEventAssetLoaded(AsyncOperationHandle<BoolEvent> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            uiDeactivationEvent = obj.Result;
            Debug.Log($"Successfully loaded asset <{uiDeactivationEvent.name}>");

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
        uiDeactivationEvent.Raise(false);
        IsComplete = true;
    }

    public override State GetNextState()
    {
        return nextState;
    }
    
}