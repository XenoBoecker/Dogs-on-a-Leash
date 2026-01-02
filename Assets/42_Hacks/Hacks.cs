using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hacks : MonoBehaviour
{
    HumanMovement human;
    ScoreManager scoreManager;
    CameraMovement camMov;


    [SerializeField] int endGameWellScore = 7500, endGameWellTimeLeft = 7;


    [SerializeField] float timeScale = 1;

    [SerializeField] bool controlTime;

    // Start is called before the first frame update
    void Start()
    {
        human = FindObjectOfType<HumanMovement>();
        scoreManager = FindObjectOfType<ScoreManager>();
        camMov = FindObjectOfType<CameraMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.O)) scoreManager.HackSetTimeLeft(0); // lose game
        // if (Input.GetKeyDown(KeyCode.P)) scoreManager.AddScore(Random.Range(1, 20) * 100);
        // if (Input.GetKeyDown(KeyCode.I)) scoreManager.AddScore(-Random.Range(1, 20) * 100);

        // if (Input.GetKeyDown(KeyCode.U)) camMov.ScreenShake(null);

        if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.LeftShift))
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.R)) FindObjectOfType<ChangeScenes>().LoadScene("Game_1");
        if (Input.GetKeyDown(KeyCode.K)) camMov.HackSkipFlythrough();
        //Add time
        if(Input.GetKeyDown(KeyCode.T))
        {
            scoreManager.HackSetTimeLeft(scoreManager.TimeLeft + 10);
        }
        if (Input.GetKeyDown(KeyCode.L)) EndGameWell();




        // if (controlTime) Time.timeScale = timeScale;
    }

    private void EndGameWell()
    {
        scoreManager.AddScore(endGameWellScore);
        scoreManager.HackSetTimeLeft(endGameWellTimeLeft);
        scoreManager.HackEndGame();
    }
}
