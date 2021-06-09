using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public GameObject gold1;
    public GameObject gold3;
    public GameObject gold5;
    public GameObject bomb;
    public bool moveLeft;
    private bool canMove = true;
    private float moveSpeed = 1.96f;
    private Animator anim;
    private bool beingDestroyed;
    public float moveFor = 0.75f;
    public float stopFor = 0.375f;
    [SerializeField] private AudioSource coinSound;
    [SerializeField] private GameObject bombSound;
    GameController ctr;
    private bool notPractice;


    private void Start() {
        if (GameObject.Find("Level_Manager") != null) notPractice = true;
        ctr = GameObject.Find("Game_Controller").GetComponent<GameController>();
        if (ctr.easy)
        {
            moveSpeed   = 1.85f;
            moveFor     = 1;
            stopFor     = 0.5f;
        }
        anim = this.GetComponent<Animator>();
        StartCoroutine( MOVE(moveFor) );
        StartCoroutine( BOX_CLOSED() );
    }

    private void Update() {
        if (canMove && !beingDestroyed)
        {
            if (moveLeft) transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            else          transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
        if (beingDestroyed) 
        {
            transform.Translate(Vector3.down * 0.5f * Time.deltaTime);
            if (moveLeft) transform.Translate(Vector3.left * 2 * Time.deltaTime);
            else          transform.Translate(Vector3.right * 2 * Time.deltaTime);
        }
        
    }

    IEnumerator MOVE(float delay)
    {
        yield return new WaitForSeconds( delay );
        if (canMove)    { canMove = false; StartCoroutine( MOVE(stopFor) );}
        else            { canMove = true;  StartCoroutine( MOVE(moveFor) );}
    }

    IEnumerator BOX_CLOSED()
    {
        yield return new WaitForSeconds(moveFor);
        anim.Play("Box_Close_Anim", -1, 0);
    }

    public int OPEN_BOX()
    {
        anim.speed = 2;
        anim.Play("Box_Open_Anim", -1, 0);

        if (gold1.activeSelf) {   
            StartCoroutine( OPENING_EFFECT(0) );
            return 1;
        }
        if (gold3.activeSelf) {   
            StartCoroutine( OPENING_EFFECT(1) );
            return 3;
        }
        if (gold5.activeSelf) {   
            StartCoroutine( OPENING_EFFECT(2) );
            return 5;
        }
        if (bomb.activeSelf)  {   
            StartCoroutine( OPENING_EFFECT(3) );
            return -3;
        }
        return 0;
    }

    private IEnumerator OPENING_EFFECT(int ver)
    {
        if      (ver == 0) {
            for (int i=0; i<1; i++)
            {
                if (notPractice) coinSound.Play();
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.25f);
            gold1.SetActive(false);

        }
        else if (ver == 1) {
            for (int i=0; i<3; i++)
            {
                if (notPractice) coinSound.Play();
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.25f);
            gold3.SetActive(false);
        }
        else if (ver == 2) {
            for (int i=0; i<5; i++)
            {
                if (notPractice) coinSound.Play();
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.25f);
            gold5.SetActive(false);
        }
        else if (ver == 3) {
            yield return new WaitForSeconds(0.25f);
            if (!notPractice) bombSound.GetComponent<AudioSource>().volume = 0; 
            bombSound.SetActive(true);
            bomb.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Hurtbox") beingDestroyed = true;
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.tag == "Hurtbox")
        {
            this.transform.localScale -= new Vector3(0.01f,0.01f,0.01f);
            if (transform.localScale.x <= 0 || transform.localScale.y <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

}
