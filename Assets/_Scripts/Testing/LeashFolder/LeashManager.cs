using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;

public class LeashManager : MonoBehaviour
{
    //This is attached to the dog leash whatever thingy
    public Transform leashTarget; // the leash ending point on the human needed for checking for intersections

    public GameObject LeashSegmentPrefab;


    public List<GameObject> leashSegments = new List<GameObject>();

    public LayerMask intersectableObjects;

    public LineRenderer lineRenderer;

    Rigidbody myDogRigidbody;

    Rigidbody humanRigidbody;

    [Header("Leash settings")]
    public float leashSegmentLengthMinimum = 0.5f;
    public float maxLeashLength = 10f;
    public float leashSegmentAngle = 30f;
    public float leashPullForce = 10f;
    public float humanPullForce = 2f;

    float currentLength = 0f;

    void Start()
    {
        Invoke("Setup", 0.2f);
    }

    void Setup()
    {
        Debug.Log("Setting up leash");
        myDogRigidbody = gameObject.GetComponent<Rigidbody>();
        leashTarget = GameObject.Find("Human").transform;
        humanRigidbody = leashTarget.GetComponent<Rigidbody>();
    }

    void Update()
    {
        UpdateLineRenderer();
        if(leashSegments.Count > 0)
        {
            UpdateLeashSegmentsDogSide();
            UpdateLeashSegmentsHumanSide();

            CheckLeashSegmentDogSide();
            CheckLeashSegmentHumanSide();

            
        }
        else
        {
            CreateLeashSegments();
        }

        // OnDrawGizmos();
    }

    void FixedUpdate()
    {
        ApplyPhysics();
    }

    void UpdateLeashSegmentsDogSide()
    {
        if(Physics.Raycast(gameObject.transform.position,  leashSegments[0].transform.position - gameObject.transform.position, out RaycastHit hit, Vector3.Distance(gameObject.transform.position, leashSegments[0].transform.position), intersectableObjects))
        {
            //If we hit something between the dog and the last leash segment we check for the angle and then maybe create a new leash segment
            // Debug.Log("Hit something");

            if(Vector3.Distance(leashSegments[0].transform.position, hit.point) > leashSegmentLengthMinimum)
            {
                if(Vector3.Angle(leashSegments[0].GetComponent<LeashSegment>().nextSegment.position - leashSegments[0].transform.position, hit.point - leashSegments[0].transform.position) < leashSegmentAngle)
                {
                    // Debug.Log("Angle is too small Dog Side");
                    return;
                }

                // Vector3 offset = (gameObject.transform.position - hit.point).normalized * 0.3f; // Adjust the offset value as needed
                GameObject newLeashSegment = Instantiate(LeashSegmentPrefab, hit.point, Quaternion.identity);
                PopulateLeashSegment(newLeashSegment, true);
                
            }
            else
            {
                // Debug.Log("Too close to the last segment");
            }
        }
        else
        {
            // Debug.Log("Hit nothing DOG CHECK");
        }
    }

    void CheckLeashSegmentDogSide()
    {
        if(leashSegments.Count < 1)
        {
            return;
        }
        if(Physics.Raycast(gameObject.transform.position, leashSegments[0].gameObject.GetComponent<LeashSegment>().nextSegment.position - gameObject.transform.position, out RaycastHit hit, Vector3.Distance(gameObject.transform.position, leashSegments[0].gameObject.GetComponent<LeashSegment>().nextSegment.position), intersectableObjects))
        {
            if(Vector3.Distance(hit.point, leashSegments[0].gameObject.GetComponent<LeashSegment>().nextSegment.position) > 0.2)
            {
                // // Debug.Log("Hit something");
                // Debug.Log(hit.transform.name);
                // GameObject thing = Instantiate(testPrefab, hit.point, Quaternion.identity);
                // Destroy(thing, 1f);
                return;
            }
        }
        

        // Debug.Log("No hit so we remove that shiii");

        if (leashSegments.Count == 1)
        {
            Vector3 dogToSegment = leashSegments[0].transform.position - gameObject.transform.position;
            Vector3 segmentToHuman = leashTarget.position - leashSegments[0].transform.position;

            float angle = Vector3.Angle(dogToSegment, segmentToHuman);

            Debug.Log("AngleDog::" + angle);
            // If the angle is too small, do not remove the segment
            if (angle > 15)
            {
                return;
            }

            GameObject myBuffer = leashSegments[0];
            leashSegments.RemoveAt(0);
            Destroy(myBuffer);
            return;
        }


        leashSegments[1].GetComponent<LeashSegment>().previousSegment = gameObject.transform;
        GameObject buffer = leashSegments[0];
        leashSegments.RemoveAt(0);
        Destroy(buffer);
    }

