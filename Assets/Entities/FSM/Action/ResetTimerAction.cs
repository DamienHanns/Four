using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTimerAction : Action {

    public TimerRunOutDecision timerDecision;
    public Decision resetDecision;

    public override void Act(StateController controller)
    {
            ResetTimerDecision(controller);
    }

    private void ResetTimerDecision(StateController controller)
    {
        if (resetDecision.Decide(controller) == true)
        {
            timerDecision.ResetTimer();
        }
    }

}
