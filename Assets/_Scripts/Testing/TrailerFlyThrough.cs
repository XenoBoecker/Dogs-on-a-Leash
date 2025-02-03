using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerFlyThrough : MonoBehaviour
{

    [SerializeField] KeyCode startFlyKey = KeyCode.E;


    [SerializeField] AnimationCurve flyCurve;

    [SerializeField] float flyDuration;


    [SerializeField] Transform startPoint, endPoint;

    Vector3 startPos, endPos;

    Camera cam;

    [SerializeField] float camStartFOV, camEndFOV;

    [SerializeField] AnimationCurve fovCurve;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();

        if (camStartFOV == 0) camStartFOV = cam.fieldOfView;

        if (startPoint == null) startPos = transform.position;
        else startPos = startPoint.position;
        endPos = endPoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(startFlyKey)) StartCoroutine(FlythroughCoroutine());
    }

    IEnumerator FlythroughCoroutine()
    {
        for (float i = 0; i < flyDuration; i+= Time.deltaTime)
        {
            float curveValue = flyCurve.Evaluate(i / flyDuration);

            float x = Mathf.Lerp(startPos.x, endPos.x, curveValue);
            float y = Mathf.Lerp(startPos.y, endPos.y, curveValue);
            float z = Mathf.Lerp(startPos.z, endPos.z, curveValue);

            transform.position = new(x, y, z);

            float fovCurveValue = fovCurve.Evaluate(i / flyDuration);

            cam.fieldOfView = Mathf.Lerp(camStartFOV, camEndFOV, fovCurveValue);

            yield return null;
        }
        transform.position = endPos;
    }
}
