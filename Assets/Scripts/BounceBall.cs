using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceBall : MonoBehaviour
{
    public float speed = 1f;
    public Vector2 direction = new Vector2(-1, 0);

    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        transform.Translate(direction * speed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var controller = collision.gameObject.GetComponent<DuplicateController>();
        if (controller != null)
        {
            controller.Kill();
        }

        var contactPoint = collision.GetContact(0).point;
        var normal = (contactPoint - (Vector2)transform.position).normalized;
        direction = Vector2.Reflect(direction, normal);
    }

    public void Throw(Vector2 direction, float speed)
    {
        this.direction = direction;
        this.speed = speed;

        transform.parent = null;
        rigid.simulated = true;

        enabled = true;
    }

    public void Catch(Transform parent)
    {
        enabled = false;

        rigid.simulated = false;

        transform.parent = parent;
        transform.localPosition = Vector3.zero;
    }
}
