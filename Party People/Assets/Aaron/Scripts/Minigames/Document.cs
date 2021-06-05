using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Document : MonoBehaviour
{
    private bool moving;            // destroy after secs
    public bool beingStamped;      // move left
    public bool beingDiscarded;    // move right
    // [SerializeField] private sorting
    [SerializeField] private GameObject approvedStamp;
    [SerializeField] public  GameObject goodDoc;
    [SerializeField] public  GameObject evilDoc;
    private float moveSpeed = 20;
    private float destoryTime = 0.4f;


    private void Update() {
        if (beingStamped)
        {
            if (!moving) { moving = true; Destroy(this.gameObject, destoryTime); }
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
        else if (beingDiscarded)
        {
            if (!moving) { moving = true; Destroy(this.gameObject, destoryTime); }
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
    }

    public void DISCARD()
    {
        this.GetComponent<SortingGroup>().sortingOrder = 2;
        beingDiscarded = true;
    }

    public void STAMPED()
    {
        approvedStamp.SetActive(true);
        this.GetComponent<SortingGroup>().sortingOrder = 2;
        beingStamped = true;
    }
}
