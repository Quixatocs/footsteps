using UnityEngine;

public class SetDataStateNode : StateNode 
{
    [SerializeField] private IntVariable intVariable;

    public override void OnEnter()
    {
        base.OnEnter();
        intVariable.Value = intVariable.DefaultValue;
        //IsComplete = true;
    }
    
    public override void OnExit()
    {
    }

    
}