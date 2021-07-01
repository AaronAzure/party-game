using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StopWatch : MonoBehaviour
{
    private float timeToStop;
    private MinigameManager manager;
    private PreviewManager pw;
    [SerializeField] private TextMeshProUGUI giantTime;

    void Start()
    {
        if (GameObject.Find("Level_Manager") != null)   manager = GameObject.Find("Level_Manager").GetComponent<MinigameManager>();
        if (GameObject.Find("Preview_Manager") != null) pw = GameObject.Find("Preview_Manager").GetComponent<PreviewManager>();

        timeToStop = Random.Range(10f,20f);
        timeToStop = Mathf.Round(timeToStop * 100f) / 100f;
        giantTime.text = timeToStop.ToString("F2");

        if (GameObject.Find("Level_Manager") != null)   manager.timeToStop = this.timeToStop;
        if (GameObject.Find("Preview_Manager") != null) pw.timeToStop = this.timeToStop;
    }
}
