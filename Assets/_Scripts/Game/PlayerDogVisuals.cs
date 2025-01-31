using UnityEngine;

public class PlayerDogVisuals : MonoBehaviour
{
    Dog dog;

    [SerializeField] DogVisuals visual;

    [SerializeField] Transform[] leashAttachmentPoints;


    [SerializeField] GameObject[] shadows;

    public Transform LeashAttachmentPoint;

    public float Height;

    private void Awake()
    {
        dog = GetComponent<Dog>();
        dog.OnDogDataChanged += GetDogInformation;

        GetDogInformation();
    }

    private void Update()
    {
        Shader.SetGlobalVector("_Player", transform.position+ Vector3.up * Height);
    }

    private void GetDogInformation()
    {
        visual.SetDogID(dog.DogData.id);
        visual.SetColorIndex(dog.ColorIndex);
        visual.SetAccessorieIndex(dog.AccessorieIndex);

        for (int i = 0; i < shadows.Length; i++)
        {
            if(i == dog.DogData.id) shadows[i].SetActive(true);
            shadows[i].SetActive(false);
        }

        LeashAttachmentPoint = leashAttachmentPoints[visual.DogID];
    }

    public int GetColorID()
    {
        return visual.GetColorIndex();
    }
}