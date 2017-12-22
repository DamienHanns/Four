using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseAction : Action {

    public override void Act(StateController controller)
    {
        if ( ! controller.bHasPath) { StartCoroutine(Chase(controller)); }
    }

    IEnumerator Chase(StateController controller)
    {
        float repathTime = 0.25f;

        controller.bHasPath = true;

        while (controller.bHasPath)
        {
            if (controller.priorityOOI != null)
            {
                controller.navMeshAgent.SetDestination(controller.priorityOOI.position);
            }

            yield return new WaitForSeconds(repathTime);
        }
    }

    //checkLOS;

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
