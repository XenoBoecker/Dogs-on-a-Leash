using UnityEngine;

public class TrailerBusStart : MonoBehaviour
{
    Bus bus;


    [SerializeField] KeyCode startBusArrivalKey = KeyCode.R;

    private void Start()
    {
        bus = FindObjectOfType<Bus>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(startBusArrivalKey)) StartCoroutine(bus.BusArrivingCoroutine());
    }
}