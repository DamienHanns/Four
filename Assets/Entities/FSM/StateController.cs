using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateController : MonoBehaviour
{
    public event System.Action OnExitState;

    public State currentState, remainInState;
    public Transform stateIndicatorHolder;
    public bool bIsCyclicalPath;

    [HideInInspector] public Rigidbody myrb;

    //pathing
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public Vector3[] waypoints;
    [HideInInspector] public int nextWaypointIndex;
    [HideInInspector] public bool bHasPath;

    [HideInInspector] public float stateTimeElapsed;

    //FOV
    [HideInInspector] public FOV fov;
    [HideInInspector] public List<Transform> visableOOI;
    public Transform priorityOOI;
    
    public Transform waypointHolder;        //TODO hide/remove this when waypoint debug not needed

    bool bAIActive;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        fov = GetComponent<FOV>();

        if (GetComponent<Rigidbody>() != null) { myrb = GetComponent<Rigidbody>(); }
    }

    private void Update()
    {
        if (!bAIActive) { return; }
        currentState.ExecuteState(this);
        print(currentState);
    }

    public void SetUpAI(bool bIsActive, Transform _waypointHolder)
    {
        bAIActive = bIsActive;
        GetWaypoints(waypointHolder);

        if (bAIActive)
        {
            navMeshAgent.enabled = true;
            currentState.gameObject.SetActive(true);
        }
        else
        {
            navMeshAgent.enabled = false;
        }
    }

    public void TransitionToState(State nextState)
    {
        if (nextState != remainInState)
        {
            ExitState();
            currentState = nextState;
            currentState.gameObject.SetActive(true);
        }
    }

    public bool CheckIfCountDownElapsed(float duration)
    {
        stateTimeElapsed += Time.deltaTime;
        return (stateTimeElapsed > duration);
    }

    void ExitState()     
    {
        if (OnExitState != null)
        {
            OnExitState();
        }

        bHasPath = false;
        currentState.StopAllCoroutines();
        currentState.gameObject.SetActive(false);
        stateTimeElapsed = 0.0f;
    }

    void GetWaypoints(Transform waypointHolder)
    {
        waypoints = new Vector3[waypointHolder.childCount];
        for (int i = 0; i < waypoints.Length; ++i)
        {
            waypoints[i] = waypointHolder.GetChild(i).position;
            waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z);
        }
    }

    void OnDrawGizmos()
    {
        if (currentState != null && stateIndicatorHolder != null)
        {
            Gizmos.color = currentState.stateIndicatorColour;
            Gizmos.DrawSphere(stateIndicatorHolder.position, 0.25f);
        }

        if (waypointHolder != null)
        {
            Vector3 startPos = waypointHolder.GetChild(0).position;
            Vector3 privousPos = startPos;

            Gizmos.color = Color.blue;

            foreach (Transform waypoint in waypointHolder)
            {
                Gizmos.DrawSphere(waypoint.position, 0.3f);
                Gizmos.DrawLine(privousPos, waypoint.position);

                privousPos = waypoint.position;
            }

            if (bIsCyclicalPath) { Gizmos.DrawLine(privousPos, startPos); }
        }
    }
}
