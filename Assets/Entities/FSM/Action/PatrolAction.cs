using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolAction : Action
{
    [SerializeField] float pauseTimeAtEndOfRoute  = 0.0f;

    public override void Act(StateController controller)
    {
        if ( ! controller.bHasPath)  { StartCoroutine(Patrol(controller)); }
    }

    IEnumerator Patrol(StateController controller)
    {
        float repathTime = 0.25f;
        //  pathfinder.Warp(waypoints[0]);

        Vector3 nextWaypoint = controller.waypoints[controller.nextWaypointIndex];

        controller.bHasPath = true;
        while (controller.bHasPath) 
        {
            float disToNextWaypoint = Vector3.Distance(controller.transform.position, nextWaypoint);

            if (disToNextWaypoint <= controller.navMeshAgent.stoppingDistance)
            {
                if (controller.bIsCyclicalPath)             //TODO switch on and off after a number patrols to introduce randomness
                {
                    int priviousWaypointIndex = controller.nextWaypointIndex;
                    controller.nextWaypointIndex++;

                    if (controller.nextWaypointIndex >= controller.waypoints.Length)
                    {
                        controller.nextWaypointIndex = 0;
                        yield return new WaitForSeconds(pauseTimeAtEndOfRoute);
                    }

                    nextWaypoint = controller.waypoints[controller.nextWaypointIndex];

                }
                else
                {
                    controller.nextWaypointIndex++;

                    if (controller.nextWaypointIndex >= controller.waypoints.Length)
                    {
                        controller.nextWaypointIndex = 0;
                        System.Array.Reverse(controller.waypoints);
                        yield return new WaitForSeconds(pauseTimeAtEndOfRoute);
                    }
                    nextWaypoint = controller.waypoints[controller.nextWaypointIndex];

                }
               
            }

            if (controller.navMeshAgent.velocity.magnitude < 0.1f) { controller.navMeshAgent.SetDestination(nextWaypoint); }

            if (controller.navMeshAgent.isOnNavMesh)
            {
                controller.navMeshAgent.SetDestination(nextWaypoint);
            } else
            {
                print(gameObject.name + " is not on nav mesh");
            }
            yield return new WaitForSeconds(repathTime);

        }
    }
}
