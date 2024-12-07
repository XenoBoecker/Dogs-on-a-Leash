using UnityEngine;

public class ZoomieAbility : Ability
{
    DogController dog;

    [SerializeField] float abilityDuration = 3f;

    private void Awake()
    {
        dog = GetComponent<DogController>();

        dog.OnZoomieStart += UseAbility;
    }
    protected override void ActivateAbility()
    {
        base.ActivateAbility();

        dog.speedMultiplier = 1.5f;


        Debug.Log("Zoomie now");

        Invoke(nameof(EndZoomie), abilityDuration);
    }

    void EndZoomie()
    {
        Debug.Log("End Zoomie");

        dog.speedMultiplier = 1f;
    }
}