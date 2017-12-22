using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : MonoBehaviour
{
    public bool bIsActionFinished;
    public abstract void Act(StateController controller);
}
