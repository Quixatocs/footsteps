using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/States/WaitForHexSelectState", order = 1)]
public class WaitForHexSelectState : State
{
    public Vector3Event MapClickedEvent;
    public override void OnEnter()
    {
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
            MapClickedEvent.Raise(worldPoint);
        }
    }
}
