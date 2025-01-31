using UnityEngine;

public class MovingText : MonoBehaviour
{

    float speed;

    bool isDone;

    private void Update()
    {
        if (isDone) return;

        if (transform.position.x < 0 && speed < 0 || transform.position.x > 0 && speed > 0)
        {
            isDone = true;

            transform.position = new Vector3(0, transform.position.y, 0);
            return;
        }

        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
}