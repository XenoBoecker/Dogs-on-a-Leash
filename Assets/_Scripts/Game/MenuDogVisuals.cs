using UnityEngine;

public class MenuDogVisuals : MonoBehaviour
{

    [SerializeField] DogVisuals visual;

    [SerializeField] GameObject[] dogShadows;

    private void Start()
    {
        visual.OnUpdateVisuals += UpdateVisuals;

        UpdateVisuals();
    }
    protected void UpdateVisuals()
    {
        for (int i = 0; i < dogShadows.Length; i++)
        {
            if (i == visual.DogID) dogShadows[i].SetActive(true);
            else dogShadows[i].SetActive(false);
        }
    }
}