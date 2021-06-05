using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class FloatingText : MonoBehaviour
{
    private TextMeshPro text;
    private float moveSpeed = 3.5f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 0.25f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector3.up * moveSpeed * Time.fixedDeltaTime);
    }
}
