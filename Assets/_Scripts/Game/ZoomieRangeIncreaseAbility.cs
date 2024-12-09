using UnityEngine;

public class ZoomieRangeIncreaseAbility : Ability
{

    [SerializeField] float dashForce = 10;


    [SerializeField] float rangeIncrease = 1.2f;

    [SerializeField] float abilityDuration = 1f;

    float baseMaxDistance;

    protected override void Awake()
    {
        base.Awake();

        dog.OnZoomieStart += UseAbility;
    }

    private void Start()
    {

        baseMaxDistance = dog.GetComponent<SpringJoint>().maxDistance;
    }
    
    protected override void ActivateAbility()
    {
        base.ActivateAbility();

        dog.GetComponent<SpringJoint>().maxDistance = baseMaxDistance * rangeIncrease;

        dog.GetComponent<Rigidbody>().AddForce(dog.transform.forward * dashForce, ForceMode.Impulse);

        Invoke(nameof(Deactivate), abilityDuration);
    }

    void Deactivate()
    {
        dog.GetComponent<SpringJoint>().maxDistance = baseMaxDistance;

        dog.GetComponent<DogController>().enabled = false;

        Invoke(nameof(ReactivateDogController), 0.2f);
    }

    void ReactivateDogController()
    {
        dog.GetComponent<DogController>().enabled = true;
    }
}