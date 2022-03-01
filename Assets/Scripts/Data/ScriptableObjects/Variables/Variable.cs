using UnityEngine;

public class Variable<T> : ScriptableObject
{
    public T DefaultValue;
    public T Value;
    
    private void OnEnable()
    {
        Value = DefaultValue;
    }
}
