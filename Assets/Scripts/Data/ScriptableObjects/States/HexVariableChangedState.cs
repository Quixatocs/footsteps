using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/States/HexVariableChangedState", order = 1)]
[Serializable]
public class HexVariableChangedState : VariableChangedState<HexVariable>
{
}
