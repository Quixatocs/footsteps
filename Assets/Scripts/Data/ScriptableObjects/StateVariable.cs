using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Variables/StateVariable", order = 1)]
public class StateVariable : ScriptableObject
{
    public IState Value;
}
