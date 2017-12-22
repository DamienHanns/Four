using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyCharger : Enemy
{
    public Transform waypointHolder;

    Rigidbody myrb;
    NavMeshAgent navMeshAgent;
    StateController stateController;

    public float chargeTimer, totalChargeTime = 2.0f, stallTimer, totalStallTime = 2.0f,
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
}
