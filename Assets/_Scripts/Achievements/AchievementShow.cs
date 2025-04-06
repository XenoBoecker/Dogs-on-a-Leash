using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AchievementShow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] private GameObject showLocked, showUnlocked;
    [SerializeField] private Image bgSelectedImage, hatImage;

    private Tooltip tooltip;

    Achievements.Achievement achievement;

    private void Awake()
    {
        showLocked.SetActive(false);
        showUnlocked.SetActive(false);
        bgSelectedImage.color = new Color(1, 1, 1, 0);
    }

    internal void SetAchievement(Achievements.Achievement achievement)
    {
        SetUnlocked(achievement.IsUnlocked);

        this.achievement = achievement;

        if (AchievementManager.Instance.HatSprites.Length > achievement.mySpriteIndex)
        {
            hatImage.sprite = AchievementManager.Instance.HatSprites[achievement.mySpriteIndex];
            if (hatImage.sprite == null) hatImage.color = new Color(1, 1, 1, 0);
        }
    }

    public void SetTooltip(Tooltip tooltip)
    {
        this.tooltip = tooltip;
    }

    public void SetUnlocked(bool isUnlocked)
    {
        showLocked.SetActive(!isUnlocked);
        showUnlocked.SetActive(isUnlocked);
    }

    public void ShowHoverInfo(bool show)
    {
        if (show)
        {
            bgSelectedImage.color = new Color(1,1,1,1);
            tooltip.Show(transform.position, achievement.Name + "\n" + achievement.Description);
        }
        else
        {
            bgSelectedImage.color = new Color(1, 1, 1, 0);
            tooltip.Hide();
        }
    }



    // Called when mouse enters the UI element
    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowHoverInfo(true);
    }

    // Called when mouse exits the UI element
    public void OnPointerExit(PointerEventData eventData)
    {
        ShowHoverInfo(false);
    }

    // Called when the UI element is selected (e.g., via controller or keyboard)
    public void OnSelect(BaseEventData eventData)
    {
        ShowHoverInfo(true);
    }

    // Called when the UI element is deselected
    public void OnDeselect(BaseEventData eventData)
    {
        ShowHoverInfo(false);
    }
}
