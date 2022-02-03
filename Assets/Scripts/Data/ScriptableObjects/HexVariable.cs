
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Variables/HexVariable", order = 1)]
public class HexVariable : ScriptableObject
{
    public Hex DefaultValue;
    public Hex Value;
    
    private void OnEnable()
    {
        Value = DefaultValue;
    }
}
