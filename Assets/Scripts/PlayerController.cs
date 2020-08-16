﻿using UnityEngine;

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

    bool hasBallExited = false;

    private void Awake()
    {
        direction = Vector2.zero;
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
        UpdateMouse();

        if (Input.GetMouseButtonDown(0) && playerBall != null)
        {
            playerBall.Throw(mouseDirection, ballSpeed);
            playerBall = null;

            hasBallExited = false;
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