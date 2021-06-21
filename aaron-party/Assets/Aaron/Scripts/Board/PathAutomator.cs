﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathAutomator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Node[] nodes = this.GetComponentsInChildren<Node>();

        // EACH PATH WILL LEAD TO THE NEXT NODE, 
        // EXCEPT THE END WILL EITHER LEAD TO A DIFFERENT NODE(S) IN A DIFFERENT PATH (JOIN / SPLIT)
        for (int i=0; i<nodes.Length - 1 ; i++) {
            nodes[i].nexts = new Node.Nexts[1];
            nodes[i].nexts[0].node = nodes[i + 1].gameObject;
        }
    }
}
