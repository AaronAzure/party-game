using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7.5f;
    [SerializeField] private int playerID = 0;
    [SerializeField] private Player player;


    // Start is called before the first frame update
    void Awake()
    {
        switch (name)
        {
            case "Winged" : playerID = 0; break;
            default :       playerID = 1; break;

        }
    }

    private void Start() {
        player = ReInput.players.GetPlayer(playerID);
    }

    // Update is called once per frame
    void Update()
    {
        MOVE();

        if (player.GetButtonDown("Talk"))
        {
            TALK();
        }
    }


    private void MOVE()
    {
        float moveHorizontal = player.GetAxis("Move Horizontal");
        float moveVertical   = player.GetAxis("Move Vertical");

        Vector3 moveDirection = new Vector3(moveHorizontal, moveVertical, 0);
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }


    private void TALK()
    {
        Debug.Log(name + ": I'm Speaking");
    }
}
