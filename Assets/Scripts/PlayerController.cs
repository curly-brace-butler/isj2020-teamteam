using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;

    [Header("Shooting")]
    public float ballSpeed = 8f;
    public float maxBallSpeed = 8f;
    public float chargeTime = 1f;
    public BounceBall playerBall;

    [Header("Indicator")]
    public Transform mouseIndicator;
    public float normalIndicatorScale = 0.2f;
    public float maxIndicatorScale = 0.5f;

    [Header("Event")]
    public GameEvent OnPlayerKilled;

    public Vector2 Movement { get; private set; }
    public Vector2 BallThrow { get; private set; }

    Vector2 mouseDirection;
    bool hasBallExited = false;
    bool hasBallInHand = true;
    float chargeBall = 0;

    private void Awake()
    {
        Movement = Vector2.zero;       
    }

    private void OnEnable()
    {
        //Disable collision between player and the ball
        var playerCollider = GetComponent<CircleCollider2D>();
        var playerBallCollider = playerBall.GetComponent<CircleCollider2D>();
        Physics2D.IgnoreCollision(playerCollider, playerBallCollider, true);
    }

    private void Update()
    {
        UpdatePosition();
        UpdateMousePosition();

        if (Input.GetMouseButtonUp(0) && hasBallInHand)
        {
            float chargeBallSpeed = ballSpeed + (maxBallSpeed - ballSpeed) * chargeBall;

            BallThrow = mouseDirection * chargeBallSpeed;
            playerBall.Throw(mouseDirection, chargeBallSpeed);

            hasBallInHand = false;
            hasBallExited = false;

            mouseIndicator.localScale = Vector2.one * normalIndicatorScale;

            chargeBall = 0;
        }
        else
        {
            if (Input.GetMouseButton(0) && hasBallInHand)
            {
                chargeBall = Math.Min(1f, chargeBall + chargeTime * Time.deltaTime);

                float indicatorChargeScale = normalIndicatorScale + (maxIndicatorScale - normalIndicatorScale) * chargeBall;
                mouseIndicator.localScale = new Vector2(indicatorChargeScale, normalIndicatorScale);
            }

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

            OnPlayerKilled.Raise();

            Destroy(playerBall.gameObject);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasBallExited && collision.tag == Constant.PlayerBallTag)
        {
            playerBall = collision.GetComponent<BounceBall>();
            playerBall.Catch(transform);

            hasBallInHand = true;
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