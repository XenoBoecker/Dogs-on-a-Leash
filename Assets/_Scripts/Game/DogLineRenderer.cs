using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogLineRenderer : MonoBehaviour
{
    Transform humanObject;

    LineRenderer lineRenderer;

    private void Awake()
    {
        humanObject = GameObject.Find("Human").transform;
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.enabled = true;
    }

    private void Update()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, humanObject.position);
    }

}
