using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class AchievementShow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] private GameObject showLocked, showUnlocked, hoverInfo;
    [SerializeField] private TMP_Text hoverInfoText;

    private void Awake()
    {
        showLocked.SetActive(false);
        showUnlocked.SetActive(false);
        hoverInfo.SetActive(false);
    }

    internal void SetAchievement(Achievements.Achievement achievement)
    {
        SetUnlocked(achievement.IsUnlocked);

        hoverInfoText.text = achievement.Description;
    }

    public void SetUnlocked(bool isUnlocked)
    {
        showLocked.SetActive(!isUnlocked);
        showUnlocked.SetActive(isUnlocked);
    }

    public void ShowHoverInfo(bool show)
    {
        hoverInfo.SetActive(show);

        if (show)
        {
            hoverInfoText.transform.SetParent(transform.parent);
            hoverInfo.transform.SetAsLastSibling();
        }
        else
        {
            hoverInfo.transform.SetParent(transform);
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