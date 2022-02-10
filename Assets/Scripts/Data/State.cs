using System;
using UnityEngine;

[Serializable]
public abstract class State : ScriptableObject
{
    public bool IsComplete;
    public State NextState;
    public abstract void OnEnter();

    public abstract void OnExit();

    public abstract void OnUpdate();

}
