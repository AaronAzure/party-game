using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Rewired;


public class MinigameManager : MonoBehaviour
{
    private string sceneName;
    public bool canPlay;
    public bool timeUp;
    public int nPlayersOut = 0;
    private int c1 = 0;
    private int c2 = 0;
    private int c3 = 0;
    private int c4 = 0;
    private int c5 = 0;
    private int c6 = 0;
    private int c7 = 0;
    private int c8 = 0;

    [SerializeField] private Image blackScreen;
    [SerializeField] private float transitionTime = 0.5f;
    [SerializeField] private TextMeshProUGUI  countdown;
    [SerializeField] private TextMeshProUGUI  timerText;
    [SerializeField] private TextMeshProUGUI  winnerText;
    [SerializeField] private MinigameControls spawnPlayers;

    // SPAWN BETWEEN POINT "_A" TO "_B"
    [SerializeField] private Transform _A;  
    [SerializeField] private Transform _B;

    // SPAWN POS FROM i=0 TO spawnPos.Length
    [SerializeField] private Transform[] spawnPos;


    [SerializeField] private AudioSource bgMusic;
    [SerializeField] private GameObject  ui;
    [SerializeField] private GameObject timeUI;

    private GameController controller;
    private int timer;


    [Header("County Bounty")]
    public int correctAns = 15;

    [SerializeField] public GameObject instances;
    public MinigameControls[] players;             // PLAYERS GAMEOBJECT
    [SerializeField] private AudioSource winMusic;
    [SerializeField] private AudioSource drawMusic;

    [SerializeField] private AudioSource startVoice;
    [SerializeField] private AudioSource finishVoice;
    private string winnerNames;
    private int[] scores;

    // *** UNIQUE *** //
    public float timeToStop;
    public bool bossBattle; // ** INSPECTOR

    [SerializeField] private GameObject whereTheBall;



    // ----------------------------------------------------------------------------------------------------------------------------

