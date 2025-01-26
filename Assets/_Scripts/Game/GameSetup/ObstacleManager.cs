using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public static ObstacleManager Instance; 

    public float pushBackForce = 2f;

    public float pushBackCD = 2f;

    public float stunTime = 1f;

    public int scoreValue = -100;

    int collisionCount;

    private void Awake()
    {
        Instance = this;
    }

    public void AddToCollisionCount()
    {
        collisionCount++;

        PlayerPrefs.SetInt("CollisionCount", collisionCount);
    }
}