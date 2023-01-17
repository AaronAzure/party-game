using System.Collections;
using System.Collections.Generic;
using TMPro;
using Rewired;
using UnityEngine;
using UnityEngine.UI;

public class AwardWinnerManager : MonoBehaviour
{
    private int playerID = 0;
    private Player player;

    private int nPrompt;
    private GameController controller;
    [SerializeField] private Image blackScreen;
    [SerializeField] private float transitionTime = 0.5f;
    [SerializeField] private Transform _A;
    [SerializeField] private Transform _B;
    [SerializeField] private AudioSource bgMusic;
    [SerializeField] private Camera cam;
    [SerializeField] private TextMeshProUGUI aaronText;
    [SerializeField] private GameObject winnerCharacter;    // SetActive(true);
    [SerializeField] private GameObject bonusOrbPrefab;
    private List<GameObject> bonusOrbs;


    public GameObject playerPrefab;
    private GameObject[] players;


    void Start()
    {
        if (GameObject.Find("Game_Controller") != null) {
            controller = GameObject.Find("Game_Controller").GetComponent<GameController>();
        }
        player = ReInput.players.GetPlayer(playerID);
        players = new GameObject[controller.nPlayers];
        bonusOrbs = new List<GameObject>();

        for ( int i=0 ; i<controller.nPlayers ; i++ )
        {
            var player = Instantiate(playerPrefab, 
                    Vector3.Lerp(_A.position, _B.position, (float) (i+1)/(controller.nPlayers+1) ), Quaternion.identity);
            player.name = "Player_" + (i+1); 
            players[i] = player.gameObject;
            CharacterHolder p = player.GetComponent<CharacterHolder>();
            p.playerID = i; p.movable = true;
        }
        
        blackScreen.CrossFadeAlpha(0, transitionTime, false);   // FROM BLACK
        TextIndex();
        // StartCoroutine( TheWinnerIs() );
    }

    private void Update() {
        if (player.GetButtonDown("A")) { nPrompt++; TextIndex(); }   
    }

    private void TextIndex()
    {
        switch (nPrompt)
        {
            case 0 :  { aaronText.text = "Welcome to the finale. Hope that you all had a blast."; break; }
            case 1 :  { aaronText.text = "It is time to reveal who is the greatest mage."; break; }
            case 3 :  { aaronText.text = "Let's get to those results!."; break; }
            case 4 :  { aaronText.text = "But before we can name the winner."; break; }
            case 5 :  { aaronText.text = "We need to identify who will recieve bonus Philosopher's Stones."; break; }
            case 6 :  { aaronText.text = "The first bonus goes to the player who had the most gold at any point."; break; }
            case 7 :  { aaronText.text = "And it goes to..."; break; }
            case 8 :  { 
                aaronText.text = "";
                (List<int> winners, int highscore) = controller.CALCULATE_RICH_WINNER();
                foreach (int winner in winners) { 
                    controller.BONUS_PRIZE(winner); 
                    GameObject bonus = (GameObject) Instantiate(bonusOrbPrefab, players[ winner ].transform.position, Quaternion.identity);
                        bonus.transform.parent = players[ winner ].transform;
                    bonusOrbs.Add(bonus);
                }
                for (int i=0; i<winners.Count ; i++) {
                    string bonusWinnerName = controller.ID_TO_NAME(winners[i]);
                    if (i == 0) { aaronText.text += bonusWinnerName; }
                    else if (i==winners.Count - 1)    { aaronText.text += " & " + bonusWinnerName; }
                    else        { aaronText.text += ", " + bonusWinnerName; }
                }
                aaronText.text += " who had " + highscore + " gold!!";
                break; 
            }
            case 9 :  { aaronText.text = "The second bonus goes to the player who had landed on the most event spaces.";
                foreach(GameObject obj in bonusOrbs) { Destroy(obj); } bonusOrbs.Clear(); break; }
            case 10 : { aaronText.text = "And it goes to..."; break; }
            case 11 : { 
                aaronText.text = "";
                (List<int> winners, int highscore) = controller.CALCULATE_EVENT_WINNER();
                foreach (int winner in winners) { 
                    controller.BONUS_PRIZE(winner); 
                    GameObject bonus = (GameObject) Instantiate(bonusOrbPrefab, players[ winner ].transform.position, Quaternion.identity);
                        bonus.transform.parent = players[ winner ].transform;
                    bonusOrbs.Add(bonus);
                }
                for (int i=0; i<winners.Count ; i++) {
                    string bonusWinnerName = controller.ID_TO_NAME(winners[i]);
                    if (i == 0) { aaronText.text += bonusWinnerName; }
                    else if (i==winners.Count - 1)    { aaronText.text += " & " + bonusWinnerName; }
                    else        { aaronText.text += ", " + bonusWinnerName; }
                }
                aaronText.text += " who landed on " + highscore + " event spaces!!";
                break; 
            }
            case 12 : { aaronText.text = "The final bonus goes to the player who has the most traps on the board.";
                foreach(GameObject obj in bonusOrbs) { Destroy(obj); } bonusOrbs.Clear();  break; }
            case 13 : { aaronText.text = "And it goes to..."; break; }
            case 14 : { 
                aaronText.text = "";
                (List<int> winners, int highscore) = controller.CALCULATE_TRAP_WINNER();
                foreach (int winner in winners) { 
                    controller.BONUS_PRIZE(winner); 
                    GameObject bonus = (GameObject) Instantiate(bonusOrbPrefab, players[ winner ].transform.position, Quaternion.identity);
                        bonus.transform.parent = players[ winner ].transform;
                    bonusOrbs.Add(bonus);
                }
                for (int i=0; i<winners.Count ; i++) {
                    string bonusWinnerName = controller.ID_TO_NAME(winners[i]);
                    if (i == 0) { aaronText.text += bonusWinnerName; }
                    else if (i==winners.Count - 1)    { aaronText.text += " & " + bonusWinnerName; }
                    else        { aaronText.text += ", " + bonusWinnerName; }
                }
                aaronText.text += " who has " + highscore + " traps on the board!!";
                break; 
            }
            case 15 : { aaronText.text = "And the winner is...";
                foreach(GameObject obj in bonusOrbs) { Destroy(obj); } bonusOrbs.Clear();  break; }
            case 16 : { StartCoroutine( TheWinnerIs() );  break; }
        }
    }

    IEnumerator TheWinnerIs()
    {
        aaronText.text = "And the winner is...";
        winnerCharacter.SetActive(true);

        yield return new WaitForSeconds(3);
        blackScreen.CrossFadeAlpha(1, transitionTime, false);   // TO BLACK
        yield return new WaitForSeconds(transitionTime);
        cam.transform.position += new Vector3(20,0,0);
        aaronText.transform.parent.gameObject.SetActive(false);

        yield return new WaitForSeconds(transitionTime);
        blackScreen.CrossFadeAlpha(0, transitionTime, false);   // FROM BLACK
    }
}
