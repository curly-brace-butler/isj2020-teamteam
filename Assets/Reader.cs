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

    public TapeReader(List<Action> tape, float startTime)
    {
        m_tape = tape;
        m_startTime = startTime;
    }

    public bool GetKey()
    {
        while (m_index < m_tape.Count && m_tape[m_index].m_time <= Time.time - m_startTime)
        {
            m_index++;
        }

        return (m_index < m_tape.Count - 1 && m_tape[m_index].m_keyStatus == Action.KeyStatus.released)
               || (m_index > 0 && m_tape[m_index - 1].m_keyStatus == Action.KeyStatus.pressed);
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
            /*
            string line = kvPair.Key.ToString();
            foreach (var action in kvPair.Value)
            {
                line = String.Concat(line, " ", action.m_time, ":", action.m_keyStatus);
            }
            Debug.Log(line);
            */
        }
    }

    public bool GetKey(KeyCode vKey)
    {
        if (!m_inputTapes.TryGetValue(vKey, out var reader))
        {
            return false;
        }

        return reader.GetKey();
    }
    
}
