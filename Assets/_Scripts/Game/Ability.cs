using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    float chargeNeeded = 5;

    float currentCharge = Mathf.Infinity;

    // Start is called before the first frame update
    void Start()
    {
        
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
}
