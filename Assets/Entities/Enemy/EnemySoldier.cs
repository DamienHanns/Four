using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))] [RequireComponent(typeof(NavMeshAgent))]
public class EnemySoldier : Enemy {

    enum State { Attacking, Chasing, Patroling };
    State state;

    [Header("Pathfinding")]
    public float moveSpeed = 10.0f;
    public Transform waypointHolder;        //TODO create data type waypoints for greater customisation of indervidual waypoints
    [SerializeField] bool bIsCyclicalPath;

    [Header("Charge Attack Settings")]
    [SerializeField] float chargeSpeedMultiplyer = 3.0f;[SerializeField] float totalChargeTime = 2.0f;
    [SerializeField] float aggroDis = 4.0f; float chargeTimer = 0.0f; float attackCD = 3.0f;
    [SerializeField] float totalStallTime = 2.0f; float stallTimer; float timeToNextAttack;
    float disToTarget;
    bool bIsAttackingTarget;

    NavMeshAgent pathfinder;    FOV fOV;
    Rigidbody myRb;
    Transform currentTarget;

    protected override void Start() {
        base.Start();
        myRb = GetComponent<Rigidbody>();
        pathfinder = GetComponent<NavMeshAgent>();  fOV = GetComponent<FOV>();
        
        pathfinder.speed = moveSpeed;

        state = State.Patroling;

        if (waypointHolder != null)
        {
            print("waypoint holder not null");
            Vector3[] startingWaypoints = GetWaypoints();
            StartCoroutine(UpdatePath(startingWaypoints));
        } 
    }

    void Update()       //TODO react if attacked.
    {
        CheckForTargets();
        if (currentTarget != null)
        {
            if (disToTarget < aggroDis && state != State.Attacking && Time.time > timeToNextAttack)    //TODO is the bool check better than the enum? 
            {
                bIsAttackingTarget = true;                              //set attacking variables
                stallTimer = totalStallTime + Time.time;
                state = State.Attacking;
                Vector3 dirToTarget = (currentTarget.position - transform.position).normalized;        //normalise to keep consistance speed when passed to co-rutine

                if (pathfinder.isOnNavMesh) { pathfinder.isStopped = true;  }      //disable pathfinder stuff before moving with rb;
                pathfinder.enabled = false;

                StartCoroutine(InitateAttack(dirToTarget));
            }
        }
    }

    Vector3[] GetWaypoints()
    {
            if (waypointHolder != null)
        {
            Vector3[] waypoints = new Vector3[waypointHolder.childCount];
            for (int i = 0; i < waypoints.Length; ++i)
            {
                waypoints[i] = waypointHolder.GetChild(i).position;
                waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z);  
            }
            return waypoints;
        }
        return null;
        
    }       

    void CheckForTargets()
        {
            //check list from FOV 

            foreach (Transform target in fOV.visableTagets)
            {
                if (target != null)
                {
                    if (target.tag == "Player") { currentTarget = target; }
                }
            }
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

        myRb.isKinematic = false;
        while (chargeTimer > Time.time)
        {
            myRb.velocity = (chargeVelocity * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }

        timeToNextAttack = Time.time + attackCD;

        myRb.isKinematic = true;
        bIsAttackingTarget = false;
        pathfinder.enabled = true;

        state = State.Chasing;
        print(state);
    }

    IEnumerator UpdatePath(Vector3[] waypoints)    
    {
        float repathTime = 0.25f;  
        pathfinder.Warp(waypoints[0]);
        
        int nextWaypointIndex = 1;
        Vector3 nextWaypoint = waypoints[nextWaypointIndex];
        float pauseTime = 3.0f;

        while (true)    //TODO swtich to a bool so corutine comes to an end if new set of way points arer desired.  
        {
            float stoppingDis = 0.1f;       //TODO need to take into account faraway ypos if ground is raised or lowered.

            if (currentTarget != null) { disToTarget = Vector3.Distance(transform.position, currentTarget.position) ; }
           
            switch (state)
            {
                case State.Patroling:
                    if (Vector3.Distance(transform.position, nextWaypoint) < stoppingDis)
                    {
                        if (bIsCyclicalPath)             //TODO switch on and off after a number patrols to introduce randomness
                        {
                            nextWaypointIndex++;

                            if (nextWaypointIndex >= waypoints.Length)
                            {
                                nextWaypointIndex = 0;
                                yield return new WaitForSeconds(pauseTime);
                            }
                            nextWaypoint = waypoints[nextWaypointIndex];
                        }
                        else 
                        {
                            nextWaypointIndex++;

                            if (nextWaypointIndex >= waypoints.Length)
                            {
                                nextWaypointIndex = 0;
                                System.Array.Reverse(waypoints);
                                yield return new WaitForSeconds(pauseTime);
                            }
                            nextWaypoint = waypoints[nextWaypointIndex];
                        }
                    }
                    break;

                case State.Chasing:
                    if (currentTarget != null)
                    {
                        nextWaypoint = currentTarget.position;
                    }
                    else
                    {
                        state = State.Patroling;
                    }
                    break;

                default:
                    Debug.Log("Pathfinding Switch for " + gameObject.name + " out of range");
                    break;
            }
            if (pathfinder.isOnNavMesh)
            {
                pathfinder.SetDestination(nextWaypoint);
            }
            yield return new WaitForSeconds(repathTime);

        }
    }

    void OnDrawGizmos()
    {
       Gizmos.color = Color.red;
       Gizmos.DrawWireSphere(transform.position, aggroDis);

        if (waypointHolder != null)     
        {
            Vector3 startPos = waypointHolder.GetChild(0).position;
            Vector3 privousPos = startPos;

            Gizmos.color = Color.blue;

            foreach (Transform waypoint in waypointHolder)
            {
                Gizmos.DrawSphere(waypoint.position, 0.3f);
                Gizmos.DrawLine(privousPos, waypoint.position);

                privousPos = waypoint.position;
            }

            if (bIsCyclicalPath) { Gizmos.DrawLine(privousPos, startPos); }
        }
    } 
}
