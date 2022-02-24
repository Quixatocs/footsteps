using System;
using UnityEngine;

[Serializable]
public abstract class State : ScriptableObject
{
    public bool IsComplete;

    public Transition[] transitions;

    protected bool IsInitialised;
    
    public virtual void OnEnter()
    {
        IsComplete = false;
        foreach (Transition transition in transitions)
        {
            transition.LoadNextStateAsset();
        }
    }

    public abstract void OnExit();

    public abstract void OnUpdate();

    public State GetNextState()
    {
        if (transitions == null || transitions.Length == 0)
        {
            Debug.LogError($"No Transitions present for state <{name}>.");
            return null;
        }

        if (transitions.Length == 1)
        {
            return transitions[0].GetNextState();
        }

        if (transitions.Length > 1)
        {
            foreach (Transition transition in transitions)
            {
                if (transition.IsOpenTransition())
                {
                    return transition.GetNextState();
                }
            }
        }
        Debug.LogError($"No open transition present for state <{name}>.");
        return null;
    }
}
