using UnityEngine;

public class DogVisuals : MonoBehaviour
{
    Dog dog;

    [SerializeField] GameObject[] dogVisuals;

    private void Awake()
    {
        dog = GetComponent<Dog>();
        dog.OnDogDataChanged += UpdateVisuals;
    }

    private void UpdateVisuals()
    {

    }
}