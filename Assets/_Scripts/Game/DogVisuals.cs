using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class DogVisuals : MonoBehaviour
{
    protected int dogID;
    int colorIndex;

    [SerializeField] GameObject[] dogModels;

    [SerializeField] Material[] dogMaterials;

    protected virtual void UpdateVisuals()
    {
        for (int i = 0; i < dogModels.Length; i++)
        {
            if (i == dogID)
            {
                dogModels[i].SetActive(true);

                dogModels[i].GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial = dogMaterials[i * 4 + colorIndex];
            }
            else dogModels[i].SetActive(false);
        }
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
}
