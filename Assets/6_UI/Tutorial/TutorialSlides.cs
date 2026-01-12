using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialSlides : MonoBehaviour
{
    [SerializeField] private GameObject[] allSlides;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject selectWhenTutorialCompleted;

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

            if(selectWhenTutorialCompleted != null) EventSystem.current.SetSelectedGameObject(selectWhenTutorialCompleted);

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

        EventSystem.current.SetSelectedGameObject(nextButton);
    }

    private void HideAllSlides()
    {
        GetComponent<Canvas>().enabled = false;
    }
}
