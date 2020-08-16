using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;

    [Header("Shooting")]
    public float ballSpeed = 8f;
    public Transform mouseIndicator;
    public BounceBall playerBall;

    Vector2 direction;
    Vector2 mouseDirection;

    void Start()
    {
        direction = Vector2.zero;
    }

    private void Update()
    {
        UpdateMouse();

        if (Input.GetMouseButtonDown(0) && playerBall != null)
        {
            playerBall.Throw(mouseDirection, ballSpeed);
            playerBall = null;
        }
    }

    private void UpdateMouse()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
        mouseDirection = mouseWorldPos - transform.position;

        mouseDirection.Normalize();

        mouseIndicator.up = mouseDirection;
        mouseIndicator.position = transform.position + (Vector3)mouseDirection;
    }

    private void FixedUpdate()
    {
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");

        transform.Translate(direction * speed * Time.fixedDeltaTime);
    }
}