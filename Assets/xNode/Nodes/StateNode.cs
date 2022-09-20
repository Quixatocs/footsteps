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
    protected int assetLoadCount;

    public virtual void OnEnter()
    {
        Debug.Log($"At State <{name}>.");
        IsComplete = false;
    }

    public virtual void OnExit()
    {
    }
    
    public override object GetValue(NodePort port) 
    {
        if (port.fieldName != "exit") return null;
        return null;
    }
    
    public StateNode GetNextStateNode()
    {
        return GetOutputPort("exit").Connection.node as StateNode;
    }
}
