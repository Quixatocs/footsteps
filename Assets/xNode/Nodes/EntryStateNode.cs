
public class EntryStateNode : StateNode 
{
    public override void OnEnter()
    {
        base.OnEnter();
        IsComplete = true;
    }

    protected override void ContinueOnAllAssetsLoaded()
    {
        throw new System.NotImplementedException();
    }

    protected override void Continue()
    {
        throw new System.NotImplementedException();
    }
}