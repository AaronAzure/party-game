using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeanStalk : MonoBehaviour
{
    public bool[] isRightLeaf;

    void Start()
    {
        isRightLeaf = new bool[75];
        for (int i=0 ; i<isRightLeaf.Length ; i++)
        {
            int rng = Random.Range(0,2);
            if (rng == 1) { isRightLeaf[i] = true; }
        }
    }
}
