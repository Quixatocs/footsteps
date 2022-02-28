using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/States/WaitForVoidEventState", order = 1)]
[Serializable]
public class WaitForVoidEventState : State
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
        
        if (voidEventListener == null)
        {
            voidEventListener = new VoidEventListener(OnEventTriggered);
        }

        if (IsInitialised)
        {
            Continue();
            return;
        }
        
        if (stateHandleOperation.HasValue) stateHandleOperation.Value.Completed += OnNextStateAssetLoaded;
        
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
        voidEvent.UnregisterListener(voidEventListener);
    }

    public override void OnUpdate()
    {
        if (!IsInitialised) return;
        if (!isTriggered) return;
        IsComplete = true;
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
        voidEvent.RegisterListener(voidEventListener);
    }

    private void OnEventTriggered()
    {
        isTriggered = true;
    }
    
    public override State GetNextState()
    {
        return nextState;
    }
}
