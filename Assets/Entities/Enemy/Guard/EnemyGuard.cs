using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyGuard : Enemy
{
    public Transform waypointHolder;

    Rigidbody myrb;
    NavMeshAgent navMeshAgent;
    StateController stateController;

    bool bIsAttackingTarget; public float chargeTimer, totalChargeTime = 2.0f, stallTimer, totalStallTime = 2.0f,
        timeToNextAttack, moveSpeed = 10.0f, chargeSpeedMultiplyer = 20.0f, attackCD = 3.0f;

    protected override void Start()
    {
        base.Start();
        stateController = GetComponent<StateController>();
        stateController.SetUpAI(true, waypointHolder);

        navMeshAgent = GetComponent<NavMeshAgent>();
        myrb = GetComponent<Rigidbody>();
        myrb.isKinematic = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
        }
    }

    IEnumerator InitateAttack(Vector3 dirToTarget)
    {
        bIsAttackingTarget = true;

        while (stallTimer > Time.time)
        {
            yield return new WaitForEndOfFrame();
        }
        chargeTimer = totalChargeTime + Time.time;
        StartCoroutine(ChargeTarget(dirToTarget));

    }

    IEnumerator ChargeTarget(Vector3 dirToTarget)
    {
        Vector3 velocity = dirToTarget * moveSpeed;
        Vector3 chargeVelocity = ((velocity) * chargeSpeedMultiplyer);

        myrb.isKinematic = false;
        while (chargeTimer > Time.time)
        {
            myrb.velocity = (chargeVelocity * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }

        timeToNextAttack = Time.time + attackCD;

        myrb.isKinematic = true;
        bIsAttackingTarget = false;
        navMeshAgent.enabled = true;
    }
}
