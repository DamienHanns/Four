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

    void GoToNextWaypoint(StateController controller, Vector3 nextWaypoint)
    {
        if (controller.navMeshAgent.isOnNavMesh)
        {
            controller.navMeshAgent.SetDestination(nextWaypoint);
        }
        else
        {
            print(gameObject.name + " is not on nav mesh");
        }
    }

    IEnumerator Patrol(StateController controller)
    {
        float repathTime = 0.25f;

        Vector3 nextWaypoint = controller.waypoints[controller.nextWaypointIndex];
        Vector3 previousWaypoint = Vector3.zero;

        controller.bHasPath = true;
        while (controller.bHasPath)
        {
            //   float disToNextWaypoint = Vector3.Distance(controller.transform.position, nextWaypoint);

            if (controller.navMeshAgent.remainingDistance <= controller.navMeshAgent.stoppingDistance)
            {
                if (controller.bIsCyclicalPath)             //TODO switch on and off after a number patrols to introduce randomness
                {
                    controller.nextWaypointIndex++;

                    if (controller.nextWaypointIndex >= controller.waypoints.Length)
                    {
                        controller.nextWaypointIndex = 0;
                        yield return new WaitForSeconds(pauseTimeAtEndOfRoute);
                    }

                    nextWaypoint = controller.waypoints[controller.nextWaypointIndex];
                    GoToNextWaypoint(controller, nextWaypoint);

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
                    GoToNextWaypoint(controller, nextWaypoint);
                }
            }
            yield return new WaitForSeconds(repathTime);

        }
    }
}
