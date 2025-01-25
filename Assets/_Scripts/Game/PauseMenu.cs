using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;

    [SerializeField] GameObject pausePanel;

    bool isPaused;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        pausePanel.SetActive(false);
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;

        isPaused = true;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;

        isPaused = false;
    }

    internal void TogglePauseGame()
    {
        if (isPaused) ResumeGame();
        else PauseGame();
    }
}
