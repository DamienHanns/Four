using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerRunOutDecision : Decision {

    [SerializeField] float setTimer = 5.0f;
    float timeLeft; 

    public override bool Decide(StateController controller)
    {
        bool bHasTimeRunOut= Timer(controller);
        return bHasTimeRunOut;
    }

    bool Timer(StateController controller)
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            return false;
        }
        else
        {
            return true;
        }
    }

    public void ResetTimer()
    {
        timeLeft = setTimer;
    }

    private void OnEnable()
    {
        timeLeft = setTimer;
    }
}
