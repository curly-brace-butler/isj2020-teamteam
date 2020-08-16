using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceBall : MonoBehaviour
{
    public float speed = 1f;
    public Vector2 direction = new Vector2(-1, 0);

    public AudioSource ballAudioSource;
    public AudioClip wallCollisionSound;
    public AudioClip duplicateCollisionSound;

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
        if (this.tag == "PlayerBall")
        {
            if (collision.collider.tag == "Wall")
            {
                ballAudioSource.PlayOneShot(wallCollisionSound, 0.4f);
            }
            else if (collision.collider.tag == "Duplicate")
            {
                ballAudioSource.PlayOneShot(duplicateCollisionSound, 0.5f);
            }
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
