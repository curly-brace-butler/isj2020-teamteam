using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEvent : MonoBehaviour
{
    public GameEvent gameEvent;

    public void Trigger()
    {
        gameEvent.Raise();
    }
}
