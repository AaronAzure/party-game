using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeSpace
{
    public string parentPath;
    public string childNode;
    public Sprite nodeType;

    public NodeSpace(string newParentPath, string newChildNode, Sprite newNodeType)
    {
        parentPath  = newParentPath;
        childNode   = newChildNode;
        nodeType    = newNodeType;
    }
}


public class CavedSpace
{
    public string parentPath;
    public string childNode;
    public int cavedSection;

    public CavedSpace(string newParentPath, string newChildNode, int newcavedSection)
    {
        parentPath   = newParentPath;
        childNode    = newChildNode;
        cavedSection = newcavedSection;
    }
}
