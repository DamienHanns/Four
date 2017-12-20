using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveStateDecision : Decision {

    public override bool Decide(StateController controller)
    {
        bool bPriorityOOIActive = controller.priorityOOI.gameObject.activeSelf;
        return bPriorityOOIActive;
    }


}
