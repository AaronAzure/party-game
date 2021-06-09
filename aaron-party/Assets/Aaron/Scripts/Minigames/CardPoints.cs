using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardPoints : MonoBehaviour
{
    public int value;
    public float moveSpeed;
    public string direction;

    [SerializeField] private TextMeshPro valueText; // INSPECTOR

    private void Start() {
        if (value > 0) valueText.text = "+" + value.ToString();
        else           valueText.text = value.ToString();
    }

    void FixedUpdate()
    {
        if (transform.position.x > 13 || transform.position.x < -13 || transform.position.y > 8 || transform.position.y < -8)
        {
            Destroy(this.gameObject);
        }

        if      (direction == "L") { transform.Translate(Vector3.left * moveSpeed * Time.fixedDeltaTime); }
        else if (direction == "R") { transform.Translate(Vector3.right * moveSpeed * Time.fixedDeltaTime); }
        else if (direction == "U") { transform.Translate(Vector3.up * moveSpeed * Time.fixedDeltaTime); }
        else if (direction == "D") { transform.Translate(Vector3.down * moveSpeed * Time.fixedDeltaTime); }
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if (other.tag == "Player")
        {
            MinigameControls player = other.GetComponent<MinigameControls>();
            if (player.points + this.value < 0) { player.points -= player.points; }
            else    { player.points += this.value; }
            Destroy(this.gameObject);
        }
    }
}
