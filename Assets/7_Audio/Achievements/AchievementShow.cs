using UnityEngine;

public class AchievementShow : MonoBehaviour
{
    [SerializeField] private GameObject showLocked, showUnlocked, hoverInfo;

    private void Awake()
    {
        showLocked.SetActive(false);
        showUnlocked.SetActive(false);
        hoverInfo.SetActive(false);
    }
    public void SetLocked(bool isLocked)
    {
        showLocked.SetActive(isLocked);
        showUnlocked.SetActive(!isLocked);
    }

    public void ShowHoverInfo(bool show)
    {
        hoverInfo.SetActive(show);
    }
}