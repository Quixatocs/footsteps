using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/States/ResetIntDataState", order = 1)]
[Serializable]
public class ResetIntDataState : ResetDataState<IntVariable>
{
    protected override void Continue()
    {
        variable.Value = variable.DefaultValue;
        
        IsComplete = true;
    }
}
