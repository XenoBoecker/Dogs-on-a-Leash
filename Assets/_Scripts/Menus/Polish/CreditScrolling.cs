using UnityEngine;

public class CreditScrolling : MonoBehaviour
{

    [SerializeField] GameObject credits;

    [SerializeField] float speed;


    [SerializeField] int pointOfReturn;

    Vector3 startPos;

    private void Start()
    {
        startPos = credits.transform.position;
    }

    private void Update()
    {
        credits.transform.Translate(Vector3.up * speed * Time.deltaTime);

        if (credits.transform.position.y > pointOfReturn) credits.transform.position = startPos;
    }
}