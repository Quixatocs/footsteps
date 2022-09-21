using System.Collections.Generic;
using UnityEngine;

public class ArgEvent<T> : ScriptableObject
{
    private List<ArgEventListener<T>> listeners = new List<ArgEventListener<T>>();
    
    public void Raise(T arg)
    {
        List<ArgEventListener<T>> listenersCopy = new List<ArgEventListener<T>>(listeners);
        for (int i = listenersCopy.Count - 1; i >= 0; i--)
        {
            Debug.Log($"<color=#FF00FF>Event <{name}> raised for listener <{listenersCopy[i]}></color>");
            listenersCopy[i].OnEventRaised(arg);
        }
    }

    public void RegisterListener(ArgEventListener<T> listener)
    {
        listeners.Add(listener);
    }
    
    public void UnregisterListener(ArgEventListener<T> listener)
    {
        listeners.Remove(listener);
    }
}
