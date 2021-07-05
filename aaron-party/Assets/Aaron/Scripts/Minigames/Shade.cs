using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shade : MonoBehaviour
{
    public int health = 3;
    [SerializeField] private CircleCollider2D _collider;
    [SerializeField] private GameObject deathEffect;


    // Start is called before the first frame update
    void Start()
    {
        if (_collider == null) _collider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Attack")
        {
            health--;
            if (health <= 0) {
                _collider.enabled = false;
                GameObject[] sprites = GetComponentsInChildren<GameObject>();
                foreach (GameObject sprite in sprites) sprite.SetActive(false);
                if (deathEffect != null)
                {
                    Instantiate(deathEffect, this.transform.position, Quaternion.identity, this.transform);
                }
                Destroy(this, 2.1f);
            }
        }
    }
}
