using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolHorizontal : MonoBehaviour
{
    private float posLeft;
    private float posRight;
    public bool moveToLeft;
    private bool readyToMove;
    public float moveSpeed = 7;
    public float restTime = 1;

    // Start is called before the first frame update
    void Start()
    {
        if (transform.position.x > 0) // ON RIGHT SIDE
        {
            moveToLeft = true;
        }
        posLeft = Mathf.Abs(transform.position.x) * -1;
        posRight = Mathf.Abs(transform.position.x);
        readyToMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveToLeft && readyToMove)
        {
            if (transform.position.x > posLeft)
            {
                transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            }
            else { StartCoroutine( REST() ); moveToLeft = false; }
        }
        else if (!moveToLeft && readyToMove)
        {
            if (transform.position.x < posRight)
            {
                transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            }
            else { StartCoroutine( REST() ); moveToLeft = true; }
        }
    }

    IEnumerator REST()
    {
        readyToMove = false;

        yield return new WaitForSeconds(restTime);
        readyToMove = true;
    }
}
