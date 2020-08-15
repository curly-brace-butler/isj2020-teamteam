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

    public GameObject duplicatePrefabs;

    List<InputAction> recorders = new List<InputAction>();

    float startTime;

    public void ResetRecorder()
    {
        startTime = Time.time;
        recorders = new List<InputAction>();
    }

    public void SpawnDuplicate(Vector2 playerInitialPosition)
    {
        recorders.Add(new InputAction { action = InputAction.Action.EndRecord, time = Time.time - startTime });

        GameObject duplicate = Instantiate(duplicatePrefabs, playerInitialPosition, Quaternion.identity);
        duplicate.GetComponent<Reader>().Initialize(new TapeReader { inputActions = recorders, intialPosition = playerInitialPosition });
    }

    private void Update()
    {
        for (int i = 0; i < registerKeyActions.Length; i++)
        {
            if (Input.GetKeyDown(registerKeyActions[i].keyCode))
            {
                Debug.Log($"GetKeyDown: {registerKeyActions[i].keyCode}");
                recorders.Add(new InputAction { action = registerKeyActions[i].action, status = InputAction.Status.Pressed, time = Time.time - startTime });
            }
            if (Input.GetKeyUp(registerKeyActions[i].keyCode))
            {
                Debug.Log($"GetKeyUp: {registerKeyActions[i].keyCode}");
                recorders.Add(new InputAction { action = registerKeyActions[i].action, status = InputAction.Status.Released, time = Time.time - startTime });
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnDuplicate(new Vector2(-5, 0));
        }
    }
}
