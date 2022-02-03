using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Variables/IntVariable", order = 1)]
public class IntVariable : ScriptableObject
{
    public int DefaultValue;
    public int Value;

    private void OnEnable()
    {
        Value = DefaultValue;
    }
}
