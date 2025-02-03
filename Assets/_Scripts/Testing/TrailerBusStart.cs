using UnityEngine;

public class TrailerBusStart : MonoBehaviour
{
    Bus bus;


    [SerializeField] KeyCode startBusArrivalKey = KeyCode.R;

    [SerializeField] KeyCode honkKey = KeyCode.Space;

    private void Start()
    {
        bus = FindObjectOfType<Bus>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(startBusArrivalKey)) StartCoroutine(bus.BusArrivingCoroutine());

        if (Input.GetKeyDown(honkKey)) StartCoroutine(bus.HonkTheBus());
    }
}