using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCollector : MonoBehaviour
{
    public MinigameControls player; // ** SPAWN PARENT

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Gold")
        {
            Destroy(other.gameObject);
            player.points++;
            player.UPDATE_POINTS();
            player.GOLD_PICKUP();
        }
    }
}
