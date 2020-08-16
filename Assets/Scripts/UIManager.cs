using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GameObject[] StartObjects;
    private GameObject[] PauseObjects;
    private GameObject[] EndObjects;
    
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    void Start()
    {
        Time.timeScale = 1;
        StartObjects = GameObject.FindGameObjectsWithTag("ShowOnStart");
        // PauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        EndObjects = GameObject.FindGameObjectsWithTag("ShowOnEnd");
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        hideGameOver();
        showStart();
        Debug.Log("Show Start");
    }



    public void showStart()
    {
        foreach (GameObject g in StartObjects)
        {
            g.SetActive(true);
        }
    }

    public void hideStart()
    {
        foreach (GameObject g in StartObjects)
        {
            g.SetActive(false);
        }
    }
    
    public void ShowGameOver()
    {
        foreach (GameObject g in EndObjects)
        {
            g.SetActive(true);
            
            Debug.Log("Activate: " + g);
        }
    }
    
    public void hideGameOver()
    {
        foreach (GameObject g in EndObjects)
        {
            g.SetActive(false);
            Debug.Log("Hide: " + g);
        }
    }
}
