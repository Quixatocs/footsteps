using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LogicStateNode : StateNode
{
    [Output(dynamicPortList = true)]public Transition[] transitions;

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
            transition.LoadLogicAsset().Completed += OnLoadLogicAssetComplete;
        }
    }

    private void OnLoadLogicAssetComplete(AsyncOperationHandle<IntVariable> obj)
    {
        if (obj.Status != AsyncOperationStatus.Succeeded) return;
        
        ContinueOnAllAssetsLoaded();
    }

    protected override void Continue()
    {
        IsComplete = true; 
    }
    
    public override StateNode GetNextStateNode()
    {
        if (transitions == null || transitions.Length == 0)
        {
            Debug.LogError($"No Transitions present for state <{name}>.");
            return GetOutputPort("exit").Connection.node as StateNode;
        }
        
        if (transitions.Length == 1)
        {
            return GetOutputPort($"transitions 0").Connection.node as StateNode;
        }

        if (transitions.Length > 1)
        {
            for (int i = 0; i < transitions.Length; i++)
            {
                if (transitions[i].IsOpenTransition())
                {
                    return GetOutputPort($"transitions {i}").Connection.node as StateNode;
                }
            }
        }

        Debug.LogError($"No open transition present for state <{name}>.");
        return GetOutputPort("exit").Connection.node as StateNode;
    }
}
