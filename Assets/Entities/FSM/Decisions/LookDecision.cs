using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookDecision : Decision {

    public override bool Decide(StateController controller)
    {
        bool bCanSeeOOI = Look(controller);
        return bCanSeeOOI;
    }

    bool Look(StateController controller)
    {
        if (controller.fov.bIsTargetInLOS)
        {
            CheckOOIs(controller);
            return true;
        }
        return false;
    }

    void CheckOOIs(StateController controller) // TODO add a list of target priorites and choose based on the prioites what action to take next 
    {
        foreach (Transform target in controller.fov.visableTagets)
         {
             if (target != null)
             {
                 if (target.tag == "Player") { controller.priorityOOI = target;  }
             }
        }
    }
}
