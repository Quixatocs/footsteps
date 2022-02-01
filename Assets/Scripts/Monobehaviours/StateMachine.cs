using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Control/StateMachine", order = 1)]
public class StateMachine : ScriptableObject
{

    private StateVariable currentState;

    public StateVariable CurrentState => currentState;

    public void SetState(StateVariable nextState) {
        
        if (currentState != null) {
            currentState.Value.OnExit();
        }
        
        currentState = nextState;
        
        if (currentState != null) {
            currentState.Value.OnEnter();
        }
    }

    /// <summary>
    /// Checks if the current state is complete
    /// </summary>
    public void CheckComplete() {
            
        if (currentState == null) return;
        
        if (currentState.Value.IsComplete) {
            SetState(currentState.Value.NextState);
        }
    }
}
