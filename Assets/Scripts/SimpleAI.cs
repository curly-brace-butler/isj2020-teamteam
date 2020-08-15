using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAI : MonoBehaviour
{
    public float speed = 5f;
    public Transform[] waypoints = new Transform[2];

    int waypointIndex = 0;

    private void FixedUpdate()
    {
        float movementMagnitude = speed * Time.deltaTime;

        Vector3 distance = waypoints[waypointIndex].position - transform.position;
        float distanceMagnitude = distance.magnitude;

        if (distanceMagnitude <= movementMagnitude)
        {
            transform.position = waypoints[waypointIndex].position;
            waypointIndex = (waypointIndex + 1) % waypoints.Length;

            movementMagnitude -= distanceMagnitude;
        }

        transform.Translate(distance.normalized * movementMagnitude);
    }
}
