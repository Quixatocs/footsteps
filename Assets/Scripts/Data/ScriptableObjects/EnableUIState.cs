using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/States/EnableUIState", order = 1)]
public class EnableUIState : State
{
    public UIVariable ui;

    public override void OnEnter()
    {
        ui.Value.SetActive(true);
    }
    
    public override void OnExit()
    {
        ui.Value.SetActive(false);
    }

    public override void OnUpdate()
    {
    }
}
