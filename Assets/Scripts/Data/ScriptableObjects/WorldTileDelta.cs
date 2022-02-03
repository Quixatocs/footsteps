using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Map/WorldTileDelta", order = 1)]
[Serializable]    
public class WorldTileDelta : ScriptableObject
{
    public int baseAmount;
    public IntVariable stat;
    public VoidEvent deltaAppliedEvent;

    public void ApplyDelta()
    {
        stat.Value -= baseAmount;
        deltaAppliedEvent.Raise();
    }
}
