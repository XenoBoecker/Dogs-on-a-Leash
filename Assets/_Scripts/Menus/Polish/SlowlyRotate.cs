using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowlyRotate : MonoBehaviour
{

    [SerializeField] float rotSpeed;
    public bool UnscaledTime = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (UnscaledTime) transform.Rotate(Vector3.up, rotSpeed * Time.unscaledDeltaTime);
        else transform.Rotate(Vector3.up, rotSpeed * Time.deltaTime);
    }
}