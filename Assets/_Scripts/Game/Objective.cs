using System;
using UnityEngine;


public enum ObjectiveType
{
    Red,
    Green,
    Blue,
    Yellow,
    Orange,
    Purple,
    Teal,
    Pink
}

[System.Serializable]
public struct VisualByType
{
    public GameObject obj;
    public ObjectiveType type;
}

public class Objective : MonoBehaviour
{
    [SerializeField]
    ObjectiveType objectiveType;
    public ObjectiveType ObjectiveType => objectiveType;

    public event Action OnTypeChanged;

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

    public void SetObjectiveType(ObjectiveType type)
    {
        objectiveType = type;

        OnTypeChanged?.Invoke();
    }
}
