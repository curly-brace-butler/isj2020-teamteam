using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct InputAction
{
    public enum Status
    {
        None,
        Pressed,
        Released,
    }

    public enum Action
    {
        None = 0x0,
        Left = 0x1,
        Right = 0x2,
        Up = 0x4,
        Down = 0x8,

        EndRecord
    }

    public float time;
    public Action action;
    public Status status;
}

public class Recorder : MonoBehaviour
{
    struct KeyActionPair
    {
        public KeyCode keyCode;
        public InputAction.Action action;
    }

    static KeyActionPair[] registerKeyActions = new KeyActionPair[] {
        new KeyActionPair { keyCode = KeyCode.A, action = InputAction.Action.Left },
        new KeyActionPair { keyCode = KeyCode.D, action = InputAction.Action.Right },
        new KeyActionPair { keyCode = KeyCode.W, action = InputAction.Action.Up },
        new KeyActionPair { keyCode = KeyCode.S, action = InputAction.Action.Down }
    };

    List<InputAction> recorders = new List<InputAction>();

    float startedTime;

    private void Start()
    {
        StartRecording(Time.time);
    }

    public void StartRecording(float startTime)
    {
        startedTime = startTime;
        recorders = new List<InputAction>();

        enabled = true;
    }

    public List<InputAction> StopRecording()
    {
        enabled = false;

        recorders.Add(new InputAction { action = InputAction.Action.EndRecord, time = Time.time - startedTime });
        return recorders;
    }

    private void Update()
    {
        for (int i = 0; i < registerKeyActions.Length; i++)
        {
            if (Input.GetKeyDown(registerKeyActions[i].keyCode))
            {
                //Debug.Log($"GetKeyDown: {registerKeyActions[i].keyCode}");
                recorders.Add(new InputAction { action = registerKeyActions[i].action, status = InputAction.Status.Pressed, time = Time.time - startedTime });
            }
            if (Input.GetKeyUp(registerKeyActions[i].keyCode))
            {
                //Debug.Log($"GetKeyUp: {registerKeyActions[i].keyCode}");
                recorders.Add(new InputAction { action = registerKeyActions[i].action, status = InputAction.Status.Released, time = Time.time - startedTime });
            }
        }
    }
}
