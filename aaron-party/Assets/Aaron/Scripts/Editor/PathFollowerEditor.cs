using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PathFollower))]
public class PathFollowerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PathFollower myScript = (PathFollower)target;
        if(GUILayout.Button("POWER UP"))
        {
            myScript.POWER_UP();
        }
    }
}
