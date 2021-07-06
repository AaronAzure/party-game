using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shade : MonoBehaviour
{
    public int health = 3;
    [SerializeField] private CircleCollider2D _collider;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private SpriteRenderer[] renders;

    public bool isRed;
    public bool isGold;
    public float fleeTime = 6.5f;
    private Coroutine co;

    private bool moveLeft;
    private float timer;
    private float moveSpeed = 6;

    private float rotateSpeed = 4f;
    private float radius = 0.1f;
    

    private Vector2 _centre;
    private float _angle;


    // Start is called before the first frame update
    void Start()
    {
        if (_collider == null) _collider = GetComponent<CircleCollider2D>();
        if (anim == null)    anim = GetComponent<Animator>();

        _centre = transform.position;        
        _collider.enabled = false;

        if (isRed)  { radius = 2;}
        if (isGold) { radius = 3.5f;  health = 1; transform.localScale /= 1.5f;}

        if (transform.position.x < 0) {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        foreach (SpriteRenderer render in renders) render.color = new Color(0,0,0,0);
        StartCoroutine( ENTER() );
        co = StartCoroutine( Flee() );
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (health > 0)
        {
            _angle += rotateSpeed * Time.fixedDeltaTime;
    
            var offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle)) * radius;
            transform.position = _centre + offset;
        }
    }

    private IEnumerator OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Attack") {
            health--;

            if (health <= 0) {
                _collider.enabled = false;
                StopCoroutine(co);
                foreach (SpriteRenderer render in renders) Destroy(render.gameObject);

                if (deathEffect != null)
                {
                    Instantiate(deathEffect, this.transform.position + new Vector3(0,1*transform.localScale.x), 
                        Quaternion.identity, this.transform);
                }
                Destroy(gameObject, 2f);
            }
            else {
                foreach (SpriteRenderer render in renders){ if (render != null) render.color = new Color(1,1,1,0.3f);}
                yield return new WaitForEndOfFrame();
                foreach (SpriteRenderer render in renders){ if (render != null) render.color = new Color(1,1,1,1);}
            }
        }
    }

    private IEnumerator ENTER()
    {
        while (renders[0] != null && renders[0].color.a < 1) {
            yield return new WaitForEndOfFrame(); yield return new WaitForEndOfFrame();
            foreach (SpriteRenderer render in renders) { if (render != null) render.color += new Color(0.1f,0.1f,0.1f,0.1f);}
        }
        _collider.enabled = true;
    }

    private IEnumerator Flee()
    {
        yield return new WaitForSeconds(fleeTime);
        _collider.enabled = false;
        while (renders[0] != null && renders[0].color.a > 0) {
            yield return new WaitForEndOfFrame(); yield return new WaitForEndOfFrame();
            foreach (SpriteRenderer render in renders) {if (render != null) render.color -= new Color(0.1f,0.1f,0.1f,0.1f);}
        }
        Destroy(gameObject);
    }
}
