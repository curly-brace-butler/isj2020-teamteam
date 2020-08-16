using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Recorder))]
public class GameManager : MonoBehaviour
{
    public PlayerController player;
    public DuplicateController duplicatePrefabs;
    public Text scoreText;
    public Text roundText;

    [Header("Round")]
    public float roundTimeBreak = 5f;

    [Header("Event")]
    public GameEvent onRoundEnd;

    int score;
    int round;
    int aliveDuplicate;

    Recorder recorder;
    List<DuplicateController> duplicates;

    bool playerIsDead = false;
    Vector3 initialPlayerPositionLastRound;

    Coroutine updatePlayerPosition;

    private void Awake()
    {
        duplicates = new List<DuplicateController>();
        recorder = GetComponent<Recorder>();
        aliveDuplicate = 1;
        round = 1;
    }

    private void Start()
    {
        updatePlayerPosition = StartCoroutine(UpdatePlayerInitialPosition());
    }

    private void Update()
    {
        if (playerIsDead && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }

    public void PlayerHasBeenKilled()
    {
        StopCoroutine(updatePlayerPosition);

        playerIsDead = true;
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
        score += round * 1;

        scoreText.text = $"Score {score}";

        if (--aliveDuplicate == 0)
        {
            StartCoroutine(AnimationNewRound());
        }
    }

    IEnumerator AnimationNewRound()
    {
        onRoundEnd.Raise();

        roundText.text = round.ToString();

        yield return new WaitForSeconds(roundTimeBreak);

        NewRound();
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
