using System;
using UnityEngine;
using XNode;

public abstract class StateNode : Node
{
    [Input] public bool value;
    [Output] public bool exit;

    [NonSerialized]
    public bool IsComplete;
    
    [NonSerialized]
    protected bool IsInitialised;
    
    [NonSerialized]
    protected int assetLoadCount;

    public virtual void OnEnter()
    {
        Debug.Log($"<color=#00FF00>At State <{name}>.</color>");
        IsComplete = false;
    }

    public virtual void OnExit()
    {
    }

    public virtual void OnUpdate()
    {
    }

    protected virtual void ContinueOnAllAssetsLoaded()
    {
        if (--assetLoadCount != 0) return;
        
        IsInitialised = true;
        Continue();
    }

    protected virtual void Continue()
    {
    }
    
    public override object GetValue(NodePort port) 
    {
        if (port.fieldName != "exit") return null;
        return null;
    }
    
    public virtual StateNode GetNextStateNode()
    {
        return GetOutputPort("exit").Connection.node as StateNode;
    }
}
