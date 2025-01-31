using UnityEngine;

public class CreditsDog : MonoBehaviour
{
    DogVisuals visual;
    Animator anim;


    [SerializeField] bool goLeft = true;
    [SerializeField] Vector3 speed;


    [SerializeField] GameObject[] shadows;

    [SerializeField] MovingText movingText;


    [SerializeField] float offsetDistance = 2.5f;

    Vector3 startPos, movingTextStartPos;

    static int lastDogIndex = -1;

    private void Start()
    {
        visual = GetComponentInChildren<DogVisuals>();

        if (goLeft)
        {
            speed = new Vector3(-speed.x, speed.y, speed.z);
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }

        movingText.SetSpeed(speed.x);

        float dist = Mathf.Abs(speed.x) * (-transform.position.y / speed.y + offsetDistance);

        if(speed.x < 0)
        {
            transform.Translate(Vector3.right * dist, Space.World);
            movingText.transform.Translate(Vector3.right * dist, Space.World);
        }
        else
        {
            transform.Translate(Vector3.left * dist, Space.World);
            movingText.transform.Translate(Vector3.left * dist, Space.World);
        }

        startPos = transform.position;
        movingTextStartPos = movingText.transform.position;
    }

    public void ResetToStartPosition()
    {
        transform.position = startPos;
        movingText.transform.position = new Vector3(movingTextStartPos.x, movingText.transform.position.y, 0);
        movingText.SetIsDone(false);

        SetupRandomDog();
    }

    public void SetupRandomDog()
    {

        int dogID = Random.Range(0, 4);

        while (dogID == lastDogIndex) dogID = Random.Range(0, 4);
        lastDogIndex = dogID;
        Debug.Log("ID: " + dogID);

        for (int i = 0; i < shadows.Length; i++)
        {
            if (i == dogID) shadows[i].SetActive(true);
            else shadows[i].SetActive(false);
        }

        visual.SetDogID(dogID);
        visual.SetColorIndex(Random.Range(0, 4));
        visual.SetAccessorieIndex(Random.Range(0, visual.AccessorieCount));

        anim = GetComponentInChildren<Animator>();

        anim.SetTrigger("OnStartWalking");
        anim.SetBool("Walking", true);

        anim.SetFloat("MovSpeedPercentage", 1);
    }

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime, Space.World);
    }
}
