using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Deltas/IntDelta", order = 1)]
[Serializable]   
public class IntDelta : ScriptableObject
{
    public int baseAmount;
    public IntVariable stat;
    public VoidEvent deltaAppliedEvent;

    public void ApplyDelta()
    {
        stat.Value += baseAmount;
        deltaAppliedEvent.Raise();
    }
}
