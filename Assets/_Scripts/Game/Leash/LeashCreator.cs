using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeashCreator : MonoBehaviour
{
    public Transform leashStart;
    public Transform leashEnd;
    public Transform leashParent;
    

    public int leashLength;

    public GameObject leashPrefab;

    List<Transform> leashSegments = new List<Transform>();

    float leashSize;
    // Start is called before the first frame update
    void Start()
    {
        GameObject lastLeashSegment = leashStart.gameObject;
        for(int i = 0; i < leashLength; i++)
        {
            GameObject leashSegment = Instantiate(leashPrefab, Vector3.Lerp(leashStart.position, leashEnd.position, (float)i / leashLength), Quaternion.identity);
            leashSegment.GetComponent<HingeJoint>().connectedBody = lastLeashSegment.GetComponent<Rigidbody>();
            leashSegment.transform.parent = leashParent;
            lastLeashSegment = leashSegment;
        }
        leashEnd.GetComponent<HingeJoint>().connectedBody = lastLeashSegment.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(leashStart.position, leashEnd.position) < leashSegments.Count * leashPrefab.transform.localScale.z)
        {
            leashEnd.GetComponent<HingeJoint>().connectedBody = leashSegments[leashSegments.Count-2].GetComponent<Rigidbody>();
            GameObject.Destroy(leashSegments[leashSegments.Count-1]);
            leashSegments.RemoveAt(leashSegments.Count-1);
        }

        // if(Vector3.Distance(leashStart.position, leashEnd.position) > leashSegments.Count * leashPrefab.transform.localScale.z)
        // {
        //     Vector3 spawnPos = Vector3.Lerp(leashEnd.position, leashStart.position, 0.5f);
        //     GameObject newLeashSegment = Instantiate(leashPrefab, leashEnd.position, Quaternion.identity);
        //     newLeashSegment.GetComponent<HingeJoint>().connectedBody = leashSegments[leashSegments.Count-1].GetComponent<Rigidbody>();
        //     newLeashSegment.transform.parent = leashParent;
        //     leashSegments.Add(newLeashSegment.transform);
        //     leashEnd.GetComponent<HingeJoint>().connectedBody = newLeashSegment.GetComponent<Rigidbody>();
        // }
    }
}
