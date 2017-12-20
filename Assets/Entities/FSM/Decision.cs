using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Decision : MonoBehaviour {

    public abstract bool Decide(StateController controller);
}
