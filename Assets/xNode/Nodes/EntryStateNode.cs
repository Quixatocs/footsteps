
public class EntryStateNode : StateNode 
{
    public override void OnEnter()
    {
        base.OnEnter();
        IsComplete = true;
    }

    public override void OnExit()
    {
    }
}