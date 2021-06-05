using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteImage : MonoBehaviour
{
    private SpriteRenderer rend;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TurnOnImage());
    }

    IEnumerator TurnOnImage()
    {
        yield return new WaitForSeconds(1f);
        rend = this.gameObject.transform.GetComponent<SpriteRenderer>();
        rend.enabled = true;
    }

}
