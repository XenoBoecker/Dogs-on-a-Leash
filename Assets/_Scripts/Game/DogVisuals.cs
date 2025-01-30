using System;
using System.Collections.Generic;
using UnityEngine;

public class DogVisuals : MonoBehaviour
{
    protected int dogID;
    public int DogID => dogID;
    int colorIndex;
    int accessorieIndex;

    [SerializeField] GameObject[] dogModels;

    [SerializeField] Material[] dogMaterials;

    [SerializeField] GameObject leash;

    [SerializeField]
    LayerMask[] silhouetteColorLayers;


    [SerializeField] List<GameObject> bernardAccessories;
    [SerializeField] List<GameObject> poodleAccessories;
    [SerializeField] List<GameObject> pugAccessories;
    [SerializeField] List<GameObject> retrieverAccessories;

    public int AccessorieCount => bernardAccessories.Count;
    public GameObject CurrentAccessory;

    public event Action OnUpdateVisuals;

    protected virtual void UpdateVisuals()
    {
        for (int i = 0; i < dogModels.Length; i++)
        {
            if(i == dogID) dogModels[dogID].SetActive(true);
            else dogModels[i].SetActive(false);
        }

        
        dogModels[dogID].GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial = dogMaterials[dogID * 4 + colorIndex];

        List<GameObject> currentDogAccessories = GetCurrentDogAccessorieList();

        for (int i = 0; i < currentDogAccessories.Count; i++)
        {
            currentDogAccessories[i].SetActive(false);
        }
        currentDogAccessories[accessorieIndex].SetActive(true);
        CurrentAccessory = currentDogAccessories[accessorieIndex];

        // Set layer of all children of dogModels[dogID] to: silhouetteColorLayers[colorIndex]
        if (dogID < dogModels.Length && colorIndex < silhouetteColorLayers.Length)
        {
            if (leash) leash.layer = 9 + colorIndex;
            // int newLayer = silhouetteColorLayers[colorIndex];
            SetLayerRecursively(dogModels[dogID], 9+colorIndex);
        }

        OnUpdateVisuals?.Invoke();
    }

    private List<GameObject> GetCurrentDogAccessorieList()
    {
        if (dogID == 0) return bernardAccessories;
        else if (dogID == 1) return poodleAccessories;
        else if (dogID == 2) return pugAccessories;
        else return retrieverAccessories;
    }

    public void SetDogID(int id)
    {
        dogID = id;

        UpdateVisuals();
    }

    public void SetColorIndex(int i)
    {
        colorIndex = i;

        UpdateVisuals();
    }

    public void SetAccessorieIndex(int i)
    {

        accessorieIndex = i;

        UpdateVisuals();
    }
    void SetLayerRecursively(GameObject obj, LayerMask newLayer)
    {
        if (obj == null) return;

        obj.layer = newLayer; // Set layer for the parent object

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer); // Recursively set layer for children
        }
    }
}