    void UpdateLeashSegmentsHumanSide()
    {
        if(Physics.Raycast(leashTarget.position, leashSegments[leashSegments.Count - 1].transform.position - leashTarget.position,  out RaycastHit hit, Vector3.Distance(leashTarget.position, leashSegments[leashSegments.Count - 1].transform.position), intersectableObjects))
        {
            //If we hit something between the human and the first leash segment we check for the angle and then maybe create a new leash segment
            // Debug.Log("Hit something");

            if(Vector3.Distance(leashSegments[leashSegments.Count-1].transform.position, hit.point) > leashSegmentLengthMinimum)
            {
                if(Vector3.Angle(leashSegments[leashSegments.Count - 1].GetComponent<LeashSegment>().previousSegment.position - leashSegments[0].transform.position, hit.point - leashSegments[0].transform.position) < leashSegmentAngle)
                {
                    // Debug.Log("Angle is too small Human Side");
                    return;
                }

                // Vector3 offset = (leashTarget.position - hit.point).normalized * 0.3f; // Adjust the offset value as needed
                GameObject newLeashSegment = Instantiate(LeashSegmentPrefab, hit.point, Quaternion.identity);
                PopulateLeashSegment(newLeashSegment, false);
                
            }
            else
            {
                // Debug.Log("Too close to the first segment");
            }
        }
        else
        {
            // Debug.Log("Hit nothing");
        }
    }

    void CheckLeashSegmentHumanSide()
    {
        if(leashSegments.Count < 1)
        {
            return;
        }
        if(Physics.Raycast(leashTarget.position, leashSegments[leashSegments.Count - 1].gameObject.GetComponent<LeashSegment>().previousSegment.position - leashTarget.position, out RaycastHit hit, Vector3.Distance(leashTarget.position, leashSegments[leashSegments.Count - 1].gameObject.GetComponent<LeashSegment>().previousSegment.position), intersectableObjects))
        {
            if(Vector3.Distance(hit.point, leashSegments[leashSegments.Count - 1].gameObject.GetComponent<LeashSegment>().previousSegment.position) > 0.2)
            {
                // GameObject thing = Instantiate(testPrefab, hit.point, Quaternion.identity);
                // Destroy(thing, 1f);
                // Debug.Log("Hit something");
                return;
            }

        }

        
        
        // Debug.Log("No hit so we remove that shiii");

        if (leashSegments.Count == 1)
        {
            Vector3 humanToSegment = leashSegments[leashSegments.Count - 1].transform.position - leashTarget.position;
            Vector3 segmentToDog = gameObject.transform.position - leashSegments[leashSegments.Count - 1].transform.position;

            float angle = Vector3.Angle(humanToSegment, segmentToDog);

            Debug.Log("AngleHuman::" + angle);
            // If the angle is too small, do not remove the segment
            if (angle < 85)
            {
                return;
            }

            GameObject myBuffer = leashSegments[leashSegments.Count - 1];
            leashSegments.RemoveAt(leashSegments.Count - 1);
            Destroy(myBuffer);
            return;
        }   

        leashSegments[leashSegments.Count - 2].GetComponent<LeashSegment>().nextSegment = leashTarget;
        GameObject buffer = leashSegments[leashSegments.Count - 1];
        leashSegments.RemoveAt(leashSegments.Count - 1);
        Destroy(buffer);
    }

    void CreateLeashSegments()
    {
        if(Physics.Raycast(transform.position, leashTarget.position - transform.position, out RaycastHit hit, Vector3.Distance(transform.position, leashTarget.position), intersectableObjects))
        {
            // Debug.Log("Hit something");
            GameObject leashSegment = Instantiate(LeashSegmentPrefab, hit.point, Quaternion.identity);
            leashSegments.Add(leashSegment);    
            PopulateLeashSegment(leashSegment, null);
        }
        else
        {
            // Debug.Log("Hit nothing");
        }
    }

