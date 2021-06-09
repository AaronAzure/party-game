using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Rewired;
using UnityEngine.SceneManagement;

public class OutroManager : MonoBehaviour
{
    private int playerID = 0;
    private Player player;

    private GameController controller;
    [SerializeField] private Image blackScreen;
    [SerializeField] private float transitionTime = 0.5f;
    [SerializeField] private Transform _A;
    [SerializeField] private Transform _B;
    [SerializeField] private AudioSource bgMusic;
    [SerializeField] private MultipleTargetCamera mtCam;

    [SerializeField] private GameObject         aaron;
    private Animator                            anim;   // ** SCRIPT
    [SerializeField] private GameObject         teleportEffectPrefab;


    private int nReady;
    private int nPlayers;
    private int nPrompt;
    private string sceneName;
    [SerializeField] private GameObject         textUI;
    [SerializeField] private TextMeshProUGUI    aaronText;

    private bool choosingCards;
    public GameObject playerPrefab;
    private GameObject[] players;
    private CharacterHolder[] gamers;   // ** SCRIPT
    private int[]        playerOrder;   // x = playerID, playerOrder[x] = turn order


    [SerializeField] private GameObject bonusOrbPrefab;
    [SerializeField] private GameObject winnerAura;
    [SerializeField] private GameObject winnerAura1;
    [SerializeField] private GameObject winnerAura2;
    private List<GameObject> bonusOrbs;



    [Header("Sound Effects")]
    [SerializeField] private AudioSource whoIsTheWinner;
    [SerializeField] private AudioSource youAreTheWinner;


    void Awake()
    {
        aaron.SetActive(false);
        blackScreen.gameObject.SetActive(false);
        textUI.SetActive(false);
    }

    IEnumerator FADE_FROM_BLACK()
    {
        for (int i=0 ; i<100 ; i++)
        {
            yield return new WaitForEndOfFrame();
            blackScreen.color -= new Color(0,0,0,0.01f);
            if (blackScreen.color.a <= 0) { yield break; }
        }
    }
    
    IEnumerator FADE_TO_BLACK()
    {
        for (int i=0 ; i<100 ; i++)
        {
            yield return new WaitForEndOfFrame();
            blackScreen.color += new Color(0,0,0,0.01f);
            // if (blackScreen.color.a >= 1) { yield break; }
        }
    }

    void FadeOut() { blackScreen.CrossFadeAlpha(0,transitionTime,false); }

    void Start()
    {
        blackScreen.gameObject.SetActive(true);
        blackScreen.canvasRenderer.SetAlpha(1f);

        if (GameObject.Find("Game_Controller") != null) {
            controller = GameObject.Find("Game_Controller").GetComponent<GameController>();
            nPlayers = controller.nPlayers;
            playerOrder = new int[nPlayers];
            gamers = new CharacterHolder[nPlayers]; 
        }
        if (aaron != null) { anim = aaron.GetComponent<Animator>(); }
        // only controlled by player 1
        player = ReInput.players.GetPlayer(playerID);   
        players = new GameObject[controller.nPlayers];
        bonusOrbs = new List<GameObject>();

        if (SceneManager.GetActiveScene().name == "Crystal_Caverns Outro")
        {
            SpawnInCircle(4);
        }
        else { SpawnInLine(); }
        
        FadeOut();
        // StartCoroutine( FADE_FROM_BLACK() );
        StartCoroutine( AaronIntroduces() );
    }
    
    void SpawnInLine()
    {
        for ( int i=0 ; i<controller.nPlayers ; i++ )
        {
            var player = Instantiate(playerPrefab, 
                    Vector3.Lerp(_A.position, _B.position, (float) (i+1)/(controller.nPlayers+1) ), Quaternion.identity);
            player.name = "Player_" + (i+1); 
            players[i] = player.gameObject;
            gamers[i] = player.GetComponent<CharacterHolder>();
            // gamers[i].intro = this.GetComponent<IntroManager>();
            // gamers[i] = p;
            gamers[i].introMode = true;
            gamers[i].playerID = i; gamers[i].movable = true; gamers[i].minSpeed = 10;
            if (mtCam != null) { mtCam.targets.Add(player.transform); }
        }
    }

