using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogProfileScreen : MonoBehaviour
{
    int currentColorIndex;

    int currentDogIndex;


    [SerializeField] GameObject[] dogPanels;


    [SerializeField] DogVisuals dogVisual;

    [SerializeField] GameObject[] dogModels;


    [SerializeField] Transform dogModelParent;

    GameObject currentDogModel;

    void Start()
    {
        ShowPanel();
    }


    public void ShowNextPanel()
    {
        currentDogIndex++;
        if (currentDogIndex >= dogPanels.Length) currentDogIndex = 0;


        ShowPanel();
    }

    public void ShowPreviousPanel()
    {
        currentDogIndex--;

        if (currentDogIndex == -1) currentDogIndex = dogPanels.Length-1;

        ShowPanel();
    }

    public void SetDogColor(int colorIndex)
    {
        currentColorIndex = colorIndex;

        ShowPanel(true);
    }

    void ShowPanel(bool keepDogRotation = false)
    {
        for (int i = 0; i < dogPanels.Length; i++)
        {
            if (i == currentDogIndex) dogPanels[i].SetActive(true);
            else dogPanels[i].SetActive(false);
        }

        ShowDog(keepDogRotation);
    }

    void ShowDog(bool keepRotation = false)
    {
        dogVisual.SetDogID(currentDogIndex);
        dogVisual.SetColorIndex(currentColorIndex);
        return;

        if (currentDogModel != null) Destroy(currentDogModel);

        for (int i = 0; i < dogModels.Length; i++)
        {
            if (i == currentDogIndex * 4 + currentColorIndex)
            {
                if (keepRotation)
                {
                    currentDogModel = Instantiate(dogModels[i], dogModelParent.transform.position, dogModelParent.transform.rotation);
                    currentDogModel.transform.Rotate(Vector3.up, -dogModels[i].transform.rotation.y);
                }
                else
                {
                    dogModelParent.transform.rotation = Quaternion.identity;
                    currentDogModel = Instantiate(dogModels[i], dogModelParent.transform.position, dogModels[i].transform.rotation);
                }

                currentDogModel.transform.SetParent(dogModelParent);
            }
        }
    }
}
