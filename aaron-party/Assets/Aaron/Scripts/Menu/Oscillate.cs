using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillate : MonoBehaviour
{
    [SerializeField] private float firstAngle;
    [SerializeField] private float secondAngle;
    [SerializeField] private float speed = 5;

    [SerializeField] private bool rotateX;
    [SerializeField] private bool rotateY;
    [SerializeField] private bool rotateZ;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine( OSCILLATE_TO_FIRST() );
    }

    public IEnumerator OSCILLATE_TO_FIRST() {
        float totalRotation = 0; 
        while(Mathf.Abs(totalRotation) < 90) 
        {
            float rotateAmt = speed * Time.deltaTime;
            yield return new WaitForSeconds(0.01f);
            if (rotateX) this.transform.Rotate(rotateAmt, 0, 0);
            if (rotateY) this.transform.Rotate(0, speed * Time.deltaTime, 0);
            if (rotateZ) this.transform.Rotate(0, 0, rotateAmt);
            totalRotation += (rotateAmt);
        }

        StartCoroutine( OSCILLATE_TO_SECOND() );
    }

    public IEnumerator OSCILLATE_TO_SECOND() {
        float totalRotation = 0; 
        while(Mathf.Abs(totalRotation) < 90) 
        {
            float rotateAmt = speed * Time.deltaTime * -1;
            yield return new WaitForSeconds(0.01f);
            if (rotateX) this.transform.Rotate(rotateAmt, 0, 0);
            if (rotateY) this.transform.Rotate(0, rotateAmt, 0);
            if (rotateZ) this.transform.Rotate(0, 0, rotateAmt);
            totalRotation += (rotateAmt);
        }

        StartCoroutine( OSCILLATE_TO_FIRST() );
    }

}
