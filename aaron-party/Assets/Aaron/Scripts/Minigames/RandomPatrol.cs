using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPatrol : MonoBehaviour
{
    [SerializeField] private Vector3 posToMove;
    [SerializeField] private float moveSpeed = 6f;
    public float lowerBound;
    public float upperBound;
    public float leftBound;
    public float rightBound;
    private float[] waitTime;


    private GameController ctr;
    private MinigameManager manager;
    private PreviewManager pw;

    [SerializeField] private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        if (anim == null) anim = GetComponent<Animator>();
        if (GameObject.Find("Preview_Manager") != null) pw = GameObject.Find("Preview_Manager").GetComponent<PreviewManager>();
        if (GameObject.Find("Level_Manager") != null) manager = GameObject.Find("Level_Manager").GetComponent<MinigameManager>();
        if (GameObject.Find("Game_Controller") != null) ctr = GameObject.Find("Game_Controller").GetComponent<GameController>();

        LEVEL_DIFFICULTY();

        if (manager == null)    StartCoroutine( GET_NEW_DESTINATION(Random.Range(0.5f, 7f)) );
        else                    StartCoroutine( GET_NEW_DESTINATION(Random.Range(0.5f, 7f)) );
    }

    void LEVEL_DIFFICULTY()
    {
        if (ctr != null && ctr.easy) { moveSpeed = 3f; waitTime = new float[]{5, 6, 7, 8}; }
        if (ctr != null && ctr.norm) { moveSpeed = 4f; waitTime = new float[]{5, 5.5f, 6, 6.5f, 4, 4.5f}; }
        if (ctr != null && ctr.hard) { moveSpeed = 5f; waitTime = new float[]{3.5f, 4, 4.5f, 4.25f, 3.25f, 3.75f}; }
        if (waitTime == null) waitTime = new float[]{5,5,6,6,4};
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position != posToMove && anim.GetCurrentAnimatorStateInfo(0).IsName("Fox_Walk_Anim"))
        {
            transform.position = Vector3.MoveTowards(transform.position, posToMove, moveSpeed * Time.deltaTime);
        }
        else {
            anim.SetTrigger("idle");
            anim.speed = 1;
        }
    }


    IEnumerator GET_NEW_DESTINATION(float delay=2, bool starting=true)
    {
        if (starting) yield return new WaitForSeconds( delay );

        if (manager != null && manager.timeUp) yield break;
        if (pw != null && pw.timeUp) yield break;

        float x = 0;
        float y = 0;
        if (transform.position.x < leftBound) {
            x = Random.Range(4f, 8f);
        }
        else if (transform.position.x > rightBound) {
            x = Random.Range(-8f, -4f);
        }
        else {
            x = Random.Range(-8f, 8f);
        }
        if (transform.position.y < lowerBound) {
            x = Random.Range(4f, 8f);
        }
        else if (transform.position.y > upperBound) {
            x = Random.Range(-8f, -4f);
        }
        else {
            y = Random.Range(-8f, 8f);
        }

        posToMove = new Vector3(x, y);
        if (x > 0 && transform.localScale.x > 0) {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        if (x < 0 && transform.localScale.x < 0) {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        anim.SetTrigger("walk");
        anim.speed = moveSpeed;
        
        yield return new WaitForSeconds( delay );
        StartCoroutine( GET_NEW_DESTINATION( waitTime[ Random.Range(0, waitTime.Length)], false ) );
    }
}
