using System;

public class VoidEventListener : IVoidEventListener
{
    private readonly Action callback;

    public VoidEventListener(Action callback)
    {
        this.callback = callback;
    }
    
    public void OnEventRaised()
    {
        callback?.Invoke();
    }
}
