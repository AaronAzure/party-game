using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lilypad : MonoBehaviour
{
    public string[] pads;

    void Start()
    {
        string[] buttons = { "A", "B", "X", "Y"};
        pads = new string[24];

        for (int i=0 ; i<pads.Length ; i++)
        {
            pads[i] = buttons[ Random.Range(0,buttons.Length) ];
            Debug.Log(pads[i].ToString());
        }
    }
}
