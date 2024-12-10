using UnityEngine;

public class DogVisuals : MonoBehaviour
{


    Dog dog;


    [SerializeField] VisualByType[] dogVisuals;

    private void Awake()
    {
        dog = GetComponent<Dog>();
        dog.OnDogDataChanged += UpdateVisuals;
    }

    private void UpdateVisuals()
    {
        foreach (VisualByType dw in dogVisuals)
        {
            if (dw.type == dog.DogData.objectiveType) dw.obj.SetActive(true);
            else dw.obj.SetActive(false);
        }
    }
}