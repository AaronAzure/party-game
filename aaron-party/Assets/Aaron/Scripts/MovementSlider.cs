using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementSlider : MonoBehaviour
{
    private Slider moveBar;
    private bool stopped = false;

    void Awake()
    {
        moveBar = this.GetComponent<Slider>();

    }
}
