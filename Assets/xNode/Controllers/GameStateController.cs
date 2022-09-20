using UnityEngine;
using XNode;

[CreateAssetMenu]
public class GameStateController : NodeGraph
{
    private StateNode currentStateNode;
    
    public void StartGraph()
    {
        foreach (var node in nodes)
        {
            if (node is not EntryStateNode) continue;
            currentStateNode = node as EntryStateNode;
            currentStateNode.OnEnter();
            break;
        }
    }

    public void CheckCurrentStateCompleted()
    {
        if (!currentStateNode.IsComplete) return;
        ContinueGraph();
    }

    private void ContinueGraph()
    {
        currentStateNode.OnExit();
        
        StateNode nextStateNode = currentStateNode.GetNextStateNode();
        
        if (nextStateNode == null) return;
        
        currentStateNode = nextStateNode;
        currentStateNode.OnEnter();
    }

    public void Update()
    {
        currentStateNode.OnUpdate();
    }

}