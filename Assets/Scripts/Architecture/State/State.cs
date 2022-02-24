using System;
using UnityEngine;

[Serializable]
public abstract class State : ScriptableObject
{
    public bool IsComplete;

    private State nextState;

    protected bool IsInitialised;

    public abstract void OnEnter();

    public abstract void OnExit();

    public abstract void OnUpdate();

    public abstract State GetNextState();
}
