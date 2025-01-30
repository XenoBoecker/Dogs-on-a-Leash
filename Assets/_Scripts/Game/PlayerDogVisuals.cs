using UnityEngine;

public class PlayerDogVisuals : MonoBehaviour
{
    Dog dog;

    [SerializeField] DogVisuals visual;

    [SerializeField] Transform[] leashAttachmentPoints;

    public Transform LeashAttachmentPoint;

    private void Awake()
    {
        dog = GetComponent<Dog>();
        dog.OnDogDataChanged += GetDogInformation;

        GetDogInformation();
    }

    private void GetDogInformation()
    {
        visual.SetDogID(dog.DogData.id);
        visual.SetColorIndex(dog.ColorIndex);
        visual.SetAccessorieIndex(dog.AccessorieIndex);

        LeashAttachmentPoint = leashAttachmentPoints[visual.DogID];
    }

    public int GetColorID()
    {
        return visual.GetColorIndex();
    }
}