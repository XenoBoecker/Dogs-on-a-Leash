using UnityEngine;

public class MenuDogVisuals : DogVisuals
{
    [SerializeField] GameObject[] dogShadows;

    protected override void UpdateVisuals()
    {
        base.UpdateVisuals();

        for (int i = 0; i < dogShadows.Length; i++)
        {
            if (i == dogID) dogShadows[i].SetActive(true);
            else dogShadows[i].SetActive(false);
        }
    }
}