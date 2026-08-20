using System;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    [SerializeField] private int startUnlockedHatCount;
    [SerializeField] private Sprite[] hatSprites;
    public Sprite[] HatSprites => hatSprites;
    public int AchievementCount => Achievements.AllAchievements.Length;

    public static AchievementManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.U)) // Hacks
        {
            LockAllAchievements();
        }

        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.L)) // Hacks
        {
            UnlockAllAchievements();
        }
    }

    private void LockAllAchievements() // Hacks
    {
        for (int i = 0; i < Achievements.AllAchievements.Length; i++)
        {
            PlayerPrefs.SetInt(Achievements.AllAchievements[i].ID, 0);
        }
    }

    private void UnlockAllAchievements()
    {
        for (int i = 0; i < Achievements.AllAchievements.Length; i++)
        {
            PlayerPrefs.SetInt(Achievements.AllAchievements[i].ID, 1);
        }
    }

    public List<int> GetUnlockedHatIndices()
    {
        List<int> unlockedHatIndices = new List<int>();

        for (int i = 0; i < startUnlockedHatCount; i++)
        {
            unlockedHatIndices.Add(i);
        }

        for (int i = 0; i < Achievements.AllAchievements.Length; i++)
        {
            if (PlayerPrefs.GetInt(Achievements.AllAchievements[i].ID) == 1)
            {
                unlockedHatIndices.Add(i + startUnlockedHatCount);
            }
        }

        return unlockedHatIndices;
    }
}

/*
 end game achievements
- get X (very high) amount of points in one round --
- Win a round with less than 5 seconds left--
- Get the exact same amount of points 2 games in a row (reset on lose)--
- Win a round where all 4 players play as the same dog--
- Play a round as 4 different Dogs--
- Complete a run without letting the human bump into anything--
- finish a round without picking up any pickups--
- Bark 50 times in one round--
- collect 5.000 points in total without ever having more than 1.000 points--

collective achievements
- Let the human bump into things 50 times--
- Dig 100 times--
- Bark 1000 times--
- Win 30 Rounds--
- Bark at Ducks 50 times

in game achievements



combined achievements


Menu achievements
- Watch our credits till the end


*/