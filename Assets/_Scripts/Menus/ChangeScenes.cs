using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{
    [SerializeField] private List<Animator> transitionAnimators = new List<Animator>();
    
    [SerializeField] private bool fadeInActive = true;


    [SerializeField] Animator currentAnimator; // Tracks the current animator
    static int currentAnimatorIndex;

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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            EndAnim();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            StartAnim();
        }
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
        if (currentAnimator != null)
        {
            Debug.Log("StartAnim " + currentAnimator.name);
            StartAnim();
        }

        yield return new WaitForSeconds(currentAnimator.runtimeAnimatorController.animationClips[0].length);

        // Load the scene using Photon or SceneManager
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.LoadLevel(sceneName);
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
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
