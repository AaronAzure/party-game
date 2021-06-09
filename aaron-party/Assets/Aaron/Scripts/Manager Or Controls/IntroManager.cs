using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Rewired;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    private int playerID = 0;
    private Player player;

    private GameController controller;
    // [SerializeField] private Image blackScreen;
    [SerializeField] private GameObject transitionScreen;
    private Animator transitionAnim;    // ** SCRIPT
    private float transitionTime = 1;
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
    private CharacterHolder[] gamers;   // ** SCRIPT
    private GameObject[] players;
    public GameObject[] cardsOnBoard;       // ** BY INSPECTOR (DRAG MagicCard into Inspector on each board)
    private GameObject[] chosenCards;       // ** BY SCRIPT (IntroManager)
    private MagicCard[] magicCards;         // ** BY SCRIPT (IntroManager)
    private int[]       playerOrder;    // x = playerID, playerOrder[x] = turn order
    private bool        isStart;        // controller.turnNumber == 1
    private bool        isBonus;        // controller.turnNumber == controller.maxTurns - 4
    public bool         canContinue = true;
    private bool        isOutro;        // controller.turnNumber == controller.maxTurns + 1
    private int         rBonusOrb;
    private List<int>   randomBonus;

    [SerializeField] private GameObject bonusOrbPrefab;
    [SerializeField] private GameObject winnerAura;
    [SerializeField] private GameObject winnerAura1;
    [SerializeField] private GameObject aaronAssistancePrefab;
    private List<GameObject> bonusOrbs;



    [Header("Sound Effects")]
    [SerializeField] private AudioSource whoIsTheWinner;
    [SerializeField] private AudioSource youAreTheWinner;



    public void SCREEN_TRANSITION(string transitionName, float normTime)
    {
        transitionAnim.Play(transitionName, -1, normTime);
    }

    void Start()
    {
        aaron.SetActive(false);
        textUI.SetActive(false);

        // GET SCENE NAME
        sceneName = SceneManager.GetActiveScene().name;
        switch (sceneName)
        {
            case "Crystal_Caverns Intro" :      sceneName = "Crystal Caverns"; break;
            case "Crystal_Caverns_Intro" :      sceneName = "Crystal Caverns"; break;
            case "Shogun_Seaport Intro" :       sceneName = "Shogun Seaport"; break;
            case "Shogun Seaport_Intro" :       sceneName = "Shogun Seaport"; break;
            default : Debug.LogError("ADD -sceneName- to IntroManager!!"); break;
        }

        if (GameObject.Find("Game_Controller") != null) {
            controller = GameObject.Find("Game_Controller").GetComponent<GameController>();
            nPlayers = controller.nPlayers;
            playerOrder = new int[nPlayers];
            gamers = new CharacterHolder[nPlayers]; 

            if      (controller.turnNumber == 1)                        { isStart = true; }
            else if (controller.turnNumber == controller.maxTurns - 4)  { isBonus = true; }
            else if (controller.turnNumber == controller.maxTurns + 1)  { isOutro = true; }
        }

        transitionAnim = transitionScreen.GetComponent<Animator>();
        transitionScreen.SetActive(true);
        if (aaron != null) { anim = aaron.GetComponent<Animator>(); }

        // only controlled by player 1
        player = ReInput.players.GetPlayer(playerID);   
        players = new GameObject[controller.nPlayers];
        bonusOrbs = new List<GameObject>();
        if (isOutro)
        {
            randomBonus = new List<int>();
            randomBonus.Add( 0 );   // rich
            randomBonus.Add( 1 );   // event
            randomBonus.Add( 2 );   // trap
            randomBonus.Add( 3 );   // red
            randomBonus.Add( 4 );   // slow
            randomBonus.Add( 5 );   // shop
        }

        // HIDE ALL CARDS ON THE BOARD
        if (cardsOnBoard != null) { foreach (GameObject card in cardsOnBoard) { card.SetActive(false); } }

        // SPAWN ALL PLAYERS
        // if (isBonus || isOutro) { SpawnAtNodes(); }
        if (SceneManager.GetActiveScene().name == "Crystal_Caverns Intro") { SpawnInCircle(4); }
        else { SpawnInLine(); }
        
        SCREEN_TRANSITION("Oval_Transition", 0.5f);
        StartCoroutine( AaronAppears() );
    }
    
    void SpawnInLine()
    {
        for ( int i=0 ; i<controller.nPlayers ; i++ )
        {
            var player = Instantiate(playerPrefab, 
                    Vector3.Lerp(_A.position, _B.position, (float) (i+1)/(controller.nPlayers+1) ), Quaternion.identity);
            player.name = "Player_" + (i+1); 
            players[i]  = player.gameObject;
            gamers[i]   = player.GetComponent<CharacterHolder>();
            gamers[i].intro = this.GetComponent<IntroManager>();
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
            gamers[i].intro = this.GetComponent<IntroManager>();
            gamers[i].introMode = true;            
            gamers[i].playerID = i; 
            gamers[i].movable = true; gamers[i].minSpeed = 10;
            gamers[i].name = "Player_" + (i+1); 

            players[i] = player;
            if (mtCam != null) { mtCam.targets.Add(player.transform); }
        }
    }

    void SpawnAtNodes()
    {
        for ( int i=0 ; i<controller.nPlayers ; i++ )
        {
            var player = Instantiate(playerPrefab, controller.GET_PLAYER_POS(i), Quaternion.identity);
            player.name = "Player_" + (i+1); 
            players[i]  = player.gameObject;
            gamers[i]   = player.GetComponent<CharacterHolder>();
            gamers[i].intro = this.GetComponent<IntroManager>();
            gamers[i].introMode = true;
            gamers[i].playerID = i; gamers[i].movable = true; gamers[i].minSpeed = 10;
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
        else if (player.GetAnyButtonDown() && !textUI.activeSelf && isBonus && canContinue)
        {
            if (!player.GetButtonDown("Left") && !player.GetButtonDown("Right") && 
                !player.GetButtonDown("Down") && !player.GetButtonDown("Up") && !player.GetButtonDown("X"))
            {
                nPrompt++; TextIndex(); 
            }
        }   
        
    }

    IEnumerator AaronAppears()
    {
        yield return new WaitForSeconds(2);
        aaron.SetActive(true);
        var eff = Instantiate(teleportEffectPrefab, aaron.transform.position, teleportEffectPrefab.transform.rotation);
        Destroy(eff, 1);

        yield return new WaitForSeconds(0.5f);
        textUI.SetActive(true);
        TextIndex();
    }


    private void TextIndex()
    {
        if (isStart)
        {
            switch (nPrompt)
            {
                case 0 :   { 
                    if (sceneName == "Crystal Caverns") { aaronText.text = "Welcome to the <b><#ff68f7>" + sceneName + "</color>."; }
                    if (sceneName == "Shogun Seaport") { aaronText.text = "Welcome to <b><#68b8ff>" + sceneName + "</color>."; }
                    break;
                }
                case 1 :   { aaronText.text = "I'll let my <b>doppelgänger</b> explain the rest."; break; }
                case 2 :   { aaronText.text = "Are you done?"; break; }
                case 3 :   { aaronText.text = "Cool!"; break; }
                case 4 :   { aaronText.text = "Let's determine the turn order"; break; }
                case 5 :   { aaronText.text = "Pick a <i>card</i>, any card (press <b>A</b>)."; REVEAL_MAGIC_CARDS(); break; }
                case 6 :   { aaronText.text = "About time! Let's begin the <i>real</i> <b>PARTY!!</b>"; break; }
                case 7 :   { StartCoroutine( START_THE_GAME() ); break; }
                case 8 :   { aaronText.text = "."; break; }
                case 9 :   { aaronText.text = "."; break; }
                case 10 :  { aaronText.text = "."; break; }
                case 11 :  { aaronText.text = "."; break; }
                case 12 :  { aaronText.text = "."; break; }
                case 13 :  { aaronText.text = "."; break; }
                case 14 :  { aaronText.text = "."; break; }
                case 15 :  { aaronText.text = "."; break; }
                case 16 :  { aaronText.text = "."; break; }
                case 17 :  { aaronText.text = "."; break; }
                case 18 :  { aaronText.text = "."; break; }
                case 19 :  { aaronText.text = "."; break; }
            }
        }
        else if (isBonus)
        {
            switch (nPrompt)
            {
                case 0 :   { aaronText.text = "Calling all winners and losers. It's the LAST 5 turns!"; break; }
                case 1 :   { aaronText.text = "Congratulations on making it this far."; break; }
                case 2 :   { aaronText.text = "Let's reveal everyone's current ranking."; REVEAL_CURRENT_RANKINGS(); break; }
                case 3 :   { aaronText.text = "Quite the disparity, I'd say."; break; }
                case 4 :   { aaronText.text = "But don't give up! Because it's time for..."; break; }
                case 5 :   { aaronText.text = "Wait for it..."; break; }
                case 6 :   { textUI.SetActive(false); canContinue = false; 
                    aaronAssistancePrefab.SetActive(true); StartCoroutine( CONTINUE_TEXT() ); break; }
                case 7 :   { aaronText.text = "What is that, you ask?"; break; }
                case 8 :   { aaronText.text = "Well, I shall now bestow 40 gold pieces to the... umm... not winners."; break; }
                case 9 :   { textUI.SetActive(false); List<int> losers = controller.THE_LOSER();
                        foreach (int loser in losers) {  StartCoroutine( gamers[ loser ].UPDATE_PLAYER_COINS(40) ); }
                        canContinue = false; break; 
                    }
                case 10 :  { foreach (CharacterHolder p in gamers) { p.UPDATE_PLAYER_DATA(); } 
                    textUI.SetActive(true); aaronText.text = "Now back to the GAME!"; break; }
                case 11 :  { aaronText.text = "Also, all spaces gain or lose double the standard amount."; break; }
                case 12 :  { foreach (CharacterHolder p in gamers) { p.UPDATE_PLAYER_DATA(); }
                    StartCoroutine(RESUME_GAME()); break; }
                case 13 :  { aaronText.text = "."; break; }
                case 14 :  { aaronText.text = "."; break; }
                case 15 :  { aaronText.text = "."; break; }
                case 16 :  { aaronText.text = "."; break; }
                case 17 :  { aaronText.text = "."; break; }
                case 18 :  { aaronText.text = "."; break; }
                case 19 :  { aaronText.text = "."; break; }
            }
        }
        else if (isOutro && !controller.isCasual)
        {
            switch (nPrompt)
            {
                case 0 :  { aaronText.text = "Welcome to the Grand Finale."; break; }
                case 1 :  { aaronText.text = "Hope that you all had a blast."; break; }
                case 2 :  { aaronText.text = "It is time to reveal who is the greatest mage."; break; }
                case 3 :  { aaronText.text = "I wonder who it could be?."; break; }
                case 4 :  { aaronText.text = "But before we can name the winner."; break; }
                case 5 :  { aaronText.text = "We need to identify who will recieve bonus <color=red>M<color=orange>a<color=yellow>g<#2DFF00>i<#00C0FF>c <#0084FF>O<#B17AFF>r<#F17AFF>b<color=white>s!"; break; }
                case 6 :  {   rBonusOrb = 0; RANDOM_BONUS_STARTING(1); break; }
                case 7 :  { aaronText.text = "And it goes to..."; break; }
                case 8 :  { aaronText.text = "";
                    (List<int> winners, int highscore) = controller.CALCULATE_RICH_WINNER();
                    foreach (int winner in winners) { 
                        controller.BONUS_PRIZE(winner); 
                        GameObject bonus = (GameObject) Instantiate(bonusOrbPrefab, players[ winner ].transform.position, 
                            Quaternion.identity);
                        bonus.transform.parent = players[ winner ].transform;
                        bonusOrbs.Add(bonus);
                        var eff1 = Instantiate(winnerAura, players[winner].transform.position, winnerAura.transform.rotation);
                        eff1.transform.parent = players[winner].transform;
                        eff1.transform.localScale *= 1.5f;
                        bonusOrbs.Add(eff1);
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
                case 9 :  {   rBonusOrb = 1; RANDOM_BONUS_STARTING(2); ;
                    foreach(GameObject obj in bonusOrbs) { Destroy(obj); } bonusOrbs.Clear(); break; }
                case 10 : { aaronText.text = "And it goes to..."; break; }
                case 11 : { aaronText.text = "";
                    (List<int> winners, int highscore) = controller.CALCULATE_EVENT_WINNER();
                    foreach (int winner in winners) { 
                        controller.BONUS_PRIZE(winner); 
                        GameObject bonus = (GameObject) Instantiate(bonusOrbPrefab, players[ winner ].transform.position, 
                            Quaternion.identity);
                        bonus.transform.parent = players[ winner ].transform;
                        bonusOrbs.Add(bonus);
                        var eff1 = Instantiate(winnerAura, players[winner].transform.position, winnerAura.transform.rotation);
                        eff1.transform.parent = players[winner].transform;
                        eff1.transform.localScale *= 1.5f;
                        bonusOrbs.Add(eff1);
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
                case 12 : {   rBonusOrb = 2; RANDOM_BONUS_STARTING(3); ;
                    foreach(GameObject obj in bonusOrbs) { Destroy(obj); } bonusOrbs.Clear();  break; }
                case 13 : { aaronText.text = "And it goes to..."; break; }
                case 14 : { aaronText.text = "";
                    (List<int> winners, int highscore) = controller.CALCULATE_TRAP_WINNER();
                    foreach (int winner in winners) { 
                        controller.BONUS_PRIZE(winner); 
                        GameObject bonus = (GameObject) Instantiate(bonusOrbPrefab, players[ winner ].transform.position, 
                            Quaternion.identity);
                        bonus.transform.parent = players[ winner ].transform;
                        bonusOrbs.Add(bonus);
                        var eff1 = Instantiate(winnerAura, players[winner].transform.position, winnerAura.transform.rotation);
                        eff1.transform.parent = players[winner].transform;
                        eff1.transform.localScale *= 1.5f;
                        bonusOrbs.Add(eff1);
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
        else if (isOutro && controller.isCasual)
        {
            switch (nPrompt)
            {
                case 0 :  { aaronText.text = "Welcome to the Grand Finale."; break; }
                case 1 :  { aaronText.text = "Hope that you all had a blast."; break; }
                case 2 :  { aaronText.text = "It is time to reveal who is the greatest mage."; break; }
                case 3 :  { aaronText.text = "I wonder who it could be?."; break; }
                case 4 :  { aaronText.text = "But before we can name the winner."; break; }
                case 5 :  { aaronText.text = "We need to identify who will recieve bonus <color=red>M<color=orange>a<color=yellow>g<#2DFF00>i<#00C0FF>c <#0084FF>O<#B17AFF>r<#F17AFF>b<color=white>s!"; break; }
                case 6 :  {  rBonusOrb = PICK_A_RANDOM_BONUS_ORB(); RANDOM_BONUS_STARTING(1); break;  }
                case 7 :  { aaronText.text = "And it goes to..."; break; }
                case 8 :  { aaronText.text = "";
                        
                    (List<int> winners, int highscore) = WINNER_OF_RANDOM_BONUS(-1);
                    foreach (int winner in winners) { 
                        controller.BONUS_PRIZE(winner); 
                        GameObject bonus = (GameObject) Instantiate(bonusOrbPrefab, players[ winner ].transform.position, 
                            Quaternion.identity);
                        bonus.transform.parent = players[ winner ].transform;
                        bonusOrbs.Add(bonus);
                        var eff1 = Instantiate(winnerAura, players[winner].transform.position, winnerAura.transform.rotation);
                        eff1.transform.parent = players[winner].transform;
                        eff1.transform.localScale *= 1.5f;
                        bonusOrbs.Add(eff1);
                    }
                    for (int i=0; i<winners.Count ; i++) {
                        string bonusWinnerName = controller.ID_TO_NAME(winners[i]);
                        if (i == 0) { aaronText.text += bonusWinnerName; }
                        else if (i==winners.Count - 1)    { aaronText.text += " & " + bonusWinnerName; }
                        else        { aaronText.text += ", " + bonusWinnerName; }
                    }
                    RANDOM_BONUS_ENDING(highscore);
                    break; 
                }
                case 9 :  {  rBonusOrb = PICK_A_RANDOM_BONUS_ORB(); RANDOM_BONUS_STARTING(2);
                    foreach(GameObject obj in bonusOrbs) { Destroy(obj); } bonusOrbs.Clear(); break; }
                case 10 : { aaronText.text = "And it goes to..."; break; }
                case 11 : { aaronText.text = "";
                    (List<int> winners, int highscore) = WINNER_OF_RANDOM_BONUS(-1);
                    foreach (int winner in winners) { 
                        controller.BONUS_PRIZE(winner); 
                        GameObject bonus = (GameObject) Instantiate(bonusOrbPrefab, players[ winner ].transform.position, 
                            Quaternion.identity);
                        bonus.transform.parent = players[ winner ].transform;
                        bonusOrbs.Add(bonus);
                        var eff1 = Instantiate(winnerAura, players[winner].transform.position, winnerAura.transform.rotation);
                        eff1.transform.parent = players[winner].transform;
                        eff1.transform.localScale *= 1.5f;
                        bonusOrbs.Add(eff1);
                    }
                    for (int i=0; i<winners.Count ; i++) {
                        string bonusWinnerName = controller.ID_TO_NAME(winners[i]);
                        if (i == 0) { aaronText.text += bonusWinnerName; }
                        else if (i==winners.Count - 1)    { aaronText.text += " & " + bonusWinnerName; }
                        else        { aaronText.text += ", " + bonusWinnerName; }
                    }
                    RANDOM_BONUS_ENDING(highscore);
                    break; 
                }
                case 12 : {    rBonusOrb = PICK_A_RANDOM_BONUS_ORB(); RANDOM_BONUS_STARTING(3);
                    foreach(GameObject obj in bonusOrbs) { Destroy(obj); } bonusOrbs.Clear();  break; }
                case 13 : { aaronText.text = "And it goes to..."; break; }
                case 14 : { aaronText.text = "";
                    (List<int> winners, int highscore) = WINNER_OF_RANDOM_BONUS(-1);
                    foreach (int winner in winners) { 
                        controller.BONUS_PRIZE(winner); 
                        GameObject bonus = (GameObject) Instantiate(bonusOrbPrefab, players[ winner ].transform.position, 
                            Quaternion.identity);
                        bonus.transform.parent = players[ winner ].transform;
                        bonusOrbs.Add(bonus);
                        var eff1 = Instantiate(winnerAura, players[winner].transform.position, winnerAura.transform.rotation);
                        eff1.transform.parent = players[winner].transform;
                        eff1.transform.localScale *= 1.5f;
                        bonusOrbs.Add(eff1);
                    }
                    for (int i=0; i<winners.Count ; i++) {
                        string bonusWinnerName = controller.ID_TO_NAME(winners[i]);
                        if (i == 0) { aaronText.text += bonusWinnerName; }
                        else if (i==winners.Count - 1)    { aaronText.text += " & " + bonusWinnerName; }
                        else        { aaronText.text += ", " + bonusWinnerName; }
                    }
                    RANDOM_BONUS_ENDING(highscore);
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


    // *************************************************************************************** //
    // **************************************** INTRO **************************************** //

    void REVEAL_MAGIC_CARDS()
    {
        anim.Play("Aaron_Cast_Anim", -1, 0);
        choosingCards = true;

        // POSSIBLE PLACEMENTS
        List<int> orderRanks = new List<int>();
        for (int i=0 ; i<nPlayers ; i++) { orderRanks.Add( i ); } 
        if (cardsOnBoard.Length > 0)
        {
            ////hosenCards = new GameObject[nPlayers];
            for (int i=0 ; i<nPlayers ; i++)
            {
                int rng = Random.Range(0,orderRanks.Count);
                cardsOnBoard[i].SetActive(true);
                var eff = Instantiate(
                    teleportEffectPrefab, cardsOnBoard[i].transform.position, teleportEffectPrefab.transform.rotation);
                Destroy(eff, 1);
                ////cardsOnBoard[ orderRanks[ rng ] ].SetActive(true);
                //// chosenCards[i] = cardsOnBoard[ orderRanks[ rng ] ].gameObject;
                orderRanks.RemoveAt(rng);
            }
        }

        magicCards = new MagicCard[nPlayers];
        for (int i=0; i<nPlayers ; i++)
        {
            magicCards[i] = cardsOnBoard[i].GetComponent<MagicCard>();
        }
        //// for (int i=0; i<chosenCards.Length ; i++)
        //// {
        ////     magicCards[i] = chosenCards[i].GetComponent<MagicCard>();
        //// }

        // POSSIBLE PLACEMENTS
        orderRanks.Clear();
        for (int i=0 ; i<nPlayers ; i++) { orderRanks.Add( i ); } 

        if (magicCards.Length > 0)
        {
            for (int i=0 ; i<nPlayers ; i++)
            {
                int rng = Random.Range(0,orderRanks.Count);
                magicCards[i].turnRank = orderRanks[ rng ];
                string place = CardinalRank(magicCards[i].turnRank);
                magicCards[i].rank.text = place;
                orderRanks.RemoveAt(rng);
            }
        }

    // DEBUGGING
        string debug = "";
        foreach (MagicCard card in magicCards)
        {
            debug += card.turnRank.ToString() + ", ";
        }
        Debug.Log(debug);
    }

    public void A_PLAYER_HAS_COLLECTED_A_CARD(int playerID, int placement)
    {
        nReady++;
        controller.playerOrder[ placement ] = playerID;
        // EVERYONE READY
        if (nReady >= nPlayers) { 
            nPrompt++; TextIndex();
            choosingCards = false; 
        }
    }

    IEnumerator START_THE_GAME()
    {
        textUI.SetActive(false);
        SCREEN_TRANSITION("Oval_Transition", 0);

        yield return new WaitForSeconds(transitionTime);
        controller.LOAD_GAME_BOARD();
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

    // *************************************************************************************** //
    // **************************************** BONUS **************************************** //


    IEnumerator RESUME_GAME()
    {
        textUI.SetActive(false);
        SCREEN_TRANSITION("Oval_Transition", 0);

        yield return new WaitForSeconds(1);
        string mySavedScene = PlayerPrefs.GetString("sceneName");
        SceneManager.LoadScene(mySavedScene);
    }
    
    void REVEAL_CURRENT_RANKINGS()
    {
        foreach (CharacterHolder p in gamers) { p.GET_PLAYER_DATA(); }; 
    }

    public void SHOW_RANKINGS()
    {
        canContinue = true;
        foreach (CharacterHolder p in gamers) { p.GET_PLAYER_DATA(); }; 
    }

    IEnumerator CONTINUE_TEXT()
    {
        yield return new WaitForSeconds(3);
        aaronAssistancePrefab.SetActive(false);
        canContinue = true;
        textUI.SetActive(true);
        nPrompt++;
        TextIndex();
    }

    // *************************************************************************************** //
    // **************************************** OUTRO **************************************** //

    int PICK_A_RANDOM_BONUS_ORB()
    {
        int index = Random.Range(0, randomBonus.Count);
        int rng = randomBonus[ index ];
        randomBonus.RemoveAt(index);

        return rng;
    }

    (List<int>, int) WINNER_OF_RANDOM_BONUS(int fixBonus)
    {
        if (fixBonus >= 0)
        {
            rBonusOrb = fixBonus;
        }

        if ( rBonusOrb == 0) { // GOLD      (RICH)
            (List<int> winners, int highscore) = controller.CALCULATE_RICH_WINNER();
            return (winners, highscore);
        }
        if ( rBonusOrb == 1) { // GREEN     (EVENT)
            (List<int> winners, int highscore) = controller.CALCULATE_EVENT_WINNER();
            return (winners, highscore);
        }
        if ( rBonusOrb == 2) { // SILVER    (TRAP)
            (List<int> winners, int highscore) = controller.CALCULATE_TRAP_WINNER();
            return (winners, highscore);
        }
        if ( rBonusOrb == 3) { // RED       (RED)
            (List<int> winners, int highscore) = controller.CALCULATE_RED_WINNER();
            return (winners, highscore);
        }
        if ( rBonusOrb == 4) { // PINK      (SLOW)
            (List<int> winners, int highscore) = controller.CALCULATE_SLOW_WINNER();
            return (winners, highscore);
        }
        if ( rBonusOrb == 5) { // ORANGE    (SHOP)
            (List<int> winners, int highscore) = controller.CALCULATE_SHOP_WINNER();
            return (winners, highscore);
        }
        else 
        {
            Debug.LogError("SOMETHING WRONG WITH BONUS ORBS");
            (List<int> winners, int highscore) = controller.CALCULATE_RICH_WINNER();
            return (winners, highscore);
        }
    }
    
    void RANDOM_BONUS_STARTING(int nBonus)
    {
        string xth = "";
        switch (nBonus)
        {
            case 1:  xth = "<i>first</i>";    break;
            case 2:  xth = "<i>second</i>";   break;
            case 3:  xth = "<i>last</i>";     break;
        }

        if      ( rBonusOrb == 0) { 
            aaronText.text  = "The " + xth + " bonus is the <color=yellow>Rich Orb</color>,"; // GOLD
            aaronText.text += " which goes to the player who had the most gold at any point."; 
        }
        else if ( rBonusOrb == 1) { 
            aaronText.text  = "The " + xth + " bonus is the <#2DFF00>Event Orb</color>,"; // GREEN
            aaronText.text += " which goes to the player who landed on the most event spaces."; 
        }
        else if ( rBonusOrb == 2) { 
            aaronText.text  = "The " + xth + " bonus is the <#B5B5B5>Trap Orb</color>,";  // SILVER
            aaronText.text += " which goes to the player who has the most traps on the board."; 
        }
        else if ( rBonusOrb == 3) { 
            aaronText.text  = "The " + xth + " bonus is the <color=red>Red Orb</color>,"; // RED
            aaronText.text += " which goes to the player who landed on the most red spaces."; 
        }
        else if ( rBonusOrb == 4) { 
            aaronText.text  = "The " + xth + " bonus is the <#B17AFF>Slow Orb</color>,"; // PINK
            aaronText.text += " which goes to the player who moved the least."; 
        }
        else if ( rBonusOrb == 5) { 
            aaronText.text  = "The " + xth + " bonus is the <color=orange>Shop Orb</color>,"; // ORANGE
            aaronText.text += " which goes to the player who shopped the most."; 
        }
        else 
        {
            Debug.LogError("SOMETHING WRONG WITH BONUS ORBS");
        }
    }
    
    void RANDOM_BONUS_ENDING(int score)
    {

        if      ( rBonusOrb == 0) { // GOLD      (RICH)
            aaronText.text += " who had " + score + " gold!!";
        }
        else if ( rBonusOrb == 1) { // GREEN     (EVENT)
            aaronText.text += " who landed on " + score + " event spaces!!";
        }
        else if ( rBonusOrb == 2) { // SILVER    (TRAP)
            aaronText.text += " who has " + score + " traps on the board!!";
        }
        else if ( rBonusOrb == 3) { // RED       (RED)
            aaronText.text += " who landed on " + score + " red spaces!!";
        }
        else if ( rBonusOrb == 4) { // PINK      (SLOW)
            aaronText.text += " who moved only " + score + " spaces!!";
        }
        else if ( rBonusOrb == 5) { // ORANGE    (SHOP)
            aaronText.text += " who spent " + score + " at the shops!!";
        }
        else 
        {
            Debug.LogError("SOMETHING WRONG WITH BONUS ORBS");
        }
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
        eff2.transform.localScale *= 3;
        aaronText.text = "<size=40>"+winnerName+"!!";
        Debug.Log(winnerName + " WINS!!");  // DEBUG
        gamers[winnerId].gameWinner = true; // CANNOT BE PUSHED
        StartCoroutine( TEXT_DISAPPEAR() );
    }

    IEnumerator TEXT_DISAPPEAR()
    {
        controller.Finalise_Data();
        yield return new WaitForSeconds(10);
        // aaronText.gameObject.SetActive(false);

        SCREEN_TRANSITION("Oval_Transition", 0);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("4Results");
    }
    

    /*   */
    private void Sample()
    {
        switch (nPrompt)
        {
            case 0 :   { aaronText.text = "."; break; }
            case 1 :   { aaronText.text = "."; break; }
            case 2 :   { aaronText.text = "."; break; }
            case 3 :   { aaronText.text = "."; break; }
            case 4 :   { aaronText.text = "."; break; }
            case 5 :   { aaronText.text = "."; break; }
            case 6 :   { aaronText.text = "."; break; }
            case 7 :   { aaronText.text = "."; break; }
            case 8 :   { aaronText.text = "."; break; }
            case 9 :   { aaronText.text = "."; break; }
            case 10 :  { aaronText.text = "."; break; }
            case 11 :  { aaronText.text = "."; break; }
            case 12 :  { aaronText.text = "."; break; }
            case 13 :  { aaronText.text = "."; break; }
            case 14 :  { aaronText.text = "."; break; }
            case 15 :  { aaronText.text = "."; break; }
            case 16 :  { aaronText.text = "."; break; }
            case 17 :  { aaronText.text = "."; break; }
            case 18 :  { aaronText.text = "."; break; }
            case 19 :  { aaronText.text = "."; break; }
        }
    }


}