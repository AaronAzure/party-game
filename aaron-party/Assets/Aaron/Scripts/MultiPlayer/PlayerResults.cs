using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Rewired;
using TMPro;

public class PlayerResults : MonoBehaviour
{
    private Player player;
    public int playerId;
    private string characterName;
    private GameController ctr;
    public Sprite[] heads;


    [Header("Ui")]
    public Image headUi;
    public TextMeshProUGUI[] scores;

    /*
     * ENDED WITH N GOLD
     * ENDED WITH N ORBS
     * WON N GOLD
     * RICH GOLD
     * N TRAPS
     * BLUE SPACE
     * RED SPACE
     * ? SPACE
     * ! SPACE
     * SHOPPING
     * DISTANCE
     * DISTANCE (AVG)
     */
    void Start()
    {
        // for (int i=0 ; i<8 ; i++)
        // {
        //     if (name.Contains( i.ToString() ))
        //     {
        //         playerId = i; break;
        //     }
        // }

        ctr = GameObject.Find("Game_Controller").GetComponent<GameController>();
        if (playerId == 0) {
            player = ReInput.players.GetPlayer(playerId);
        }
        if (playerId >= ctr.nPlayers) gameObject.SetActive(false);


        switch (playerId)
        {
            case 0 :    characterName = ctr.characterName1;    break;
            case 1 :    characterName = ctr.characterName2;    break;
            case 2 :    characterName = ctr.characterName3;    break;
            case 3 :    characterName = ctr.characterName4;    break;
            case 4 :    characterName = ctr.characterName5;    break;
            case 5 :    characterName = ctr.characterName6;    break;
            case 6 :    characterName = ctr.characterName7;    break;
            case 7 :    characterName = ctr.characterName8;    break;
        }
        DISPLAY_CHARACTER_HEAD();

        
        if (playerId >= ctr.nPlayers)       gameObject.SetActive(false);
        else DISPLAY_RESULTS();
    }

    void DISPLAY_RESULTS()
    {
        // ENDED WITH N GOLD
            Debug.Log(">> allGold = " + ctr.allGold.Count);
        scores[0].text = ctr.allGold[playerId].ToString();
        // ENDED WITH N ORBS
            Debug.Log(">> allOrb = " + ctr.allOrb.Count);
        scores[1].text = ctr.allOrb[playerId].ToString();
        // WON N GOLD
        scores[2].text = ctr.questOrb[playerId].ToString();
        // RICH GOLD
        scores[3].text = ctr.richOrb[playerId].ToString();
        // N TRAPS
        scores[4].text = ctr.trapOrb[playerId].ToString();
        // BLUE SPACE
        scores[5].text = ctr.blueOrb[playerId].ToString();
        // RED SPACE
        scores[6].text = ctr.redOrb[playerId].ToString();
        // (? SPACE
        scores[7].text = ctr.eventOrb[playerId].ToString();
        // (! SPACE
        // SHOPPING
        scores[9].text = ctr.shopOrb[playerId].ToString();
        // DISTANCE
        scores[10].text = ctr.slowOrb[playerId].ToString();
        // DISTANCE (AVG)
        scores[11].text = ( (float) ctr.slowOrb[playerId] / (float) ctr.maxTurns ).ToString("F1");
    }

    void DISPLAY_CHARACTER_HEAD()
    {
        foreach(Sprite head in heads)
        {
            if (head.name.Contains(characterName))
            {
                headUi.sprite = head;
                break;
            }
        }
    }

    private void Update() {
        if (playerId == 0)
        {
            RESTART();
        }
    }

    void RESTART()
    {
        if (player.GetButtonDoublePressDown("Start"))
        {
            Destroy(ctr.gameObject);
            SceneManager.LoadScene(0);
        }
    }
}
