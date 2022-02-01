using UnityEngine;

public class IState : ScriptableObject
{
    public bool IsComplete;
    public StateVariable NextState;
    public virtual void OnEnter()
    {
        
    }
    
    public virtual void OnExit()
    {
        
    }

    public virtual void OnUpdate()
    {
        
    }
    
}
