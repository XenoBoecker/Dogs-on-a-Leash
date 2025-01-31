using UnityEngine;

public class CreditsDog : MonoBehaviour
{
    DogVisuals visual;
    Animator anim;


    [SerializeField] Vector3 speed;


    [SerializeField] GameObject[] shadows;

    [SerializeField] MovingText movingText;


    [SerializeField] float offsetDistance = 2.5f;

    private void Start()
    {
        int dogID = Random.Range(0, 4);

        for (int i = 0; i < shadows.Length; i++)
        {
            if (i == dogID) shadows[i].SetActive(true);
            else shadows[i].SetActive(false);
        }

        visual = GetComponentInChildren<DogVisuals>();

        visual.SetDogID(dogID);
        visual.SetColorIndex(Random.Range(0, 4));
        visual.SetAccessorieIndex(Random.Range(0, visual.AccessorieCount));

        anim = GetComponentInChildren<Animator>();

        anim.SetTrigger("OnStartWalking");
        anim.SetBool("Walking", true);

        anim.SetFloat("MovSpeedPercentage", 1);

        movingText.SetSpeed(speed.x);

        float dist = 10 + Mathf.Abs(transform.position.x) + Mathf.Abs(speed.x) * -transform.position.y + offsetDistance;

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
    }

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime, Space.World);
    }
}
