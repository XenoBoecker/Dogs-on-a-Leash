using UnityEngine;

public class CamJustMove : MonoBehaviour
{

    [SerializeField] float speed = 10;

    private void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime) ;
    }
}