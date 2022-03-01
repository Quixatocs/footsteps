using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/States/ResetIntDataState", order = 1)]
[Serializable]
public class ResetIntDataState : ResetDataState<IntVariable>
{
}
