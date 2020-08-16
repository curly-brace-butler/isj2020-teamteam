using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;

    [Header("Shooting")]
    public float ballSpeed = 8f;
    public Transform mouseIndicator;
    public BounceBall playerBall;

    public Vector2 Movement { get; private set; }
    public Vector2 BallThrow { get; private set; }

    Vector2 mouseDirection;
    bool hasBallExited = false;

    private void Awake()
    {
        Movement = Vector2.zero;
    }

    void Start()
    {
        //Disable collision between player and the ball
        var playerCollider = GetComponent<CircleCollider2D>();
        var playerBallCollider = playerBall.GetComponent<CircleCollider2D>();
        Physics2D.IgnoreCollision(playerCollider, playerBallCollider);
    }

    private void Update()
    {
        UpdatePosition();
        UpdateMousePosition();

        if (Input.GetMouseButtonDown(0) && playerBall != null)
        {
            BallThrow = mouseDirection * ballSpeed;
            playerBall.Throw(mouseDirection, ballSpeed);
            playerBall = null;

            hasBallExited = false;
        }
        else
        {
            BallThrow = Vector2.zero;
        }
    }

    private void UpdateMousePosition()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
        mouseDirection = mouseWorldPos - transform.position;

        mouseDirection.Normalize();

        mouseIndicator.up = mouseDirection;
        mouseIndicator.position = transform.position + (Vector3)mouseDirection;
    }

    private void UpdatePosition()
    {
        Vector2 input;
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        Movement = input * speed;

        transform.Translate(Movement * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == Constant.DuplicateBallTag)
        {
            Debug.LogWarning("Player Death");
            //Kill();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasBallExited && collision.tag == Constant.PlayerBallTag)
        {
            playerBall = collision.GetComponent<BounceBall>();
            playerBall.Catch(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!hasBallExited && collision.tag == Constant.PlayerBallTag)
        {
            hasBallExited = true;
        }
    }
}