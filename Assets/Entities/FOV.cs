using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOV : MonoBehaviour {

    [SerializeField] internal float viewRadius;
    [SerializeField] [Range (0.0f, 360.0f)] internal float viewAngle;

    [SerializeField] LayerMask targetMask;
    [SerializeField] LayerMask obsticleMask;

    internal List<Transform> visableTagets = new List<Transform>();
    internal bool bIsTargetInLOS;

    private void Start()
    {
        StartCoroutine(CheckForTargets());
    }

    IEnumerator CheckForTargets()
    {
        float checkRefreshTimer = 0.2f;
        while (true)
        {
            FindVisableTargets();
            yield return new WaitForSeconds(checkRefreshTimer);
        }
    }

    void FindVisableTargets()
    {
        bIsTargetInLOS = false;
        visableTagets.Clear();
       
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; ++i)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float disToTarget = Vector3.Distance(transform.position, target.position);

                if ( ! Physics.Raycast(transform.position, dirToTarget, disToTarget, obsticleMask))
                {
                    visableTagets.Add(target);
                    bIsTargetInLOS = true;
                }
            }
        }
    }
    
    internal Vector3 DirFromAngle(float angleInDegrees, bool bIsAngleGlobal)
    {
        if ( ! bIsAngleGlobal )
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin (angleInDegrees * Mathf.Deg2Rad), 0.0f, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