    void SpawnInCircle(float radius)
    {
        for ( int i=0 ; i<controller.nPlayers ; i++ )
        {
            /* Distance around the circle */  
            var radians = 2 * Mathf.PI / controller.nPlayers * i;
            
            /* Get the vector direction */ 
            var vertical = Mathf.Sin(radians);
            var horizontal = Mathf.Cos(radians); 
            
            var spawnDir = new Vector3 (-horizontal, vertical);
            
            /* Get the spawn position */ 
            Vector3 spawnPos = _A.transform.position + (spawnDir * radius); // Radius is just the distance away from the point
            
            /* Now spawn */
            var player = Instantiate(playerPrefab, spawnPos , Quaternion.identity);
            gamers[i] = player.GetComponent<CharacterHolder>();
            gamers[i].introMode = true;            
            gamers[i].playerID = i; 
            gamers[i].movable = true; gamers[i].minSpeed = 10;
            gamers[i].name = "Player_" + (i+1); 

            players[i] = player;
            if (mtCam != null) { mtCam.targets.Add(player.transform); }
        }
    }

    private void Update() {
        if (player.GetAnyButtonDown() && textUI.activeSelf && !choosingCards) 
        { 
            if (!player.GetButtonDown("Left") && !player.GetButtonDown("Right") && 
                !player.GetButtonDown("Down") && !player.GetButtonDown("Up") && !player.GetButtonDown("X"))
            {
                nPrompt++; TextIndex(); 
            }
        }   
    }

    IEnumerator AaronIntroduces()
    {
        yield return new WaitForSeconds(2);
        aaron.SetActive(true);
        var eff = Instantiate(teleportEffectPrefab, aaron.transform.position, teleportEffectPrefab.transform.rotation);
        Destroy(eff, 1);

        yield return new WaitForSeconds(0.5f);
        textUI.SetActive(true);
        TextIndex();
    }

    IEnumerator REVEAL_THE_WINNER()
    {
        string winnerName = controller.THE_WINNER();
        bgMusic.Stop();
        whoIsTheWinner.Play();

        yield return new WaitForSeconds(5);
        youAreTheWinner.Play();
        whoIsTheWinner.Stop();

        yield return new WaitForEndOfFrame();
        foreach (CharacterHolder p in gamers) { p.GET_PLAYER_DATA(); }; 
        int winnerId = controller.NAME_TO_ID(winnerName);
        var eff1 = Instantiate(winnerAura, gamers[winnerId].transform.position, winnerAura.transform.rotation);
        eff1.transform.parent = gamers[winnerId].transform;
        eff1.transform.localScale *= 2;
        var eff2 = Instantiate(winnerAura1, gamers[winnerId].transform.position + new Vector3(0,1), winnerAura.transform.rotation);
        eff2.transform.parent = gamers[winnerId].transform;
        eff2.transform.localScale *= 2;
        aaronText.text = "<size=50>"+winnerName+"!!";
    }

    private string CardinalRank(int n)
    {
        switch (n)
        {
            case 0 : return "1<sup>st";
            case 1 : return "2<sup>nd";
            case 2 : return "3<sup>rd";
            case 3 : return "4<sup>th";
            case 4 : return "5<sup>th";
            case 5 : return "6<sup>th";
            case 6 : return "7<sup>th";
            case 7 : return "8<sup>th";
        }
        return "?";
    }   


    private void TextIndex()
    {
        switch (nPrompt)
        {
            case 0 :  { aaronText.text = "Welcome to the finale. Hope that you all had a blast."; break; }
            case 1 :  { aaronText.text = "It is time to reveal who is the greatest mage."; break; }
            case 3 :  { aaronText.text = "Let's get to those results!."; break; }
            case 4 :  { aaronText.text = "But before we can name the winner."; break; }
            case 5 :  { aaronText.text = "We need to identify who will recieve bonus <color=red>M<color=orange>a<color=yellow>g<#2DFF00>i<#00C0FF>c <#0084FF>O<#B17AFF>r<#F17AFF>b<color=white>s!"; break; }
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
            case 16 : { 
                StartCoroutine( REVEAL_THE_WINNER() );  
                break; }
        }
    }

    
}