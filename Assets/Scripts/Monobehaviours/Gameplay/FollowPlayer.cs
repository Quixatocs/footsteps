﻿using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField]
    //TODO Turn this to WorldObjects
    private Grid grid;
    public void MoveToPlayer(Hex hex)
    {
        Vector3 newPosition = grid.HexToWorld(hex);
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
    }
}
