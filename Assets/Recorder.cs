using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Action
{
    public enum KeyStatus
    {
        pressed,
        released
    }
    public float m_time;
    public KeyStatus m_keyStatus;

    public Action(KeyStatus status)
    {
        m_time = Time.time;
        m_keyStatus = status;
    }
}

public class Recorder : MonoBehaviour
{
    public GameObject Duplicate;
    
    Dictionary<KeyCode, List<Action>> m_inputTapes = new Dictionary<KeyCode, List<Action>>();
    
    void Update()
    {
        foreach(KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
        {
            if(Input.GetKeyDown(vKey))
            {
                if (!m_inputTapes.TryGetValue(vKey, out List<Action> inputTape))
                {
                    inputTape = new List<Action>();
                    m_inputTapes.Add(vKey, inputTape);
                }
                Action action = new Action(Action.KeyStatus.pressed);
                inputTape.Add(action);
            }
            if(Input.GetKeyUp(vKey))
            {
                if (!m_inputTapes.TryGetValue(vKey, out List<Action> inputTape))
                {
                    inputTape = new List<Action>();
                    m_inputTapes.Add(vKey, inputTape);
                }
                Action action = new Action(Action.KeyStatus.released);
                inputTape.Add(action);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject duplicate = Instantiate(Duplicate);
            duplicate.GetComponent<Reader>().Initialize(m_inputTapes);
        }
    }
}
