using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeSpace
{
    public string parentPath;
    public string childNode;
    public string nodeType;

    public NodeSpace(string newParentPath, string newChildNode, string newNodeType)
    {
        parentPath  = newParentPath;
        childNode   = newChildNode;
        nodeType    = newNodeType;
    }
}
