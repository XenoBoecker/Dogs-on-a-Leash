using System;
using UnityEngine;

public class ObjectiveCollector: MonoBehaviour
{
    [SerializeField]
    ObjectiveType objectiveType;

    int scoreCount;
    public int ScoreCount => scoreCount;

    public event Action OnScoreChanged;

    internal bool TryCollectObjective(Objective objective)
    {
        if(objective.ObjectiveType == objectiveType)
        {
            CollectObjective(objective);

            return true;
        }

        return false;
    }

    private void CollectObjective(Objective objective)
    {
        scoreCount++;

        OnScoreChanged?.Invoke();
    }
}
