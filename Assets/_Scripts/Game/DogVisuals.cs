using UnityEngine;

public class DogVisuals : MonoBehaviour
{
    Dog dog;

    [SerializeField] GameObject[] dogVisuals;

    private void Awake()
    {
        dog = GetComponent<Dog>();
        dog.OnDogDataChanged += UpdateVisuals;

        int rand = Random.Range(0, dogVisuals.Length);

        for (int i = 0; i < dogVisuals.Length; i++)
        {
            if (i == rand) dogVisuals[i].SetActive(true);
            else dogVisuals[i].SetActive(false);
        }
    }

    private void UpdateVisuals()
    {

    }
}