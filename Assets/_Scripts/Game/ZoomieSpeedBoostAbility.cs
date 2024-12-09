using UnityEngine;

public class ZoomieSpeedBoostAbility : Ability
{

    [SerializeField] float abilityDuration = 3f;


    [SerializeField] float speedMultiplier = 1.5f;

    protected override void Awake()
    {
        base.Awake();

        dog.OnZoomieStart += UseAbility;
    }
    protected override void ActivateAbility()
    {
        base.ActivateAbility();

        dog.speedMultiplier = speedMultiplier;


        Debug.Log("Zoomie now");

        Invoke(nameof(EndZoomie), abilityDuration);
    }

    void EndZoomie()
    {
        Debug.Log("End Zoomie");

        dog.speedMultiplier = 1f;
    }
}
