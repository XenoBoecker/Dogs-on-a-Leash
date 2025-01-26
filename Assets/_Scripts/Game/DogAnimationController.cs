using UnityEngine;

public class DogAnimationController : MonoBehaviour
{
    Animator anim;
    DogStateObserver dogStateTracker;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        dogStateTracker = GetComponent<DogStateObserver>();

        dogStateTracker.OnStartWalking += () => anim.SetBool("Walking", true);
        dogStateTracker.OnStopWalking += () => anim.SetBool("Walking", false);
        dogStateTracker.OnStartWalking += () => anim.SetTrigger("OnStartWalking");
        dogStateTracker.OnStopWalking += () => anim.SetTrigger("OnStopWalking");

        dogStateTracker.OnBark += () => anim.SetTrigger("OnBark");
        dogStateTracker.OnInteract += () => anim.SetTrigger("OnInteract");
    }

    private void Update()
    {
        anim.SetFloat("MovSpeedPercentage", dogStateTracker.CurrentSpeedPercentage);
        anim.SetInteger("LeanDirInt", dogStateTracker.LeanDirInt);
        anim.SetBool("Digging", dogStateTracker.Digging);
    }
}
