using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DebugLines : MonoBehaviour
{
    [SerializeField] private Transform[] nodes;
    public void DrawLines()
    {
        // Transform[] nodes = GetComponentsInChildren<Transform>();
        for (int i=0 ; i<nodes.Length ; i++)
        {
            for (int j=0 ; j<nodes.Length ; j++)
            {
                Debug.DrawLine(nodes[i].position, nodes[j].position, Color.green, 5);
            }
        }
    }
}

[CustomEditor(typeof(DebugLines))]
public class DebugLinesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DebugLines myScript = (DebugLines)target;
        if(GUILayout.Button("Show Path"))
        {
            myScript.DrawLines();
        }
    }

}