using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Control/StateMachine", order = 1)]
public class StateMachine : ScriptableObject
{
    private StateVariable currentState;

    public void SetState(StateVariable nextState) {
        
        if (currentState != null) {
            currentState.Value.OnExit();
        }
        Debug.Log($"Exiting: <{currentState}> | Entering <{nextState}>");
        currentState = nextState;
        
        if (currentState != null) {
            currentState.Value.OnEnter();
        }
    }

    public void StateUpdate()
    {
        if (currentState == null) return;
        
        currentState.Value.OnUpdate();
    }

    /// <summary>
    /// Checks if the current state is complete
    /// </summary>
    public void CheckCurrentStateCompleted() {
            
        if (currentState == null) return;
        
        if (currentState.Value.IsComplete) {
            SetState(currentState.Value.NextState);
        }
    }
}
