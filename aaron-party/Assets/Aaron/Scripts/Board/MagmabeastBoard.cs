using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagmabeastBoard : MonoBehaviour
{
    [HideInInspector] public bool canMove = false;
    [SerializeField] private Transform[] posToMove;
    [SerializeField] private Camera cam;
    private int ind;
    private float moveSpeed = 15f;


    [SerializeField] private GameObject burningStand;
    [SerializeField] private GameObject lavaTrail;
    [SerializeField] private Image startUi;
    [SerializeField] private Animator startUiAnim;
    public bool endOfTurn; 

    [SerializeField] private GameManager manager;
    [SerializeField] private GameController ctr;
    [SerializeField] private int x;


    // Start is called before the first frame update
    void Start()
    {
        if (manager != null) manager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (GameObject.Find("Game_Controller") != null) ctr = GameObject.Find("Game_Controller").GetComponent<GameController>();
        if (ctr.turnNumber == 1) {
            PlayerPrefs.SetInt("magmabeast-" + x, 0);
        }
        else {
            ind = PlayerPrefs.GetInt("magmabeast-" + x);
            if (ind < posToMove.Length) transform.position = posToMove[ind].position;
        }
    }


    public IEnumerator START(bool endOfTurn=true) {
        yield return new WaitForSeconds(0.5f);
        startUi.gameObject.SetActive(true);
        startUiAnim.Play("Character_Circle_Anim", -1, 0);

        yield return new WaitForSeconds(1f);
        startUiAnim.Play("Character_Circle_End_Anim", -1, 0);

        yield return new WaitForSeconds(0.75f);
        startUiAnim.Rebind();
        startUi.gameObject.SetActive(false);
    }


    public IEnumerator PICK_A_DEST()
    {
        cam.gameObject.SetActive(true);

        StartCoroutine( START() );
        if (posToMove.Length <= 1) { 
            ind = 0;  
            // MOVING LEFT
            if (transform.position.x > posToMove[ind].position.x && transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            // MOVING RIGHT
            else if (transform.position.x < posToMove[ind].position.x && transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }

            yield return new WaitForSeconds(2);
            canMove = true;  
            StartCoroutine( LavaTrail() );

            yield break; 
        }
        List<int> indexes = new List<int>();
        for (int i=0 ; i<posToMove.Length ; i++) { indexes.Add(i); }
        indexes.Remove(ind);
        ind = indexes[ Random.Range(0, indexes.Count) ];
        PlayerPrefs.SetInt("magmabeast-" + x, ind);

        // MOVING LEFT
        if (transform.position.x > posToMove[ind].position.x && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        // MOVING RIGHT
        else if (transform.position.x < posToMove[ind].position.x && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        yield return new WaitForSeconds(2);
        canMove = true;
        StartCoroutine( LavaTrail() );
    }


    private void FixedUpdate() 
    {
        if (canMove) 
        {
            transform.position = Vector3.MoveTowards(transform.position, posToMove[ind].position, moveSpeed * Time.deltaTime);    

            // REST
            if (Vector3.Distance(transform.position, posToMove[ind].position) < 0.001f)
            {
                canMove = false;
                burningStand.SetActive(true);
                StartCoroutine( DELAY_SET_CAM_ACTIVE(false, 2) );

                if (endOfTurn)  {
                    StartCoroutine( manager.INCREMENT_TURN(1) );
                }
                else {
                    // StartCoroutine( manager.EVENT_OVER_RETURN_TO_PLAYER("Laser Countdown") ); 
                }
            }
        }
    }

    IEnumerator DELAY_SET_CAM_ACTIVE(bool active=true, float startDelay=1)
    {
        yield return new WaitForSeconds(startDelay);
        cam.gameObject.SetActive(active);
    }


    IEnumerator LavaTrail()
    {
        if (lavaTrail == null || !canMove) yield break;
        yield return new WaitForSeconds(0.2f);
        if (burningStand.activeSelf) burningStand.SetActive(false);
        var trail = Instantiate(lavaTrail, transform.position, lavaTrail.transform.rotation); 

        Destroy(trail, 1.5f);
        StartCoroutine( LavaTrail() );
    }

}
