using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float knockbackPower = 30f;
    private float knockbackDuration = 0.1f;
    private float rotateAmount = 1;
    [SerializeField] private bool straight;
    [SerializeField] private float moveSpeed = 15;
    

    private void FixedUpdate() {
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
        if (!straight) transform.Rotate(new Vector3(0,0,2.5f));
    }
}
