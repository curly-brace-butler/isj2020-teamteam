using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

    Vector2 direction;

    void Start()
    {
        direction = Vector2.zero;
    }

    private void FixedUpdate()
    {
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");

        transform.Translate(direction * speed * Time.fixedDeltaTime);
    }
}