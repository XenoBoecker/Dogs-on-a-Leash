using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class Bus : MonoBehaviour
{

    [SerializeField] Transform start, stop, end;
    [SerializeField] AnimationCurve busArrivalPositionCurve, busLeavePositionCurve;
    [SerializeField] float busArrivalDuration;

    [SerializeField] Transform doorStart, doorOut, doorEnd;
    [SerializeField] Transform door;
    [SerializeField] AnimationCurve doorAnimCurve;
    [SerializeField] float doorAnimDuration;

    [SerializeField] float humanDoorTriggerDistance = 1.5f;

    [SerializeField] float permaBobbleScale, permaBobbleSpeed;

    [SerializeField] float busBobbleScale, busBobbleSpeed;
    [SerializeField] float busBobbleRotScale;
    [SerializeField] Transform busCarossery;

    [SerializeField] float busBobbleDuration;


    [SerializeField] Transform[] wheels;

    Coroutine permaBusUpAndDown;

    private void Start()
    {
        transform.position = start.position;
        door.SetParent(busCarossery);
    }

    public IEnumerator BusArrivingCoroutine()
    {
        // TODO: Carosserie hochgelegt beim fahren und beim halten runter

        for (float i = 0; i < busArrivalDuration; i += Time.unscaledDeltaTime)
        {
            transform.position = Vector3.Lerp(start.position, stop.position, busArrivalPositionCurve.Evaluate(i / busArrivalDuration));

            yield return null;
        }
        transform.position = stop.position;

        permaBusUpAndDown = StartCoroutine(PermaBusUpAndDown());
    }

    private IEnumerator PermaBusUpAndDown()
    {
        Vector3 carosseryStartPos = busCarossery.transform.position;
        float time = 0;
        // bobble the bus
        while (true)
        {
            float sin = Mathf.Sin(2 * Mathf.PI * time * permaBobbleSpeed);

            busCarossery.transform.position = carosseryStartPos + (Vector3.up * sin * permaBobbleScale);

            time += Time.unscaledDeltaTime;
            yield return null;
        }
    }

    public IEnumerator BusLeavingCoroutine()
    {
        yield return StartCoroutine(OpenDoorsCoroutine());

        yield return StartCoroutine(HumanEnterBusCoroutine());

        yield return StartCoroutine(CloseDoorsCoroutine());

        yield return StartCoroutine(BusDriveAwayCoroutine());
    }

    public IEnumerator BusDriveAwayCoroutine()
    {
        StopCoroutine(permaBusUpAndDown);

        // Bus leaving
        for (float i = 0; i < busArrivalDuration; i += Time.unscaledDeltaTime)
        {
            transform.position = Vector3.Lerp(stop.position, end.position, busLeavePositionCurve.Evaluate(i / busArrivalDuration));

            yield return null;
        }
        transform.position = end.position;
    }

    private IEnumerator HumanEnterBusCoroutine()
    {
        // wait for human to get in
        HumanMovement human = FindObjectOfType<HumanMovement>();
        human.enabled = false;

        float addedVelocity = 0;

        while(Vector3.Distance(human.transform.position, doorOut.position) > humanDoorTriggerDistance)
        {
            human.transform.Translate((doorOut.position - human.transform.position).normalized * human.speed * (2 + addedVelocity) * Time.deltaTime, Space.World);

            addedVelocity += Time.deltaTime;

            yield return null;
        }

        human.gameObject.SetActive(false);

        PlayerDogController[] dogs = FindObjectsOfType < PlayerDogController > ();

        for (int i = 0; i < dogs.Length; i++)
        {
            dogs[i].gameObject.SetActive(false);
        }

        yield return StartCoroutine(BobbleTheBus());
    }

    IEnumerator BobbleTheBus()
    {
        if(permaBusUpAndDown != null) StopCoroutine(permaBusUpAndDown);

        Vector3 carosseryStartPos = busCarossery.transform.position;
        // bobble the bus
        for (float i = 0; i < busBobbleDuration; i += Time.unscaledDeltaTime)
        {
            float sin = Mathf.Sin(2 * Mathf.PI * i * busBobbleSpeed);

            busCarossery.transform.position = carosseryStartPos + (Vector3.up * sin * busBobbleScale);
            busCarossery.transform.Rotate(0, 0, (sin - transform.rotation.z) * busBobbleRotScale);
            // busCarossery.transform.rotation = Quaternion.Euler(0, 0, sin * busBobbleRotScale);
            yield return null;
        }
        busCarossery.transform.position = carosseryStartPos;
        busCarossery.transform.rotation = Quaternion.identity;
    }

    private IEnumerator OpenDoorsCoroutine()
    {
        for (float i = 0; i < doorAnimDuration / 2; i += Time.unscaledDeltaTime)
        {
            door.transform.position = Vector3.Lerp(doorStart.position, doorOut.position, doorAnimCurve.Evaluate(i / doorAnimDuration * 2));

            yield return null;
        }
        door.transform.position = doorOut.position;

        for (float i = 0; i < doorAnimDuration / 2; i += Time.unscaledDeltaTime)
        {
            door.transform.position = Vector3.Lerp(doorOut.position, doorEnd.position, doorAnimCurve.Evaluate(i / doorAnimDuration * 2));

            yield return null;
        }
        door.transform.position = doorEnd.position;
    }

    private IEnumerator CloseDoorsCoroutine()
    {
        for (float i = 0; i < doorAnimDuration / 2; i += Time.unscaledDeltaTime)
        {
            door.transform.position = Vector3.Lerp(doorEnd.position, doorOut.position, doorAnimCurve.Evaluate(i / doorAnimDuration * 2));

            yield return null;
        }
        door.transform.position = doorOut.position;

        for (float i = 0; i < doorAnimDuration / 2; i += Time.unscaledDeltaTime)
        {
            door.transform.position = Vector3.Lerp(doorOut.position, doorStart.position, doorAnimCurve.Evaluate(i / doorAnimDuration * 2));

            yield return null;
        }
        door.transform.position = doorStart.position;
    }
}
