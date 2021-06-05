using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShogunDoor : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private int nAtDoorway;


    private void Start() {
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player")
        {
            // Debug.Log("entered");
            nAtDoorway++;
            if (nAtDoorway == 1)
            {
                anim.Play("Door_Anim", -1, 0);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player")
        {
            // Debug.Log("exited");
            nAtDoorway--;
            if (nAtDoorway <= 0 ) { 
                anim.Play("Door_Close_Anim", -1, 0);
            }
        }
    }
}
