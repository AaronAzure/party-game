using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MagicCard : MonoBehaviour
{
    public int turnRank;
    public bool collected;
    public TextMeshPro rank;
    [SerializeField] private SpriteRenderer card;   // ** INSPECTOR
    [SerializeField] private GameObject[] toHide;   // ** INSPECTOR
    [SerializeField] private GameObject toReveal;   // ** INSPECTOR
    
    private void Start() {
        card.color = new Color(1,1,1,0.4f);
    }

    public void COLLECT()
    {
        if (!collected)
        {
            Debug.Log("collected");
            collected = true;
            foreach (GameObject obj in toHide) { obj.SetActive(false); }
            toReveal.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if (other.tag == "Player")
        {
            card.color = new Color(1,1,1,1);
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.tag == "Player")
        {
            card.color = new Color(1,1,1,0.4f);
        }
    }
}
