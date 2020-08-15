using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    List<Vector2> recordedPositions;

    Vector2 direction;

    void Start()
    {
        direction = Vector2.zero;
        recordedPositions = new List<Vector2>();
        recordedPositions.Add(transform.position);
    }

    private void Update()
    {
        direction.x = Input.GetAxis("Horizontal");
        direction.y = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        transform.Translate(direction * speed * Time.fixedDeltaTime);
        recordedPositions.Add(transform.position);
    }

    public List<Vector2> GetPositionsReference()
    {
        var records = recordedPositions;
        recordedPositions = new List<Vector2>();
        return records;
    }
}
