using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private GameObject tooltipObject;
    [SerializeField] private TMP_Text tooltipText;

    private void Awake()
    {
        tooltipObject.SetActive(false);
    }

    internal void Hide()
    {
        tooltipObject.SetActive(false);
    }

    internal void Show(Vector3 position, string description)
    {
        tooltipObject.SetActive(true);
        tooltipObject.transform.position = position;
        tooltipText.text = description;
    }
}