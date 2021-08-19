using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PathAutomator : MonoBehaviour
{
    public Node[] nodes;
    public GameObject[] arrows;
    public GameObject arrowHolder;


    void Start()
    {
        nodes = GetComponentsInChildren<Node>();

        // EACH PATH WILL LEAD TO THE NEXT NODE, 
        // EXCEPT THE END WILL EITHER LEAD TO A DIFFERENT NODE(S) IN A DIFFERENT PATH (JOIN / SPLIT)
        for (int i=0 ; i<nodes.Length - 1 ; i++) {
            if (nodes[i].nexts.Length == 0) nodes[i].nexts = new Node.Nexts[1];
            nodes[i].nexts[0].node = nodes[i + 1].gameObject;
        }

        // AUTOMATICALLY CREATE ARROW POINTING TO NEXT NODE
        if (arrows.Length > 0) {
            for (int i=0 ; i<nodes.Length ; i++) 
            {
                for (int j=0 ; j<nodes[i].nexts.Length ; j++) 
                {
                    if (nodes[i].nexts.Length <= 0 || nodes[i].nexts[j].dontCreateArrow || nodes[i].nexts[0].node == null) continue;

                    // POINT TO NEXT NODE
                    //* CALCULATE MIDPOINT
                    Vector2 midPos = nodes[i].transform.position + 
                        (nodes[i].nexts[j].node.transform.position - nodes[i].transform.position) / 2;

                    float distance = Vector2.Distance(nodes[i].transform.position, nodes[i].nexts[j].node.transform.position);
                    distance -= 4;
                    if (distance > 0) distance *= 2.5f;
                    int ind = (int)distance;

                    //* OUT OF BOUNDS
                    if (ind < 0) ind = 0;
                    if (ind >= arrows.Length)  ind = arrows.Length - 1;
                    
                    //* ANGLE OF DIRECTION OF ARROW
                    Vector2 direction = nodes[i].nexts[j].node.transform.position - nodes[i].transform.position;
                    direction.Normalize();
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;     
                    
                    //* CREATE ARROW OBJ
                    if (arrowHolder != null) {
                        // Debug.LogError("->  " + arrows[ind].name + " : " + ind);
                        Instantiate(arrows[ind], midPos, Quaternion.Euler(Vector3.forward * (angle - 90f)), arrowHolder.transform);
                    }
                    else {
                        // Debug.LogError("->  " + arrows[ind].name + " : " + ind);
                        Instantiate(arrows[ind], midPos, Quaternion.Euler(Vector3.forward * (angle - 90f)));
                    }
                }
            }
        }
    }

    public void HOW_MANY_NODES()
    {
        Debug.Log(transform.childCount);
    }
}

[CustomEditor(typeof(PathAutomator))]
[CanEditMultipleObjects]
public class PathAutomatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PathAutomator myScript = (PathAutomator)target;
        if(GUILayout.Button("Log Path Length"))
        {
            myScript.HOW_MANY_NODES();
        }
    }
}


