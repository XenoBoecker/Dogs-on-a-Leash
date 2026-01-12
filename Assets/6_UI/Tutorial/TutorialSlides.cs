using UnityEngine;

public class TutorialSlides : MonoBehaviour
{
    [SerializeField] private GameObject[] allSlides;

    private int currentSlideIndex = 0;

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
    }

    private void HideAllSlides()
    {
        GetComponent<Canvas>().enabled = false;
    }
}
