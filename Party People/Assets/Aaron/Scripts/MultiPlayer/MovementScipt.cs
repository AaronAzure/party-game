using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class MovementScipt : MonoBehaviour
{
    public int playerID;
    public Player player;
    public float moveSpeed = 5;
    public Rigidbody2D rb;


    void Start() 
    {
        player = ReInput.players.GetPlayer(playerID);
    }

    // Update is called once per frame
    void Update()
    {
        MOVEMENT();
    }

    void MOVEMENT()
    {
        // float moveHorizontal = player.GetAxis("Move Horizontal");
        // float moveVertical = player.GetAxis("Move Vertical");


        // // FLIP CHARACTER WHEN MOVING RIGHT
        // if (moveHorizontal > 0 )
        // {
        //     character.transform.localScale = new Vector3(-scaleX, scaleX, scaleX);
        // }
        // else if (moveHorizontal < 0)
        // {
        //     character.transform.localScale = new Vector3(scaleX, scaleX, scaleX);
        // }
        // Vector3 direction = new Vector3(moveHorizontal, moveVertical, 0);
        // rb.MovePosition(transform.position + direction * moveSpeed * Time.fixedDeltaTime);
        // // rb.velocity = direction * moveSpeed;

        // // ANIMATION
        // if (moveHorizontal != 0 || moveVertical != 0)
        // {
        //     _anim.SetBool("IsWalking", true);
        //     _anim.speed =  moveSpeed * (Mathf.Abs(moveHorizontal) + Mathf.Abs(moveVertical)) ;
        // }
        // else { 
        //     _anim.SetBool("IsWalking", false); 
        //     _anim.speed = 1;
        // }
    }
}
