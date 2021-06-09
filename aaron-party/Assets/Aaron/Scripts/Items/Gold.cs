using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    public GoldSpawner spawner;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player")
        {
            if (spawner != null) spawner.SpawnAgain();
        }
    }
}
