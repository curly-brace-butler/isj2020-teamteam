using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceBall : MonoBehaviour
{
    public float speed = 1f;
    public Vector2 direction = new Vector2(-1, 0);

    public AudioSource ballAudioSource;
    public AudioClip wallCollisionSound;  // drum
    public AudioClip duplicateCollisionSound; // pluck

    public Camera mainCamera;

    public ParticleSystem sparks;

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
            var contactPoint = collision.GetContact(0).point;
            var normal = (contactPoint - (Vector2)transform.position).normalized;
            if (this.tag == "PlayerBall")
            {
                if (collision.collider.tag == "Wall")
                {
                    ballAudioSource.PlayOneShot(wallCollisionSound, 0.4f);
                // Does this give a vector from the gameObject to the camera?
                var vectorToCamera = mainCamera.transform.position - this.gameObject.transform.position;
                // What is the up axis for my object?
                var upAxisForObject = Vector3.up;
                // Points object axis toward camera. 
                var rotation = Quaternion.LookRotation(vectorToCamera, upAxisForObject);

                    Instantiate(sparks, contactPoint, rotation);
                }
                else if (collision.collider.tag == "Duplicate")
                {
                    ballAudioSource.PlayOneShot(duplicateCollisionSound, 0.5f);
                }
            }
            direction = Vector2.Reflect(direction, normal);
        }

    public void Throw(Vector2 force)
    {
        Throw(force.normalized, force.magnitude);
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
