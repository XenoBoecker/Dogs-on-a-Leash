using UnityEngine;

public class PlayerDogVisuals : DogVisuals
{
    Dog dog;

    [SerializeField] Transform[] leashAttachmentPoints;

    public Transform LeashAttachmentPoint;

    private void Awake()
    {
        dog = GetComponent<Dog>();
        dog.OnDogDataChanged += GetDogIDAndColor;

        GetDogIDAndColor();
    }

    private void GetDogIDAndColor()
    {
        SetDogID(dog.DogData.id);
        SetColorIndex(dog.ColorIndex);

        LeashAttachmentPoint = leashAttachmentPoints[dogID];

        UpdateVisuals();
    }
}