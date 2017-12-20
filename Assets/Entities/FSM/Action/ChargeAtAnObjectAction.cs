using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAtAnObjectAction : Action {

    [SerializeField] float totalStallTime = 1.5f;
    float stallTimer = 0.0f;
    [SerializeField] float totalChargeTime = 2.0f;
    [SerializeField] float chargeSpeedMultiplyer = 10.0f;
    float chargeTimer;
    float waitTimer = 0.0f;
    public bool bIsAttackingTarget;

    public override void Act(StateController controller)
    {
        ChargeAtObject(controller);
    }

    void ChargeAtObject(StateController controller)
    {
        if (!bIsAttackingTarget && Time.time > waitTimer) StartCoroutine (InitateAttack(controller));
    }

     IEnumerator InitateAttack(StateController controller)
    {
            Vector3 dirToTarget = controller.priorityOOI.position - controller.transform.position;

            stallTimer = totalStallTime + Time.time;
            bIsAttackingTarget = true;
            controller.navMeshAgent.enabled = false;

            while (stallTimer > Time.time)
            {
                yield return new WaitForEndOfFrame();
            }
            chargeTimer = totalChargeTime + Time.time;
            StartCoroutine(ChargeTarget(controller, dirToTarget));
    }

    IEnumerator ChargeTarget(StateController controller, Vector3 dirToTarget)
    {
        Vector3 velocity = (dirToTarget * controller.navMeshAgent.speed);
        Vector3 chargeVelocity = ((velocity) * chargeSpeedMultiplyer);

        controller.myrb.isKinematic = false;
        while (chargeTimer > Time.time)
        {
            controller.myrb.velocity = (chargeVelocity * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }

        waitTimer = 100.0f + Time.time;

        controller.myrb.isKinematic = true;
        bIsAttackingTarget = false;
        controller.navMeshAgent.enabled = true;
    }

    private void OnDisable()
    {
        waitTimer = 0.0f;
    }
}
