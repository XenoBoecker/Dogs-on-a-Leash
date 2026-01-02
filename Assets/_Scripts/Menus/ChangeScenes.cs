using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{
    [SerializeField] private List<Animator> transitionAnimators = new List<Animator>();
    
    [SerializeField] private bool fadeInActive = true;


    [SerializeField] Animator currentAnimator; // Tracks the current animator

    public static ChangeScenes Instance;

    static int currentAnimatorIndex;

    bool isChangingScene;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentAnimator = transitionAnimators[currentAnimatorIndex];

        if (fadeInActive && currentAnimator != null)
        {
            EndAnim();
        }

        // Subscribe to the sceneLoaded event to handle behavior after loading a scene
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the sceneLoaded event to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (fadeInActive && currentAnimator != null)
        {
            EndAnim();
        }
    }

    public void LoadScene(string sceneName)
    {
        Debug.Log("Load Scene: " + sceneName);

        // Parse the animator index from the scene name (e.g., "SceneName_0" for index 0)
        string[] sceneParts = sceneName.Split('_');
        string baseSceneName = sceneParts[0];
        currentAnimatorIndex = 0;

        if (sceneParts.Length > 1 && int.TryParse(sceneParts[1], out int parsedIndex))
        {
            currentAnimatorIndex = Mathf.Clamp(parsedIndex, 0, transitionAnimators.Count - 1);
        }

        // Assign the appropriate animator
        currentAnimator = transitionAnimators[currentAnimatorIndex];

        // Start the transition coroutine
        StartCoroutine(LoadSceneRoutine(baseSceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        if (isChangingScene) yield break;

        isChangingScene = true;
        if (currentAnimator != null)
        {
            Debug.Log("StartAnim " + currentAnimator.name);
            StartAnim();
        }
        Debug.Log("Wait for animation: " + currentAnimator.runtimeAnimatorController.animationClips[0].length);
        yield return new WaitForSeconds(currentAnimator.runtimeAnimatorController.animationClips[0].length);

        isChangingScene = false;

        Debug.Log("Now Load Scene");
        SceneManager.LoadScene(sceneName);
    }

    void EndAnim()
    {
        currentAnimator.SetTrigger("End");
    }
    void StartAnim()
    {
        currentAnimator.SetTrigger("Start");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
