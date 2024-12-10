using UnityEngine;

public class ZoomieDashAbility : Ability
{

    [SerializeField] float dashForce = 10;
    protected override void Awake()
    {
        base.Awake();

        dog.OnZoomieStart += UseAbility;
    }
    protected override void ActivateAbility()
    {
        base.ActivateAbility();

        dog.GetComponent<Rigidbody>().AddForce(dog.transform.forward * dashForce, ForceMode.Impulse);
    }
}
