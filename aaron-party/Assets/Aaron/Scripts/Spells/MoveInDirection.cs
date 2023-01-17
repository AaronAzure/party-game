using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInDirection : MonoBehaviour
{

    public enum Directions { left, right, up, down }
    public Directions direction;
    public float moveSpeed = 30;


    // Update is called once per frame
    void FixedUpdate()
    {
        if      (direction == Directions.left)
        {
            transform.position = 
                Vector3.MoveTowards(transform.position, transform.position + new Vector3(-20,0), moveSpeed * Time.deltaTime);    
        }
        else if (direction == Directions.right)
        {
            transform.position = 
                Vector3.MoveTowards(transform.position, transform.position + new Vector3(20,0), moveSpeed * Time.deltaTime);    

        }
        else if (direction == Directions.up)
        {
            transform.position = 
                Vector3.MoveTowards(transform.position, transform.position + new Vector3(0,20), moveSpeed * Time.deltaTime);
        }
        else if (direction == Directions.down)
        {
            transform.position = 
                Vector3.MoveTowards(transform.position, transform.position + new Vector3(0,-20), moveSpeed * Time.deltaTime);
        }
    }
}
