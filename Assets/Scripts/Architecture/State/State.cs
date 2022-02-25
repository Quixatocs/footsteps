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
    
    protected State nextState;
    protected AsyncOperationHandle<State> stateHandleOperation;

    [NonSerialized]
    protected bool IsInitialised;
    
    protected int assetLoadCount;

    public virtual void OnEnter()
    {
        IsComplete = false;
        if (!IsInitialised && nextStateReference != null)
        {
            ++assetLoadCount;
            stateHandleOperation = nextStateReference.LoadAssetAsync<State>();
        }
    }

    public abstract void OnExit();

    public abstract void OnUpdate();

    protected abstract void ContinueOnAllAssetsLoaded();

    public abstract State GetNextState();
}
