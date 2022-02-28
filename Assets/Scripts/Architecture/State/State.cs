using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[Serializable]
public abstract class State : ScriptableObject
{
    [NonSerialized]
    public bool IsComplete;

    [SerializeField]
    protected AssetReference nextStateReference;
    
    [NonSerialized]
    protected State nextState;
    [NonSerialized]
    protected AsyncOperationHandle<State>? stateHandleOperation;

    [NonSerialized]
    protected bool IsInitialised;
    
    protected int assetLoadCount;

    public virtual void OnEnter()
    {
        IsComplete = false;
        stateHandleOperation = null;
        if (!IsInitialised && !string.IsNullOrEmpty(nextStateReference.AssetGUID))
        {
            //stateHandleOperation = new AsyncOperationHandle<State>();
            ++assetLoadCount;
            stateHandleOperation = nextStateReference.LoadAssetAsync<State>();
        }
    }

    public abstract void OnExit();

    public abstract void OnUpdate();

    protected abstract void ContinueOnAllAssetsLoaded();

    protected abstract void Continue();

    public abstract State GetNextState();
}
