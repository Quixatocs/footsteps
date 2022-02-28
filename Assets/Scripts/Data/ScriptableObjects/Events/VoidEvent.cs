using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Event/VoidEvent", order = 1)]
public class VoidEvent : ScriptableObject
{
    private List<IVoidEventListener> listeners = new List<IVoidEventListener>();

    public void Raise()
    {
        List<IVoidEventListener> listenersCopy = new List<IVoidEventListener>(listeners);
        for (int i = listenersCopy.Count - 1; i >= 0; i--)
        {
            Debug.Log($"Event <{name}> raised for listener <{listenersCopy[i]}>");
            listenersCopy[i].OnEventRaised();
        }
    }

    public void RegisterListener(IVoidEventListener listenerMono)
    {
        listeners.Add(listenerMono);
    }
    
    public void UnregisterListener(IVoidEventListener listenerMono)
    {
        listeners.Remove(listenerMono);
    }
}
