using System;
using UnityEngine;

public class DuplicateController : MonoBehaviour
{
    Rigidbody2D m_body;
    Reader m_reader;

    float m_horizontal;
    float m_vertical;

    public float RunSpeed = 20.0f;

    void Start ()
    {
        m_body = GetComponent<Rigidbody2D>();
        m_reader = GetComponent<Reader>();
    }

    void Update()
    {
        m_horizontal = Convert.ToInt32(m_reader.GetKey(KeyCode.D)) - Convert.ToInt32(m_reader.GetKey(KeyCode.A));
        m_vertical = Convert.ToInt32(m_reader.GetKey(KeyCode.W)) - Convert.ToInt32(m_reader.GetKey(KeyCode.S));
    }

    void FixedUpdate()
    {
        m_body.velocity = new Vector2(m_horizontal * RunSpeed * Time.deltaTime, m_vertical * RunSpeed * Time.deltaTime);
    }
}