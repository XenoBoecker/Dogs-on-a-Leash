using System;
using UnityEngine;

public class CreditScrolling : MonoBehaviour
{

    [SerializeField] GameObject credits;

    [SerializeField] float speed;


    [SerializeField] int pointOfReturn;


    [SerializeField]
    CreditsDog[] creditDogs;

    Vector3 startPos;

    public event Action OnResetPosition;

    private void Start()
    {
        startPos = credits.transform.position;

        for (int i = 0; i < creditDogs.Length; i++)
        {
            creditDogs[i].SetupRandomDog();
        }
    }

    private void Update()
    {
        credits.transform.Translate(Vector3.up * speed * Time.deltaTime);

        if (credits.transform.position.y > pointOfReturn)
        {
            OnResetPosition?.Invoke();
            credits.transform.position = startPos;
            for (int i = 0; i < creditDogs.Length; i++)
            {
                creditDogs[i].ResetToStartPosition();
            }
        }
    }
}