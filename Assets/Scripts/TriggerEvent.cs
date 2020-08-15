using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvent : MonoBehaviour
{
    public GameEvent triggerEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        triggerEvent.Raise();
    }
}
