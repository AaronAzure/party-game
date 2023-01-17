using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainLoss : MonoBehaviour
{
    private float moveSpeed = 1.1f;
    private float destroyTime = 2f;
    private RectTransform rt;


    void Start()
    {
        Destroy(gameObject, destroyTime);
        // this.transform.localPosition += new Vector3(0, 2, 0);
        // rt = this.transform.GetComponent<RectTransform>();
        // rt.localPosition += new Vector3(0, 10, 0);

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        // rt.localPosition += new Vector3(0, 0.01f, 0);
    }
}
