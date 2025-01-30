using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class IKFootSolver : MonoBehaviour
{
    [SerializeField] LayerMask terrainLayer = default;
    [SerializeField] Transform body = default;
    [SerializeField] IKFootSolver otherFoot = default;
    [SerializeField] float speed = 1;
    [SerializeField] float stepDistance = 4;
    [SerializeField] float stepLength = 4;
    [SerializeField] float stepHeight = 1;
    [SerializeField] Vector3 footOffset = default;
    float footSpacing;
    Vector3 oldPosition, currentPosition, newPosition;
    Vector3 oldNormal, currentNormal, newNormal;
    float lerp;

    Rigidbody rb;

    public event Action OnTakeStep;

    private void Start()
    {
        rb = GameObject.Find("Human").GetComponent<Rigidbody>();
        footSpacing = transform.localPosition.x;
        currentPosition = newPosition = oldPosition = transform.position;
        currentNormal = newNormal = oldNormal = transform.up;
        lerp = 1;
    }

    // Update is called once per frame

    

    void Update()
    {
        transform.position = currentPosition;
        transform.up = currentNormal;

        Ray ray = new Ray(body.position + (body.right * footSpacing), Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 10, terrainLayer.value))
        {
            if (Vector3.Distance(newPosition, hit.point) > stepDistance && !otherFoot.IsMoving() && lerp >= 1)
            {
                OnTakeStep?.Invoke();

                lerp = 0;

                // Calculate the movement direction based on the rigidbody's velocity
                Vector3 moveDirection = rb.velocity.normalized;

                // Determine if the movement is primarily sideways or forward
                float stepLengthAdjusted = stepLength;
                if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.z))
                {
                    // If moving sideways, adjust the step length
                    stepLengthAdjusted *= 1.5f; // Adjust this factor as needed
                }

                // Adjust newPosition to account for both forward and sideways movement
                newPosition = hit.point + (moveDirection * stepLengthAdjusted) + footOffset;

                newNormal = hit.normal;
            }
        }

        if (lerp < 1)
        {
            Vector3 tempPosition = Vector3.Lerp(oldPosition, newPosition, lerp);
            tempPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            currentPosition = tempPosition;
            currentNormal = Vector3.Lerp(oldNormal, newNormal, lerp);
            lerp += Time.deltaTime * speed;
        }
        else
        {
            oldPosition = newPosition;
            oldNormal = newNormal;
        }
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(newPosition, 0.5f);
    }



    public bool IsMoving()
    {
        return lerp < 1;
    }



}
