using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    public bool usePauser = true;

    // Game Manager, Player, Enemy
    [Header("Pause will disable the following:")]
    public GameObject[] unpauseActors;

    // Demo object & its children
    [Header("Pause will enable the following:")]
    public GameObject[] pauseActors;

    // Start is called before the first frame update
    public void Start()
    {
        if (!usePauser)
        {
            DeactivatePauseActors();
        }
        else
        {
            this.Pause();
        }
    }

    public void Pause()    
    {
        foreach (GameObject o in unpauseActors)
        {
            o.SetActive(false);
        }
        foreach (GameObject o in pauseActors)
        {
            o.SetActive(true);
        }
    }

    public void Unpause()
    {
        Debug.Log("Pauser tries to enable.");
        DeactivatePauseActors();
        foreach (GameObject o in unpauseActors)
        {
            o.SetActive(true);
        }
    }

    private void DeactivatePauseActors()
    {
        foreach (GameObject o in pauseActors)
        {
            o.SetActive(false);
        }
    }
}
