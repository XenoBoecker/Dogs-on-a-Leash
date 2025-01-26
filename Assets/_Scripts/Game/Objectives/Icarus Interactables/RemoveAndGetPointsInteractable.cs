using UnityEngine;

public class RemoveAndGetPointsInteractable : Interactable
{
    [SerializeField] int pointReward = 100;


    [SerializeField] float destroyTimeDelay = 0.3f;
    internal override void CompleteTask()
    {
        base.CompleteTask();

        ScoreManager.Instance.AddScore(pointReward);

        Destroy(gameObject, destroyTimeDelay);
    }
}