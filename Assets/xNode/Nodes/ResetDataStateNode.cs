using UnityEngine;

public abstract class ResetDataStateNode<T> : StateNode 
{
    [SerializeField] private Variable<T> variable;

    public override void OnEnter()
    {
        base.OnEnter();
        variable.Value = variable.DefaultValue;
        IsComplete = true;
    }
}