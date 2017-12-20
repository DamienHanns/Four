using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour {

    public Action[] actions;
    public Transition[] transitions;

    public Color stateIndicatorColour;

     public void ExecuteState(StateController controller)
    {
        PerformActions(controller);
        CheckTransitions(controller);
    }
  
    void PerformActions(StateController controller)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].Act(controller);
        }
    }

    void CheckTransitions(StateController controller)
    {
        for (int i = 0; i < transitions.Length; i++)
        {
            bool bDecisionReturnsTrue = transitions[i].decision.Decide(controller);

            if (bDecisionReturnsTrue)
            {
                controller.TransitionToState(transitions[i].trueState);
            } else
            {
                controller.TransitionToState(transitions[i].falseState);
            }
        }
    }
}
