using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.VFX;

public class HumanMovement : MonoBehaviour
{

    [SerializeField] Vector3 debugRBVel;

    public Transform LeashAttachmentPoint;


    [SerializeField] float startWalkingDelay = 3f;
    [SerializeField] float minSpeed = 1f;

    [SerializeField] float acceleration = 2f;
    public float speed = 5f; // Speed of movement
    public float rotationSpeed = 5f; // Speed of rotation
    private Rigidbody rb; // Rigidbody for physical movement

    float stunTime;
    bool isStunned => stunTime > 0;

    public bool IsOnStreet;

    public int BumpedCount;


    [SerializeField] VisualEffect bumpPointLossVFX, stunVFX;

    [SerializeField] CanvasGroup bumpVignette;
    [SerializeField] AnimationCurve vignettePopCurve;
    [SerializeField] float vignetteDuration;

    [SerializeField] float grassShaderActivationHeight = 2.5f;

    public event Action<Obstacle> OnHitObstacle;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("No Rigidbody component found on this GameObject. Please attach one.");
        }

        bumpPointLossVFX.Stop();
        stunVFX.Stop();
        stunVFX.gameObject.SetActive(false);
    }
    private void Update()
    {
        Shader.SetGlobalVector("_Human", transform.position + Vector3.up * grassShaderActivationHeight);
    }

    void FixedUpdate()
    {
        startWalkingDelay -= Time.deltaTime;
        if (startWalkingDelay > 0) return;

        if (PhotonNetwork.IsConnected && !PhotonNetwork.IsMasterClient) return;

        stunTime -= Time.fixedDeltaTime;
        
        rb.rotation = Quaternion.Euler(0, rb.rotation.eulerAngles.y, 0); // constrain to only y rotation

        if (isStunned) return;

        stunVFX.Stop();
        stunVFX.gameObject.SetActive(false);

        MoveForward();
    }

    private void MoveForward()
    {
        if (PhotonNetwork.IsConnected && !PhotonNetwork.IsMasterClient) return;

        if (isStunned)
        {
            return;
        }

        Vector3 direction = Vector3.right;

        if (rb.velocity.x < speed)
        {
            rb.AddForce(acceleration * direction);
        }

        debugRBVel = rb.velocity;

        if (rb.velocity.x < minSpeed)
        {
            rb.velocity = new Vector3(minSpeed, rb.velocity.y, rb.velocity.z);
            rb.AddForce(50 * Vector3.up);
        }

        // decrease z velocity

        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z * 0.99f);


        Quaternion targetRotation = Quaternion.LookRotation(direction);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed));

        // AddSidewaysForce();
    }

    public void ObstacleCollision(Obstacle obstacle)
    {
        // Debug.Log("Human obstacle collision with " + obstacle.name + ": stunTime = " + obstacle.stunTime + "; force = " + obstacle.CurrentPushBackForce * rb.mass);

        Vector3 dir = (transform.position - obstacle.transform.position).normalized;

        dir.y = 0;

        bumpPointLossVFX.Play();

        Stun(obstacle.stunTime);

        rb.AddForce(dir * obstacle.CurrentPushBackForce * rb.mass, ForceMode.Impulse);

        BumpedCount++;

        StartCoroutine(BumpVignettePopCoroutine());

        OnHitObstacle?.Invoke(obstacle);
    }


    IEnumerator BumpVignettePopCoroutine()
    {
        for (float i = 0; i < vignetteDuration; i+=Time.unscaledDeltaTime)
        {
            bumpVignette.alpha = vignettePopCurve.Evaluate(i / vignetteDuration);
            yield return null;
        }
        bumpVignette.alpha = 0;
    }

    void Stun(float stunTime)
    {
        this.stunTime = stunTime;

        stunVFX.gameObject.SetActive(true);
        stunVFX.Play();
    }

    internal void SetIsOnStreet(bool v)
    {
        Debug.Log("IsOnStreet: " + v);
        IsOnStreet = v;
    }
}