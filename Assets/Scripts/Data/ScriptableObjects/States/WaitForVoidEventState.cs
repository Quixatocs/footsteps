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
        
        if (IsInitialised) return;
        
        stateHandleOperation.Completed += OnNextStateAssetLoaded;
        
        ++assetLoadCount;
        Addressables.LoadAssetAsync<VoidEvent>(voidEventReference).Completed += OnVoidEventAssetLoaded;
    }
    
    private void OnNextStateAssetLoaded(AsyncOperationHandle<State> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            --assetLoadCount;
            nextState = obj.Result;
            Debug.Log($"Successfully loaded asset <{nextState.name}>");

            if (assetLoadCount == 0)
            {
                IsInitialised = true;
            }
        }
    }
    
    private void OnVoidEventAssetLoaded(AsyncOperationHandle<VoidEvent> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            --assetLoadCount;
            voidEvent = obj.Result;
            Debug.Log($"Successfully loaded asset <{voidEvent.name}>");
            
            voidEvent.RegisterListener(voidEventListener);

            if (assetLoadCount == 0)
            {
                IsInitialised = true;
            }
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

    private void OnEventTriggered()
    {
        isTriggered = true;
    }
    
    public override State GetNextState()
    {
        return nextState;
    }
}
