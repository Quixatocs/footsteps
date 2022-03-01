using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/States/ResetHexDataState", order = 1)]
[Serializable]
public class ResetHexDataState : ResetDataState<HexVariable>
{
}