    void Start()
    {

        countdown.text = "3";
        controller = GameObject.Find("Game_Controller").GetComponent<GameController>();
        players = new MinigameControls[controller.nPlayers];
        sceneName = SceneManager.GetActiveScene().name;

        if (_A == null && GameObject.Find("SPAWN_A") != null)   _A = GameObject.Find("SPAWN_A").transform;
        if (_B == null && GameObject.Find("SPAWN_B") != null)   _B = GameObject.Find("SPAWN_B").transform;

        // TIMER MAP-SPECIFIC
        if (sceneName == "Sneak_And_Snore") 
        {
            timer = 30;
            SpawnPlayers(1);
        }
        else if (sceneName == "Food_Festival") 
        {
            timer = 10;
            SpawnPlayers(2);
        }
        else if (sceneName == "Colour_Chaos") 
        {
            timer = 60;
            SpawnPlayers_CIRCLE(1.2f, 2.25f);
        }
        else if (sceneName == "Card_Collectors") 
        {
            timer = 30;
            SpawnPlayers(1);
        }
        else if (sceneName == "Leaf_Leap") 
        {
            timer = 30;
            SpawnPlayers_POINTS(1);
        }
        else if (sceneName == "Lava_Or_Leave_'Em") 
        {
            timer = 45;
            SpawnPlayers(1);
        }
        else if (sceneName == "Lilypad_Leapers") 
        {
            timer = 45;
            SpawnPlayers_POINTS(1);
        }
        else if (sceneName == "Stop_Watchers") 
        {
            timer = 30;
            SpawnPlayers(1.5f);
            timeUI.SetActive(false);
        }
        else if (sceneName == "Spotlight-Fight") 
        {
            timer = 30;
            SpawnPlayers_CIRCLE(1, 3);
        }
        else if (sceneName == "Pushy-Penguins") 
        {
            timer = 45;
            SpawnPlayers(1.2f);
        }
        else if (sceneName == "Fun_Run") 
        {
            timer = 60;
            SpawnPlayers(1.5f);
        }
        else if (sceneName == "Money_Belt") 
        {
            timer = 25;
            if (controller.easy) { timer = 30; }
            timeUI.SetActive(false);
            SpawnPlayers_POINTS(1);
        }
        else if (sceneName == "Stamp-By-Me") 
        {
            timer = 25;
            SpawnPlayers_POINTS(1);
        }
        else if (sceneName == "Shocking-Situation") 
        {
            timer = 20;
            SpawnPlayers_CIRCLE(0.75f, 2);
        }
        else if (sceneName == "Attack-On-Titan") 
        {
            timer = 60;
            SpawnPlayers_CIRCLE(0.75f, 2);
        }
        else if (sceneName == "Flower-Shower") 
        {
            timer = 30;
            SpawnPlayers_CIRCLE(0.75f, 2);
        }
        else if (sceneName == "Don't-Be-A-Zombie") 
        {
            SpawnPlayers_CIRCLE(1.25f, 3);
            timer = 60;
            SetupMultiTargetCam();
        }
        else if (sceneName == "Barrier_Bearers") 
        {
            timer = 60;
            SpawnPlayers_CIRCLE(0.75f, 2);
        }
        else if (sceneName == "Plunder-Ground") 
        {
            timer = 30;
            SpawnPlayers_CIRCLE(1, 7.5f);
            SetupMultiTargetCam();
        }
        else if (sceneName == "Pinpoint-The-Endpoint") 
        {
            timer = 17;
            timeUI.SetActive(false);
            SpawnPlayers(1);
        }
        else if (sceneName == "Camo-Cutters") 
        {
            timer = 45;
            SpawnPlayers_CIRCLE(1, 2);
        }
        else if (sceneName == "County-Bounty") 
        {
            timer = 30;
            SpawnPlayers();
        }
        else if (sceneName == "Slay-The-Shades") 
        {
            timer = 45;
            SpawnPlayers();
        }

        
        else if (sceneName == "Aaron-Boss-Battle") 
        {
            timer = 300;
            SpawnPlayers_CIRCLE(0.75f, 2);
        }
        else Debug.LogError("  ERROR : Have not added (spawning players) to PreviewManager");
        


        // PLAY WITHOUT COUNTDOWN
        if (sceneName == "Dojo") 
        {
            timerText.text = timer.ToString();
            blackScreen.gameObject.SetActive(true);
            blackScreen.CrossFadeAlpha(0f, transitionTime, false);  // FADE OUT
            canPlay = true;
            countdown.text = "";
            bgMusic.Play();

            timer = 3000;
            timeUI.SetActive(false);
            SpawnPlayers_CIRCLE(0.75f, 2);
        }
        else {
            timerText.text = timer.ToString();
            blackScreen.gameObject.SetActive(true);
            blackScreen.CrossFadeAlpha(0f, transitionTime, false);  // FADE OUT

            startVoice.Play();
            StartCoroutine( Countdown() );
        }
    }

    private void SpawnPlayers(float ratio=1)
    {
        if (controller.nPlayers < 5) {
            for ( int i=0 ; i<controller.nPlayers ; i++ )
            {
                var player = Instantiate(spawnPlayers, 
                        Vector3.Lerp(_A.position, _B.position, (float) (i+1)/(controller.nPlayers+1) ), Quaternion.identity);
                player.transform.parent = instances.transform;
                player.transform.localScale *= ratio;
                player.playerID = i; 
                player.name = "Player_" + (i+1); 
                player.manager = this.gameObject.GetComponent<MinigameManager>();;
                player.sceneName = this.sceneName;

                players[i] = player;
                if (controller.easy && (sceneName == "Fun_Run") )
                {
                    player.transform.position -= new Vector3(0,150,0);
                }
                if (controller.hard && (sceneName == "Fun_Run") )
                {
                    player.transform.position += new Vector3(0,150,0);
                }
            }
        }
        else {
            for ( int i=0 ; i<controller.nPlayers ; i++ )
            {
                var player = Instantiate(spawnPlayers, 
                        Vector3.Lerp(_A.position, _B.position, (float) (i)/(controller.nPlayers-1) ), Quaternion.identity);
                player.transform.parent = instances.transform;
                player.transform.localScale *= ratio;
                player.playerID = i; 
                player.name = "Player_" + (i+1); 
                player.manager = this.gameObject.GetComponent<MinigameManager>();;
                player.sceneName = this.sceneName;

                players[i] = player;
                if (controller.easy && (sceneName == "Fun_Run") )
                {
                    player.transform.position -= new Vector3(0,150,0);
                }
                if (controller.hard && (sceneName == "Fun_Run") )
                {
                    player.transform.position += new Vector3(0,150,0);
                }
            }
        }
    }

