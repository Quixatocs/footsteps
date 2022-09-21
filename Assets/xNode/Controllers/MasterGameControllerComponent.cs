using System;
using UnityEngine;

public class MasterGameControllerComponent : MonoBehaviour
{
    [SerializeField] private GameStateController gameStateController;

    private void Start()
    {
        gameStateController.StartGraph();
    }

    private void Update()
    {
        gameStateController.CheckCurrentStateCompleted();
        gameStateController.Update();
    }
}
