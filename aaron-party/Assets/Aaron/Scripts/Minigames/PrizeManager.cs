using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PrizeManager : MonoBehaviour
{
    private GameController controller;
    [SerializeField] private string characterName;
    [SerializeField] private Image headUi;
    [SerializeField] private Sprite[] heads;
    
    [SerializeField] private int uiNumber; // {1, 2, ..., 8}
    [SerializeField] private TextMeshProUGUI coins;
    [SerializeField] private TextMeshProUGUI orbs;
    [SerializeField] private TextMeshProUGUI mana;
    [SerializeField] private Slider          mpBar;
    [SerializeField] private GameObject      manaPrizePrefab;   
    [SerializeField] private TextMeshProUGUI manaPrizeAmount;   // SHOW HOW MUCH MANA WAS WON
    [SerializeField] private GameObject      prizePrefab;        
    [SerializeField] private TextMeshProUGUI prizeAmount;       // SHOW HOW MUCH GOLD WAS WON
    [SerializeField] private AudioSource     coinCollectSound;
    [SerializeField] private AudioSource     manaCollectSound;
    [SerializeField] private TextMeshProUGUI ranking;
    private PlayerPrevData data;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("Game_Controller").GetComponent<GameController>();
        manaPrizePrefab.SetActive(false);
        ranking.gameObject.SetActive(false);

        if (uiNumber <= controller.nPlayers) {
            SET_PLAYER_HEAD();
            
            data = controller.GET_PLAYER_DATA(uiNumber - 1);
            // controller.playerData[uiNumber - 1].coins += data.prize;
            coins.text = (data.coins - data.prize).ToString();
            orbs.text  = data.orbs.ToString();
            mana.text  = data.mp + "/5";
            mpBar.value = data.mp;
            prizeAmount.text = "+" + (data.prize).ToString();
            
            if (data.firstPlace) {
                manaPrizePrefab.SetActive(true);
                mana.text  = (data.mp) + "/5";
                manaPrizeAmount.text = "+1";
            }

            StartCoroutine( GET_PRIZE_MONEY() );
        }
        else {  this.gameObject.SetActive(false);  }
    }

    private void SET_PLAYER_HEAD()
    {
        switch (name)
        {
            case "Player_1" : characterName = controller.characterName1;    break;
            case "Player_2" : characterName = controller.characterName2;    break;
            case "Player_3" : characterName = controller.characterName3;    break;
            case "Player_4" : characterName = controller.characterName4;    break;
            case "Player_5" : characterName = controller.characterName5;    break;
            case "Player_6" : characterName = controller.characterName6;    break;
            case "Player_7" : characterName = controller.characterName7;    break;
            case "Player_8" : characterName = controller.characterName8;    break;
            default :      Debug.LogError("ERROR : Incorrect Player Name"); break;
        }
        for (int i=0 ; i<heads.Length ; i++) {
            if (heads[i].name.Contains(characterName)) {
                headUi.sprite = heads[i];
                break;
            }
            if (i == heads.Length - 1) {
                Debug.LogError("ERROR : Have not assign character to name (" + characterName + ")");
            }
        }
    }

    private IEnumerator GET_PRIZE_MONEY()
    {
        yield return new WaitForSeconds(2);

        for (int i=1; i<=data.prize ; i++) 
        {
            yield return new WaitForSeconds(0.1f);
            coinCollectSound.Play();
            coins.text = (data.coins - data.prize + i).ToString();
            prizeAmount.text = "+" + (data.prize - i).ToString();
        }
        prizePrefab.SetActive(false);

        // FIRST PLACE GAINS AN EXTRA MP
        if (data.firstPlace) {
            yield return new WaitForSeconds(0.1f);
            if (controller.playerData[uiNumber - 1].mp != 5) controller.playerData[uiNumber - 1].mp++;
            //! DELETE
            // if (controller.p1[0].mp != 5 && uiNumber == 1) controller.p1[0].mp++;
            // if (controller.p2[0].mp != 5 && uiNumber == 2) controller.p2[0].mp++;
            // if (controller.p3[0].mp != 5 && uiNumber == 3) controller.p3[0].mp++;
            // if (controller.p4[0].mp != 5 && uiNumber == 4) controller.p4[0].mp++;
            // if (controller.p5[0].mp != 5 && uiNumber == 5) controller.p5[0].mp++;
            // if (controller.p6[0].mp != 5 && uiNumber == 6) controller.p6[0].mp++;
            // if (controller.p7[0].mp != 5 && uiNumber == 7) controller.p7[0].mp++;
            // if (controller.p8[0].mp != 5 && uiNumber == 8) controller.p8[0].mp++;
            mana.text  = (data.mp) + "/5";
            prizeAmount.text = "+0";
            manaCollectSound.Play();
            mpBar.value = data.mp;
            manaPrizePrefab.SetActive(false);
        }
        ranking.gameObject.SetActive(true);
        DISPLAY_PLAYER_RANKINGS( controller.MY_RANKING( uiNumber - 1) );

    }

    public void DISPLAY_PLAYER_RANKINGS(int xth)
    {
        switch (xth)
        {
            case 0:   ranking.text = "<#ECC233>1<sup>st";   break;
            case 1:   ranking.text = "2<sup>nd";   break;
            case 2:   ranking.text = "3<sup>rd";   break;
            case 3:   ranking.text = "4<sup>th";   break;
            case 4:   ranking.text = "5<sup>th";   break;
            case 5:   ranking.text = "6<sup>th";   break;
            case 6:   ranking.text = "7<sup>th";   break;
            case 7:   ranking.text = "8<sup>th";   break;
        }
    }
}
