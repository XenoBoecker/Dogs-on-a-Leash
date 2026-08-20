using System;
using System.Collections.Generic;
using UnityEngine;


public static class Achievements
{
    public static readonly string TimeLeftLessThan5 = "a_timeLeft<=5s";
    public static readonly string NoBumps = "a_noBumps";
    public static readonly string NoPickups = "a_noPickups";
    public static readonly string Bark50 = "a_bark50";
    public static readonly string Total5kPoints1kMax = "a_5000PointsWithoutEverMoreThan1000";

    public static readonly string AllTime100Dig = "a_allTime100Dig";
    public static readonly string AllTime50Bumps = "a_allTime50Bumps";
    public static readonly string AllTime1000Bark = "a_allTime1000Bark";
    public static readonly string AllTime50BarkAtDuck = "a_allTime50BarkAtDuck";
    public static readonly string AllTime30GamesWon = "a_allTime30GamesWon";

    public static readonly string AllSameDog = "a_allSameDog";
    public static readonly string AllDifferentDogs = "a_allDifferentDogs";

    public static readonly string SameScoreTwice = "a_sameScoreTwice";
    public static readonly string Highscore10k = "a_highscore10k";

    public static readonly Achievement[] AllAchievements = new Achievement[]
    {
        new Achievement(TimeLeftLessThan5, "Close One", "arrive at the bus with less than 5 seconds time left"),
        new Achievement(NoBumps, "Guide Dog", "complete a run without letting the human bump into anything"),
        new Achievement(NoPickups, "Clean Paws", "complete a run without picking up any bones or digging any holes"),
        new Achievement(Bark50, "Bark", "bark 50 times in one round"),
        new Achievement(Total5kPoints1kMax, "Lots of Action", "collect 5.000 points in total without ever having more than 1.000 points"),
        new Achievement(AllTime100Dig, "Digging Master", "dig 100 times in total"),
        new Achievement(AllTime50Bumps, "Ouch!", "let the human bump into things 50 times in total"),
        new Achievement(AllTime1000Bark, "Bark Bark", "bark 1000 times in total"),
        new Achievement(AllTime50BarkAtDuck, "DUCK!", "bark at ducks 50 times in total"),
        new Achievement(AllTime30GamesWon, "Winner!", "win 30 rounds in total"),
        new Achievement(AllSameDog, "Clone Dogs", "play a round where all 4 players play as the same dog"),
        new Achievement(AllDifferentDogs, "Diversity is key", "play a round as 4 different dogs"),
        new Achievement(SameScoreTwice, "Deja Vu", "get the exact same amount of points 2 games in a row (reset on lose)"),
        new Achievement(Highscore10k, "Highscore!", "get a highscore of 10.000 points in one round")
    };

    public struct Achievement
    {
        public string ID;
        public string Name;
        public string Description;
        public bool IsUnlocked;
        public Achievement(string id, string name, string description)
        {
            ID = id;
            Name = name;
            Description = description;
            IsUnlocked = PlayerPrefs.GetInt(id) == 1;
        }
    }

    internal static bool AchievementExists(string achievementID)
    {
        for (int i = 0; i < AllAchievements.Length; i++)
        {
            if (AllAchievements[i].ID == achievementID) return true;
        }
        return false;
    }

    internal static void Unlock(string achievementID)
    {
        if (!AchievementExists(achievementID))
        {
            Debug.LogWarning($"Achievement ID {achievementID} not found in sortedAchievementIDs list.");
        }
        if (PlayerPrefs.GetInt(achievementID) == 1)
        {
            return;
        }

        Debug.Log("Unlock achievement " + achievementID);

        PlayerPrefs.SetInt(achievementID, 1);

        for (int i = 0; i < AllAchievements.Length; i++)
        {
            if (AllAchievements[i].ID == achievementID)
            {
                AllAchievements[i].IsUnlocked = true;
            }
        }
    }
}

public class AchievementManager : MonoBehaviour
{
    [SerializeField] private int startUnlockedHatCount;
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