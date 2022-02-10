using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Control/StateMachine", order = 1)]
public class StateMachine : ScriptableObject
{
    private State currentState;

    public void SetState(State nextState) {
        
        if (currentState != null) {
            currentState.OnExit();
        }

        Debug.Log($"Exiting: <{currentState}> | Entering <{nextState}>");
        currentState = nextState;
        
        if (currentState != null) {
            currentState.OnEnter();
        }
    }

    public void StateUpdate()
    {
        if (currentState == null) return;
        
        currentState.OnUpdate();
    }

    /// <summary>
    /// Checks if the current state is complete
    /// </summary>
    public void CheckCurrentStateCompleted() {
            
        if (currentState == null) return;
        
        if (currentState.IsComplete) {
            SetState(currentState.NextState);
        }
    }
}
