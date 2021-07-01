using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutterBlades : MonoBehaviour
{
    [SerializeField] private Vector3 targetPos;
    private float moveSpeed = 6f;
    private bool reached;
    private float[] waitTime;
    private bool started;


    private GameController ctr;
    private LevelManager manager;
    private PreviewManager pw;


    // Start is called before the first frame update
    void Start()
    {
        waitTime = new float[]{0, 0, 0, 0.1f, 0.25f, 0.25f, 0.5f, 0.5f, 0.75f, 1f, 2.5f};

        if (GameObject.Find("Preview_Manager") != null) pw = GameObject.Find("Preview_Manager").GetComponent<PreviewManager>();
        if (GameObject.Find("Level_Manager") != null) manager = GameObject.Find("Level_Manager").GetComponent<LevelManager>();

        LEVEL_DIFFICULTY();

        if (manager == null)    StartCoroutine( GET_NEW_DESTINATION(1 + 1) );
        else                    StartCoroutine( GET_NEW_DESTINATION(4 + 1) );
    }

    void LEVEL_DIFFICULTY()
    {
        ctr = GameObject.Find("Game_Controller").GetComponent<GameController>();
        if (ctr.hard) { moveSpeed = 9f; }
    }


    private void FixedUpdate() 
    {
        if (manager != null && manager.timeUp) {started = false; reached = true; }

        if (started && this.transform.position != targetPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        }
        else if (started && !reached && this.transform.position == targetPos)
        {
            reached = true;
            StartCoroutine( GET_NEW_DESTINATION(waitTime[Random.Range(0, waitTime.Length)]) );
        }
    }

    IEnumerator GET_NEW_DESTINATION(float delay=2)
    {
        yield return new WaitForSeconds( delay );
        started = true;
        reached = false;

        if (ctr.easy) {
            int r = Random.Range(0, 4);
            switch (r)
            {
                case 0: targetPos = transform.position + new Vector3(0, 2);  break;
                case 1: targetPos = transform.position + new Vector3(2, 0);  break;
                case 2: targetPos = transform.position + new Vector3(0, -2); break;
                case 3: targetPos = transform.position + new Vector3(-2, 0); break;
            }
            if (targetPos.x > 7)    targetPos -= new Vector3(4, 0);
            if (targetPos.x < -7)   targetPos += new Vector3(4, 0);
            if (targetPos.y > 3)    targetPos -= new Vector3(0, 4);
            if (targetPos.y < -3)   targetPos += new Vector3(0, 4);
        }
        else {
            int r = Random.Range(0, 8);
            switch (r)
            {
                case 0: targetPos = transform.position + new Vector3(0, 2);  break;
                case 1: targetPos = transform.position + new Vector3(2, 0);  break;
                case 2: targetPos = transform.position + new Vector3(0, -2); break;
                case 3: targetPos = transform.position + new Vector3(-2, 0); break;

                case 4: targetPos = transform.position + new Vector3(2, 2);   break;
                case 5: targetPos = transform.position + new Vector3(2, -2);  break;
                case 6: targetPos = transform.position + new Vector3(-2, 2);  break;
                case 7: targetPos = transform.position + new Vector3(-2, -2); break;
            }
            if (targetPos.x > 7)    targetPos -= new Vector3(4, 0);
            if (targetPos.x < -7)   targetPos += new Vector3(4, 0);
            if (targetPos.y > 3)    targetPos -= new Vector3(0, 4);
            if (targetPos.y < -3)   targetPos += new Vector3(0, 4);
        }
    }
}
