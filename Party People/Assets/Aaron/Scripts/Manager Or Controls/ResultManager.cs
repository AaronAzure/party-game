using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    private GameController ctr;
    public GameObject rankingHeader;
    public PlayerResults pr;

    void Start()
    {
        ctr = GameObject.Find("Game_Controller").GetComponent<GameController>();
        
        for (int i=-1; i<ctr.nPlayers ; i++)
        {
            if (i==-1) {
                var obj = Instantiate(rankingHeader, transform.position, Quaternion.identity);
                
            }
        }
    }
}
