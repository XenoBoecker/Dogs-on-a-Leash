using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TutorialSlides : MonoBehaviour
{
    [SerializeField] private GameObject[] allSlides;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject selectWhenTutorialCompleted;

    [SerializeField] private Sprite normalNextButtonSprite;
    [SerializeField] private Sprite finalSlideNextButtonSprite;

    private int currentSlideIndex = 0;


    public event Action OnTutorialCompleted;

    // Start is called before the first frame update
    void Start()
    {
        HideAllSlides();
    }

    public void ShowNextSlide()
    {
        currentSlideIndex++;
        if (currentSlideIndex >= allSlides.Length)
        {
            HideAllSlides();
            currentSlideIndex = 0;

            EventSystem.current.SetSelectedGameObject(selectWhenTutorialCompleted);

            OnTutorialCompleted?.Invoke();
            return;
        }
        else
        {
            ShowSlide(currentSlideIndex);
        }
    }

    public void ShowSlide(int index)
    {
        GetComponent<Canvas>().enabled = true;
        for (int i = 0; i < allSlides.Length; i++)
        {
            allSlides[i].SetActive(i == index);
        }

        if(index == allSlides.Length - 1)
        {
            nextButton.GetComponent<Image>().sprite = finalSlideNextButtonSprite;
        }
        else
        {
            nextButton.GetComponent<Image>().sprite = normalNextButtonSprite;
        }

        EventSystem.current.SetSelectedGameObject(nextButton);
    }

    private void HideAllSlides()
    {
        GetComponent<Canvas>().enabled = false;
    }
}
