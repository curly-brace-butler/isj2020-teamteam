using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuplicateAI : MonoBehaviour
{
    List<Vector2> recordedPositions;

    int positionIndex = 0;

    private void FixedUpdate()
    {
        transform.position = recordedPositions[positionIndex];
        positionIndex = (positionIndex + 1) % recordedPositions.Count;
    }

    public void Restore()
    {
        positionIndex = 0;
        transform.position = recordedPositions[0];
    }

    public void SetPositionsReference(List<Vector2> references)
    {
        recordedPositions = references;
    }
}
