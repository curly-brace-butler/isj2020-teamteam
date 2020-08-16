using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Recorder))]
public class GameManager : MonoBehaviour
{
    public PlayerController player;
    public DuplicateController duplicatePrefabs;

    int round;
    int aliveDuplicate;

    Recorder recorder;

    List<DuplicateController> duplicates;

    Vector3 initialPlayerPositionLastRound;

    private void Awake()
    {
        duplicates = new List<DuplicateController>();
        recorder = GetComponent<Recorder>();
        aliveDuplicate = 1;
        round = 0;
    }

    private void Start()
    {
        StartCoroutine(UpdatePlayerInitialPosition());
    }

    public void SpawnDuplicate(Vector3 initialPlayerPosition)
    {
        var tape = recorder.StopRecording();

        DuplicateController duplicate = Instantiate(duplicatePrefabs, initialPlayerPosition, Quaternion.identity);
        duplicate.GetComponent<Reader>().Setup(tape);

        duplicates.Add(duplicate);
    }

    public void NewRound()
    {
        SpawnDuplicate(initialPlayerPositionLastRound);

        initialPlayerPositionLastRound = player.transform.position;

        aliveDuplicate = duplicates.Count;
        round++;

        foreach (var duplic in duplicates)
        {
            duplic.Restore();
        }

        recorder.StartRecording();
    }

    public void DuplicateHasBeenKilled()
    {
        if (--aliveDuplicate == 0)
        {
            NewRound();
        }
    }

    IEnumerator UpdatePlayerInitialPosition()
    {
        initialPlayerPositionLastRound = player.transform.position;

        yield return new WaitForSeconds(recorder.maxRecordTime);

        while (true)
        {
            initialPlayerPositionLastRound = player.transform.position;

            yield return new WaitForEndOfFrame();
        }
    }
}
