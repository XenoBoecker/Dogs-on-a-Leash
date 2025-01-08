using UnityEngine;

public class RemoveAndGetPointsInteractable : Interactable
{
    [SerializeField] int pointReward = 100;
    internal override void CompleteTask()
    {
        base.CompleteTask();

        ScoreManager.Instance.AddScore(pointReward);

        Destroy(gameObject);
    }
}