using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electricity : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private Vector3 posToMove;
    private bool newDir;
    private Vector3[] movePos;
    private bool readyToMove;
    private LevelManager manager;
    [SerializeField] private GameObject electricity;
    [SerializeField] private GameObject prefabHolder;
    [SerializeField] private bool tooMany;  // ** INSPECTOR
    [SerializeField] private bool hardMode;  // ** INSPECTOR
    private float waitTime = 0.1f;



    // Start is called before the first frame update
    void Start()
    {
        movePos = new Vector3[4];
        movePos[0] = new Vector3(2, 0);  // Right
        movePos[1] = new Vector3(-2,0);  // Left
        movePos[2] = new Vector3(0, 2);  // Up
        movePos[3] = new Vector3(0,-2);  // Down

        GameController controller = GameObject.Find("Game_Controller").GetComponent<GameController>();
        if (tooMany)
        {
            if (controller.norm) { Destroy(this.gameObject); }
        }
        if (!controller.hard && hardMode) { Destroy(this.gameObject); }

        if (GameObject.Find("Level_Manager") != null) manager = GameObject.Find("Level_Manager").GetComponent<LevelManager>();
        int rng = Random.Range(0, movePos.Length);
        posToMove = transform.position + movePos[rng];

        if (manager == null)    StartCoroutine( StartMovingIn(1) );
        else                    StartCoroutine( StartMovingIn(4) );
    }

    // Update is called once per frame
    void Update()
    {
        if (readyToMove && !newDir)
        {

            if (transform.position.x > 8 || transform.position.x < -8 ||
                transform.position.y > 4 || transform.position.y < -4)
            {
                if (transform.position.x > 8)  transform.position = new Vector3(8,transform.position.y);
                if (transform.position.x < -8) transform.position = new Vector3(-8,transform.position.y);
                if (transform.position.y > 4)     transform.position = new Vector3(transform.position.x,4);
                if (transform.position.y < -4)    transform.position = new Vector3(transform.position.x,-4);
                newDir = true;
                posToMove = transform.position;
                CHOOSE_NEW_DIRECTION();
            }
            if (!newDir) transform.position = Vector3.MoveTowards(transform.position, posToMove, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, posToMove) < 0.001f && !newDir)
            {
                CHOOSE_NEW_DIRECTION();
            }
        }
        if (manager != null) if (manager.timeUp) Destroy(this.gameObject);
    }

    void CHOOSE_NEW_DIRECTION()
    {
        int rng = Random.Range(0, movePos.Length);
        posToMove += movePos[rng];
        newDir = false;
    }

    IEnumerator StartMovingIn(float x)
    {
        yield return new WaitForSeconds(x);
        readyToMove = true;
        StartCoroutine( ELECTRICAL_TRAIL() );
    }

    IEnumerator ELECTRICAL_TRAIL()
    {
        yield return new WaitForSeconds(waitTime);
        var obj = Instantiate(electricity, transform.position, Quaternion.identity);
        obj.transform.parent = prefabHolder.transform;
        Destroy(obj, 0.2f);
        StartCoroutine( ELECTRICAL_TRAIL() );
    }
}
