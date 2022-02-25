using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/States/RestartSceneState", order = 1)]
[Serializable]
public class RestartSceneState : State
{
    public override void OnEnter()
    {
    }
    public override void OnExit()
    {
        SceneManager.LoadScene(0);
    }

    public override void OnUpdate()
    {
        IsComplete = true;
    }

    protected override void ContinueOnAllAssetsLoaded()
    {
    }

    public override State GetNextState()
    {
        return nextState;
    }
}
