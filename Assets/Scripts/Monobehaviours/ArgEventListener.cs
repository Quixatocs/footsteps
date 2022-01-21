using UnityEngine;
using UnityEngine.Events;

public class ArgEventListener<T> : MonoBehaviour
{
    public ArgEvent<T> Event;
    public UnityEvent<T> Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(T arg)
    {
        Response.Invoke(arg);
    }
}
