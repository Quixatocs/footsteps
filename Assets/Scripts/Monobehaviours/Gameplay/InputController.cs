using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public Vector3Event MapClickedEvent;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
            MapClickedEvent.Raise(worldPoint);
        }
    }
}
