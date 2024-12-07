using UnityEngine;


public enum ObjectiveType
{
    Tree,
    Puddle,
    Stick
}

public class Objective : MonoBehaviour
{
    [SerializeField]
    ObjectiveType objectiveType;
    public ObjectiveType ObjectiveType => objectiveType;

    private void OnTriggerEnter(Collider other)
    {
        ObjectiveCollector objectiveCollector = other.GetComponent<ObjectiveCollector>();
        if (objectiveCollector != null)
        {
            if (objectiveCollector.TryCollectObjective(this)) RemoveObjective();
        }
    }

    private void RemoveObjective()
    {
        Destroy(gameObject);
    }
}
