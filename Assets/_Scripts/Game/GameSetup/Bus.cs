using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bus : MonoBehaviour
{

    [SerializeField] Transform start, end;
    [SerializeField] Transform[] wheels;


    [SerializeField] AnimationCurve busPositionCurve;
    [SerializeField] float busArrivalDuration;

    private void Start()
    {
        transform.position = start.position;
    }

    public IEnumerator BusArrivingCoroutine()
    {
        for (float i = 0; i < busArrivalDuration; i += Time.unscaledDeltaTime)
        {
            transform.position = Vector3.Lerp(start.position, end.position, busPositionCurve.Evaluate(i / busArrivalDuration));

            yield return null;
        }
    }
}
