using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    protected DogController dog;
    
    float chargeNeeded = 5;

    float currentCharge = Mathf.Infinity;

    protected virtual void Awake()
    {
        dog = GetComponent<DogController>();
    }

    // Update is called once per frame
    void Update()
    {
        currentCharge += Time.deltaTime;
    }

    public void UseAbility()
    {
        if (currentCharge < chargeNeeded) return;

        ActivateAbility();
    }

    protected virtual void ActivateAbility()
    {

        currentCharge = 0;
    }

    public float CurrentChargePercentage()
    {
        return currentCharge / chargeNeeded;
    }
}
