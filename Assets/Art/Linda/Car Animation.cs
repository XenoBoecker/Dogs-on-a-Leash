using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    public float border;
    public float speed = 0.005f;
    bool maxrange = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y +speed, transform.position.z);
        if (transform.position.y == border ) { speed = -speed; }
    }
}
