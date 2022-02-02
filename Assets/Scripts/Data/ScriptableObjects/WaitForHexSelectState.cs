using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/States/WaitForHexSelectState", order = 1)]
[Serializable]
public class WaitForHexSelectState : State
{
    public HexEvent hexClickedEvent;
    public WorldObjectManager WorldObjectManager;
    private Grid grid;
    public override void OnEnter()
    {
        grid = WorldObjectManager.GetComponent<Grid>();
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
            
            Hex clickedHex = grid.WorldToHex(worldPoint); 
            Debug.Log($"STATE: V3: <{worldPoint}> HX: <{clickedHex.ToVector3Int()}> UN: <{clickedHex.ToUnityCoords()}>");
            hexClickedEvent.Raise(clickedHex);
        }
    }
}
