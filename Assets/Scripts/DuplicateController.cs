using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Reader))]
public class DuplicateController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    public float timeRespawn = 5f;

    [Header("Shooting")]
    public float ballSpeed = 8f;
    public BounceBall ballPrefabs;

    [Header("Event")]
    public GameEvent OnDuplicateDeath;

    Reader reader;
    PlayerInput remainInput;

    Vector2 initialPosition;
    bool hasShoot = false;
    Coroutine respawn;

    private void Awake()
    {
        initialPosition = transform.position;
        reader = GetComponent<Reader>();

        respawn = StartCoroutine(RespawnAfterTime());
    }

    public void Restore()
    {
        reader.Restart();
        transform.position = initialPosition;
        gameObject.SetActive(true);

        respawn = StartCoroutine(RespawnAfterTime());
        hasShoot = false;
    }

    public void Kill()
    {
        gameObject.SetActive(false);
        OnDuplicateDeath?.Raise();

        StopCoroutine(respawn);
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
        }

        remainInput.deltaTime -= deltaTime;
    }

    private PlayerInput GetNextFrame()
    {
        if (!reader.HasNext())
        {
            if (!hasShoot)
            {
                Vector2 direction;
                direction.x = UnityEngine.Random.Range(0f, 1f);
                direction.y = UnityEngine.Random.Range(0f, 1f);
                Shoot(direction.normalized, ballSpeed);
            }

            return default;
        }

        return reader.GetFrame();
    }

    private void Shoot(Vector2 direction, float speed)
    {
        var ball = Instantiate(ballPrefabs, transform.position, Quaternion.identity);
        ball.Throw(direction, speed);

        hasShoot = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == Constant.PlayerBallTag)
        {
            Kill();
        }
    }

    private IEnumerator RespawnAfterTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeRespawn);

            transform.position = initialPosition;
            hasShoot = false;

            reader.Restart();
        }
    }
}