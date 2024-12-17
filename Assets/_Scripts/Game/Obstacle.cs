using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    [SerializeField] float pushBackForce = 50f;

    [SerializeField] float stunTime = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (!rb) return;

        rb.AddForce((other.transform.position - transform.position).normalized * pushBackForce, ForceMode.Impulse);

        HumanMovement human = other.GetComponent<HumanMovement>();

        if (human)
        {
            human.Stun(stunTime);
        }
    }
}
