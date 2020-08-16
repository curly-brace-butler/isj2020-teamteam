using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerInput
{
    public float deltaTime;
    public Vector2 movement;
    public Vector2 shoot;

    public Vector2 position;
}

public class Recorder : MonoBehaviour
{
    public float maxRecordTime = 5f;
    public PlayerController controller;

    List<PlayerInput> recorders = new List<PlayerInput>();

    float accumulateTime = 0;

    // Update is called once per frame
    void LateUpdate()
    {
        recorders.Add(new PlayerInput
        {
            movement = controller.Movement,
            shoot = controller.BallThrow,
            deltaTime = Time.deltaTime,
            position = controller.transform.position
        });

        accumulateTime += Time.deltaTime;
        if (accumulateTime >= maxRecordTime)
        {
            accumulateTime -= recorders[0].deltaTime;
            recorders.RemoveAt(0);
        }
    }

    public void StartRecording()
    {
        accumulateTime = 0;
        recorders = new List<PlayerInput>();

        enabled = true;
    }

    public List<PlayerInput> StopRecording()
    {
        enabled = false;

        return recorders;
    }
}
