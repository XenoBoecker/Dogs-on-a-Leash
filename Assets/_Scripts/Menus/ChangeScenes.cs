using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{

    [SerializeField] Animator transitionAnimator;

    [SerializeField] float transitionTime = 1;

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));

    }

    IEnumerator LoadSceneRoutine(string sceneName)
    {
        transitionAnimator.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        if (PhotonNetwork.IsConnected) PhotonNetwork.LoadLevel(sceneName);
        else SceneManager.LoadScene(sceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
