using System;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/States/LogicState", order = 1)]
[Serializable]
public class LogicState : State
{
    public Transition[] transitions;

    public override void OnEnter()
    {
        base.OnEnter();

        if (IsInitialised)
        {
            Continue();
            return;
        }
        
        foreach (Transition transition in transitions)
        {
            ++assetLoadCount;
            //transition.LoadNextStateAsset().Completed += OnLoadNextStateAssetComplete;
        }
    }

    private void OnLoadNextStateAssetComplete(AsyncOperationHandle<State> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            ContinueOnAllAssetsLoaded();
        }
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        if (!IsInitialised) return;

        IsComplete = true;
    }

    protected override void ContinueOnAllAssetsLoaded()
    {
        if (--assetLoadCount == 0)
        {
            Debug.Log($"All states loaded on transitions for LogicState <{name}>");
            IsInitialised = true;
            Continue();
        }
    }

    protected override void Continue()
    {
    }

    public override State GetNextState()
    {
        if (transitions == null || transitions.Length == 0)
        {
            Debug.LogError($"No Transitions present for state <{name}>.");
            return null;
        }

        if (transitions.Length == 1)
        {
            //return transitions[0].GetNextState();
        }

        if (transitions.Length > 1)
        {
            foreach (Transition transition in transitions)
            {
                if (transition.IsOpenTransition())
                {
                    //return transition.GetNextState();
                }
            }
        }

        Debug.LogError($"No open transition present for state <{name}>.");
        return null;
    }
}