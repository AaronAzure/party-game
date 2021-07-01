using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBall : MonoBehaviour
{

    private GameController ctr;
    private MinigameManager manager;
    private PreviewManager pw;

    private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sr;
    public float speed = 5;
    [SerializeField] private GameObject area;
    
    [SerializeField] private RotateAroundAxis rotation;

    [SerializeField] private Transform[] startingPos;

    


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();    
        rotation.enabled = false;

        if (startingPos.Length > 0)
        {
            int r = Random.Range(0, startingPos.Length);
            this.transform.position = startingPos[r].position;
        }
        GameController ctr = GameObject.Find("Game_Controller").GetComponent<GameController>();
        if (ctr.easy)   speed = Random.Range(1f, 3f);
        if (ctr.norm)   speed = Random.Range(3f, 5f);
        if (ctr.hard)   speed = Random.Range(5f, 7f);
        
        if (GameObject.Find("Preview_Manager") != null) pw = GameObject.Find("Preview_Manager").GetComponent<PreviewManager>();
        if (GameObject.Find("Level_Manager") != null) manager = GameObject.Find("Level_Manager").GetComponent<MinigameManager>();

        if (manager == null)    StartCoroutine( StartMovingIn(1) );
        else                    StartCoroutine( StartMovingIn(4) );
    }

    IEnumerator StartMovingIn(float delay)
    {
        yield return new WaitForSeconds(delay);
        rotation.enabled = true;
        float x = Random.Range(0, 2) == 0 ? -1 : 1;
        float y = Random.Range(0, 2) == 0 ? -1 : 1;
        rb.velocity = new Vector2(speed * x, speed * y);


        yield return new WaitForSeconds(5);
        while (sr.color.a > 0) {
            yield return new WaitForSeconds(0.01f);
            sr.color = new Color(1,1,1,sr.color.a - 0.05f);
        }

        yield return new WaitForSeconds(5);
        rb.velocity = Vector2.zero;
        speed = 0;
        rotation.enabled = false;
    }

    public void CHECK_DISTANCE()
    {
        Debug.Log("  working as intended");
        sr.color = new Color(1,1,1,1);
        area.SetActive(true);
    }
}
