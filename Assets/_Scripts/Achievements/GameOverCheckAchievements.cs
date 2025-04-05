using System.Collections.Generic;
using UnityEngine;

public class GameOverCheckAchievements : MonoBehaviour // in in game over scene
{
    GameOver gameOverManager;

    // Start is called before the first frame update
    void Start()
    {
        gameOverManager = FindAnyObjectByType<GameOver>();
        gameOverManager.OnFinalScoreCalculated += OnFinalScoreCalculated;

        if(PlayerPrefs.GetInt("TimeLeft") <= 5)
        {
            Achievements.Unlock(Achievements.TimeLeftLessThan5);
        }

        if(PlayerPrefs.GetInt("BumpedCount") == 0)
        {
            Achievements.Unlock(Achievements.NoBumps);
        }

        if (PlayerPrefs.GetInt("PickupCount") == 0)
        {
            Achievements.Unlock(Achievements.NoPickups);
        }

        if (PlayerPrefs.GetInt("BarkCount") >= 50)
        {
            Achievements.Unlock(Achievements.Bark50);
        }
        if(PlayerPrefs.GetInt("TotalCollectedPoints") >= 5000 && PlayerPrefs.GetInt("ThisRoundsHighScore") <= 1000)
        {
            Achievements.Unlock(Achievements.Total5kPoints1kMax);
        }
        if(PlayerPrefs.GetInt("AllTime_DigCount") >= 100)
        {
            Achievements.Unlock(Achievements.AllTime100Dig);
        }

        if (PlayerPrefs.GetInt("AllTime_BumpedCount") >= 50)
        {
                Achievements.Unlock(Achievements.AllTime50Bumps);
        }

        if (PlayerPrefs.GetInt("AllTime_BarkCount") >= 1000)
        {
            Achievements.Unlock(Achievements.AllTime1000Bark);
        }

        if (PlayerPrefs.GetInt("AllTime_BarkAtDuckCount") >= 50)
        {
            Achievements.Unlock(Achievements.AllTime50BarkAtDuck);
        }

        if (PlayerPrefs.GetInt("AllTime_GamesWon") >= 30)
        {
            Achievements.Unlock(Achievements.AllTime30GamesWon);
        }

        CheckAllSameOrAllDifferentDogs();
    }

    private void CheckAllSameOrAllDifferentDogs()
    {
        List<int> dogIDs = new List<int>();

        for (int i = 0; i < 4; i++)
        {
            dogIDs.Add(PlayerPrefs.GetInt("Dog_" + i + "_ID"));
        }

        if(dogIDs[0] == dogIDs[1] && dogIDs[0] == dogIDs[2] && dogIDs[0] == dogIDs[3])
        {
            Achievements.Unlock(Achievements.AllSameDog);
        }
        else if (dogIDs[0] != dogIDs[1] && dogIDs[0] != dogIDs[2] && dogIDs[0] != dogIDs[3]
                && dogIDs[1] != dogIDs[2] && dogIDs[1] != dogIDs[3]
                && dogIDs[2] != dogIDs[3])
        {
            Achievements.Unlock(Achievements.AllDifferentDogs);
        }
    }

    private void OnFinalScoreCalculated(int finalScore)
    {
        if(PlayerPrefs.GetInt("LastGameFinalScore") == finalScore)
        {
            Achievements.Unlock(Achievements.SameScoreTwice);
        }

        PlayerPrefs.SetInt("LastGameFinalScore", finalScore);

        if (finalScore > 10000)
        {
            Achievements.Unlock(Achievements.Highscore10k);
        }
    }
}

/*
- Bark at Ducks 50 times
Menu achievements
- Watch our credits till the end


*/