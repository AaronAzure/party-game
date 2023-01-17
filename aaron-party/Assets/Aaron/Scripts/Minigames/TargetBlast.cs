using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBlast : MonoBehaviour
{
    public MinigameControls player;
    public Vector3 dest;

    public IEnumerator MOVE_TO_POSITION(Vector3 destination, float time)
    {
        Vector3 currentPos = this.transform.position;
        dest = destination;
        float t = 0f;
        while(t < 1)
        {
            t += Time.deltaTime / time;
            transform.position = Vector3.Lerp(currentPos, destination, t);
            yield return null;
        }
        Destroy(this.gameObject, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (player != null && other.tag == "Enemy")   { player.POINT(1); }
        if (player != null && other.tag == "Special") { player.POINT(2); }
        if (player != null && other.tag == "Gold")    { player.POINT(5); }
    }
}
