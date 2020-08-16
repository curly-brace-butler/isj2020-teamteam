using System;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Reader))]
public class DuplicateController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;

    [Header("Shooting")]
    public float ballSpeed = 8f;
    public BounceBall ballPrefabs;

    [Header("Event")]
    public GameEvent OnDuplicateDeath;

    Reader reader;
    PlayerInput remainInput;

    Vector2 initialPosition;
    bool hasShoot = false;

    private void Awake()
    {
        initialPosition = transform.position;
        reader = GetComponent<Reader>();
    }

    public void Restore()
    {
        transform.position = initialPosition;
        gameObject.SetActive(true);
    }

    public void Kill()
    {
        gameObject.SetActive(false);
        OnDuplicateDeath?.Raise();
    }

    private void Update()
    {
        float remainDeltaTime = Time.deltaTime;
        do
        {
            remainInput = GetNextFrame();
            remainDeltaTime = Mathf.Min(remainDeltaTime, remainInput.deltaTime);

            ApplyInputAction(remainDeltaTime);
            remainDeltaTime -= remainDeltaTime;
        } 
        while (remainDeltaTime != 0);
    }

    private void ApplyInputAction(float deltaTime)
    {
        transform.Translate(remainInput.movement * deltaTime);

        if (remainInput.shoot != Vector2.zero)
        {
            Shoot(remainInput.shoot.normalized, remainInput.shoot.magnitude);
            hasShoot = true;
        }

        remainInput.deltaTime -= deltaTime;
    }

    private PlayerInput GetNextFrame()
    {
        if (!reader.HasNext())
        {
            reader.Restart();
            transform.position = initialPosition;

            if (!hasShoot)
            {
                Vector2 direction;
                direction.x = UnityEngine.Random.Range(0f, 1f);
                direction.y = UnityEngine.Random.Range(0f, 1f);
                Shoot(direction.normalized, ballSpeed);
            }

            hasShoot = false;
        }

        return reader.GetFrame();
    }

    private void Shoot(Vector2 direction, float speed)
    {
        var ball = Instantiate(ballPrefabs, transform.position, Quaternion.identity);
        ball.Throw(direction, speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == Constant.PlayerBallTag)
        {
            Kill();
        }
    }
}