using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainingDummy : MonoBehaviour
{
    private GameObject character;


    void Start() {
        character = this.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Safe") {
            StartCoroutine(Damaged());
        }
    }

    private IEnumerator Damaged()
    {
        foreach (Transform child in character.transform)  
        {  
            if (child.name != "Shadow") {
                if (child.TryGetComponent(out SpriteRenderer cs)) { cs.color = new Color(1,0,0); }
            }
            foreach (Transform grandChild in child)
            {
                if (grandChild.name != "Shadow") {
                    if (grandChild.TryGetComponent(out SpriteRenderer gs)) { gs.color = new Color(1,0,0); }
                }
            }
        }

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        foreach (Transform child in character.transform)  
        {  
            if (child.name != "Shadow") {
                if (child.TryGetComponent(out SpriteRenderer cs)) { cs.color = new Color(1,1,1,1); }
            }
            foreach (Transform grandChild in child)
            {
                if (grandChild.name != "Shadow") {
                    if (grandChild.TryGetComponent(out SpriteRenderer gs)) { gs.color = new Color(1,1,1,1); }
                }
            }
        }
    }
}
