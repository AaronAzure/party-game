using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundAxis : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    private Vector3 axis;

    void Start() {
        axis = new Vector3(0, 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(axis * speed * Time.deltaTime, Space.Self);
    }
}
