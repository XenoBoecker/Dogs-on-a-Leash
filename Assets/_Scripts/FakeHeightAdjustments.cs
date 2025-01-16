using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeHeightAdjustments : MonoBehaviour
{

    [SerializeField] Transform[] raycastStartPoints;


    [SerializeField] Transform target;
    float targetStartHeight;

    float targetGoalHeight;

    List<float> baseDistances;

    [SerializeField] LayerMask groundLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        targetStartHeight = target.position.y;
        targetGoalHeight = targetStartHeight;

        for (int i = 0; i < raycastStartPoints.Length; i++)
        {
            if(Physics.Raycast(new Ray(raycastStartPoints[i].transform.position, Vector3.down), out RaycastHit hit, 10, groundLayerMask))
            {
                baseDistances.Add(raycastStartPoints[i].position.y - hit.point.y);
            }

        }


    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < raycastStartPoints.Length; i++)
        {
            if (Physics.Raycast(new Ray(raycastStartPoints[i].transform.position, Vector3.down), out RaycastHit hit, 10, groundLayerMask))
            {
                float dist = raycastStartPoints[i].position.y - hit.point.y;

            }
        }
    }
}
