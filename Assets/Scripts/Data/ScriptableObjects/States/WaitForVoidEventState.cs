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
        IsComplete = false;
        isTriggered = false;

        if (voidEventListener == null)
        {
            voidEventListener = new VoidEventListener(OnEventTriggered);
        }
        
        if (IsInitialised) return;
        
        Addressables.LoadAssetAsync<VoidEvent>(voidEventReference).Completed += OnVoidEventAssetLoaded;
    }
    
    private void OnVoidEventAssetLoaded(AsyncOperationHandle<VoidEvent> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            voidEvent = obj.Result;
            Debug.Log($"Successfully loaded asset <{voidEvent.name}>");
            
            voidEvent.RegisterListener(voidEventListener);

            IsInitialised = true;
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
}
