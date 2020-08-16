using System;
using UnityEngine;

[RequireComponent(typeof(Reader))]
public class DuplicateController : MonoBehaviour
{
    public float speed = 5f;
    public GameEvent OnDuplicateDeath;

    PlayerInput remainInput;

    Reader reader;

    Vector2 initialPosition;

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
            Debug.Log("Shoot");
        }

        remainInput.deltaTime -= deltaTime;
    }

    private PlayerInput GetNextFrame()
    {
        if (!reader.HasNext())
        {
            reader.Restart();
            transform.position = initialPosition;
        }

        return reader.GetFrame();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == Constant.PlayerBallTag)
        {
            Kill();
        }
    }
}