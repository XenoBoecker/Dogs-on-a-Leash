using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAchievementUnlockScreen : MonoBehaviour
{
    [SerializeField] private Transform achievementShowParent;
    [SerializeField] private AchievementShow achievementShowPrefab;

    [SerializeField] private Tooltip tooltip;

    // Start is called before the first frame update
    void Start()
    {
        List<int> unlockedAchievements = AchievementManager.Instance.GetUnlockedHatIndices();

        for (int i = 0; i < AchievementManager.Instance.AchievementCount; i++)
        {
            AchievementShow achievementShow = Instantiate(achievementShowPrefab, achievementShowParent);

            achievementShow.SetAchievement(Achievements.AllAchievements[i]);
            achievementShow.SetTooltip(tooltip);
        }
    }
}
