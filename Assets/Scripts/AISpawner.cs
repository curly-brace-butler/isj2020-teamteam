using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour
{
    public PlayerMovement player;
    public GameObject duplicatePrefabs;

    List<DuplicateAI> duplicates;

    private void Awake()
    {
        duplicates = new List<DuplicateAI>();
    }

    public void SpawnDuplicate()
    {
        List<Vector2> records = player.GetPositionsReference();
        FlipRecordX(records);

        var duplicate = Instantiate(duplicatePrefabs, records[0], Quaternion.identity);
        var duplicateAI = duplicate.GetComponent<DuplicateAI>();
        duplicateAI.SetPositionsReference(records);

        duplicates.Add(duplicateAI);
    }

    private void FlipRecordX(List<Vector2> records)
    {
        for (int i = 0; i < records.Count; i++)
        {
            records[i] = new Vector2(-records[i].x, records[i].y);
        }
    }
}
