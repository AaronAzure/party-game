﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using Rewired;
using Rewired.Integration.UnityUI;

public class LobbyControls : MonoBehaviour
{
    public int playerID;
    [SerializeField] private Player player;
    [SerializeField] private float moveSpeed = 15f;
    private float moveHorizontal;
    private float moveVertical;
    private float movePower;
    private float timer;
    private bool  longPress;

    [Header("Character")]
    [SerializeField] private GameObject[] characters;
    [SerializeField] private string characterName;
    private GameObject character;       // ** SCRIPT
    private float scaleX;

    [SerializeField] private LobbyManager manager;
    [SerializeField] private GameController controller;
    [SerializeField] private Rigidbody2D rb;
    private Vector3 moveDir;
    [SerializeField] private Animator characterAnimator;
    [SerializeField] private Camera cam;
    public bool touchingBoard;
    public string boardToPlay;  // INTRO
    public string boardName;    // BOARD
    private bool boardSelected;

    private bool sideQuest;
    public GameObject aaron;
    private LobbyQuest playSideQuest;
    [SerializeField] private GameObject minigame;   // TENT

    [SerializeField] private TextMeshProUGUI maxTurns;
    [SerializeField] private TextMeshProUGUI gameStyle;
    [SerializeField] private TextMeshProUGUI gameDifficulty;
    [SerializeField] private TextMeshProUGUI orderStyle;
    [SerializeField] private TextMeshProUGUI restoreMpTurn;
    [SerializeField] private TextMeshPro backToCharacter;
    [SerializeField] private TextMeshPro resumeLastGame;
    private bool resuming;
    private bool canResume;
    [SerializeField] private GameObject timeCirclePrefab;
    [SerializeField] private GameObject timeMaster;

    private int nSetting = 0;
    [Header("Board Settings")]
    [SerializeField] private GameObject _settings;
    [SerializeField] private Image[] settings;
    [SerializeField] private GameObject[] currentSetting;
    [SerializeField] private Image preview;
    [SerializeField] private TextMeshProUGUI pwUi;
    [SerializeField] private GameObject hightlight;
    [SerializeField] private GameObject notEnough;
    private bool setAll = true;
    [SerializeField] private TextMeshProUGUI questNameUi;


    [Header("Quest Settings")]
    [SerializeField] private GameObject _quests;
    [SerializeField] private Image[] questSettings;
    [SerializeField] private TextMeshProUGUI[] settingTexts;
    private string easyMode = "Noob";
    private string normMode = "Gamer";
    private string hardMode = "Master";


    private float knockbackPower = 0.25f;
    private float knockbackDuration = 0.12f;
    private bool dashing;
    private bool beingKnocked;
    private float chargePower;
    private float chargeKb;
    public MultipleTargetCamera multipleTargetCamera;
    [SerializeField] private Minigame questToPlay;
    [SerializeField] private GameObject questListPrefab;
    [SerializeField] private Button[] questList;
    string sceneName;


    [Header("Rewired")]    
    [SerializeField] private RewiredStandaloneInputModule rInput;


    // Start is called before the first frame update
    void Start()
    {
        controller  = GameObject.Find("Game_Controller").GetComponent<GameController>();
        if (playerID == 0) {
            manager     = GameObject.Find("Lobby_Manager").GetComponent<LobbyManager>();
            cam         = GameObject.Find("Main Camera").GetComponent<Camera>();
            sceneName   = SceneManager.GetActiveScene().name;
            rInput.RewiredInputManager = GameObject.Find("Rewired_Input_Manager").GetComponent<InputManager>();
            
            if (sceneName == "3Quests")
            {
                for (int i=0 ; i<settingTexts.Length ; i++)
                {
                    if (i==0)   maxTurns        = settingTexts[i];
                    if (i==1)   gameStyle       = settingTexts[i];
                    if (i==2)   gameDifficulty  = settingTexts[i];
                }
            }
            else if (sceneName == "2Lobby")
            {
                questList = questListPrefab.GetComponentsInChildren<Button>();
                WHAT_MINIGAMES_ARE_AVAILABLE();
            }
        }
        
        rb = this.GetComponent<Rigidbody2D>();
        if (_settings != null)  _settings.SetActive(false);
        if (_quests != null)    _quests.SetActive(false);


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
        }
        // CHANGE CHARACTER
        for (int i=0; i<characters.Length ; i++) {
            if (characterName == characters[i].name) {
                    var obj = Instantiate(characters[i], transform.position, Quaternion.identity, this.transform); 
                    scaleX = obj.transform.localScale.x;
                    character = obj.gameObject;  obj.transform.parent = this.transform;  
                    characterAnimator = obj.GetComponent<Animator>();
                    if (characterAnimator == null) Debug.LogError("no animator :(");
                    break;
                }
                if (i == characters.Length - 1) {
                    Debug.LogError("ERROR : Have not assign character to name (" + characterName + ")");
                }
        }

