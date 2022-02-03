using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Variables/StateMachineVariable", order = 1)]
public class StateMachineVariable : ScriptableObject
{
    public StateMachine Value;
}
