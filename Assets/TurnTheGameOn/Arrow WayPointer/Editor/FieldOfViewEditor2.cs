using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldOfView2))]
public class FieldOfViewEditor2 : Editor
{
    void OnSceneGUI()
    {
        FieldOfView2 fow = (FieldOfView2)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRadius2);
        Vector3 viewAngleA = fow.DirFromAngle(-fow.viewAngle2 / 2, false);
        Vector3 viewAngleB = fow.DirFromAngle(fow.viewAngle2 / 2, false);

        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius2);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius2);

        Handles.color = Color.red;
        foreach (Transform visible in fow.visibleTargets2)
        {
            Handles.DrawLine(fow.transform.position, visible.transform.position);
        }
    }
}