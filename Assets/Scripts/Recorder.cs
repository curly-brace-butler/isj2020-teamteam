using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerInput
{
    public float deltaTime;
    public Vector2 movement;
    public Vector2 shoot;
}

public class Recorder : MonoBehaviour
{
    public PlayerController controller;

    List<PlayerInput> recorders = new List<PlayerInput>();

    // Update is called once per frame
    void LateUpdate()
    {
        recorders.Add(new PlayerInput
        {
            movement = controller.Movement,
            shoot = controller.BallThrow,
            deltaTime = Time.deltaTime
        });
    }

    public void StartRecording()
    {
        recorders = new List<PlayerInput>();

        enabled = true;
    }

    public List<PlayerInput> StopRecording()
    {
        enabled = false;

        return recorders;
    }
}
