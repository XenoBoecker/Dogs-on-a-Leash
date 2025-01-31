using UnityEngine;

public class PlayerDogVisuals : MonoBehaviour
{
    Dog dog;

    [SerializeField] DogVisuals visual;

    [SerializeField] Transform[] leashAttachmentPoints;


    [SerializeField] GameObject[] shadows;

    [SerializeField] GameObject[] circles;

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

        int accessoryUsedCount = PlayerPrefs.GetInt("Accessory_" + dog.AccessorieIndex.ToString());
        PlayerPrefs.SetInt("Accessory_" + dog.AccessorieIndex.ToString(), accessoryUsedCount+1);

        for (int i = 0; i < shadows.Length; i++)
        {
            if(i == dog.DogData.id) shadows[i].SetActive(true);
            shadows[i].SetActive(false);
        }

        for (int i = 0; i < circles.Length; i++)
        {
            if (i == dog.ColorIndex) circles[i].SetActive(true);
            else circles[i].SetActive(false);
        }

        LeashAttachmentPoint = leashAttachmentPoints[visual.DogID];
    }

    public int GetColorID()
    {
        return visual.GetColorIndex();
    }
}