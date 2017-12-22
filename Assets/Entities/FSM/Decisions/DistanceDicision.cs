using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceDicision : Decision {

    [SerializeField] float attackingDistance = 5.0f;

    public override bool Decide(StateController controller)
    {
        bool withinDistance = CheckDistance(controller);
        return withinDistance;
    }

    bool CheckDistance(StateController controller)
    {
        float disToTarget = Vector3.Distance(controller.transform.position, controller.priorityOOI.position);

        if (disToTarget < attackingDistance && controller.priorityOOI.gameObject.activeSelf != false) 
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackingDistance);
    }
}
