using System;
using UnityEngine;

[Serializable]
public abstract class State : ScriptableObject
{
    public bool IsComplete;
    public StateVariable NextState;
    public abstract void OnEnter();

    public abstract void OnExit();

    public abstract void OnUpdate();

}
