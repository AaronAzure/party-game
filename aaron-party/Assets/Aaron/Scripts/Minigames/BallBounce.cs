using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBounce : MonoBehaviour
{

    public float speed = 5;
    private Rigidbody2D rb;
    // private Vector3 lastVelocity;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();    
    }
    
    // Start is called before the first frame update
    void Start()
    {
        float x = Random.Range(0, 2) == 0 ? -1 : 1;
        float y = Random.Range(0, 2) == 0 ? -1 : 1;
        rb.velocity = new Vector2(speed * x, speed * y);
    }
}
