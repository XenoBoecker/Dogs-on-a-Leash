using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    [SerializeField] string menuSceneName;

    public static PauseMenu Instance;

    bool isPaused;

    bool canPause => Time.timeScale > 0;

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
        if (!canPause) return;

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

    public void ReturnToMenu()
    {
        Time.timeScale = 1;
        ChangeScenes.Instance.LoadScene(menuSceneName);
    }
}
