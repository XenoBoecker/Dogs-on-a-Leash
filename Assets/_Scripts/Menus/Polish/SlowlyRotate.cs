using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowlyRotate : MonoBehaviour
{

    [SerializeField] float rotSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, rotSpeed * Time.deltaTime);
    }
}