using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using BeautifulTransitions.Scripts.Transitions.Components.Camera;

public class GameManager : MonoBehaviour
{
    // GLOBAL VARIABLES

    [SerializeField] private int nPlayers;
    //private Text textPlayer;

    [Header("Player Info")]
    private int playerOrder;
    private PathFollower player1;
    private PathFollower player2;
    private PathFollower player3;
    private PathFollower player4;
    private PathFollower player5;
    private PathFollower player6;
    private PathFollower player7;
    private PathFollower player8;
    private PathFollower[] gamers;  // ** SCRIPT
    [SerializeField] private GameObject UIcanvas;

    private Camera mainCam;
    [SerializeField] private GameObject sideQuest;
    [SerializeField] private FadeCamera transitionCam;
    private float transitionTime = 1;
    [SerializeField] private Image blackScreen;
    [SerializeField] private GameObject transitionScreen;
    private Animator transitionAnim;
    [SerializeField] private Text turnNumber;
    private int currentTurn;
    private int maxTurn;       // CONST
    private GameController controller;

    [Header("Shogun Seaport")]
    [SerializeField] private GameObject boats;  // ** INSPECTOR
    private Animator boatAnim;
    [SerializeField] private string currentBoat;

    // -------------------------------------------------------------------------------
    // RECEIVE INFO/DATA FROM GAME CONTROLLER

    private void BASED_ON_NPLAYERS()
    {
        switch (nPlayers)
        {
            case 1:
                player2.gameObject.SetActive(false);
                player3.gameObject.SetActive(false);
                player4.gameObject.SetActive(false);
                player5.gameObject.SetActive(false);
                player6.gameObject.SetActive(false);
                player7.gameObject.SetActive(false);
                player8.gameObject.SetActive(false);
                break;
            case 2:
                player3.gameObject.SetActive(false);
                player4.gameObject.SetActive(false);
                player5.gameObject.SetActive(false);
                player6.gameObject.SetActive(false);
                player7.gameObject.SetActive(false);
                player8.gameObject.SetActive(false);
                break;
            case 3:
                player4.gameObject.SetActive(false);
                player5.gameObject.SetActive(false);
                player6.gameObject.SetActive(false);
                player7.gameObject.SetActive(false);
                player8.gameObject.SetActive(false);
                break;
            case 4:
                player5.gameObject.SetActive(false);
                player6.gameObject.SetActive(false);
                player7.gameObject.SetActive(false);
                player8.gameObject.SetActive(false);
                break;
            case 5:
                player6.gameObject.SetActive(false);
                player7.gameObject.SetActive(false);
                player8.gameObject.SetActive(false);
                break;
            case 6:
                player7.gameObject.SetActive(false);
                player8.gameObject.SetActive(false);
                break;
            case 7:
                player8.gameObject.SetActive(false);
                break;
        }
    }

    // -------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        // blackScreen.canvasRenderer.SetAlpha(1.0f);
        // blackScreen.gameObject.SetActive(true);

        playerOrder = 0;
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        controller = GameObject.Find("Game_Controller").GetComponent<GameController>();
        this.maxTurn = controller.maxTurns;
        nPlayers = controller.nPlayers;
        mainCam.gameObject.SetActive(false);
        if (transitionScreen != null) { transitionAnim = transitionScreen.gameObject.GetComponent<Animator>(); }
        if (sideQuest != null) sideQuest.SetActive(false);

        player1 = GameObject.Find("Player_1").GetComponent<PathFollower>();
        player2 = GameObject.Find("Player_2").GetComponent<PathFollower>();
        player3 = GameObject.Find("Player_3").GetComponent<PathFollower>();
        player4 = GameObject.Find("Player_4").GetComponent<PathFollower>();
        player5 = GameObject.Find("Player_5").GetComponent<PathFollower>();
        player6 = GameObject.Find("Player_6").GetComponent<PathFollower>();
        player7 = GameObject.Find("Player_7").GetComponent<PathFollower>();
        player8 = GameObject.Find("Player_8").GetComponent<PathFollower>();

