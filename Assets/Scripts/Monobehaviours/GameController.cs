using UnityEngine;

public class GameController : MonoBehaviour
{
    public StateMachineVariable stateMachineVariable;
    public StateVariable initialState;

    private void Start()
    {
        stateMachineVariable.Value.SetState(initialState);
    }

    private void Update()
    {
        stateMachineVariable.Value.CheckCurrentStateCompleted();
        stateMachineVariable.Value.StateUpdate();
    }
}
