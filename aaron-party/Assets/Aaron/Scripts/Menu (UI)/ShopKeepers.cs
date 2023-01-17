using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeepers : MonoBehaviour
{



    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player")
        {
            if (this.tag == "Shop-1") other.GetComponent<PathFollower>().shop1 = true; Debug.Log(other.name + " at shop!");
            if (this.tag == "Shop-2") other.GetComponent<PathFollower>().shop2 = true; Debug.Log(other.name + " at shop!");
            if (this.tag == "Shop-3") other.GetComponent<PathFollower>().shop3 = true; Debug.Log(other.name + " at shop!");
            if (this.tag == "Shop-4") other.GetComponent<PathFollower>().shop4 = true; Debug.Log(other.name + " at shop!");
            Debug.Log(other.name + "apsd");
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player")
        {
            if (this.tag == "Shop-1") other.GetComponent<PathFollower>().shop1 = false;
            if (this.tag == "Shop-2") other.GetComponent<PathFollower>().shop2 = false;
            if (this.tag == "Shop-3") other.GetComponent<PathFollower>().shop3 = false;
            if (this.tag == "Shop-4") other.GetComponent<PathFollower>().shop4 = false;
        }
    }
}
