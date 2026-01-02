using UnityEngine;

public class HumanSpawner : MonoBehaviour
{

    [SerializeField] GameObject human;

    public void Setup()
    {
        Instantiate(human, Vector3.zero, Quaternion.identity);
    }
}
