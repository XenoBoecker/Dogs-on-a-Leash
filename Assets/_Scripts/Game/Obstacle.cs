using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    float pushBackForce = 2f;
    public float CurrentPushBackForce => pushBackForce * (playerCollisionCounter + 1);

    [HideInInspector]
    public float stunTime = 1f;


    float pushBackCD = 2f;
    float pushBackTimer;

    [HideInInspector]
    public int scoreValue = -100;
    public int CurrentScoreValue => scoreValue * (playerCollisionCounter + 1);

    int playerCollisionCounter;

    HumanMovement human;

    private void Start()
    {
        pushBackForce = ObstacleManager.Instance.pushBackForce;
        pushBackCD = ObstacleManager.Instance.pushBackCD;
        stunTime = ObstacleManager.Instance.stunTime;
        scoreValue = ObstacleManager.Instance.scoreValue;
    }

    private void Update()
    {
        pushBackTimer -= Time.deltaTime;

        if (human && pushBackTimer <= 0)
        {
            DoCollision();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (!rb) return;

        human = other.GetComponent<HumanMovement>();

        if (human && pushBackTimer > 0)
        {
            DoCollision();
        }
    }

    void DoCollision()
    {
        human.ObstacleCollision(this);

        playerCollisionCounter++;
        pushBackTimer = pushBackCD;

        ObstacleManager.Instance.AddToCollisionCount();

        Debug.Log("Obstacle push Human (" + name + ")");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<HumanMovement>() == human)
        {
            human = null;
        }
    }
}
