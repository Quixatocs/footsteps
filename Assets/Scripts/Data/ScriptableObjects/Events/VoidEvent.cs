using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Event/VoidEvent", order = 1)]
public class VoidEvent : ScriptableObject
{
    private List<VoidEventListener> listeners = new List<VoidEventListener>();

    public void Raise()
    {
        List<VoidEventListener> listenersCopy = new List<VoidEventListener>(listeners);
        for (int i = listenersCopy.Count - 1; i >= 0; i--)
        {
            listenersCopy[i].OnEventRaised();
        }
    }

    public void RegisterListener(VoidEventListener listener)
    {
        listeners.Add(listener);
    }
    
    public void UnregisterListener(VoidEventListener listener)
    {
        listeners.Remove(listener);
    }
}
