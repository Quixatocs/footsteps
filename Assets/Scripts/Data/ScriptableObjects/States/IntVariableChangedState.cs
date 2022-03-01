using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/States/IntVariableChangedState", order = 1)]
[Serializable]
public class IntVariableChangedState : VariableChangedState<IntVariable>
{
}
