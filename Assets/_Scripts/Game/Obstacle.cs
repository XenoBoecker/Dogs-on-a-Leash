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

        HumanMovement human = other.GetComponent<HumanMovement>();

        if (human)
        {
            Vector3 dir = other.transform.position - transform.position;

            dir.y = 0;

            human.Stun(stunTime);

            rb.AddForce(dir.normalized * pushBackForce, ForceMode.Impulse);
        }
    }
}
