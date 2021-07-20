using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            nodes[i].nexts[0].node = nodes[i + 1].gameObject;
        }

        if (arrows.Length > 0) {
            for (int i=0 ; i<nodes.Length ; i++) 
            {
                for (int j=0 ; j<nodes[i].nexts.Length ; j++) 
                {
                    if (nodes[i].nexts[j].dontCreateArrow) continue;

                    Vector2 midPos = nodes[i].transform.position + 
                        (nodes[i].nexts[j].node.transform.position - nodes[i].transform.position) / 2;

                    float distance = Vector2.Distance(nodes[i].transform.position, nodes[i].nexts[j].node.transform.position);
                    int avg = (int)distance ;
                    
                    Vector2 direction = nodes[i].nexts[j].node.transform.position - nodes[i].transform.position;
                    direction.Normalize();
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;     
                    if (avg >= arrows.Length)  avg = arrows.Length - 1;
                    
                    if (arrowHolder != null) {
                        Instantiate(arrows[avg], midPos, Quaternion.Euler(Vector3.forward * (angle - 90f)), arrowHolder.transform);
                    }
                    else {
                        Instantiate(arrows[avg], midPos, Quaternion.Euler(Vector3.forward * (angle - 90f)));
                    }
                }
            }
        }
    }
}
