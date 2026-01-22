using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    [SerializeField] string menuSceneName;

    [SerializeField] private GameObject continueButton;

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

        EventSystem.current.SetSelectedGameObject(continueButton);
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
