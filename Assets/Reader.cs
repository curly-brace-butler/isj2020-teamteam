using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public class TapeReader
{
    private int m_index = 0;
    private List<Action> m_tape;

    private float m_startTime;
    private int m_lastFramePressed = -1;
    private int m_lastFrameReleased = -1;

    public TapeReader(List<Action> tape, float startTime)
    {
        m_tape = tape;
        m_startTime = startTime;
        // If first recorded action is release, key must have been pressed at start
        if (m_tape[0].m_keyStatus == Action.KeyStatus.released)
        {
            m_lastFramePressed = 0;
        }
    }

    public bool GetKeyDown()
    {
        // Check if key press was already recorded for this frame
        if (m_lastFramePressed == Time.frameCount)
        {
            return true;
        }
        // Return early if no key press is found this frame
        if (m_tape[m_index].m_time > Time.time - m_startTime)
        {
            return false;
        }
        // Otherwise, record key press that took place this frame
        m_lastFrameReleased = -1;
        m_lastFramePressed = Time.frameCount;
        m_index++;
        return true;
    }

    public bool GetKey()
    {
        while (m_index < m_tape.Count && m_tape[m_index].m_time <= Time.time - m_startTime)
        {
            m_index++;
        }

        return (m_index > 0 && m_tape[m_index - 1].m_keyStatus == Action.KeyStatus.pressed)
               || (m_index < m_tape.Count - 1 && m_tape[m_index].m_keyStatus == Action.KeyStatus.released);
    }

    public bool GetKeyUp()
    {
        // Check if key press was already released for this frame
        if (m_lastFrameReleased == Time.frameCount)
        {
            return true;
        }
        // Return early if no key release is found this frame
        if (m_tape[m_index].m_time > Time.time - m_startTime)
        {
            return false;
        }
        // Otherwise, record key release that took place this frame
        m_lastFramePressed = -1;
        m_lastFrameReleased = Time.frameCount;
        m_index++;
        return true;
    }

    public bool isDone()
    {
        return m_index == m_tape.Count
            || m_lastFramePressed == Time.frameCount
            || m_lastFrameReleased == Time.frameCount;
    }

    public void Update()
    {
        
    }
}

public class Reader : MonoBehaviour
{
    float m_startTime;
    Dictionary<KeyCode, TapeReader> m_inputTapes = new Dictionary<KeyCode, TapeReader>();

    public void Initialize(Dictionary<KeyCode, List<Action>> inputTapes)
    {
        m_startTime = Time.time;
        foreach (KeyValuePair<KeyCode, List<Action>> kvPair in inputTapes)
        {
            m_inputTapes.Add(kvPair.Key, new TapeReader(new List<Action>(kvPair.Value), m_startTime));
            string line = kvPair.Key.ToString();
            foreach (var action in kvPair.Value)
            {
                line = String.Concat(line, " ", action.m_time, ":", action.m_keyStatus);
            }
            Debug.Log(line);
        }
    }

    public void Update()
    {
        
    }

    public bool GetKeyDown(KeyCode vKey)
    {
        if (!m_inputTapes.TryGetValue(vKey, out var reader) || reader.isDone())
        {
            return false;
        }

        return reader.GetKeyDown();
    }

    public bool GetKey(KeyCode vKey)
    {
        if (!m_inputTapes.TryGetValue(vKey, out var reader))
        {
            return false;
        }

        return reader.GetKey();
    }

    public bool GetKeyUp(KeyCode vKey)
    {
        if (!m_inputTapes.TryGetValue(vKey, out var reader) || reader.isDone())
        {
            return false;
        }

        return reader.GetKeyUp();
    }
    
}
