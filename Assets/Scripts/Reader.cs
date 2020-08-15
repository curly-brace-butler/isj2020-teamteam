using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public struct TapeReader
{
    public Vector2 intialPosition;
    public List<InputAction> inputActions;

    public TapeReader(Vector2 intialPosition, List<InputAction> tape)
    {
        this.intialPosition = intialPosition;
        this.inputActions = tape;
    }
}

public class Reader : MonoBehaviour
{
    public enum Axis
    { 
        Horizontal,
        Vertical
    }

    public Action OnTapeFinish;

    TapeReader tape;
    int tapeIndex;

    float startTime;
    InputAction.Action prevAxisAction = InputAction.Action.None;
    InputAction.Action axisAction = InputAction.Action.None;

    public void Initialize(TapeReader tapeReader)
    {
        tapeIndex = 0;
        tape = tapeReader;
    }

    public void Restore(float currentTime)
    {
        tapeIndex = 0;
        startTime = currentTime;
    }

    private void Update()
    {
        float currentTime = Time.time - startTime;
        var inputAction = tape.inputActions[tapeIndex];
        if (inputAction.time <= currentTime)
        {
            if (inputAction.action == InputAction.Action.EndRecord)
            {
                Restore(Time.time);

                OnTapeFinish?.Invoke();
            }
            else
            {
                InputAction.Action newAxisAction = axisAction;
                switch (inputAction.status)
                {
                    case InputAction.Status.Pressed:
                        newAxisAction |= inputAction.action;
                        break;
                    case InputAction.Status.Released:
                        newAxisAction &= ~inputAction.action;
                        break;
                }

                prevAxisAction = axisAction;
                axisAction = newAxisAction;

                tapeIndex++;
            }
        }
    }

    public float GetAxisRaw(Axis axis)
    {
        float axisValue = 0;
        switch (axis)
        {
            case Axis.Horizontal:
                axisValue -= axisAction.HasFlag(InputAction.Action.Left) ? 1 : 0;
                axisValue += axisAction.HasFlag(InputAction.Action.Right) ? 1 : 0;
                break;
            case Axis.Vertical:
                axisValue -= axisAction.HasFlag(InputAction.Action.Down) ? 1 : 0;
                axisValue += axisAction.HasFlag(InputAction.Action.Up) ? 1 : 0;
                break;
        }
        return axisValue;
    }
    

}
