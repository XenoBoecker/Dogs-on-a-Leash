using UnityEngine;

public class DogVisuals : MonoBehaviour
{
    Dog dog;

    [SerializeField] GameObject[] dogVisuals;

    [SerializeField] Transform[] leashAttachmentPoints;

    public Transform LeashAttachmentPoint;

    private void Awake()
    {
        dog = GetComponent<Dog>();
        dog.OnDogDataChanged += UpdateVisuals;

        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        int dogVisualsIndex = dog.DogData.id * 4 + dog.ColorIndex;

        Debug.Log("Visual index: " + dogVisualsIndex + "; dogDataID: " + dog.DogData.id + "; Color index: " + dog.ColorIndex);

        for (int i = 0; i < dogVisuals.Length; i++)
        {
            if (i == dogVisualsIndex)
            {
                dogVisuals[i].SetActive(true);
                LeashAttachmentPoint = leashAttachmentPoints[i];
            }
            else dogVisuals[i].SetActive(false);
        }
    }
}