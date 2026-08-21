using UnityEngine;

public class UIAchievementUnlockScreen : MonoBehaviour
{
    [SerializeField] private Transform achievementShowParent;
    [SerializeField] private AchievementShow achievementShowPrefab;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < AchievementManager.Instance.AchievementCount; i++)
        {
            AchievementShow achievementShow = Instantiate(achievementShowPrefab, achievementShowParent);

            achievementShow.SetAchievement(Achievements.AllAchievements[i]);
        }
    }
}
