using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageTimer : MonoBehaviour
{
    public bool stopped;
    private bool gameStart;
    public float timer;
    // private MinigameControls player;
    private MinigameManager manager;
    [SerializeField] private TextMeshPro timeText;
    // private float nextActionTime = 0;
    // private float period = 0.01f;


    void Start()
    {
        if (GameObject.Find("Level_Manager") != null) StartCoroutine( BEGIN() );
        else  { gameStart = true; }
    }

    void Update () 
    {
        if (gameStart) 
        {
            if (!stopped) {
                timer += Time.deltaTime;
                if (timeText != null && timer <= 3.5f) timeText.text = "0" + timer.ToString("F2");
                else if (timeText != null && timer > 3.5f && !stopped) timeText.text = "";
            }
        }
    }

    public void StopTimer()
    {
        stopped = true;
        GetComponent<Animator>().Play("Game_Stand_Pressed", -1, 0);
    }

    public void ShowStoppedTime()
    {
        if      (timer < 10) timeText.text = "0" + timer.ToString("F2");
        else if (timer < 30) timeText.text = timer.ToString("F2");

        else timeText.text = "--.--";
    }

    IEnumerator BEGIN()
    {
        yield return new WaitForSeconds(4);
        gameStart = true;
    }

}
