using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideEvent : MonoBehaviour
{
    public GameEvent collideEvent;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collideEvent.Raise();
    }
}
