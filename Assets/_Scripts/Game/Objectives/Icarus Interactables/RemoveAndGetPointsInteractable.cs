using UnityEngine;

public class RemoveAndGetPointsInteractable : Interactable
{
    [SerializeField] int pointReward = 100;
    internal override void CompleteTask()
    {
        return;

        bool wasCompleted = isCompleted;

        base.CompleteTask();

        if (wasCompleted) return;

        if(ScoreManager.Instance != null) ScoreManager.Instance.AddScore(pointReward);

        Destroy(gameObject, spawnVFXTimeDelay + 0.1f);
    }
}