    void PopulateLeashSegment(GameObject newLeashSegment, bool? front)
    {


        if(front == false) // if we want to add from the front ie. the dog is the front of the leash
        {
            leashSegments.Add(newLeashSegment);
        }

        if(front == true) // if we want to add from the back ie. the human is the back
        {
            leashSegments.Insert(0, newLeashSegment);
        }

        if(leashSegments.Count == 1) // if it is the first segment simply populate it with the dogs leash position and the target position since it's in between those 2
        {
            newLeashSegment.GetComponent<LeashSegment>().nextSegment = leashTarget; // the leash ending point on the human needed for checking for intersections
            newLeashSegment.GetComponent<LeashSegment>().previousSegment = gameObject.transform; // the previous segment is the one attached to the dog
            return;
        }

        if(leashSegments.Count > 1 && front == true) // if we already have leash segments and want to add from the front ie. the dog is the front of the leash
        {
            newLeashSegment.GetComponent<LeashSegment>().nextSegment = leashSegments[1].transform; // add the previous leash segment as the next segment
            newLeashSegment.GetComponent<LeashSegment>().previousSegment = gameObject.transform;

            leashSegments[1].GetComponent<LeashSegment>().previousSegment = newLeashSegment.transform; // update the other segment to reflect changes
        }

        if(leashSegments.Count > 1 && front == false) // if we already have leash segments and want to add from the back ie. the human is the back
        {
            newLeashSegment.GetComponent<LeashSegment>().nextSegment = leashTarget; // the leash ending point on the human needed for checking for intersections
            newLeashSegment.GetComponent<LeashSegment>().previousSegment = leashSegments[leashSegments.Count - 2].transform; // add the previous leash segment as the previous segment

            leashSegments[leashSegments.Count - 2].GetComponent<LeashSegment>().nextSegment = newLeashSegment.transform; // update the other segment to reflect changes
        }


    }


    void UpdateLineRenderer()
    {
        if(leashSegments.Count == 0)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, gameObject.transform.position);
            lineRenderer.SetPosition(1, leashTarget.position);

            // Debug.Log("No leash segments");
            return;
        }
        // Debug.Log("Updating line renderer1");
        lineRenderer.positionCount = leashSegments.Count + 2;
        lineRenderer.SetPosition(0, gameObject.transform.position);
        
        for(int i = 0; i < leashSegments.Count; i++)
        {
            // Debug.Log("Updating line renderer::" + i);
            lineRenderer.SetPosition(i + 1, leashSegments[i].transform.position);
            if(i == leashSegments.Count - 1)
            {
                // Debug.Log("Last segment");
                lineRenderer.SetPosition(i + 2, leashTarget.position);
            }
        }

    }

    

    void ApplyPhysics()
    {
        currentLength = 0f;
        if(leashSegments.Count == 0)
        {
            currentLength += Vector3.Distance(gameObject.transform.position, leashTarget.position);
        }
        else
        {
            currentLength += Vector3.Distance(gameObject.transform.position, leashSegments[0].transform.position);
        }

        for(int i = 0; i <leashSegments.Count; i++)
        {
            currentLength += Vector3.Distance(leashSegments[i].transform.position, leashSegments[i].GetComponent<LeashSegment>().nextSegment.position);

        }

        if(currentLength > maxLeashLength)
        {
            if(leashSegments.Count > 0)
            {
                myDogRigidbody.AddForce((leashSegments[0].transform.position - gameObject.transform.position).normalized * leashPullForce * currentLength);
                humanRigidbody.AddForce((leashSegments[leashSegments.Count - 1].transform.position - leashTarget.position).normalized * humanPullForce);
            }
            else
            {
                myDogRigidbody.AddForce((leashTarget.position - gameObject.transform.position).normalized * leashPullForce * currentLength);
                humanRigidbody.AddForce((gameObject.transform.position - leashTarget.position).normalized * humanPullForce);
            }

            

            
        }

        // Debug.Log("Current length::" + currentLength); 
    }


    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;

    //     // Draw raycast from dog to first leash segment
    //     if (leashSegments.Count > 0)
    //     {
    //         Gizmos.DrawLine(gameObject.transform.position, leashSegments[0].transform.position);
    //     }

    //     // Draw raycast from human to last leash segment
    //     if (leashSegments.Count > 0)
    //     {
    //         Gizmos.DrawLine(leashTarget.position, leashSegments[leashSegments.Count - 1].transform.position);
    //     }

    //     // Draw raycasts between leash segments
    //     for (int i = 0; i < leashSegments.Count - 1; i++)
    //     {
    //         Gizmos.DrawLine(leashSegments[i].transform.position, leashSegments[i + 1].transform.position);
    //     }

    //     // Draw raycast from dog to human if no leash segments
    //     if (leashSegments.Count == 0)
    //     {
    //         Gizmos.DrawLine(gameObject.transform.position, leashTarget.position);
    //     }

    //     if (leashSegments.Count > 1)
    //     {
    //         Gizmos.DrawLine(gameObject.transform.position, leashSegments[0].GetComponent<LeashSegment>().nextSegment.position);
    //         Gizmos.DrawLine(leashTarget.position, leashSegments[leashSegments.Count - 1].GetComponent<LeashSegment>().previousSegment.position);
    //     }
    // }
    
        
}
