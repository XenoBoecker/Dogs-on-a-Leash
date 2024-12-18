using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrisbeeObject : MonoBehaviour
{
    GameObject ownerDog;

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ownerDog = collision.gameObject;
            gameObject.transform.parent = ownerDog.transform.Find("HoldingPosition");
        }
    }
}
