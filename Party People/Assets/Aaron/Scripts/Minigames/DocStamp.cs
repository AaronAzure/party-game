using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DocStamp : MonoBehaviour
{
    public bool[] isGood;
    private bool[] goodOrBad = {true, true, true, false};


    // Start is called before the first frame update
    void Awake()
    {
        isGood = new bool[100];
        for (int i=0 ; i<isGood.Length ; i++)
        {
            if (i==0 || i==1 || i==2) { isGood[i] = true; }
            isGood[i] = goodOrBad[ Random.Range(0, goodOrBad.Length) ];
        }
    }
}
