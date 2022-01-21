#if false


using System.Collections;
using UnityEngine;

public class StateMachine
{

    private IState currentState;

    public IState CurrentState => currentState;

    protected void SetState(IState nextState) {
        
        // Exit previous state
        if (currentState != null) {
            currentState.OnExit();
        }
        
        currentState = nextState;
        
        if (currentState != null) {
            currentState.OnEnter(this);
            
            if (stateCompleteCheck == null) {
                stateCompleteCheck = StartCoroutine(CheckComplete());
            }
        }
        else {
            if (stateCompleteCheck != null) {
                StopCoroutine(stateCompleteCheck);
                stateCompleteCheck = null;
            }
        }
    }


    /// <summary>
    /// Checks if the current state is complete
    /// </summary>
    private IEnumerator CheckComplete() {
        while (true) {
            yield return new WaitForEndOfFrame();
            
            if (currentState == null) { 
                // We are leaving the state machine so we can stop running the check
                stateCompleteCheck = null;
                yield break;
            }
            
            if (currentState.IsComplete) {
                SetState(currentState.NextState);
            }
        }
    }
}
#endif