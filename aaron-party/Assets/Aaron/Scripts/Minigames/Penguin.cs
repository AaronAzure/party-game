using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penguin : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    public float knockbackPower = 1.3f;
    private float knockbackDuration = 0.1f;
    private bool  sliding;
    
    
    void FixedUpdate()
    {
        // rb.velocity = new Vector3(-1, 0) * moveSpeed;
        transform.Translate(Vector3.left * moveSpeed * Time.fixedDeltaTime);
        if (transform.position.y < -12)     Destroy(this.gameObject);
        if (sliding)    transform.Translate(Vector3.down * moveSpeed * Time.fixedDeltaTime);

    }

    private void OnCollisionStay2D(Collision2D other) 
    {
        if (other.gameObject.tag == "Player")
        {
            MinigameControls opponent = other.gameObject.GetComponent<MinigameControls>();
            if (!opponent.isOut)
            {
                StartCoroutine( opponent.KnockBackCo(knockbackDuration, knockbackPower, this.transform, 0) );
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Hurtbox") { sliding = true; }
    }
}

