using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    private GameController controller;
    private CharacterHolder[] players;             // PLAYERS GAMEOBJECT
    public CharacterHolder spawnPlayers;
    public Transform _A;  
    public Transform _B;

    public bool sideQuestTransition;


    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("Game_Controller").GetComponent<GameController>();
        players = new CharacterHolder[controller.nPlayers];
    }

    private void SpawnPlayers(float ratio)
    {
        if (controller.nPlayers < 5) {
            for ( int i=0 ; i<controller.nPlayers ; i++ )
            {
                var player = Instantiate(spawnPlayers, 
                        Vector3.Lerp(_A.position, _B.position, (float) (i+1)/(controller.nPlayers+1) ), Quaternion.identity);
                player.transform.parent = this.transform;
                player.transform.localScale *= ratio;
                player.playerID = i; 
                player.name = "Player_" + (i+1);

                players[i] = player;
            }
        }
        else {
            for ( int i=0 ; i<controller.nPlayers ; i++ )
            {
                var player = Instantiate(spawnPlayers, 
                        Vector3.Lerp(_A.position, _B.position, (float) (i)/(controller.nPlayers-1) ), Quaternion.identity);
                player.transform.parent = this.transform;
                player.transform.localScale *= ratio;
                player.playerID = i; 
                player.name = "Player_" + (i+1);

                players[i] = player;
            }
        }
    }

    
}