    // EACH INDIVIDUAL SPAWN POINT
    private void SpawnPlayers_POINTS(float ratio=1)
    {
        for ( int i=0 ; i<controller.nPlayers ; i++ )
        {
            var player = Instantiate(spawnPlayers, spawnPos[i].position, Quaternion.identity);
            player.transform.parent = instances.transform;
            player.transform.localScale *= ratio;
            player.playerID = i; 
            player.name = "Player_" + (i+1); 
            player.manager = this.gameObject.GetComponent<MinigameManager>();;
            player.sceneName = this.sceneName;

            players[i] = player;
        }
    }

    private void SpawnPlayers_CIRCLE(float ratio, float radius)
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
            var player = Instantiate(spawnPlayers, spawnPos , Quaternion.identity);
            player.transform.parent = instances.transform;
            player.transform.localScale *= ratio;
            player.playerID = i; 
            player.name = "Player_" + (i+1); 
            player.manager = this.gameObject.GetComponent<MinigameManager>();;
            player.sceneName = this.sceneName;

            players[i] = player;
        }
    }

    private void SetupMultiTargetCam()
    {
        MultipleTargetCamera mtCam = GameObject.Find("Main Camera").GetComponent<MultipleTargetCamera>();
        if (mtCam.targets == null) mtCam.targets = new List<Transform>();
        
        for (int i=0 ; i<players.Length ; i++)
        {
            mtCam.targets.Add( players[i].transform );
        }
    }

    // 3, 2, 1, START!
    private IEnumerator Countdown()
    {
        yield return new WaitForSeconds(1);
        if      (countdown.text == "3")  { countdown.text = "2"; StartCoroutine( Countdown() ); }
        else if (countdown.text == "2")  { countdown.text = "1"; StartCoroutine( Countdown() ); }
        else if (countdown.text == "1")  { countdown.text = "START!"; StartCoroutine( Countdown() ); bgMusic.Play(); }
        else   { countdown.text = ""; canPlay = true; StartCoroutine( TIMER_DECREMENT() ); }
    }

    // TIMER DECREMENTS
    private IEnumerator TIMER_DECREMENT()
    {
        if (countdown.text == "Finished!") yield break;

        yield return new WaitForSeconds(1);
        timer--;
        timerText.text = timer.ToString();
        if (timerText.text == "5" && sceneName == "Stop_Watchers") { timeUI.SetActive(true); }
        // PINPOINT THE ENDPOINT
        if (timerText.text == "7" && sceneName == "Pinpoint-The-Endpoint") { whereTheBall.SetActive(true); }
        if (timerText.text == "5" && sceneName == "Pinpoint-The-Endpoint") { timeUI.SetActive(true); }
        if (timerText.text == "Finished!" && sceneName == "Pinpoint-The-Endpoint") { bgMusic.Stop(); }

        // TIMES UP (STOP WATCHERS)
        if (sceneName == "Stop_Watchers")
        {
            if (timer > 0) { StartCoroutine( TIMER_DECREMENT() ); }
            else           { StartCoroutine( EventGameOver() ); }
        }
        else if (sceneName == "County-Bounty")
        {
            if (timer > 0) { StartCoroutine( TIMER_DECREMENT() ); }
            else           { StartCoroutine( EventGameOver() ); }
        }
        else if (sceneName == "Pinpoint-The-Endpoint")
        {
            if (timer > 0) { StartCoroutine( TIMER_DECREMENT() ); }
            else           { StartCoroutine( EventGameOver() ); }
        }
        // TIMES UP
        else 
        {
            if (timer > 0) { StartCoroutine( TIMER_DECREMENT() ); }
            else           { StartCoroutine( GameOver() ); }
        }
    }

    // IF x == 1, THEN LAST MAN STANDING; ELSE x == 0, THEN UNTIL ALL PLAYERS ARE ELIMINATED;
    public void CheckIfEveyoneIsOut(int x)
    {
        // SOMETHING HAPPENS BEFPORE FINISH SCREEN
        if (sceneName == "Stop_Watchers" && nPlayersOut >= controller.nPlayers - x)
        {
            StartCoroutine( EventGameOver() );
        }
        else if (nPlayersOut >= controller.nPlayers - x)     { StartCoroutine( GameOver() ); }
    }


    // todo : CALCULATES WINNER, GIVES REWARD, LOADS VICTORY SCREEN
    public IEnumerator GameOver()
    {
        if (timeUp) { yield break; }
        finishVoice.Play();
        timeUp = true;
        canPlay = false;
        countdown.text = "Finished!";

        // DON'T DISPLAY RANKING UNTIL CALCULATED COIN MINIGAME
        if (sceneName != "Money_Belt" && sceneName != "Sneak_And_Snore" 
            && sceneName != "Shocking-Situation" && !bossBattle) {
            DISPLAY_PLAYER_RANKINGS();
        }
        STOP_OTHER_SCRIPTS();

        // LOWER BACKGROUND MUSIC VOLUME
        if (bgMusic != null) 
        {
            while (bgMusic.volume > 0) {
                yield return new WaitForSeconds(0.1f);
                bgMusic.volume -= 0.1f;
            }
        }

        yield return new WaitForSeconds(1);
        if (ui != null) ui.SetActive(false);
        
        countdown.text = "";

        // COIN MINIGAME
        if (sceneName == "Sneak_And_Snore") 
        {
            Debug.Log("    GOLD QUEST");
            for (int i=0 ; i<players.Length ; i++)
            {
                players[i].COIN_MINIGAME();
            }
            CALCULATE_MOST_GOLD();
            controller.MINIGAME_PRIZE(c1,c2,c3,c4,c5,c6,c7,c8);     // COINS WON IN MINIGAME (QUEST)
            winMusic.Play();
        }
        else if (sceneName == "Money_Belt")
        {
            Debug.Log("    GOLD QUEST");
            for (int i=0 ; i<players.Length ; i++)
            {
                players[i].COIN_MINIGAME();
            }
            CALCULATE_MOST_GOLD();
            controller.MINIGAME_PRIZE(c1,c2,c3,c4,c5,c6,c7,c8);
            winMusic.Play();
            DISPLAY_PLAYER_RANKINGS();
        }
        else if (sceneName == "Shocking-Situation")
        {
            Debug.Log("    GOLD QUEST");
            for (int i=0 ; i<players.Length ; i++)
            {
                players[i].COIN_MINIGAME();
            }
            CALCULATE_MOST_GOLD();
            controller.MINIGAME_PRIZE(c1,c2,c3,c4,c5,c6,c7,c8);
            winMusic.Play();
            DISPLAY_PLAYER_RANKINGS();
        }
        // RANK-BASED MINIGAME (SCORE-BASED)
        else 
        {
            if (sceneName == "Stop_Watchers") CALCULATE_LEAST_POINTS();
            if (sceneName == "County-Bounty") CALCULATE_LEAST_POINTS();
            else CALCULATE_MOST_POINTS();

            controller.MINIGAME_PRIZE(c1,c2,c3,c4,c5,c6,c7,c8);     // COINS WON IN MINIGAME (QUEST)
            yield return new WaitForSeconds(1);
            if (winnerNames != "Draw!" && winnerNames != "Too Bad!") { winnerNames += "\nWins!"; winMusic.Play(); }
            else { drawMusic.Play(); }
            winnerText.text = winnerNames;
        }

        yield return new WaitForSeconds(3);

        blackScreen.CrossFadeAlpha(1, transitionTime, false);

        yield return new WaitForSeconds(transitionTime);
        // RETURN TO MINIGAME TENT
        if (controller.minigameMode)
        {
            SceneManager.LoadScene("3Quests");
        }
        // BOARD GAME OVER, GO TO FINAL RESULTS
        else if (controller.turnNumber > controller.maxTurns) {
            controller.LOAD_CUTAWAY();
        }
        // RESUME BOARD
        else {
            for ( int i=0 ; i<controller.nPlayers ; i++)    { controller.RICH_ORB_UPDATE(i); }
            SceneManager.LoadScene("0Prize_Money");
        }
    }

    private IEnumerator EventGameOver()
    {
        if (sceneName == "Stop_Watchers")
        {
            for (int i=0 ; i<players.Length ; i++)
            {
                players[i].ShowTime();
            }
            yield return new WaitForSeconds(1);
            StartCoroutine( GameOver() );
        }
        else if (sceneName == "County-Bounty")
        {
            if (GameObject.Find("TO_BE_COUNTED") != null) {
                correctAns = GameObject.Find("TO_BE_COUNTED").transform.childCount;
            }
            for (int i=0 ; i<players.Length ; i++)
            {
                players[i].SHOW_ANSWER();
            }

            GameObject.Find("Correct_Speech").GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);
            GameObject.Find("Correct_Answer").GetComponent<TextMeshPro>().text = correctAns.ToString();

            yield return new WaitForSeconds(1);
            StartCoroutine( GameOver() );
        }
        else if (sceneName == "Pinpoint-The-Endpoint")
        {
            canPlay = false;
            countdown.text = "Finished!";
            bgMusic.Stop();
            MagicBall mb = GameObject.Find("BALL").GetComponent<MagicBall>();
            mb.CHECK_DISTANCE();
        }
    }

    private void STOP_OTHER_SCRIPTS()
    {
        if (sceneName == "Colour_Chaos") {
            AaronColourChaos foundObjects = GameObject.Find("Aaron_Colour").GetComponent<AaronColourChaos>();
            foundObjects.enabled = false;
        }
        else if (sceneName == "Pushy-Penguins") {
            GameObject[] penguins = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject peggy in penguins) { peggy.GetComponent<Penguin>().enabled = false; }

        }
        else if (sceneName == "Spotlight-Fight") {
            // REMOVE ALL SPOTLIGHTS IN MINIGAME
            GameObject[] spotlights = GameObject.FindGameObjectsWithTag("Safe");
            foreach (GameObject light in spotlights) { Destroy(light.gameObject); }
        }
    }

    private void DISPLAY_PLAYER_RANKINGS()
    {
        float[] arr = new float[controller.nPlayers];
        int[] pid = new int[controller.nPlayers];

        // RANGE OF SCORES/POINTS
        for (int i=0 ; i<controller.nPlayers ; i++) { arr[i] = players[i].points; pid[i] = i; }

        // SORT EVERYONE'S SCORES
        if (sceneName != "Stop_Watchers" && sceneName != "County-Bounty") 
        {
            // ASCENDING ORDER
            for ( int i=0 ; i<arr.Length ; i++ )
            {
                for ( int j=0 ; j<arr.Length ; j++ )
                {
                    if (arr[i] > arr[j])
                    {
                        float temp = arr[i];
                        arr[i] = arr[j];
                        arr[j] = temp;
                    }
                    if (pid[i] > pid[j])
                    {
                        int tempId = pid[i];
                        pid[i] = pid[j];
                        pid[j] = tempId;
                    }
                }
            }
        }
        else
        {
            // DESCENDING ORDER
            for ( int i=0 ; i<arr.Length ; i++ )
            {
                for ( int j=0 ; j<arr.Length ; j++ )
                {
                    if (arr[i] < arr[j])
                    {
                        float temp = arr[i];
                        arr[i] = arr[j];
                        arr[j] = temp;
                    }
                    if (pid[i] < pid[j])
                    {
                        int tempId = pid[i];
                        pid[i] = pid[j];
                        pid[j] = tempId;
                    }
                }
            }
        }

        // int same = 0;
        string results = "";
        foreach (float score in arr) { results += score.ToString() + ", "; }
        // Debug.Log(results);

        // FIRST -> LAST
        for ( int i=0 ; i<arr.Length ; i++ )
        {
            // IF THE SCORE IS THE SAME AS THE PREVIOUS SCORE, THEN SKIP
            if (i != 0) if (arr[i] == arr[i-1]) continue;

            // ALL PLAYERS WHO HAD THAT SCORE
            for ( int j=0 ; j<controller.nPlayers ; j++)
            {
                if (players[j].points == arr[i])
                {
                    players[j].DisplayRankPlacement_MANAGER(i);
                }
            }
        }

        // IF THE PLAYER ORDER IS NOT SET, THEN SET NEW PLAYER ORDER BASED ON RANKINGS
        if (!controller.isSetOrder) {
            controller.playerOrder = pid;
        }
    }


    // ------------------------------------------------------------------------------------------

    public void PLAYER_WON_N_COINS(int coins, string name)
    {
        switch (name)
        {
            case "Player_1" :   c1 = coins;   controller.p1[0].prize = coins; break;
            case "Player_2" :   c2 = coins;   controller.p2[0].prize = coins; break;
            case "Player_3" :   c3 = coins;   controller.p3[0].prize = coins; break;
            case "Player_4" :   c4 = coins;   controller.p4[0].prize = coins; break;
            case "Player_5" :   c5 = coins;   controller.p5[0].prize = coins; break;
            case "Player_6" :   c6 = coins;   controller.p6[0].prize = coins; break;
            case "Player_7" :   c7 = coins;   controller.p7[0].prize = coins; break;
            case "Player_8" :   c8 = coins;   controller.p8[0].prize = coins; break;
            default : Debug.LogError("-- WRONG PLAYER NAME (MINIGAME)"); break;
        }
    }

    // FIRST PLACE WINNER RECOVERS A MANA POINT 
    public void FIRST_PLACE_PRIZE(string name)
    {
        switch (name)
        {
            case "Player_1" :   controller.p1[0].firstPlace = true;  break;
            case "Player_2" :   controller.p2[0].firstPlace = true;  break;
            case "Player_3" :   controller.p3[0].firstPlace = true;  break;
            case "Player_4" :   controller.p4[0].firstPlace = true;  break;
            case "Player_5" :   controller.p5[0].firstPlace = true;  break;
            case "Player_6" :   controller.p6[0].firstPlace = true;  break;
            case "Player_7" :   controller.p7[0].firstPlace = true;  break;
            case "Player_8" :   controller.p8[0].firstPlace = true;  break;
            default : Debug.LogError("-- WRONG PLAYER NAME (MINIGAME)"); break;
        }
    }

    private void CALCULATE_MOST_POINTS()
    {
        float[] arr = new float[controller.nPlayers];

        // RANGE OF SCORES/POINTS
        for (int i=0 ; i<controller.nPlayers ; i++)
        {
            arr[i] = players[i].points;
        }

        // SORT THE HIGHEST POINT VALUES (ASCENDING ORDER)
        for ( int i=0 ; i<arr.Length ; i++ )
        {
            for ( int j=0 ; j<arr.Length ; j++ )
            {
                if (arr[i] < arr[j])
                {
                    float temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                }
            }
        }

        // GET REWARD BASED ON SCORE
        for ( int i=0 ; i<controller.nPlayers ; i++)
        {
            for ( int j=0 ; j<controller.nPlayers ; j++ )
            {
                if (players[j].points == arr[i])
                {
                    // FIRST PLAYER GETS AN EXTRA MP
                    if (i == controller.nPlayers - 1) 
                    { 
                        if (players[j].points >= 0 && sceneName != "Lilypad_Leapers")
                        {
                            FIRST_PLACE_PRIZE( players[j].name ); 
                            winnerNames += players[j].characterName + " ";
                        }
                        else if (sceneName == "Lilypad_Leapers")
                        {
                            FIRST_PLACE_PRIZE( players[j].name ); 
                            winnerNames += players[j].characterName + " ";
                        }

                        // IF PLAYER IN FIRST ALSO GOT ELIMINATED AT THE SAME TIME
                        if (players[j].points < 0) {
                            int tied = PLAYER_RANKING(i - 1);
                            PLAYER_WON_N_COINS(tied, players[j].name);
                            if (bossBattle) winnerNames = "Too Bad!";
                            else winnerNames = "Draw!";
                            continue;
                        }
                    }

                    int prize = PLAYER_RANKING(i);
                    PLAYER_WON_N_COINS(prize, players[j].name);
                }
            }
        }
    }

    private void CALCULATE_MOST_GOLD()
    {
        float[] arr = new float[controller.nPlayers];
        // int[] pid = new int[controller.nPlayers];

        // RANGE OF SCORES/POINTS
        for (int i=0 ; i<controller.nPlayers ; i++)
        {
            arr[i] = players[i].points;
        }

        // SORT THE HIGHEST POINT VALUES (ASCENDING ORDER)
        for ( int i=0 ; i<arr.Length ; i++ )
        {
            for ( int j=0 ; j<arr.Length ; j++ )
            {
                if (arr[i] < arr[j])
                {
                    float temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                }
            }
        }

        // GET REWARD BASED ON SCORE
        for ( int i=0 ; i<controller.nPlayers ; i++)
        {
            for ( int j=0 ; j<controller.nPlayers ; j++ )
            {
                if (players[j].points == arr[i])
                {
                    // FIRST PLAYER GETS AN EXTRA MP
                    if (i == controller.nPlayers - 1) 
                    { 
                        FIRST_PLACE_PRIZE( players[j].name ); 

                        // IF PLAYER IN FIRST ALSO GOT ELIMINATED AT THE SAME TIME
                        if (players[j].points < 0) {
                            if (bossBattle) winnerNames = "Too Bad!";
                            else winnerNames = "Draw!";
                            continue;
                        }
                    }
                }
            }
        }
    }

    private void CALCULATE_LEAST_POINTS()
    {
        float[] arr = new float[controller.nPlayers];

        // RANGE OF SCORES/POINTS
        for (int i=0 ; i<controller.nPlayers ; i++)
        {
            arr[i] = players[i].points;
        }

        // SORT THE HIGHEST POINT VALUES (DESCENDING ORDER)
        for ( int i=0 ; i<arr.Length ; i++ )
        {
            for ( int j=0 ; j<arr.Length ; j++ )
            {
                if (arr[i] > arr[j])
                {
                    float temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                }
            }
        }

        // GET REWARD BASED ON SCORE
        for ( int i=0 ; i<controller.nPlayers ; i++)
        {
            for ( int j=0 ; j<controller.nPlayers ; j++ )
            {
                // ALL PLAYERS WHO HAD THAT SCORE
                if (players[j].points == arr[i])
                {
                    // FIRST PLAYER GETS AN EXTRA MP
                    if (i == controller.nPlayers - 1) 
                    { 
                        FIRST_PLACE_PRIZE( players[j].name ); 
                        winnerNames += players[j].characterName + " ";

                        // IF PLAYER IN FIRST ALSO GOT ELIMINATED AT THE SAME TIME
                        if (players[j].points < 0) {
                            int tied = PLAYER_RANKING(i - 1);
                            PLAYER_WON_N_COINS(tied, players[j].name);
                            winnerNames = "Draw!";
                            continue;
                        }
                    }

                    int prize = PLAYER_RANKING(i);
                    PLAYER_WON_N_COINS(prize, players[j].name);
                }
            }
        }
    }

    // HOW MUCH COINS WON BY PLAYER BASED ON THEIR SCORE (IN COMPARISON TO OTHER PLAYERS)
    private int PLAYER_RANKING(int place)
    {
        // LESS GOLD IN COMPETITIVE MODE
        if (!controller.isCasual)
        {
            switch (controller.nPlayers)
            {
                // 0 = last
                case 2 :
                    if (place == 0) return 3;
                    if (place == 1) return 10;
                    break;
                case 3 :
                    if (place == 0) return 3;
                    if (place == 1) return 5;
                    if (place == 2) return 10;
                    break;
                case 4 :
                    if (place == 0) return 0;
                    if (place == 1) return 3;
                    if (place == 2) return 5;
                    if (place == 3) return 10;
                    break;
                case 5 :
                    if (place == 0) return 0;
                    if (place == 1) return 3;
                    if (place == 2) return 5;
                    if (place == 3) return 10;
                    if (place == 4) return 15;
                    break;
                case 6 :
                    if (place == 0) return 0;
                    if (place == 1) return 3;
                    if (place == 2) return 5;
                    if (place == 3) return 8;
                    if (place == 4) return 10;
                    if (place == 5) return 15;
                    break;
                case 7 :
                    if (place == 0) return 0;
                    if (place == 1) return 0;
                    if (place == 2) return 3;
                    if (place == 3) return 5;
                    if (place == 4) return 8;
                    if (place == 5) return 10;
                    if (place == 6) return 15;
                    break;
                case 8 :
                    if (place == 0) return 0;
                    if (place == 1) return 0;
                    if (place == 2) return 3;
                    if (place == 3) return 5;
                    if (place == 4) return 8;
                    if (place == 5) return 10;
                    if (place == 6) return 15;
                    if (place == 7) return 20;
                    break;
            }
        }
        // MORE GOLD IN CASUAL MODE
        else
        {
            switch (controller.nPlayers)
            {
                // 0 = last
                case 2 :
                    if (place == 0) return 5;
                    if (place == 1) return 15;
                    break;
                case 3 :
                    if (place == 0) return 5;
                    if (place == 1) return 10;
                    if (place == 2) return 15;
                    break;
                case 4 :
                    if (place == 0) return 5;
                    if (place == 1) return 8;
                    if (place == 2) return 10;
                    if (place == 3) return 15;
                    break;
                case 5 :
                    if (place == 0) return 3;
                    if (place == 1) return 5;
                    if (place == 2) return 8;
                    if (place == 3) return 10;
                    if (place == 4) return 15;
                    break;
                case 6 :
                    if (place == 0) return 3;
                    if (place == 1) return 5;
                    if (place == 2) return 8;
                    if (place == 3) return 10;
                    if (place == 4) return 12;
                    if (place == 5) return 15;
                    break;
                case 7 :
                    if (place == 0) return 0;
                    if (place == 1) return 3;
                    if (place == 2) return 5;
                    if (place == 3) return 8;
                    if (place == 4) return 10;
                    if (place == 5) return 12;
                    if (place == 6) return 15;
                    break;
                case 8 :
                    if (place == 0) return 0;
                    if (place == 1) return 3;
                    if (place == 2) return 5;
                    if (place == 3) return 8;
                    if (place == 4) return 10;
                    if (place == 5) return 12;
                    if (place == 6) return 15;
                    if (place == 7) return 20;
                    break;
            }
        }

        return 0;
    }
}
