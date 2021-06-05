using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceEffects : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(effectOver());
    }

    private IEnumerator effectOver()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
    }
}
