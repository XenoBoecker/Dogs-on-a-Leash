using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleDogHatPanel : MonoBehaviour
{
    [SerializeField] private GameObject hatPanel;
    [SerializeField] private GameObject dogPanel;

    [SerializeField] private GameObject dogPanelOpenStartSelectedObject, hatPanelOpenStartSelectedObject;

    public void OpenDogPanel()
    {
        hatPanel.SetActive(false);
        dogPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(dogPanelOpenStartSelectedObject);
    }

    public void OpenHatPanel()
    {
        hatPanel.SetActive(true);
        dogPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(hatPanelOpenStartSelectedObject);
    }
}