        gamers = new PathFollower[nPlayers];
        for (int i=0 ; i<nPlayers ; i++)
        {
            string p = "Player_" + (i+1);
            gamers[i] = GameObject.Find(p).GetComponent<PathFollower>();
        }

        BASED_ON_NPLAYERS();
        // for (int i=0 ; i<(8 - nPlayers) ; i++)
        // {
        //     gamers[ 7-i ].gameObject.SetActive(false);
        // }
        /***********************************************************************************************/
        //Time.timeScale = 1.5f;  // DELETE
        /***********************************************************************************************/

        // ** Shogun Seaport exclusive
        if (boats != null) { boatAnim = boats.GetComponent<Animator>(); }

        // TURN 2+
        if (controller.hasStarted)
        {
            // ** Shogun Seaport exclusive
            if (boats != null)
            {
                currentBoat = PlayerPrefs.GetString("CurrentBoat");
                Debug.Log("manager.currentBoat = " + currentBoat);
            }

            currentTurn = controller.turnNumber;
            for (int i=0 ; i<nPlayers ; i++)
            {
                controller.RICH_ORB_UPDATE(i);
            }
            controller.ResetAllTraps();
            if (SceneManager.GetActiveScene().name != "Shogun_Seaport") StartCoroutine( DelayChoosingMagicOrbSpace(1) );
            gamers[ controller.playerOrder[playerOrder] ].BEGIN(); 
            StartCoroutine( CAMERA_TRANSITION(0.5f, 1f, false));
            // StartCoroutine(CAMERA_TRANSITION());
            // SCREEN_TRANSITION("Oval_Transition", 0.5f);
        }
        // TURN 1 ONLY
        else
        {
            // ** Shogun Seaport exclusive
            if (boats != null)
            {
                currentBoat = "Norm";
                PlayerPrefs.SetString("CurrentBoat", currentBoat);
            }

            HIDE_UI();
            currentTurn = 1;
            // UPDATE_ALL_INFO();
            if (SceneManager.GetActiveScene().name == "Crystal_Caverns")
            {
                controller.orbCam.gameObject.SetActive(true);
                StartCoroutine( DelayChoosingMagicOrbSpace(0) );
                SCREEN_TRANSITION("Oval_Transition", 0.5f);
            }
            else 
            {
                StartCoroutine( controller.START_THE_GAME(true) );
                mainCam.gameObject.SetActive(true);
                SCREEN_TRANSITION("Oval_Transition", 0.5f);
            }
            // SCREEN FADES FROM BLACK
            // blackScreen.CrossFadeAlpha(0f, transitionTime, false);
            // transitionAnim.Play("Oval_Transition", -1, 0);
        }

        // LAST 5 TURNS
        if ((maxTurn - currentTurn) == 4) { controller.turnMultiplier = 2; }

        // ** Shogun Seaport exclusive
        if (boats != null)
        {
            switch (currentBoat)
            {
                case "Norm" :  boatAnim.Play("BOATS_NORM_IDLE", -1, 0); break;
                case "Gold" :  boatAnim.Play("BOATS_GOLD_IDLE", -1, 0); break;
                case "Evil" :  boatAnim.Play("BOATS_EVIL_IDLE", -1, 0); break;
            }
        }
        
