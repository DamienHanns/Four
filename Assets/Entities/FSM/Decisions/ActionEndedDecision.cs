using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEndedDecision : Decision {

    public ChargeAtAnObjectAction action;

    public override bool Decide(StateController controller)
    {
        bool actionEnded = action.bIsCharging;
        return actionEnded;
    }
}