        player = ReInput.players.GetPlayer(playerID);
        
        if (PlayerPrefsElite.VerifyArray("board-settings")) {
            canResume = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_settings.activeSelf && !_quests.activeSelf) MOVEMENT();

        // LOBBY SCENE
        if (playerID == 0 && sceneName == "2Lobby" && !resuming)
        {
            // ADJUSTING SETTINGS
            if (_settings.activeSelf)
            {
                ADJUST_SETTINGS();
            }
            // GO BACK TO CHARACTER SELECT SCREEN
            else if (backToCharacter.gameObject.activeSelf && player.GetButtonDown("A"))
            {
                Debug.Log("restarting"); RESTART_GAME();
            }
            // RESUME LAST GAME
            else if (resumeLastGame.gameObject.activeSelf && player.GetButtonDown("A") && !resuming && canResume)
            {
                resuming = true;
                StartCoroutine( TIMEMASTER_MAGIC() );
            }
            // GO INTO MINIGAME TENT
            else if (minigame.activeSelf && player.GetButtonDown("A"))
            {
                StartCoroutine( manager.PLAY_MINIGAMES() );
                minigame.gameObject.SetActive(false);
            }
            // vs AARON
            else if (player.GetButtonDown("Y")) StartSideQuest();
            // QUICK MINIGAME (TEST)    //! DELETE 
            else if (player.GetButtonDoublePressDown("Minus")) 
            {
                if (controller.playLatestQuest) controller.QUICK_PLAY();
                else controller.LOAD_MINIGAME();
            }
            else if (boardName != null && Application.CanStreamedLevelBeLoaded(boardName) && player.GetButtonDoublePressDown("X")) 
            {
                if (controller.noList.Count < controller.quests.Count) StartCoroutine( manager.FADE_AND_LOAD_BOARD(boardName) );
            }
            // PLAY BOARD, OPEN SETTINGS
            else if (player.GetButtonDown("A") && touchingBoard && !boardSelected && boardToPlay != null
                && !_settings.activeSelf)
            {
                // BOARD_SELECTED();
                _settings.SetActive(true);
                for (int i=0 ; i<currentSetting.Length ; i++)
                {
                    if (i==0) currentSetting[i].SetActive(true);
                    else currentSetting[i].SetActive(false);
                }
                // foreach (Image setting in settings)  { setting.color = new Color(1,1,1,0.3f); }
                // settings[nSetting].color = new Color(1,1,1,1);
                DISPLAY_DIFFICULTY();
            }
            else if (player.GetButtonDown("X")) moveSpeed = 55;
            else if (player.GetButtonUp("X"))   moveSpeed = 15;
        }
        // QUESTS SCENE
        else if (playerID == 0 && sceneName == "3Quests")
        {
            if (player.GetButtonDown("Start") && !_quests.activeSelf) 
            {
                _quests.SetActive(true);
                foreach (Image setting in questSettings)  { setting.color = new Color(1,1,1,0.3f); }
                questSettings[nSetting].color = new Color(1,1,1,1);
                        
                DISPLAY_DIFFICULTY();
            }
            // ADJUSTING SETTINGS
            else if (_quests.activeSelf)
            {
                ADJUST_QUEST_SETTINGS();
            }
            else if (player.GetButtonDown("A") && questToPlay != null)
            {
                PlayQuest();
            }
            else if (player.GetButtonDoublePressDown("X") && questToPlay != null)
            {
                QuickPlay();
            }
            else if (player.GetButtonDoublePressDown("B"))
            {
                controller.minigameMode = false;
                SceneManager.LoadScene("2Lobby");
            } 
            else if (player.GetButtonDown("X")) moveSpeed = 45;
            else if (player.GetButtonUp("X"))   moveSpeed = 15;
        }
    }

    private void FixedUpdate() 
    {
        if (!_settings.activeSelf && !_quests.activeSelf) MOVE();
    }

    void MOVEMENT()
    {
        moveHorizontal = player.GetAxis("Move Horizontal");
        moveVertical = player.GetAxis("Move Vertical");

        // // FLIP CHARACTER WHEN MOVING RIGHT
        // if (moveHorizontal > 0) { character.transform.localScale = new Vector3(-scaleX, scaleX, scaleX); }
        // else if (moveHorizontal < 0) { character.transform.localScale = new Vector3(scaleX, scaleX, scaleX); }

        // Vector3 direction = new Vector3(moveHorizontal, moveVertical, 0);
        // rb.MovePosition(transform.position + direction * moveSpeed * Time.fixedDeltaTime);
        // // rb.velocity = direction * moveSpeed;

        // // ANIMATION
        // if (moveHorizontal != 0 || moveVertical != 0)
        // {
        //     characterAnimator.SetBool("IsWalking", true);
        //     characterAnimator.speed = moveSpeed * (Mathf.Abs(moveHorizontal) + Mathf.Abs(moveVertical));
        //     movePower = moveSpeed * (Mathf.Abs(moveHorizontal) + Mathf.Abs(moveVertical));
        // }
        // else { 
        //     characterAnimator.SetBool("IsWalking", false); 
        //     characterAnimator.speed = 1;
        //     movePower = 0;
        // }
    }

    void MOVE()
    {
        // FLIP CHARACTER WHEN MOVING RIGHT
        if (moveHorizontal > 0) { character.transform.localScale = new Vector3(-scaleX, scaleX, scaleX); }
        else if (moveHorizontal < 0) { character.transform.localScale = new Vector3(scaleX, scaleX, scaleX); }

        Vector3 direction = new Vector3(moveHorizontal, moveVertical, 0);
        rb.MovePosition(transform.position + direction * moveSpeed * Time.fixedDeltaTime);

        // ANIMATION
        if (moveHorizontal != 0 || moveVertical != 0)
        {
            characterAnimator.SetBool("IsWalking", true);

            if (Mathf.Abs(moveHorizontal) > Mathf.Abs(moveVertical)) 
            { 
                characterAnimator.speed = Mathf.Clamp(moveSpeed * Mathf.Abs(moveHorizontal), 0, 5); 
                movePower = moveSpeed * Mathf.Abs(moveHorizontal);
            }
            else 
            { 
                characterAnimator.speed = Mathf.Clamp(moveSpeed * Mathf.Abs(moveVertical), 0, 5); 
                movePower = moveSpeed * Mathf.Abs(moveVertical);
            }

        }
        else { 
            characterAnimator.SetBool("IsWalking", false); 
            characterAnimator.speed = 1;
            movePower = 0;
        }
    }

    //* CALLED BY BUTTON
    public void PLAY_BOARD()
    {
        if (controller.noList.Count < controller.quests.Count)
        {
            _settings.gameObject.SetActive(false);
            touchingBoard = false;
            if(SceneUtility.GetBuildIndexByScenePath(boardToPlay) >= 0)
            {
                boardSelected = false;
                StartCoroutine( manager.FADE_AND_LOAD_BOARD(boardToPlay) );
            }
            else
            {
                Debug.LogError("BOARD DOES NOT EXIST = (" + boardToPlay + ")");
            }
        }
    }

    // CHOOSE WHICH SIDE QUEST SHOULD AND SHOULDN'T BE PLAYED
    public void SELECT_QUESTS()
    {
        if (currentSetting[0].activeSelf)
        {
            currentSetting[0].SetActive(false); 
            currentSetting[1].SetActive(true);
        }
        else 
        {
            currentSetting[0].SetActive(true); 
            currentSetting[1].SetActive(false);
        }
    }

    private void StartSideQuest()
    {
        if (sideQuest && playSideQuest != null)
        {
            if (playSideQuest.sideQuestMode) return;
            StartCoroutine( playSideQuest.START_SIDE_QUEST() );
            
            StartCoroutine(CAM_OUT());
        }
    }

    IEnumerator CAM_OUT()
    {
        while (multipleTargetCamera.minZoom < 20)
        {
            yield return new WaitForSeconds(0.01f);
            multipleTargetCamera.minZoom += 0.1f;
        }
    }
    public IEnumerator CAM_IN()
    {
        while (multipleTargetCamera.minZoom > 10)
        {
            yield return new WaitForSeconds(0.01f);
            multipleTargetCamera.minZoom -= 0.05f;
        }
    }


    public void PlayQuest()
    {
        if (questToPlay.minigameName == "???") { SceneManager.LoadScene("Dojo"); return; }

        controller.questToPlay = questToPlay.minigameName;
        SceneManager.LoadScene("Overlay");
    }
    
    public void QuickPlay()
    {
        SceneManager.LoadScene(questToPlay.quickName);
    }


    // PRESS 'L' OR 'R' BUTTON
    private void ADJUST_SETTINGS()
    {
        // STOP ADJUSTING SETTING
        if      (player.GetButtonDown("B") || player.GetButtonDown("Start"))
        {
            if (currentSetting[1].activeSelf)
            {
                currentSetting[1].SetActive(false); currentSetting[0].SetActive(true);
            }
            else if (currentSetting[0].activeSelf)
            {
                _settings.gameObject.SetActive(false);
            }
        }
        // SIDE QUEST CUSTOMIZATION
        else if (currentSetting[1].activeSelf)
        {
            if (player.GetButtonDown("X"))
            {
                TOGGLE_ALL();
            }
            DISPLAY_QUEST_PREVIEW();
        }
        // BOARD CUSTOMIZATION
        else if (currentSetting[0].activeSelf)
        {
            // ADJUST SETTING
            if (player.GetButtonDown("R")) {
                switch (EventSystem.current.currentSelectedGameObject.name) 
                {
                    case "Turn_Panel":          controller.maxTurns++; break;
                    case "Style_Panel":         CHANGE_GAME_STYLE(); break;
                    case "Difficulty_Panel":    CHANGE_MINIGAME_DIFFICULTY(true); break;
                    case "Order_Panel":         CHANGE_PLAYER_ORDER_STYLE(); break;
                    case "MP_Restore_Panel":    CHANGE_N_TURN_TO_RECOVER_MP(true); break;
                }
            }
            else if (player.GetButtonDown("L")) {
                switch (EventSystem.current.currentSelectedGameObject.name) 
                {
                    case "Turn_Panel":          controller.maxTurns--; break;
                    case "Style_Panel":         CHANGE_GAME_STYLE(); break;
                    case "Difficulty_Panel":    CHANGE_MINIGAME_DIFFICULTY(false); break;
                    case "Order_Panel":         CHANGE_PLAYER_ORDER_STYLE(); break;
                    case "MP_Restore_Panel":    CHANGE_N_TURN_TO_RECOVER_MP(false); break;
                }
            }
            // LONG PRESS
            else if (player.GetButton("R") && EventSystem.current.currentSelectedGameObject.name == "Turn_Panel")
            {
                INCREMENT_TURN();
            }
            else if (player.GetButton("L") && EventSystem.current.currentSelectedGameObject.name == "Turn_Panel")
            {
                DECREMENT_TURN();
            }
            // NO LONGER LONG PRESS
            else if (!player.GetButton("L") && !player.GetButton("R"))
            {
                timer = 0;
                longPress = false;
            }
        }

        DISPLAY_SETTINGS();
    }


    void ADJUST_QUEST_SETTINGS()
    {
        // STOP ADJUSTING SETTING
        if      (player.GetButtonDown("B") || player.GetButtonDown("Start"))
        {
            _quests.gameObject.SetActive(false);
        }
        // ADJUST SETTING
        else if (player.GetButtonDown("R")) {
            CHANGE_MINIGAME_DIFFICULTY(true);
        }
        else if (player.GetButtonDown("L")) {
            CHANGE_MINIGAME_DIFFICULTY(false);
        }
        // CHOOSE CHARACTERS AGAIN
        else if (player.GetButtonDown("A")) { 
            CHANGE_MINIGAME_DIFFICULTY(true); 
        }

        DISPLAY_DIFFICULTY();
    }

    // LONG PRESS
    void INCREMENT_TURN()
    {
        timer += Time.deltaTime;
        if (timer > 0.3f && !longPress)
        {
            longPress = true;
            timer = 0;
            controller.maxTurns++;
            maxTurns.text = controller.maxTurns + " Turns";
        }
        else if (timer > 0.06f && longPress)
        {
            timer = 0;
            controller.maxTurns++;
            maxTurns.text = controller.maxTurns + " Turns";
        }
    }

    // LONG PRESS
    void DECREMENT_TURN()
    {
        if (controller.maxTurns > 1)
        {
            timer += Time.deltaTime;

            if (timer > 0.3f && !longPress)
            {
                longPress = true;
                timer = 0;
                controller.maxTurns--;
                maxTurns.text = controller.maxTurns + " Turns";
            }
            else if (timer > 0.06f && longPress)
            {
                timer = 0;
                controller.maxTurns--;
                maxTurns.text = controller.maxTurns + " Turns";
            }
        }
    }

    public void CHANGE_GAME_STYLE()
    {
        controller.isCasual = !controller.isCasual;

        if (controller.isCasual)    { gameStyle.text = "Casual"; }
        else                        { gameStyle.text = "Competitive"; }
    }
    
    public void CHANGE_PLAYER_ORDER_STYLE()
    {
        controller.isSetOrder = !controller.isSetOrder;

        if (controller.isSetOrder)    { orderStyle.text = "Fixed"; }
        else                          { orderStyle.text = "Flexible"; }
    }

    private void CHANGE_MINIGAME_DIFFICULTY(bool increase)
    {
        if (increase)
        {
            if      (controller.easy)   {controller.easy = false; controller.norm = true ; controller.hard = false;}
            else if (controller.norm)   {controller.easy = false; controller.norm = false; controller.hard = true ;}
            else if (controller.hard)   {controller.easy = true ; controller.norm = false; controller.hard = false;}
        }
        else
        {
            if      (controller.easy)   {controller.easy = false; controller.norm = false; controller.hard = true ;}
            else if (controller.norm)   {controller.easy = true ; controller.norm = false; controller.hard = false;}
            else if (controller.hard)   {controller.easy = false; controller.norm = true ; controller.hard = false;}
        }
    }

    private void CHANGE_N_TURN_TO_RECOVER_MP(bool increase=true)
    {
        if (controller.restoreMpTurn < 10 && increase) {
            controller.restoreMpTurn++;
        }
        else if (controller.restoreMpTurn > 0 && !increase) {
            controller.restoreMpTurn--;
        }
    }

    private void RESTART_GAME()
    {
        Destroy(controller.gameObject);    
        SceneManager.LoadScene(0);
    }


    // todo ---------------------------------------------------------------
    void DISPLAY_SETTINGS()
    {
        // NUMBER OF TURNS
        maxTurns.text = controller.maxTurns + " Turns";
        if (controller.maxTurns == 1) maxTurns.text = "1 Turn";

        // GAME STYLE ( RANDOM || SKILL & STRATEGY )
        if (controller.isCasual) { gameStyle.text = "Casual"; }
        else                     { gameStyle.text = "Competitive"; }

        // PLAYER ORDER STYLE ( DETERMINED BY SIDE QUEST RANKING )
        if (controller.isSetOrder) { orderStyle.text = "Fixed"; }
        else                       { orderStyle.text = "Flexible"; }

        // EVERY X TURNS TO RESTORE MP
        restoreMpTurn.text = "Every " + controller.restoreMpTurn + " Turns";
        if (controller.restoreMpTurn == 1) restoreMpTurn.text = "Every Turn";
        if (controller.restoreMpTurn == 0) restoreMpTurn.text = "Never";

        DISPLAY_DIFFICULTY();
    }

    void DISPLAY_DIFFICULTY()
    {
        // MINIGAME DIFFICULTY
        if      (controller.easy) gameDifficulty.text = easyMode;
        else if (controller.norm) gameDifficulty.text = normMode;
        else if (controller.hard) gameDifficulty.text = hardMode;
    }

    void DISPLAY_QUEST_PREVIEW()
    {
        if (EventSystem.current.currentSelectedGameObject != null) {
            if (EventSystem.current.currentSelectedGameObject.name != "Select All (Button)")
            {
                preview.sprite = EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite;
                questNameUi.text = EventSystem.current.currentSelectedGameObject.name;
            }
            hightlight.transform.position = EventSystem.current.currentSelectedGameObject.transform.position;
        }
    }

    // **************** BUTTONS **************** //
    private void WHAT_MINIGAMES_ARE_AVAILABLE()
    {
        foreach (Button quest in questList) {
            if (controller.noList.Contains(quest.name)) {
                quest.gameObject.GetComponent<Image>().color = new Color(1, 0.1f, 0.1f, 1);
            }
        }
    }

    public void TOGGLE_MINIGAME()
    {
        string minigameName = EventSystem.current.currentSelectedGameObject.name;
        Debug.Log(">>> "+minigameName);
        if (!controller.ADD_TO_NO_LIST(minigameName))
        {
            EventSystem.current.currentSelectedGameObject.GetComponent<Image>().color = new Color(1, 0.1f, 0.1f, 1);
        }
        else
        {
            EventSystem.current.currentSelectedGameObject.GetComponent<Image>().color = new Color(1,1,1,1);
        }

        NO_SIDE_QUEST();
    }

    public void TOGGLE_ALL()
    {
        // SET ALL (ADD ALL TO NO LIST)
        if (setAll)
        {
            foreach (Button button in questList)
            {
                // ALREADY SELECTED
                if (button.GetComponent<Image>().color == new Color(1, 0.1f, 0.1f, 1))
                {
                    continue;
                }
                if (!controller.ADD_TO_NO_LIST(button.name))
                {
                    button.GetComponent<Image>().color = new Color(1, 0.1f, 0.1f, 1);
                }
            }
            pwUi.text = "All";
            setAll = false;
        }
        // SET NONE (REMOVE ALL TO NO LIST)
        else 
        {
            foreach (Button button in questList)
            {
                // NOT SELECTED
                if (button.GetComponent<Image>().color == new Color(1, 1, 1, 1))
                {
                    continue;
                }
                if (controller.ADD_TO_NO_LIST(button.name))
                {
                    button.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                }
            }
            pwUi.text = "None";
            setAll = true;
        }
    
        NO_SIDE_QUEST();
    }

    // CHECK IF THERE IS AT LEAST ONE MINIGAME AVAILABLE
    private void NO_SIDE_QUEST()
    {
        if (controller.noList.Count >= controller.quests.Count) { notEnough.SetActive(true); }
        else { notEnough.SetActive(false); }
    }


    private IEnumerator TIMEMASTER_MAGIC()
    {
        StartCoroutine( REWINDING_TIME() );
        if (timeMaster != null && timeCirclePrefab != null) {
            Instantiate(timeCirclePrefab, timeMaster.transform.position, timeCirclePrefab.transform.rotation);
            yield return new WaitForSeconds(0.5f);
        }

    }

    private IEnumerator REWINDING_TIME()
    {
        yield return new WaitForSeconds(2f);
        StartCoroutine( manager.JUST_FADE() );

        yield return new WaitForSeconds(0.8f);
        Debug.Log(" resuming last game"); 
        controller.RESUME_LAST_GAME();
    }


    private void OnTriggerEnter2D(Collider2D other) 
    {
        // TALKING TO HOST
        if (other.tag == "Safe" && playerID == 0)
        {
            backToCharacter.gameObject.SetActive(true);
        }    
        // TENT POP-UP TEXT
        if (other.tag == "Node" && playerID == 0)
        {
            minigame.SetActive(true);
        }    
        // MINIGAME TENTS
        if (other.tag == "Special" && playerID == 0)
        {
            questToPlay = other.GetComponent<Minigame>();
        }    
        // TIME MASTER
        if (other.tag == "Respawn" && playerID == 0)
        {
            if (timeMaster == null) timeMaster = other.gameObject;
            resumeLastGame.gameObject.SetActive(true);
        }    
    }
    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.tag == "Safe" && playerID == 0 && !_settings.activeSelf)
        {
            backToCharacter.gameObject.SetActive(false);
        }    
        if (other.tag == "Node" && playerID == 0)
        {
            minigame.SetActive(false);
        }   
        // MINIGAME TENTS
        if (other.tag == "Special" && playerID == 0)
        {
            questToPlay = null;
        }    
        // TIME MASTER
        if (other.tag == "Respawn" && playerID == 0)
        {
            resumeLastGame.gameObject.SetActive(false);
        }    
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.tag == "Player")
        {
            LobbyControls opponent = other.gameObject.GetComponent<LobbyControls>();
            if (opponent != null)
            {
                StartCoroutine( opponent.KnockBackCo(
                    knockbackDuration, 0.25f * movePower, this.transform, 0.1f) );
            }
        }
        
        if (other.gameObject.tag == "Attack" && playerID == 0)
        {
            if (playSideQuest == null)
            {
                playSideQuest = other.gameObject.GetComponent<LobbyQuest>();
                playSideQuest.playerCalled = this.GetComponent<LobbyControls>();
            }
            sideQuest = true;
        }
        if (other.gameObject.tag == "Hurtbox")
        {
            StartCoroutine( ProjectileCo( knockbackDuration, 8, other.transform.position, 0.1f) );
        }
        if (other.gameObject.tag == "AOE")
        {
            StartCoroutine( ProjectileCo( knockbackDuration, 15, other.transform.position, 0.1f) );
        }
    }

    private void OnCollisionExit2D(Collision2D other) 
    {
        if (other.gameObject.tag == "Attack")
        {
            sideQuest = false;
        }
    }

    public IEnumerator KnockBackCo(float knockbackDuration, float knockbackPower, Transform opponent, float chargeDelay)
    {
        float timer = 0;
        beingKnocked = true;
        characterAnimator.speed = 5;
        // if (chargingParticle != null) chargingParticle.SetActive(false); chargePower = 0;
        yield return new WaitForEndOfFrame();

        while (knockbackDuration > timer)
        {
            timer += Time.deltaTime;
            Vector2 direction = ( opponent.transform.position - this.transform.position ).normalized;
            rb.AddForce(-direction * knockbackPower, ForceMode2D.Impulse);
        }

        yield return new WaitForSeconds(knockbackDuration);
        chargeKb = 0;
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(chargeDelay);
        beingKnocked = false;
    }

    public IEnumerator ProjectileCo(float knockbackDuration, float knockbackPower, Vector3 opponent, float chargeDelay)
    {
        if (beingKnocked) yield break;
        float timer = 0;
        beingKnocked = true;
        characterAnimator.speed = 5;
        // if (chargingParticle != null) chargingParticle.SetActive(false); chargePower = 0;
        yield return new WaitForEndOfFrame();

        while (knockbackDuration > timer)
        {
            timer += Time.deltaTime;
            Vector2 direction = ( opponent - this.transform.position ).normalized;
            rb.AddForce(-direction * knockbackPower, ForceMode2D.Impulse);
            if (opponent == null) yield break;
        }

        yield return new WaitForSeconds(knockbackDuration);
        chargeKb = 0;
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(chargeDelay);
        beingKnocked = false;
    }
}
