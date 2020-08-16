using System;
using UnityEngine;

[RequireComponent(typeof(Reader))]
public class DuplicateController : MonoBehaviour
{
    public float speed = 5f;

    public GameEvent OnDuplicateDeath;

    Vector2 input;
    Vector2 direction;
    Reader reader;

    Vector2 intialPosition;

    private void Awake()
    {
        intialPosition = transform.position;

        input = Vector2.zero;
        direction = Vector2.zero;
        reader = GetComponent<Reader>();

        reader.OnTapeFinish += Restore;
    }

    public void Restore()
    {
        transform.position = intialPosition;
        UpdateInputs();

        direction = input.normalized;

        reader.Restore();

        gameObject.SetActive(true);
    }

    public void Kill()
    {
        gameObject.SetActive(false);
        OnDuplicateDeath?.Raise();
    }

    private void UpdateInputs()
    {
        input.x = reader.GetAxisRaw(Reader.Axis.Horizontal);
        input.y = reader.GetAxisRaw(Reader.Axis.Vertical);
    }

    private void Update()
    {
        UpdateInputs();
    }

    private void FixedUpdate()
    {
        direction = input.normalized;

        transform.Translate(direction * speed * Time.fixedDeltaTime);
    }
}