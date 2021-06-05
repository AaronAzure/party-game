using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] private bool straight;
    private float moveSpeed = 30;
    

    private void FixedUpdate() {
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Obstacle" || other.tag == "Attack")
        {
            Destroy(this.gameObject);
        }
    }
}
