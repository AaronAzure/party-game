using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public float minValue;
    public float maxValue;
    public float value;
    public Image arrow;
    // public Image mask;
    // public Image fill;
    public Color color;

    // Update is called once per frame
    void Update()
    {
        GetCurrentFill();
    }
    
    public void GetCurrentFill()
    {
        // float currentOffset = (float) value - (float) minValue;
        // float maximumOffset = (float) maxValue - (float) minValue;
        // float fillAmount    = currentOffset / maximumOffset;
        arrow.transform.rotation = Quaternion.Euler(0f, 0f,-value); 
        // mask.fillAmount     = fillAmount;
        // fill.fillAmount     = fillAmount;
    }

}
