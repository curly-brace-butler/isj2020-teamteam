using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceBall : MonoBehaviour
{
    public float speed = 1f;
    public Vector2 direction = new Vector2(-1, 0);

    Vector2 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void FixedUpdate()
    {
        transform.Translate(direction * speed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var contactPoint = collision.GetContact(0).point;
        var normal = (contactPoint -(Vector2)transform.position).normalized;
        direction = Vector2.Reflect(direction, normal);
    }

    public void Restore()
    {
        direction = new Vector2(-1, 0);
        transform.position = initialPosition;
    }
}
