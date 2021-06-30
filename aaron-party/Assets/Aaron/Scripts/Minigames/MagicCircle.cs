using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircle : MonoBehaviour
{
    [SerializeField] private AudioSource fivePoint;
    [SerializeField] private AudioSource threePoint;
    [SerializeField] private AudioSource twoPoint;
    [SerializeField] private AudioSource onePoint;
    [SerializeField] private Animator anim;
    private int nReached = 0;

    private PreviewManager pw;


    private void Start() {
        if (GameObject.Find("Preview_Manager")) pw = GameObject.Find("Preview_Manager").GetComponent<PreviewManager>();
    }    

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player")
        {
            StartCoroutine( CLOSEST_PLAYER() );
        }
    }

    IEnumerator CLOSEST_PLAYER()
    {
        anim.speed = 0;
        if (pw == null)
        {
            switch (nReached)
            {
                case 0:     nReached++; fivePoint.Play();     break;
                case 1:     nReached++; threePoint.Play();    break;
                case 2:     nReached++; twoPoint.Play();      break;
                default:    onePoint.Play();    break;
            }
        }

        yield return new WaitForSeconds(1);
        anim.speed = 1;
    }
}
