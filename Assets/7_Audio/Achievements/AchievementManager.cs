using System.Collections.Generic;
using System.Linq;
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


    public static IEnumerable<string> GetAll()
    {
        return typeof(Achievements)
            .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
            .Where(f => f.FieldType == typeof(string))
            .Select(f => f.GetValue(null) as string);
    }
}

public class AchievementManager : MonoBehaviour
{
    [SerializeField] private int startUnlockedHatCount;

    private List<string> sortedAchievementIDs;
    public int AchievementCount => sortedAchievementIDs.Count;

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

        sortedAchievementIDs = Achievements.GetAll().ToList();
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
        for (int i = 0; i < sortedAchievementIDs.Count; i++)
        {
            PlayerPrefs.SetInt(sortedAchievementIDs[i], 0);
        }
    }

    private void UnlockAllAchievements()
    {
        for (int i = 0; i < sortedAchievementIDs.Count; i++)
        {
            PlayerPrefs.SetInt(sortedAchievementIDs[i], 1);
        }
    }

    public List<int> GetUnlockedHatIndices()
    {
        List<int> unlockedHatIndices = new List<int>();

        for (int i = 0; i < startUnlockedHatCount; i++)
        {
            unlockedHatIndices.Add(i);
        }

        for (int i = 0; i < sortedAchievementIDs.Count; i++)
        {
            if (PlayerPrefs.GetInt(sortedAchievementIDs[i]) == 1)
            {
                unlockedHatIndices.Add(i + startUnlockedHatCount);
            }
        }

        return unlockedHatIndices;
    }
    public void UnlockAchievement(string achievementID)
    {
        if (!sortedAchievementIDs.Contains(achievementID))
        {
            Debug.LogWarning($"Achievement ID {achievementID} not found in sortedAchievementIDs list.");
        }
        if (PlayerPrefs.GetInt(achievementID) == 1) return;

        Debug.Log("Unlock achievement " + achievementID);

        PlayerPrefs.SetInt(achievementID, 1);

        // steam achievements
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