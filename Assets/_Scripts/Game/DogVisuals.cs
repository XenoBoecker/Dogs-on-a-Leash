using UnityEngine;

public class DogVisuals : MonoBehaviour
{

    [System.Serializable]
    struct DogVisualByType
    {
        public GameObject obj;
        public ObjectiveType type;
    }

    Dog dog;


    [SerializeField] DogVisualByType[] dogVisuals;

    private void Awake()
    {
        dog = GetComponent<Dog>();
        dog.OnDogDataChanged += UpdateVisuals;
    }

    private void UpdateVisuals()
    {
        foreach (DogVisualByType dw in dogVisuals)
        {
            if (dw.type == dog.DogData.objectiveType) dw.obj.SetActive(true);
            else dw.obj.SetActive(false);
        }
    }
}