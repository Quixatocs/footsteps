using UnityEngine;

public class RaiseVoidEventStateNode : StateNode
{
    [SerializeField] private VoidEvent voidEvent;
    
    public override void OnEnter()
    {
        base.OnEnter();
        voidEvent.Raise();
        IsComplete = true;
    }
}
