using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FOV))]
public class FOVEditor : Editor {
    
    void OnSceneGUI()
    {
        FOV fOV = (FOV)target;

        Handles.color = Color.blue;
        Handles.DrawWireArc(fOV.transform.position, Vector3.up, Vector3.forward, 360.0f, fOV.viewRadius);
        Vector3 viewAngleA = fOV.DirFromAngle(-fOV.viewAngle / 2, false);
        Vector3 viewAngleB = fOV.DirFromAngle(fOV.viewAngle / 2, false);

        Handles.color = Color.red;
        Handles.DrawLine(fOV.transform.position, fOV.transform.position + viewAngleA * fOV.viewRadius);
        Handles.DrawLine(fOV.transform.position, fOV.transform.position + viewAngleB * fOV.viewRadius);

        foreach (Transform visableTarget in fOV.visableTagets)
        {
            Handles.DrawLine(fOV.transform.position, visableTarget.position);
        }
    }
}
