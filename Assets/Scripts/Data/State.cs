using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;

[Serializable]
public abstract class State : ScriptableObject
{
    public bool IsComplete;
    public AssetReference NextStateReference;

    public Transition[] transitions;

    protected bool IsInitialised;

    public abstract void OnEnter();

    public abstract void OnExit();

    public abstract void OnUpdate();

    public virtual State GetNextState()
    {
        if (transitions == null || transitions.Length == 0)
        {
            Debug.LogError($"No Transitions present for state <{name}>.");
            return null;
        }

        if (transitions.Length == 1)
        {
            return transitions[0].NextState;
        }

        if (transitions.Length > 1)
        {
            foreach (Transition transition in transitions)
            {
                if (transition.IsOpenTransition())
                {
                    return transition.NextState;
                }
            }
        }
        Debug.LogError($"No open transition present for state <{name}>.");
        return null;
    }
}
