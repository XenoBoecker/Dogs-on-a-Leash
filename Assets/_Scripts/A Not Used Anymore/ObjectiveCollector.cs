using System;
using UnityEngine;
/*
public class ObjectiveCollector: MonoBehaviour
{
    Dog dog;

    public event Action<int> OnScoreChanged;

    private void Awake()
    {
        dog = GetComponent<Dog>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Objective objective = other.GetComponent<Objective>();
        if (objective != null)
        {
            OnScoreChanged?.Invoke(objective.scoreCount);

            objective.RemoveObjective();
        }
    }
}
*/