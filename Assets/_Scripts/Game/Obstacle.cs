using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    public float pushBackForce = 2f;
    public float CurrentPushBackForce => pushBackForce * (playerCollisionCounter + 1);

    public float stunTime = 1f;


    [SerializeField] float pushBackCD = 2f;
    float pushBackTimer;

    public int scoreValue = -100;
    public int CurrentScoreValue => scoreValue * (playerCollisionCounter + 1);

    int playerCollisionCounter;

    HumanMovement human;

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

        Debug.Log("Obstacle push Human");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<HumanMovement>() == human)
        {
            human = null;
        }
    }
}
