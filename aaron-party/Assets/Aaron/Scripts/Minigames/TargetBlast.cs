using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBlast : MonoBehaviour
{

    public IEnumerator MOVE_TO_POSITION(Vector3 destination, float time)
    {
        Vector3 currentPos = this.transform.position;
        float t = 0f;
        while(t < 1)
        {
            t += Time.deltaTime / time;
            transform.position = Vector3.Lerp(currentPos, destination, t);
            yield return null;
        }
    }
}
