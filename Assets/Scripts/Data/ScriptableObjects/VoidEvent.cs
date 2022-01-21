using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Event/VoidEvent", order = 1)]
public class VoidEvent : ScriptableObject
{
    private List<EventListener> listeners = new List<EventListener>();

    public void Raise()
    {
        List<EventListener> listenersCopy = new List<EventListener>(listeners);
        for (int i = listenersCopy.Count - 1; i >= 0; i--)
        {
            listenersCopy[i].OnEventRaised();
        }
    }

    public void RegisterListener(EventListener listener)
    {
        listeners.Add(listener);
    }
    
    public void UnregisterListener(EventListener listener)
    {
        listeners.Remove(listener);
    }
}
