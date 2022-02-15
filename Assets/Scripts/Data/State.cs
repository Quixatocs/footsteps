using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

[Serializable]
public abstract class State : ScriptableObject
{
    public bool IsComplete;
    public AssetReference NextStateReference;

    [NonSerialized]
    public State nextState;
    protected bool IsInitialised;

    public abstract void OnEnter();

    public abstract void OnExit();

    public abstract void OnUpdate();

}
