using UnityEngine;

public class StateMachineController : MonoBehaviour
{
    public StateMachineVariable stateMachineVariable;

    private void Update()
    {
        stateMachineVariable.Value.CheckComplete();
    }
}
