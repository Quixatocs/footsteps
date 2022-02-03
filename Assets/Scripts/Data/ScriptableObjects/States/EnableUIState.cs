using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/States/EnableUIState", order = 1)]
public class EnableUIState : State
{
    public BoolEvent uiActivationEvent;

    public override void OnEnter()
    {
        IsComplete = false;
        uiActivationEvent.Raise(true);
    }
    
    public override void OnExit()
    {
        uiActivationEvent.Raise(false);
    }

    public override void OnUpdate()
    {
    }

    public void ReturnFromUI()
    {
        IsComplete = true;
    }
    
}
