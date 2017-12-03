using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))] [RequireComponent (typeof (NavMeshAgent))]
public class EnemySlime : GameObject {

    enum State { Attacking, Chasing, Idle };
    State state;

    public float moveSpeed = 10.0f;

    [Header ("Charge Attack Settings")]
    [SerializeField] float chargeSpeedMultiplyer = 3.0f; [SerializeField] float totalChargeTime = 2.0f;
    [SerializeField] float aggroDis = 4.0f; float chargeTimer = 0.0f; float attackCD = 3.0f;
    [SerializeField]  float totalStallTime = 2.0f; float stallTimer; float timeToNextAttack;

    NavMeshAgent pathfinder;
    Rigidbody myRb;
    Transform currentTarget;
    private bool bIsAttackingTarget;

    // Use this for initialization
    protected override void Start () {
        base.Start();
        myRb = GetComponent<Rigidbody>();
        pathfinder = GetComponent<NavMeshAgent>();
        currentTarget = FindObjectOfType<PlayerController>().transform;
        state = State.Chasing;
        StartCoroutine(UpdatePath());
       
    }

    private void Update()
    {
        float disToTarget = Vector3.Distance (transform.position, currentTarget.position);
      
        if (disToTarget < aggroDis && state != State.Attacking && Time.time > timeToNextAttack )    //TODO is the bool check than the enum? 
        {
            bIsAttackingTarget = true;                              //set attacking variables
            stallTimer = totalStallTime + Time.time;
            state = State.Attacking;
            Vector3 dirToTarget = (currentTarget.position - transform.position).normalized;        //normalise to keep consistance speed when passed to co-rutine
            
            pathfinder.isStopped = true;                            //disable pathfinder stuff before moving with rb;
            pathfinder.enabled = false;

            StartCoroutine(InitateAttack(dirToTarget)) ;
        }
        else if ( state == State.Chasing )
        {
            transform.LookAt(currentTarget);
        }
    }

    IEnumerator InitateAttack(Vector3 dirToTarget)
    {
        while  (stallTimer > Time.time)
        {
            yield return new WaitForEndOfFrame();
        }
        chargeTimer = totalChargeTime + Time.time;
        StartCoroutine(ChargeTarget(dirToTarget));
        StopCoroutine(InitateAttack(Vector3.zero));
    }

    IEnumerator ChargeTarget(Vector3 dirToTarget)
    {
        StopCoroutine(InitateAttack(Vector3.zero));

        Vector3 velocity = dirToTarget * moveSpeed;
        Vector3 chargeVelocity = ((velocity) * chargeSpeedMultiplyer);

        myRb.isKinematic = false;
        while (chargeTimer > Time.time)
        {
            myRb.velocity =  (chargeVelocity * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }

        timeToNextAttack = Time.time + attackCD;

        myRb.isKinematic = true;
        bIsAttackingTarget = false;
        state = State.Chasing;
        pathfinder.enabled = true;
        StopCoroutine(ChargeTarget(Vector3.zero));
    }

    IEnumerator UpdatePath()
    {
        float repathTime = 0.25f;

        while (currentTarget != null)
        {
            if (state == State.Chasing)
            {
                pathfinder.SetDestination(currentTarget.position);
            }

            yield return new WaitForSeconds(repathTime);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, aggroDis);
    }
}
