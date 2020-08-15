using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D m_body;
    Reader m_reader;

    float m_horizontal;
    float m_vertical;

    public float RunSpeed = 20.0f;
    
    void Start ()
    {
        m_body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        m_horizontal = Input.GetAxisRaw("Horizontal");
        m_vertical = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        m_body.velocity = new Vector2(m_horizontal * RunSpeed * Time.deltaTime, m_vertical * RunSpeed * Time.deltaTime);
    }
}