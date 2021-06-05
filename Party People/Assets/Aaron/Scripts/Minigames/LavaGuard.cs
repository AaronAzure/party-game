using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaGuard : MonoBehaviour
{
    [SerializeField] private Transform[] target;
    [SerializeField] private float startIn = 0;
    [SerializeField] private GameObject lavaTrail;
    private GameController ctr;
    private LevelManager manager;
    private PreviewManager pw;
    private int index;
    private float moveSpeed = 5;
    private bool readyToMove;
    private float lScale;


    void Start()
    {
        if (GameObject.Find("Preview_Manager") != null) pw = GameObject.Find("Preview_Manager").GetComponent<PreviewManager>();
        if (GameObject.Find("Level_Manager") != null) manager = GameObject.Find("Level_Manager").GetComponent<LevelManager>();
        index  = Random.Range(0, target.Length);
        lScale = transform.localScale.x;

        if      (transform.position.x < target[index].position.x)
        {
            transform.localScale = new Vector3(lScale,lScale,lScale);
        }
        else if (transform.position.x > target[index].position.x)
        {
            transform.localScale = new Vector3(-lScale,lScale,lScale);
        }
        
        LEVEL_DIFFICULTY();
        
        if (manager == null)    StartCoroutine( StartMovingIn(startIn + 1) );
        else                    StartCoroutine( StartMovingIn(startIn + 4) );
    }

    void LEVEL_DIFFICULTY()
    {
        ctr = GameObject.Find("Game_Controller").GetComponent<GameController>();
        if (ctr.easy) { moveSpeed = 3.5f; }
        if (ctr.hard) { moveSpeed = 7.5f; }
    }

    void FixedUpdate()
    {
        if (manager == null && readyToMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, target[index].position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target[index].position) < 0.001f)
            {
                // CHANGE WHERE THE GUARDIAN MOVES.
                index = Random.Range(0, target.Length);
                readyToMove = false;
                StartCoroutine( Rest() );
            }
        }
        else if (readyToMove && manager.canPlay)
        {
            transform.position = Vector3.MoveTowards(transform.position, target[index].position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target[index].position) < 0.001f)
            {
                // CHANGE WHERE THE GUARDIAN MOVES.
                index = Random.Range(0, target.Length);
                readyToMove = false;
                StartCoroutine( Rest() );
            }
        }
    }

    IEnumerator StartMovingIn(float x)
    {
        yield return new WaitForSeconds(x);
        readyToMove = true;
        StartCoroutine( LavaTrail() );
    }

    IEnumerator Rest()
    {
        if (ctr.hard) yield return new WaitForSeconds(0.15f);
        else yield return new WaitForSeconds(0.25f);
        readyToMove = true;
        if      (transform.position.x < target[index].position.x)
        {
            transform.localScale = new Vector3(lScale,lScale,lScale);
        }
        else if (transform.position.x > target[index].position.x)
        {
            transform.localScale = new Vector3(-lScale,lScale,lScale);
        }
    }

    IEnumerator LavaTrail()
    {
        if (ctr.hard) yield return new WaitForSeconds(0.2f);
        else yield return new WaitForSeconds(0.4f);
        if (manager != null) { if (manager.timeUp) yield break; }
        var trail = Instantiate(lavaTrail, transform.position, lavaTrail.transform.rotation); 
        if (pw != null)    trail.transform.parent = pw.transform;

        if (ctr.hard) Destroy(trail, 1.5f);
        else Destroy(trail, 2.5f);
        StartCoroutine( LavaTrail() );
    }
}
