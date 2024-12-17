using System;
using UnityEngine;

public class ObjectiveCollector: MonoBehaviour
{
    Dog dog;

    public event Action<int> OnScoreChanged;

    private void Awake()
    {
        dog = GetComponent<Dog>();
    }

    internal bool TryCollectObjective(Objective objective)
    {
        if(objective.ObjectiveType == dog.DogData.objectiveType)
        {
            CollectObjective(objective);

            return true;
        }

        return false;
    }

    private void CollectObjective(Objective objective)
    {
        OnScoreChanged?.Invoke(objective.scoreCount);
    }
}