        turnNumber.text = "Turn: " + currentTurn + " / " + maxTurn;
    }

    public IEnumerator DelayChoosingMagicOrbSpace(int method)
    {
        yield return new WaitForSeconds(1f);
        // DOES NOT HAVE A SPECIFIC MAGIC ORB (RANDOM) - TURN 1
        if (method == 0) {
            mainCam.gameObject.SetActive(false);
            controller.CHOOSE_MAGIC_ORB_SPACE("");
        }
        // DOES HAVE A SPECIFIC MAGIC ORB (SINGLE)
        else {
            controller.ResetCurrentMagicOrb();
        }
    }

    public void p1Start() { StartCoroutine( CAMERA_TRANSITION(0,1,false) ); }

    // -------------------------------------------------------------------------------
    public IEnumerator INCREMENT_TURN()
    {
        SCREEN_TRANSITION("Oval_Transition", 0);
        yield return new WaitForSeconds(transitionTime);

        mainCam.gameObject.SetActive(true);
        playerOrder++;
        // SCREEN_TRANSITION("Oval_Transition", 1);
        StartCoroutine( JUST_NEXT_TURN(0) );

    }

    private void UPDATE_ALL_INFO()
    {
        // for (int i=0 ; i<nPlayers ; i++)
        // {
        //     gamers[i].UPDATE_INFORMATION(true);
        // }
        player1.UPDATE_INFORMATION(true);
        if (nPlayers >= 2) player2.UPDATE_INFORMATION(true);
        if (nPlayers >= 3) player3.UPDATE_INFORMATION(true);
        if (nPlayers >= 4) player4.UPDATE_INFORMATION(true);
        if (nPlayers >= 5) player5.UPDATE_INFORMATION(true);
        if (nPlayers >= 6) player6.UPDATE_INFORMATION(true);
        if (nPlayers >= 7) player7.UPDATE_INFORMATION(true);
        if (nPlayers >= 8) player8.UPDATE_INFORMATION(true);
    }



    // ** UI BOARD PLACEMENT REAL-TIME
    public void CHECK_RANKINGS()
    {
        int[] score = new int[nPlayers];
        int[] pID   = new int[nPlayers];    // playerID

        for (int i=0 ; i<pID.Length ; i++) { pID[i] = i; }
        
        for (int i=0 ; i<gamers.Length ; i++)
        {
            score[i] += gamers[i].coins;
            score[i] += (gamers[i].orbs * 1000);
        }

        // SORT EVERYONE'S SCORES (DESCENDING)
        for ( int i=0 ; i<score.Length ; i++ )
        {
            for ( int j=0 ; j<score.Length ; j++ )
            {
                if (score[i] > score[j])
                {
                    int temp = score[i];
                    score[i] = score[j];
                    score[j] = temp;

                    int tempID = pID[i];
                    pID[i] = pID[j];
                    pID[j] = tempID;
                }
            }
        }

        // FIRST -> LAST
        for ( int i=0 ; i<score.Length ; i++ )
        {
            // IF THE SCORE IS THE SAME IS AS THE PREVIOUS SCORE, THEN SKIP
            if (i != 0) if (score[i] == score[i-1]) continue;

            // ALL PLAYERS WHO HAD THAT SCORE
            for ( int j=0 ; j<nPlayers ; j++)
            {
                if (score[i] == (gamers[j].coins + (1000 * gamers[j].orbs)))
                {
                    gamers[ j ].DISPLAY_PLAYER_RANKINGS(i);
                }
            }
        }
    }

    public void NEXT_PLAYER_TURN()
    {
        if (playerOrder < nPlayers)
        {
            mainCam.gameObject.SetActive(false);

            Debug.Log("|| player " + (controller.playerOrder[playerOrder]+1) + " 's TURN");
            int gamer = controller.playerOrder[playerOrder];

            // switch (gamer)
            // {
            //     case 0: transitionCam.transform.parent = player1.transform; 
            //                 transitionCam.CrossTransition(player1._cam);  StartCoroutine( player1.YOUR_TURN() );  break;
            //     case 1: transitionCam.transform.parent = player2.transform; 
            //                 transitionCam.CrossTransition(player2._cam);  StartCoroutine( player2.YOUR_TURN() );  break;
            //     case 2: transitionCam.transform.parent = player3.transform; 
            //                 transitionCam.CrossTransition(player3._cam);  StartCoroutine( player3.YOUR_TURN() );  break;
            //     case 3: transitionCam.transform.parent = player4.transform; 
            //                 transitionCam.CrossTransition(player4._cam);  StartCoroutine( player4.YOUR_TURN() );  break;
            //     case 4: transitionCam.transform.parent = player5.transform; 
            //                 transitionCam.CrossTransition(player5._cam);  StartCoroutine( player5.YOUR_TURN() );  break;
            //     case 5: transitionCam.transform.parent = player6.transform; 
            //                 transitionCam.CrossTransition(player6._cam);  StartCoroutine( player6.YOUR_TURN() );  break;
            //     case 6: transitionCam.transform.parent = player7.transform; 
            //                 transitionCam.CrossTransition(player7._cam);  StartCoroutine( player7.YOUR_TURN() );  break;
            //     case 7: transitionCam.transform.parent = player8.transform; 
            //                 transitionCam.CrossTransition(player8._cam);  StartCoroutine( player8.YOUR_TURN() );  break;
            // }
            switch (gamer)
            {
                case 0:     Debug.Log("Player 1 turn");   StartCoroutine( player1.YOUR_TURN() );  break;
                case 1:     Debug.Log("Player 2 turn");   StartCoroutine( player2.YOUR_TURN() );  break;
                case 2:     Debug.Log("Player 3 turn");   StartCoroutine( player3.YOUR_TURN() );  break;
                case 3:     Debug.Log("Player 4 turn");   StartCoroutine( player4.YOUR_TURN() );  break;
                case 4:     Debug.Log("Player 5 turn");   StartCoroutine( player5.YOUR_TURN() );  break;
                case 5:     Debug.Log("Player 6 turn");   StartCoroutine( player6.YOUR_TURN() );  break;
                case 6:     Debug.Log("Player 7 turn");   StartCoroutine( player7.YOUR_TURN() );  break;
                case 7:     Debug.Log("Player 8 turn");   StartCoroutine( player8.YOUR_TURN() );  break;
            }
        }
        // ALL PLAYERS HAD THEIR TURN
        else
        {
            StartCoroutine( SIDE_QUEST_TIME() );
            // UPDATE_ALL_INFO();

            // // MINIGAME TIME!
            // controller.GAME_START();
            // controller.NEXT_TURN();
            // PlayerPrefs.SetString("CurrentBoat", currentBoat);
            // PlayerPrefs.SetString("sceneName", SceneManager.GetActiveScene().name);
            // PlayerPrefs.Save();

            // // PLAYER TURN AGAIN
            // // playerOrder = 0;
            // ENDING_TRANSITION();

            // // MINIGAME TIME
            // controller.LOAD_MINIGAME();
        }
    }

    public void SCREEN_TRANSITION(string transitionName, float normTime)
    {
        transitionAnim.Play(transitionName, -1, normTime);
    }

    // FADE FROM BLACK, AND NEXT PLAYER TURN
    public IEnumerator CAMERA_TRANSITION(float normTime, float delay, bool useBeaut)
    {
        // bool useBeaut = false;
        // CROSS FADE CAMERAS
        if (useBeaut)
        {
            NEXT_PLAYER_TURN();
        }
        // SCREEN FADES FROM BLACK
        else
        {
            Debug.Log("MANAGER - TRANSITION");
            transitionAnim.Play("Oval_Transition", -1, normTime);
            yield return new WaitForSeconds(delay);
            NEXT_PLAYER_TURN();
        }
    }

    // NO FADING, JUST NEXT PLAYER TURN
    private IEnumerator JUST_NEXT_TURN(float delay)
    {
        yield return new WaitForSeconds(delay);
        NEXT_PLAYER_TURN();
    }

    IEnumerator ENDING_TRANSITION()
    {
        transitionAnim.Play("Oval_Transition", -1, 0);

        yield return new WaitForSeconds(0.5f);
        blackScreen.canvasRenderer.SetAlpha(1);
        blackScreen.gameObject.SetActive(true);
        mainCam.gameObject.SetActive(true);
    }


    // SCREEN FADES TO BLACK IN 0.4s
    public void FADE_TO_BLACK()
    {
        Debug.Log("SCREEN FADING");
        blackScreen.gameObject.SetActive(true);
        blackScreen.CrossFadeAlpha(1f, transitionTime, false);
    }

    // SCREEN FADES FROM BLACK IN 0.4s
    public void FADE_FROM_BLACK()
    {
        blackScreen.gameObject.SetActive(true);
        blackScreen.CrossFadeAlpha(0f, transitionTime, false);
    }

    private IEnumerator SIDE_QUEST_TIME() 
    {
        // HIDE UI STUFF
        UPDATE_ALL_INFO();
        HIDE_UI();
        for (int i=0 ; i<gamers.Length ; i++) { gamers[ i ].HIDE_DATA_CANVAS(); }

        // CAMERA STEUP AND MUTE BACKGROUND MUSIC
        if (GameObject.Find("quest-board") != null) {
            Transform questBoard = GameObject.Find("quest-board").GetComponent<Transform>();
            mainCam.transform.position = questBoard.position + new Vector3(0,3,-30);
            mainCam.orthographicSize = 7.5f;
        }
        if (GameObject.Find("BACKGROUND_MUSIC") != null) {
            AudioSource bgMusic = GameObject.Find("BACKGROUND_MUSIC").GetComponent<AudioSource>();
            bgMusic.volume = 0;
        }
        mainCam.gameObject.SetActive(true);
        SCREEN_TRANSITION("Oval_Transition", 0.5f);

        // SIDE QUEST TIME! UI 
        yield return new WaitForSeconds(transitionTime);
        sideQuest.SetActive(true);
        StartCoroutine(CAMERA_ZOOM());

        // SCREEN FADES FROM BLACK
        yield return new WaitForSeconds(2);
        SCREEN_TRANSITION("Fade_Transition", 0);

        yield return new WaitForSeconds(0.5f);

        // MINIGAME TIME!
        controller.GAME_START();
        controller.NEXT_TURN();
        PlayerPrefs.SetString("CurrentBoat", currentBoat);
        PlayerPrefs.SetString("sceneName", SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();

        // PLAYER TURN AGAIN
        // playerOrder = 0;
        ENDING_TRANSITION();

        // MINIGAME TIME
        controller.LOAD_MINIGAME();
    }

    IEnumerator CAMERA_ZOOM() 
    {
        while (mainCam.orthographicSize > 3) {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            mainCam.orthographicSize -= 0.05f;
            mainCam.transform.position += new Vector3(0,0.02f);
        }
    }


    // WHEN VIEWING MAP, HIDE UI
    public void HIDE_UI() { UIcanvas.SetActive(false); }

    // WHEN NOT VIEWING MAP, UNHIDE UI
    public void UNHIDE_UI() { UIcanvas.SetActive(true); }

    // -------------------------------------------------------------------- //
    // -------------- PAYING SOMEONE COINS FROM TRAP ---------------------- //

    public void PAYING_SOMEONE(string whom, int amount) 
    {
        if      (whom == controller.characterName1) {
            player1.PLAYER_CAM_ON(); player1.beingPayed = true;
            StartCoroutine( player1.UPDATE_PLAYER_COINS(amount) );
        }
        else if (whom == controller.characterName2) {
            player2.PLAYER_CAM_ON(); player2.beingPayed = true;
            StartCoroutine( player2.UPDATE_PLAYER_COINS(amount) );
        }
        else if (whom == controller.characterName3) {
            player3.PLAYER_CAM_ON(); player3.beingPayed = true;
            StartCoroutine( player3.UPDATE_PLAYER_COINS(amount) );
        }
        else if (whom == controller.characterName4) {
            player4.PLAYER_CAM_ON(); player4.beingPayed = true;
            StartCoroutine( player4.UPDATE_PLAYER_COINS(amount) );
        }
        else if (whom == controller.characterName5) {
            player5.PLAYER_CAM_ON(); player5.beingPayed = true;
            StartCoroutine( player5.UPDATE_PLAYER_COINS(amount) );
        }
        else if (whom == controller.characterName6) {
            player6.PLAYER_CAM_ON(); player6.beingPayed = true;
            StartCoroutine( player6.UPDATE_PLAYER_COINS(amount) );
        }
        else if (whom == controller.characterName7) {
            player7.PLAYER_CAM_ON(); player7.beingPayed = true;
            StartCoroutine( player7.UPDATE_PLAYER_COINS(amount) );
        }
        else if (whom == controller.characterName8) {
            player8.PLAYER_CAM_ON(); player8.beingPayed = true;
            StartCoroutine( player8.UPDATE_PLAYER_COINS(amount) );
        }
    }

    public void STEALING_ORB(string whom, int amount) 
    {
        if      (whom == controller.characterName1) {
            player1.PLAYER_CAM_ON(); player1.beingPayed = true;
            StartCoroutine( player1.ORB_GAINED(amount) );
        }
        else if (whom == controller.characterName2) {
            player2.PLAYER_CAM_ON(); player2.beingPayed = true;
            StartCoroutine( player2.ORB_GAINED(amount) );
        }
        else if (whom == controller.characterName3) {
            player3.PLAYER_CAM_ON(); player3.beingPayed = true;
            StartCoroutine( player3.ORB_GAINED(amount) );
        }
        else if (whom == controller.characterName4) {
            player4.PLAYER_CAM_ON(); player4.beingPayed = true;
            StartCoroutine( player4.ORB_GAINED(amount) );
        }
        else if (whom == controller.characterName5) {
            player5.PLAYER_CAM_ON(); player5.beingPayed = true;
            StartCoroutine( player5.ORB_GAINED(amount) );
        }
        else if (whom == controller.characterName6) {
            player6.PLAYER_CAM_ON(); player6.beingPayed = true;
            StartCoroutine( player6.ORB_GAINED(amount) );
        }
        else if (whom == controller.characterName7) {
            player7.PLAYER_CAM_ON(); player7.beingPayed = true;
            StartCoroutine( player7.ORB_GAINED(amount) );
        }
        else if (whom == controller.characterName8) {
            player8.PLAYER_CAM_ON(); player8.beingPayed = true;
            StartCoroutine( player8.ORB_GAINED(amount) );
        }
    }


    // todo ------------------------------------------------------------------ //
    // todo --------------------- CRYSTAL CAVERNS ---------------------------- //

    public IEnumerator MANAGER_CAVING_IN(PathFollower playerToResume)
    {
        // FADE_FROM_BLACK();

        StartCoroutine( controller.CAVING_IN() );
        // yield return new WaitForSeconds(transitionTime);

        yield return new WaitForSeconds(9);
        playerToResume.PLAYER_CAM_ON();

        StartCoroutine(playerToResume.UPDATE_PLAYER_COINS(0));
    }


    // todo ------------------------------------------------------------------ //
    // todo --------------------- SHOGUN SEAPORT ----------------------------- //

    public string WHICH_BOAT_IS_IT() { return currentBoat; }

    public IEnumerator HAPPEN_TRIGGERED(PathFollower playerToResume)
    {
        if (boats == null) { yield break; }
        // FADE_FROM_BLACK();
        mainCam.gameObject.SetActive(true);

        yield return new WaitForSeconds(transitionTime);
        boatAnim.speed = 0.5f;
        switch (currentBoat)
        {
            case "Norm" :  boatAnim.SetTrigger("newBoat"); currentBoat = "Gold";  break;
            case "Gold" :  boatAnim.SetTrigger("newBoat"); currentBoat = "Evil";  break;
            case "Evil" :  boatAnim.SetTrigger("newBoat"); currentBoat = "Norm";  break;
            default : Debug.LogError("manager -- AORJNVanwoejpvn"); break;
        }

        yield return new WaitForSeconds(3.5f);
        mainCam.gameObject.SetActive(false);
        playerToResume.PLAYER_CAM_ON();

        if (playerToResume._movesRemaining > 0 || !playerToResume.diceRolled)
        {
            playerToResume.LEAVE_PORT();
        }
        else 
        {
            StartCoroutine(playerToResume.UPDATE_PLAYER_COINS(0));
        }
    }

}