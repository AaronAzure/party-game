using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using Rewired;
using Rewired.Integration.UnityUI;

public class MinigameControls : MonoBehaviour
{
    // public static MinigameControls itself;

    [SerializeField] public  int playerID;  // ASSIGN FROM LevelManager
    [SerializeField] private Player player;
    [SerializeField] private GameObject  placeBubble;
    [SerializeField] private TextMeshPro placetext;
    private float moveSpeed;
    private float jumpSpeed;
    [SerializeField] private GameObject crystalised;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private Collider2D cursorCollider;
    private bool clicked;
    private float scaleX;


    private bool bouncePhysics;
    private float movePower;
    private float knockbackPower = 0.25f;
    private float knockbackDuration = 0.11f;

    [Header("Pause")]
    [SerializeField] private GameObject pauseUi;
    [SerializeField] private GameObject[] freePlayButtons;
    [SerializeField] private TextMeshProUGUI whoPaused;

    [Header("Rewired")]
    [SerializeField] private RewiredStandaloneInputModule rInput;


    // *********************** ASSIGN FROM LevelManager *********************** //
    [Header("Character")]
    public string characterName;
    private Animator _anim;
    private SpriteRenderer cursorSprite;
    [SerializeField] private GameObject character;   // UNIVERSAL REFERENCE TO CHARACTER GAMEOBJECT (ANY)
    [SerializeField] private GameObject[] characters;   // ** INSPECTOR (ALL CHARACTER PREFABS)
    // [SerializeField] private GameObject[] heads;        // ** INSPECTOR (ALL CHARACTER HEADS)
    [SerializeField] private GameObject[] cursors;      // ** INSPECTOR (ALL CHARACTER CURSORS)


    [SerializeField] private GameObject felixH;
    [SerializeField] private GameObject jacobH;
    [SerializeField] private GameObject laurelH;
    [SerializeField] private GameObject mauriceH;
    [SerializeField] private GameObject mimiH;
    [SerializeField] private GameObject pinkinsH;
    [SerializeField] private GameObject sweeterellaH;
    [SerializeField] private GameObject thanatosH;
    [SerializeField] private GameObject charlotteH;


    [SerializeField] public LevelManager manager;
    [SerializeField] public PreviewManager pw;
    public string sceneName;    // SET BY PreviewManager 
    private GameController controller;
    

    // ---------------- IN GAME RELATED ---------------- //
    [Header("In Game Related")]
    private Vector3 moveDir;    // DIRECTION OF MOVEMENT 
    private float moveHorizontal;    // DIRECTION OF MOVEMENT 
    private float moveVertical;    // DIRECTION OF MOVEMENT 
    [SerializeField] private GameObject instances;  // GAMEOBJECT CONTAINING INSTANTIATE CHILD
    [SerializeField] private GameObject floatingTextPrefab;
    [SerializeField] private GameObject goldPrefab;
    [SerializeField] private GameObject mistakeEmoji;
    private int prevPoints = 10;
    [SerializeField] private GameObject dumplingsPrefab;
    [SerializeField] private GameObject stagePrefab;
    private StageTimer stageTimer;  // SET IN SCRIPT
    [SerializeField] private Dumplings  foodPlate;
    [SerializeField] private GameObject progressBar;    // CARD COLLECTORS
    [SerializeField] private BeanStalk  leafs;      // Find()
    private int nleaf;
    private GameObject topLeaf;
    private float origXpos;
    private float origYpos;
    private float leafHeight = 3.9f;
    private bool leapLeft;
    private bool leapRight;
    [SerializeField] private GameObject leafPrefab; // INSPECTOR
    [SerializeField] private GameObject layout;
    [SerializeField] public TextMeshProUGUI score;
    [SerializeField] public TextMeshPro scoreHead;
    [SerializeField] private GameObject goldImg;
    private bool outsideOfEffect;
    private int coins;
    public float points;
    public bool cursorMode; // ACCESSED BY LevelManager OR PreviewManager
    private SortingGroup sortingGroup;
    [SerializeField] private Camera splitCam;
    [SerializeField] private GameObject splitCanvas;
    [SerializeField] private Image      splitBorder;
    private bool isInvincible;


    [Header("Platform Related")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask whatIsGround;
    private float maxFallSpeed;
    private bool isGrounded;
    private bool inAir;
    public  bool isOut;
    private bool isFinished;


    [Header("Lilypad Leapers")]
    [SerializeField] private GameObject lilypadPrefab;
    [SerializeField] private GameObject waterBG;
    [SerializeField] private GameObject finishBG;
    [SerializeField] private GameObject sandBG;
    [SerializeField] private GameObject startBG;
    private Transform[] lilypads;
    private bool isCorrect;
    private bool isWrong;
    private bool isLeaping;
    private Lilypad lp;  // FIND()
    private int nPad;
    private float timer;
    private Vector3 finalPos;


    [Header("Spotlight Fight")]
    [SerializeField] private GameObject chargingParticle;
    [SerializeField] private GameObject magicCircle;
    [SerializeField] private SpriteRenderer dashDirection;
    [SerializeField] private Sprite[] dashPowerSprites;
    private bool dashing;
    private bool beingKnocked;
    private float chargePower;
    private float chargeKb;
    private float rotationSpeed = 15;


    [Header("Fun Run")]
    [SerializeField] private AudioSource checkpointSound;
    [SerializeField] private GameObject checkpointEffect;
    private GameObject respawnPoint;    // RUN TIME


    [Header("Money Belt")]
    private GameObject boxOpener;
    private Box boxToOpen;


    [Header("Stamp By Me")]
    [SerializeField] private GameObject docPrefab;
    [SerializeField] private GameObject stamperPrefab;
    private Animator stamper;
    private bool justStamped;
    private Document currentDoc;
    private int nDoc;
    private bool[] docs;


    [Header("Attack Of Titan")]
    [SerializeField] private GameObject floatingSpellTextPrefab;
    [SerializeField] private Sprite titanSpellIcon;
    private float accRate = 0.3f;
    private float accSpeed = 30;
    private float titanFactor = 3;
    private float oldSize;
    private bool  canAcc;
    public  bool  isTitan;
    public  FreeSpellNode currentCircle;

    private bool isZombie;

    [Header("Barrier Bearers")]
    private bool isMoving;
    private bool barrierOn;
    private float moveTimer;
    [SerializeField] private GameObject mpBar;
    [SerializeField] private GameObject mpFill;
    private float mp = 100;
    [SerializeField] private TextMeshPro nMp;


    [Header("Plunder-Ground")]
    [SerializeField] private GameObject[] heads;
    [SerializeField] private GameObject mask;
    [SerializeField] private Transform maskPos;
    TreasureSpawner ts;  // **

    private float sTimer = 0;
    private float nMask = 3;    // { 1, 2, 3, 4, 5}


    [Header("Sound Effects")]
    [SerializeField] private AudioSource coinPickup;
    [SerializeField] private AudioSource electrocuted;
    [SerializeField] private AudioSource fivePointSound;
    [SerializeField] private AudioSource threePointSound;
    [SerializeField] private AudioSource twpPointSound;
    [SerializeField] private AudioSource onePointSound;
    
    [Header("Prefab Effects")]
    [SerializeField] private GameObject dizzyEffect;
    [SerializeField] private GameObject teleportEffect;
    [SerializeField] private GameObject teleBlueEffect;
    [SerializeField] private GameObject barrierEffect;
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private GameObject deathElectricEffect;


    [Header("Boss Battles")]
    [SerializeField] private GameObject restorePrefab;
    [SerializeField] private GameObject hurtPrefab;
    int characterN;
    int nShots = 1;
    bool canShootAgain = true;
    bool shooting;
    bool hasMoved;
    GameObject shot;    // ** RUNTIME
    [SerializeField] private GameObject[] projectileEffect;
    [SerializeField] private GameObject healthBar;
    private int health = 5;
    [SerializeField] private SpriteRenderer hp;
    [SerializeField] private Sprite[]   hearts;
    private string bossScene;
    [SerializeField] private GameObject challenge;
    

    // ------------------------------------------------------------------ // 

    void Start()
    {
        if (pauseUi != null) pauseUi.SetActive(false);
        if (mask != null)   mask.SetActive(false);

        controller  = GameObject.Find("Game_Controller").GetComponent<GameController>();
        layout.SetActive(false);

        // CHANGE CHARACTER
        switch (name)
        {
            case "Player_1" : characterName = controller.characterName1;    playerID = 0; break;
            case "Player_2" : characterName = controller.characterName2;    playerID = 1; break;
            case "Player_3" : characterName = controller.characterName3;    playerID = 2; break;
            case "Player_4" : characterName = controller.characterName4;    playerID = 3; break;
            case "Player_5" : characterName = controller.characterName5;    playerID = 4; break;
            case "Player_6" : characterName = controller.characterName6;    playerID = 5; break;
            case "Player_7" : characterName = controller.characterName7;    playerID = 6; break;
            case "Player_8" : characterName = controller.characterName8;    playerID = 7; break;
        }
        // Gameobject character
        if (!cursorMode)
        {
            dashDirection.gameObject.SetActive(false);
            for (int i=0 ; i<characters.Length ; i++) {
                if (characterName == characters[i].name) {
                    var obj = Instantiate(characters[i], transform.position, Quaternion.identity, this.transform); 
                    character = obj.gameObject; obj.transform.parent = instances.transform;  _anim = obj.GetComponent<Animator>();
                    break;
                }
                if (i == characters.Length - 1) {
                    Debug.LogError("ERROR : Have not assign character to name (" + characterName + ")");
                }
            }
            scaleX = character.transform.localScale.x;
        }
        // CHANGE CURSOR
        else 
        {
            for (int i=0 ; i<cursors.Length ; i++) {
                if (cursors[i].name.Contains(characterName)) {
                    var obj = Instantiate(cursors[i], transform.position + new Vector3(0.75f, -0.75f), Quaternion.identity, this.transform); 
                    character = obj.gameObject; obj.transform.parent = instances.transform;
                    cursorSprite = obj.GetComponent<SpriteRenderer>();
                    break;
                }
                if (i == cursors.Length - 1) {
                    Debug.LogError("ERROR : Have not assign character to name (" + characterName + ")");
                }
            }
        }
        // UI SCORE CHARACTER FACE
        switch (characterName)
        {
            case "Sweeterella" :    sweeterellaH.SetActive(true); break;
            case "Laurel" :         laurelH.SetActive(true);      break;
            case "Thanatos" :       thanatosH.SetActive(true);    break;
            case "Mimi" :           mimiH.SetActive(true);        break;
            case "Maurice" :        mauriceH.SetActive(true);     break;
            case "Jacob" :          jacobH.SetActive(true);       break;
            case "Pinkins" :        pinkinsH.SetActive(true);     break;
            case "Felix" :          felixH.SetActive(true);       break;
            case "Charlotte" :      charlotteH.SetActive(true);       break;
            default :       Debug.LogError("ERROR : Have not assign character to name (" + characterName + ")"); break;
        }


        // character.transform.localScale = new Vector3(scaleX,scaleX,scaleX);

        player = ReInput.players.GetPlayer(playerID);

        // PLAYER SPEED, etc
        if (sceneName == "Sneak_And_Snore" || sceneName == "Sneak And Snore")
        {
            GAME_SETUP();
            moveSpeed = 2;
            goldImg.SetActive(true); 
            rb = GetComponent<Rigidbody2D>();
            RESET_PLAYER_UI();
        }
        else if (sceneName == "Food_Festival" || sceneName == "Feast-ival")
        {
            GAME_SETUP();
            moveSpeed = 0;
            RESET_PLAYER_UI();
        }
        else if (sceneName == "Colour_Chaos" || sceneName == "Colour Chaos")    // KNOCKBACK
        {
            moveSpeed = 5;
            rb = GetComponent<Rigidbody2D>();
            bouncePhysics = true;
            knockbackPower = 0.25f;
        }
        else if (sceneName == "Card_Collectors" || sceneName == "Card Collectors")
        {
            moveSpeed = 8;
            rb = GetComponent<Rigidbody2D>();
            _collider.enabled = false;
            cursorCollider.enabled = false;
            RESET_PLAYER_UI();
        }
        else if (sceneName == "Leaf_Leap" || sceneName == "Leaf Leap")
        {
            moveSpeed = 8;
            jumpSpeed = 25;
            if (manager != null) splitCam.gameObject.transform.parent = manager.instances.transform;
            if (pw != null)      splitCam.gameObject.transform.parent = pw.transform;

            splitCam.transform.position = new Vector3(splitCam.transform.position.x, this.transform.position.y + 3, -10);
            if (controller.nPlayers == 4 || controller.nPlayers >= 7) {  
                splitCam.orthographicSize *= 1.25f;
            }
            transform.position -= new Vector3(1.1f,0);
            origXpos = transform.position.x;
            origYpos = transform.position.y;
            rb = GetComponent<Rigidbody2D>();
            leafs = GameObject.Find("Bean_Stalk").GetComponent<BeanStalk>();
            rb.gravityScale = 6.5f;
            RESET_PLAYER_UI();
            if (manager != null) SPLIT_SCREEN_SETUP();
            if (pw != null)      SPLIT_SCREEN_PREVIEW();
            SPAWN_FIRST_LEAF();
            score.text = "0m";
            scoreHead.text = "0m";
        }
        else if (sceneName == "Lava_Or_Leave_'Em" || sceneName == "Lava Or Leave 'Em")  // KNOCKBACK
        {
            moveSpeed = 5;
            rb = GetComponent<Rigidbody2D>();
            bouncePhysics = true;
        }
        else if (sceneName == "Lilypad_Leapers" || sceneName == "Lilypad Leapers")
        {
            moveSpeed = 7.5f;
            if (manager != null) splitCam.gameObject.transform.parent = manager.instances.transform;
            if (pw != null)      splitCam.gameObject.transform.parent = pw.transform;

            splitCam.transform.position = new Vector3(splitCam.transform.position.x, this.transform.position.y + 3, -10);

            lp = GameObject.Find("Lilypad").GetComponent<Lilypad>();
            lilypads = new Transform[25];

            if (manager != null) SPLIT_SCREEN_SETUP();
            if (pw != null)      SPLIT_SCREEN_PREVIEW();
            GAME_SETUP();
            // StartCoroutine( WHY() );
        }
        else if (sceneName == "Stop_Watchers" || sceneName == "Stop Watchers")
        {
            GAME_SETUP();
            moveSpeed = 0;
            RESET_PLAYER_UI();
        }
        else if (sceneName == "Spotlight-Fight" || sceneName == "Spotlight Fight")  // KNOCKBACK
        {
            RESET_PLAYER_UI();
            moveSpeed = 3;
            knockbackPower = 0.5f;
            rb = GetComponent<Rigidbody2D>();
            bouncePhysics = true;

            points = 0;
            if (manager != null)    score.text = points.ToString("F1");
            else                    scoreHead.text = points.ToString("F1");
        }
        else if (sceneName == "Pushy-Penguins" || sceneName == "Pushy Penguins")    // KNOCKBACK
        {
            // RESET_PLAYER_UI();
            moveSpeed = 4;
            knockbackPower = 0.2f;
            rb = GetComponent<Rigidbody2D>();
            bouncePhysics = true;
        }
        else if (sceneName == "Fun_Run" || sceneName == "Fun Run")  // KNOCKBACK
        {
            // RESET_PLAYER_UI();
            moveSpeed = 5;
            knockbackPower = 0.25f;
            rb = GetComponent<Rigidbody2D>();
            bouncePhysics = true;
            if (manager != null) SPLIT_SCREEN_SETUP();
            if (pw != null)      SPLIT_SCREEN_PREVIEW();
            splitCam.orthographicSize *= 1.5f;
        }
        else if (sceneName == "Money_Belt" || sceneName == "Coin-veyor")
        {
            RESET_PLAYER_UI();
            goldImg.SetActive(true);
        }
        else if (sceneName == "Stamp-By-Me" || sceneName == "Stamp By Me")
        {
            if (characterName == "Felix")               { _anim.Play("Felix_Invisible", -1, 0); }
            else if (characterName == "Jacob")          { _anim.Play("Jacob_Invisible", -1, 0); }
            else if (characterName == "Laurel")         { _anim.Play("Laurel_Invisible", -1, 0); }
            else if (characterName == "Maurice")        { _anim.Play("Maurice_Invisible", -1, 0); }
            else if (characterName == "Mimi")           { _anim.Play("Mimi_Invisible", -1, 0); }
            else if (characterName == "Pinkins")        { _anim.Play("Pink_Pumpkin_Invisible", -1, 0); }
            else if (characterName == "Sweeterella")    { _anim.Play("Sweeterella_Invisible", -1, 0); }
            else if (characterName == "Thanatos")       { _anim.Play("Thanatos_Invisible", -1, 0); }
            character.transform.position -= new Vector3(-2,3);
            character.transform.localScale *= 4;
            if (manager != null) splitCam.gameObject.transform.parent = manager.instances.transform;
            if (pw != null)      splitCam.gameObject.transform.parent = pw.transform;

            if ( GameObject.Find("Doc_Stamp") != null) {
                docs = GameObject.Find("Doc_Stamp").GetComponent<DocStamp>().isGood;
                var doc = Instantiate(docPrefab, transform.position, Quaternion.identity);
                doc.transform.parent = instances.transform;
                currentDoc = doc.gameObject.GetComponent<Document>();
                currentDoc.goodDoc.SetActive(true);
            }
            if ( stamperPrefab != null) {
                var obj = Instantiate(stamperPrefab, transform.position + new Vector3(0,3,0), Quaternion.identity);
                obj.transform.parent = instances.transform;
                stamper = obj.GetComponent<Animator>();
            }

            RESET_PLAYER_UI();

            if (manager != null) SPLIT_SCREEN_SETUP();
            if (pw != null)      SPLIT_SCREEN_PREVIEW();
            splitCam.transform.position = new Vector3(this.transform.position.x,0,-10);
            
            score.text = "0";
            scoreHead.text = "0";
        }
        else if (sceneName == "Shocking Situation" || sceneName == "Shocking-Situation")
        {
            moveSpeed = 4f;
            knockbackPower = 0.25f;
            rb = GetComponent<Rigidbody2D>();
            bouncePhysics = true;
            goldImg.SetActive(true);
            RESET_PLAYER_UI();
        }
        else if (sceneName == "Attack Of Titan" || sceneName == "Attack-On-Titan")
        {
            moveSpeed = 4f;
            knockbackPower = 0.25f;
            rb = GetComponent<Rigidbody2D>();
            bouncePhysics = true;
            oldSize = this.transform.localScale.x;
            titanFactor *= this.transform.localScale.x;
        }
        else if (sceneName == "Flower Shower" || sceneName == "Flower-Shower")
        {
            moveSpeed = 5;
            knockbackPower = 0.25f;
            rb = GetComponent<Rigidbody2D>();
            bouncePhysics = true;
            RESET_PLAYER_UI();
        }
        else if (sceneName == "Don't Be A Zombie" || sceneName == "Don't-Be-A-Zombie")
        {
            moveSpeed = 5;
            knockbackPower = 0.25f;
            rb = GetComponent<Rigidbody2D>();
            bouncePhysics = true;
        }
        else if (sceneName == "Barrier Bearers" || sceneName == "Barrier_Bearers")
        {
            moveSpeed = 5;
            knockbackPower = 0.25f;
            rb = GetComponent<Rigidbody2D>();
            bouncePhysics = true;
            mpBar.SetActive(true);
        }
        else if (sceneName == "Plunder-Ground" || sceneName == "Plunder Ground")
        {
            character.SetActive(false);
            mask.SetActive(true);
            moveSpeed = 7;
            knockbackPower = 0;
            rb = GetComponent<Rigidbody2D>();
            if (manager != null) mask.transform.parent = manager.instances.transform;
            if (pw != null) mask.transform.parent = pw.transform;
            points = 0;
            RESET_PLAYER_UI();
            DISPLAY_CHARACTER_HEAD();
            ts = GameObject.Find("TREASURE_SPAWNER").GetComponent<TreasureSpawner>();
            if (ts == null) Debug.LogError("-- COULDN'T FIND TREASURE SP");
        }
       
       
       else if (sceneName == "Aaron Boss Battle" || sceneName == "Aaron-Boss-Battle" || sceneName == "Dojo")
        {
            switch (characterName)
            {
                case "Felix" :        characterN = 0;    break;
                case "Jacob" :        characterN = 1;    break;
                case "Laurel" :       characterN = 2;    break;
                case "Maurice" :      characterN = 3;    break;
                case "Mimi" :         characterN = 4;    break;
                case "Pinkins" :      characterN = 5;    break;
                case "Sweeterella" :  characterN = 6;    break;
                case "Thanatos" :     characterN = 7;    break;
                case "Charlotte" :    characterN = 8;    break;
                default :       Debug.LogError("ERROR : Have not assign character to name"); break;
            }
            moveSpeed = 5;
            knockbackPower = 0.25f;
            mpBar.SetActive(true);
            rb = GetComponent<Rigidbody2D>();
            bouncePhysics = true;
            healthBar.SetActive(true);
            hp.sprite = hearts[health];
        }


        if (whoPaused != null) whoPaused.text = characterName + " Paused";
        rInput.RewiredInputManager = GameObject.Find("Rewired_Input_Manager").GetComponent<InputManager>();
        if (rInput.RewiredPlayerIds != null) rInput.RewiredPlayerIds[0] = playerID;
        if (!controller.minigameMode) { 
            foreach (GameObject button in freePlayButtons)
            {
                button.SetActive(false);
            }
        }
    }

    void DISPLAY_CHARACTER_HEAD()
    {
        foreach(GameObject head in heads)
        {
            if (head.name.Contains(characterName))
            {
                var obj = Instantiate(head, mask.transform.position, Quaternion.identity);
                obj.transform.parent = mask.transform;
                obj.GetComponent<HeadCollector>().player = this.GetComponent<MinigameControls>();
            }
        }
    }

    // SPAWN OBJECTS (based on map)
    private void GAME_SETUP()
    {
        if (sceneName == "Sneak_And_Snore" || sceneName == "Sneak And Snore")
        {
            // var o1 = Instantiate(goldPrefab, transform.position + new Vector3(-1,0,0), Quaternion.identity);
            // o1.transform.parent = instances.transform;
            var o2 = Instantiate(goldPrefab, transform.position + new Vector3(-2,0,0), Quaternion.identity);
            if (pw != null) o2.transform.parent = pw.transform;
            var o3 = Instantiate(goldPrefab, transform.position + new Vector3(-4,0,0), Quaternion.identity);
            if (pw != null) o3.transform.parent = pw.transform;

            var o4 = Instantiate(goldPrefab, transform.position + new Vector3(-6.1f,-0.1f,0), Quaternion.identity);
            if (pw != null) o4.transform.parent = pw.transform;
            var o5 = Instantiate(goldPrefab, transform.position + new Vector3(-5.9f,-0.1f,0), Quaternion.identity);
            if (pw != null) o5.transform.parent = pw.transform;
            var o6 = Instantiate(goldPrefab, transform.position + new Vector3(-6,0,0), Quaternion.identity);
            if (pw != null) o6.transform.parent = pw.transform;

            var o7 = Instantiate(goldPrefab, transform.position + new Vector3(-9.1f,-0.1f,0), Quaternion.identity);
            if (pw != null) o7.transform.parent = pw.transform;
            var o8 = Instantiate(goldPrefab, transform.position + new Vector3(-9.1f,0.1f,0), Quaternion.identity);
            if (pw != null) o8.transform.parent = pw.transform;
            var o9 = Instantiate(goldPrefab, transform.position + new Vector3(-8.9f,-0.1f,0), Quaternion.identity);
            if (pw != null) o9.transform.parent = pw.transform;
            var o10 = Instantiate(goldPrefab, transform.position + new Vector3(-8.9f,0.1f,0), Quaternion.identity);
            if (pw != null) o10.transform.parent = pw.transform;
            var o11 = Instantiate(goldPrefab, transform.position + new Vector3(-9,0,0), Quaternion.identity);
            if (pw != null) o11.transform.parent = pw.transform;

            var o12 = Instantiate(goldPrefab, transform.position + new Vector3(-11.1f,-0.1f,0), Quaternion.identity);
            if (pw != null) o12.transform.parent = pw.transform;
            var o13 = Instantiate(goldPrefab, transform.position + new Vector3(-11.1f,0.1f,0), Quaternion.identity);
            if (pw != null) o13.transform.parent = pw.transform;
            var o14 = Instantiate(goldPrefab, transform.position + new Vector3(-10.9f,-0.1f,0), Quaternion.identity);
            if (pw != null) o14.transform.parent = pw.transform;
            var o15 = Instantiate(goldPrefab, transform.position + new Vector3(-10.9f,0.1f,0), Quaternion.identity);
            if (pw != null) o15.transform.parent = pw.transform;
            var o16 = Instantiate(goldPrefab, transform.position + new Vector3(-11,0,0), Quaternion.identity);
            if (pw != null) o16.transform.parent = pw.transform;

            score.color = new Color (1,0,0);
            scoreHead.color = new Color (1,0,0);
        }
        else if (sceneName == "Food_Festival" || sceneName == "Feast-ival")
        {
            var obj = Instantiate(dumplingsPrefab, new Vector3(transform.position.x, transform.position.y + 0.5f), Quaternion.identity);
            obj.transform.parent = instances.transform;
            foodPlate = obj.GetComponent<Dumplings>();
            GetComponent<SortingGroup>().enabled = false;
        }
        else if (sceneName == "Lilypad_Leapers" || sceneName == "Lilypad Leapers")
        {
            for (int i=0 ; i<7 ; i++)
            {
                if (i == 0)
                {
                    var start = Instantiate(startBG,
                        new Vector3(transform.position.x, transform.position.y + (10*i)), Quaternion.identity);
                    if (manager != null) start.transform.parent = manager.instances.transform;
                    if (pw != null)      start.transform.parent = pw.transform;
                }
                else if (i == 5)
                {
                    var fin = Instantiate(finishBG,
                        new Vector3(transform.position.x, transform.position.y + (10*i)), Quaternion.identity);
                    if (manager != null) fin.transform.parent = manager.instances.transform;
                    if (pw != null)      fin.transform.parent = pw.transform;
                }
                else if (i == 6)
                {
                    var fin = Instantiate(sandBG,
                        new Vector3(transform.position.x, transform.position.y + (10*i)), Quaternion.identity);
                    if (manager != null) fin.transform.parent = manager.instances.transform;
                    if (pw != null)      fin.transform.parent = pw.transform;
                }
                else{
                    var obj = Instantiate(waterBG,
                        new Vector3(transform.position.x, transform.position.y + (10*i)), Quaternion.identity);
                    if (manager != null) obj.transform.parent = manager.instances.transform;
                    if (pw != null)      obj.transform.parent = pw.transform;
                }
            }

            if (lp == null) Debug.LogError("WHYYYYYYYYYY!!!!!!!");
            for (int i=0 ; i<lp.pads.Length ; i++)
            {
                if (i % 2 == 0) // EVEN (left)
                {
                    var obj = Instantiate(lilypadPrefab,
                        new Vector3(transform.position.x - 1, transform.position.y + (2*i) + 2), Quaternion.identity);
                    if (manager != null) obj.transform.parent = manager.instances.transform;
                    if (pw != null)      obj.transform.parent = pw.transform;
                    obj.GetComponent<Pad>().button.text = lp.pads[i];
                    lilypads[i] = obj.transform;
                }
                else    // ODD  (right)
                {
                    var obj2 = Instantiate(lilypadPrefab,
                        new Vector3(transform.position.x + 1, transform.position.y + (2*i) + 2), Quaternion.identity);
                    if (manager != null) obj2.transform.parent = manager.instances.transform;
                    if (pw != null)      obj2.transform.parent = pw.transform;
                    obj2.GetComponent<Pad>().button.text = lp.pads[i];
                    lilypads[i] = obj2.transform;
                }
            }

        }
        else if (sceneName == "Stop_Watchers" || sceneName == "Stop Watchers")
        {
            var obj = Instantiate(stagePrefab, new Vector3(transform.position.x, transform.position.y+0.1f), Quaternion.identity);
            obj.transform.parent = instances.transform;
            stageTimer = obj.gameObject.GetComponent<StageTimer>();
        }
    }

    private void SPLIT_SCREEN_SETUP()
    {
        splitCam.gameObject.SetActive(true);
        splitCanvas.SetActive(true);
        splitCanvas.transform.SetParent(null, false);
        splitBorder.transform.localScale = new Vector3(1,1,1);
        // splitCanvas.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
        // splitCanvas.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
        // splitCanvas.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        
        if (controller.nPlayers == 2)
        {
            switch (name)
            {
                // **                             new Rect(xPos, yPos, width, height)
                case "Player_1" : splitCam.rect = new Rect(  0,  0,  0.5f,  1); break;
                case "Player_2" : splitCam.rect = new Rect(1/2f, 0,  0.5f,  1); break;
            }
        }
        else if (controller.nPlayers == 3)
        {
            switch (name)
            {
                case "Player_1" : splitCam.rect = new Rect(   0,  0,  1/3f,  1); break;
                case "Player_2" : splitCam.rect = new Rect(1/3f,  0,  1/3f,  1); break;
                case "Player_3" : splitCam.rect = new Rect(2/3f,  0,  1/3f,  1); break;
            }
        }
        else if (controller.nPlayers == 4)
        {
            switch (name)
            {
                case "Player_1" : splitCam.rect = new Rect(   0,  0,  1/4f,  1); break;
                case "Player_2" : splitCam.rect = new Rect(1/4f,  0,  1/4f,  1); break;
                case "Player_3" : splitCam.rect = new Rect(2/4f,  0,  1/4f,  1); break;
                case "Player_4" : splitCam.rect = new Rect(3/4f,  0,  1/4f,  1); break;
            }
        }
        else if (controller.nPlayers == 5)
        {
            switch (name)
            {
                case "Player_1" : splitCam.rect = new Rect(   0,  0.5f,  1/3f,  0.5f); break;
                case "Player_2" : splitCam.rect = new Rect(1/3f,  0.5f,  1/3f,  0.5f); break;
                case "Player_3" : splitCam.rect = new Rect(2/3f,  0.5f,  1/3f,  0.5f); break;
                case "Player_4" : splitCam.rect = new Rect(      0,  0,  1/3f,  0.5f); break;
                case "Player_5" : splitCam.rect = new Rect(   1/3f,  0,  1/3f,  0.5f); break;
            }
        }
        else if (controller.nPlayers == 6)
        {
            switch (name)
            {
                case "Player_1" : splitCam.rect = new Rect(   0,  0.5f,  1/3f,  0.5f); break;
                case "Player_2" : splitCam.rect = new Rect(1/3f,  0.5f,  1/3f,  0.5f); break;
                case "Player_3" : splitCam.rect = new Rect(2/3f,  0.5f,  1/3f,  0.5f); break;
                case "Player_4" : splitCam.rect = new Rect(   0,  0,  1/3f,  0.5f); break;
                case "Player_5" : splitCam.rect = new Rect(1/3f,  0,  1/3f,  0.5f); break;
                case "Player_6" : splitCam.rect = new Rect(2/3f,  0,  1/3f,  0.5f); break;
            }
        }
        else if (controller.nPlayers == 7)
        {
            switch (name)
            {
                case "Player_1" : splitCam.rect = new Rect(   0,  1/2f,  1/4f,  0.5f); break;
                case "Player_2" : splitCam.rect = new Rect(1/4f,  1/2f,  1/4f,  0.5f); break;
                case "Player_3" : splitCam.rect = new Rect(2/4f,  1/2f,  1/4f,  0.5f); break;
                case "Player_4" : splitCam.rect = new Rect(3/4f,  1/2f,  1/4f,  0.5f); break;
                case "Player_5" : splitCam.rect = new Rect(   0,  0,  1/4f,  0.5f); break;
                case "Player_6" : splitCam.rect = new Rect(1/4f,  0,  1/4f,  0.5f); break;
                case "Player_7" : splitCam.rect = new Rect(2/4f,  0,  1/4f,  0.5f); break;
            }
        }
        else if (controller.nPlayers == 8)
        {
            switch (name)
            {
                case "Player_1" : splitCam.rect = new Rect(   0,  1/2f,  1/4f,  0.5f); break;
                case "Player_2" : splitCam.rect = new Rect(1/4f,  1/2f,  1/4f,  0.5f); break;
                case "Player_3" : splitCam.rect = new Rect(2/4f,  1/2f,  1/4f,  0.5f); break;
                case "Player_4" : splitCam.rect = new Rect(3/4f,  1/2f,  1/4f,  0.5f); break;
                case "Player_5" : splitCam.rect = new Rect(   0,  0,  1/4f,  0.5f); break;
                case "Player_6" : splitCam.rect = new Rect(1/4f,  0,  1/4f,  0.5f); break;
                case "Player_7" : splitCam.rect = new Rect(2/4f,  0,  1/4f,  0.5f); break;
                case "Player_8" : splitCam.rect = new Rect(3/4f,  0,  1/4f,  0.5f); break;
            }
        }
    }

    private void SPLIT_SCREEN_PREVIEW()
    {
        splitCam.gameObject.SetActive(true);
        splitCanvas.SetActive(true);
        splitCanvas.transform.SetParent(pw.transform, false);
        splitBorder.transform.localScale = new Vector3(1,1,1);
        // splitCanvas.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
        // splitCanvas.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
        // splitCanvas.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        
        if (controller.nPlayers == 2)
        {
            switch (name)
            {
                case "Player_1" : splitCam.rect = new Rect( 0.040f, 0.3f, 0.305f, 0.6f); break;
                case "Player_2" : splitCam.rect = new Rect( 0.345f, 0.3f, 0.305f, 0.6f); break;
            }
        }
        else if (controller.nPlayers == 3)
        {
            switch (name)
            {
                case "Player_1" : splitCam.rect = new Rect( 0.0400f, 0.3f, 0.2033f, 0.6f); break;
                case "Player_2" : splitCam.rect = new Rect( 0.2433f, 0.3f, 0.2033f, 0.6f); break;
                case "Player_3" : splitCam.rect = new Rect( 0.4466f, 0.3f, 0.2033f, 0.6f); break;
            }
        }
        else if (controller.nPlayers == 4)
        {
            switch (name)
            {
                case "Player_1" : splitCam.rect = new Rect( 0.0400f, 0.3f, 0.1525f, 0.6f); break;
                case "Player_2" : splitCam.rect = new Rect( 0.1925f, 0.3f, 0.1525f, 0.6f); break;
                case "Player_3" : splitCam.rect = new Rect( 0.3450f, 0.3f, 0.1525f, 0.6f); break;
                case "Player_4" : splitCam.rect = new Rect( 0.4975f, 0.3f, 0.1525f, 0.6f); break;
            }
        }
        else if (controller.nPlayers == 5)
        {
            switch (name)
            {
                case "Player_1" : splitCam.rect = new Rect( 0.0400f, 0.6f, 0.2033f, 0.3f); break;
                case "Player_2" : splitCam.rect = new Rect( 0.2433f, 0.6f, 0.2033f, 0.3f); break;
                case "Player_3" : splitCam.rect = new Rect( 0.4466f, 0.6f, 0.2033f, 0.3f); break;
                case "Player_4" : splitCam.rect = new Rect( 0.0400f, 0.3f, 0.2033f, 0.3f); break;
                case "Player_5" : splitCam.rect = new Rect( 0.2433f, 0.3f, 0.2033f, 0.3f); break;
            }
        }
        else if (controller.nPlayers == 6)
        {
            switch (name)
            {
                case "Player_1" : splitCam.rect = new Rect( 0.0400f, 0.6f, 0.2033f, 0.3f); break;
                case "Player_2" : splitCam.rect = new Rect( 0.2433f, 0.6f, 0.2033f, 0.3f); break;
                case "Player_3" : splitCam.rect = new Rect( 0.4466f, 0.6f, 0.2033f, 0.3f); break;
                case "Player_4" : splitCam.rect = new Rect( 0.0400f, 0.3f, 0.2033f, 0.3f); break;
                case "Player_5" : splitCam.rect = new Rect( 0.2433f, 0.3f, 0.2033f, 0.3f); break;
                case "Player_6" : splitCam.rect = new Rect( 0.4466f, 0.3f, 0.2033f, 0.3f); break;
            }
        }
        else if (controller.nPlayers == 7)
        {
            switch (name)
            {
                case "Player_1" : splitCam.rect = new Rect( 0.0400f, 0.6f, 0.1525f, 0.3f); break;
                case "Player_2" : splitCam.rect = new Rect( 0.1925f, 0.6f, 0.1525f, 0.3f); break;
                case "Player_3" : splitCam.rect = new Rect( 0.3450f, 0.6f, 0.1525f, 0.3f); break;
                case "Player_4" : splitCam.rect = new Rect( 0.4975f, 0.6f, 0.1525f, 0.3f); break;
                case "Player_5" : splitCam.rect = new Rect( 0.0400f, 0.3f, 0.1525f, 0.3f); break;
                case "Player_6" : splitCam.rect = new Rect( 0.1925f, 0.3f, 0.1525f, 0.3f); break;
                case "Player_7" : splitCam.rect = new Rect( 0.3450f, 0.3f, 0.1525f, 0.3f); break;
            }
        }
        else if (controller.nPlayers == 8)
        {
            switch (name)
            {
                case "Player_1" : splitCam.rect = new Rect( 0.0400f, 0.6f, 0.1525f, 0.3f); break;
                case "Player_2" : splitCam.rect = new Rect( 0.1925f, 0.6f, 0.1525f, 0.3f); break;
                case "Player_3" : splitCam.rect = new Rect( 0.3450f, 0.6f, 0.1525f, 0.3f); break;
                case "Player_4" : splitCam.rect = new Rect( 0.4975f, 0.6f, 0.1525f, 0.3f); break;
                case "Player_5" : splitCam.rect = new Rect( 0.0400f, 0.3f, 0.1525f, 0.3f); break;
                case "Player_6" : splitCam.rect = new Rect( 0.1925f, 0.3f, 0.1525f, 0.3f); break;
                case "Player_7" : splitCam.rect = new Rect( 0.3450f, 0.3f, 0.1525f, 0.3f); break;
                case "Player_8" : splitCam.rect = new Rect( 0.4975f, 0.3f, 0.1525f, 0.3f); break;
            }
        }
    }

    private void RESET_PLAYER_UI()
    {
        if (manager != null)
        {
            scoreHead.gameObject.SetActive(false);
            RectTransform rt = layout.transform.GetComponent<RectTransform>();
            layout.SetActive(true);
            float axis = 0;
            float percent = 0;
            if (controller.nPlayers <= 4) {
                float id =  (float) playerID+1;
                float max = (float) controller.nPlayers + 1;
                percent = id/max;
                axis = percent * 800;
            }
            else {
                float id =  (float) playerID+1;
                float max = (float) controller.nPlayers + 1;
                percent = id/max;
                axis = percent * 800;
            }

            rt.anchoredPosition = new Vector3(axis, 30, 0);
        }
        // PREVIEW LEVELS
        else
        {
            layout.SetActive(false);
            scoreHead.gameObject.SetActive(true);
        }
    }


    // ******************************************************************************************************************** //
    void Update()
    {
        if (manager != null)
        {
            if (player.GetButtonDown("Start") && Time.timeScale != 0 && manager.canPlay)
            {
                Time.timeScale = 0;
                pauseUi.SetActive(true);
            }
        }

        if (Time.timeScale == 0)
        {
            
        }

        else if (sceneName == "Sneak_And_Snore" && manager.canPlay && !manager.timeUp) 
        {
            if (outsideOfEffect)
            {
                transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            }
            else if (!crystalised.activeSelf) 
            {
                if (!player.GetButton("A")) { MOVEMENT_H(); }
                CAMOFLAGE();
            }
        }
        else if (sceneName == "Sneak And Snore" && pw.canPlay) 
        {
            if (outsideOfEffect)
            {
                transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            }
            else if (!crystalised.activeSelf) 
            {
                if (!player.GetButton("A")) { MOVEMENT_H(); }
                CAMOFLAGE();
            }
        }

        else if (sceneName == "Food_Festival" && manager.canPlay  && !manager.timeUp)
        {
            EATING();
        }
        else if (sceneName == "Feast-ival" && pw.canPlay) 
        {
            EATING();
        }
    
        else if (sceneName == "Colour_Chaos" && manager.canPlay && !manager.timeUp) 
        {
            MOVEMENT();
        }
        else if (sceneName == "Colour Chaos" && pw.canPlay) 
        {
            MOVEMENT();
        }
    
        else if (sceneName == "Card_Collectors" && manager.canPlay && !manager.timeUp) 
        {
            TRANSLATION();
            BOUNDARIES();
            if (player.GetButtonDown("A") && !clicked) StartCoroutine( CLICK() );
            UPDATE_SCORE_UI();
        }
        else if (sceneName == "Card Collectors" && pw.canPlay) 
        {
            TRANSLATION();
            BOUNDARIES();
            if (player.GetButtonDown("A") && !clicked) StartCoroutine( CLICK() );
            UPDATE_SCORE_UI();
        }
    
        else if (sceneName == "Leaf_Leap" && manager.canPlay && !manager.timeUp) 
        {
            LEAP();
        }
        else if (sceneName == "Leaf Leap" && pw.canPlay) 
        {
            LEAP();
        }
        

        else if (sceneName == "Lava_Or_Leave_'Em" && manager.canPlay && !manager.timeUp && !isOut) 
        {
            MOVEMENT();
        }
        else if (sceneName == "Lava Or Leave 'Em" && pw.canPlay && !isOut) 
        {
            MOVEMENT();
        }

        else if (sceneName == "Lilypad_Leapers" && manager.canPlay && !manager.timeUp && !isOut) 
        {
            LILYPAD();
        }
        else if (sceneName == "Lilypad Leapers" && pw.canPlay && !isOut) 
        {
            LILYPAD();
        }

        else if (sceneName == "Stop_Watchers" && manager.canPlay && !manager.timeUp && !isOut) 
        {
            StopWatch();
        }
        else if (sceneName == "Stop Watchers" && pw.canPlay && !isOut) 
        {
            StopWatch();
        }
        
        else if (sceneName == "Spotlight-Fight" && manager.canPlay && !manager.timeUp && !isOut) 
        {
            if (!dashDirection.gameObject.activeSelf && !dashing) { MOVEMENT(); }
            if (!dashing && !beingKnocked) CHARGING_DIRECTION(); 
        }
        else if (sceneName == "Spotlight Fight" && pw.canPlay && !isOut) 
        {
            if (!dashDirection.gameObject.activeSelf && !dashing) { MOVEMENT(); }
            if (!dashing && !beingKnocked) CHARGING_DIRECTION(); 
        }
        
        else if (sceneName == "Pushy-Penguins" && manager.canPlay && !manager.timeUp && !isOut) 
        {
            MOVEMENT();
        }
        else if (sceneName == "Pushy Penguins" && pw.canPlay && !isOut) 
        {
            MOVEMENT();
        }

        else if (sceneName == "Fun_Run" && manager.canPlay && !manager.timeUp && !isOut) 
        {
            if (!dashDirection.gameObject.activeSelf && !dashing) { MOVEMENT(); }
            if (!dashing && !beingKnocked) CHARGING_DIRECTION(); 
        }
        else if (sceneName == "Fun Run" && pw.canPlay && !isOut) 
        {
            if (!dashDirection.gameObject.activeSelf && !dashing) { MOVEMENT(); }
            if (!dashing && !beingKnocked) CHARGING_DIRECTION(); 
        }

        else if (sceneName == "Money_Belt" && manager.canPlay && !manager.timeUp && !isOut) 
        {
            OPEN_BOX();
        }
        else if (sceneName == "Coin-veyor" && pw.canPlay && !isOut) 
        {
            OPEN_BOX();
        }

        else if (sceneName == "Stamp-By-Me" && manager.canPlay && !manager.timeUp && !isOut) 
        {
            STAMP();
        }
        else if (sceneName == "Stamp By Me" && pw.canPlay && !isOut) 
        {
            STAMP();
        }

        else if (sceneName == "Shocking-Situation" && manager.canPlay && !manager.timeUp && !isOut )
        {
            MOVEMENT();
        }
        else if (sceneName == "Shocking Situation" && pw.canPlay && !isOut) 
        {
            MOVEMENT();
        }

        else if (sceneName == "Attack-On-Titan" && manager.canPlay && !manager.timeUp && !isOut )
        {
            if (player.GetButtonDown("A")) CHECK_SPELL();
            if (!isTitan) MOVEMENT();
            else CALC_ACCELERATE();
        }
        else if (sceneName == "Attack Of Titan" && pw.canPlay && !isOut) 
        {
            if (player.GetButtonDown("A")) CHECK_SPELL();
            if (!isTitan) MOVEMENT();
            else CALC_ACCELERATE();
        }
        
        else if (sceneName == "Flower-Shower" && manager.canPlay && !manager.timeUp && !isOut )
        {
            MOVEMENT();
        }
        else if (sceneName == "Flower Shower" && pw.canPlay && !isOut) 
        {
            MOVEMENT();
        }
        
        else if (sceneName == "Don't-Be-A-Zombie" && manager.canPlay && !manager.timeUp && !isOut )
        {
            MOVEMENT();
        }
        else if (sceneName == "Don't Be A Zombie" && pw.canPlay && !isOut) 
        {
            MOVEMENT();
        }
        
        else if (sceneName == "Barrier_Bearers" && manager.canPlay && !manager.timeUp && !isOut )
        {
            MOVEMENT();
            if (player.GetButton("A") && !barrierOn)     { barrierOn = true;  barrierEffect.SetActive(true); moveSpeed = 2; }
            if (player.GetButtonUp("A"))   { barrierOn = false; barrierEffect.SetActive(false); moveSpeed = 5; }
        }
        else if (sceneName == "Barrier Bearers" && pw.canPlay && !isOut) 
        {
            MOVEMENT();
            if (player.GetButton("A") && !barrierOn)     { barrierOn = true;  barrierEffect.SetActive(true); moveSpeed = 2; }
            if (player.GetButtonUp("A"))   { barrierOn = false; barrierEffect.SetActive(false); moveSpeed = 5; }
        }

        else if (sceneName == "Plunder-Ground" && manager.canPlay && !manager.timeUp && !isOut )
        {
            MOVEMENT();
            CHANGE_MASK();
        }
        else if (sceneName == "Plunder Ground" && pw.canPlay && !pw.timeUp && !isOut )
        {
            MOVEMENT();
            CHANGE_MASK();
        }


        // BOSS BATTLE
        else if (manager != null && manager.bossBattle && manager.canPlay && !manager.timeUp && !isOut )
        {
            if (sceneName == "Dojo" && challenge.activeSelf && player.GetButtonDown("A") && playerID == 0) {
                SceneManager.LoadScene(bossScene); 
            }
            MOVEMENT();
            if (!barrierOn) SPAWN_PROJECTILES();

            // BARRIER
            if (!barrierOn && player.GetButtonDown("L") || !barrierOn && player.GetButtonDown("ZL"))     
            { 
                barrierOn = true;  barrierEffect.SetActive(true); moveSpeed = 2; 
            }
            if (!barrierOn && player.GetButtonDown("R") || !barrierOn && player.GetButtonDown("ZR"))     
            { 
                barrierOn = true;  barrierEffect.SetActive(true); moveSpeed = 2; 
            }
            if (player.GetButtonUp("L") || player.GetButtonUp("R") || player.GetButtonUp("ZL") || player.GetButtonUp("ZR"))   
            {
                barrierOn = false; barrierEffect.SetActive(false); moveSpeed = 5; 
            }
        }


        // *** leaf leap extra
        if (sceneName == "Leaf_Leap" || sceneName == "Leaf Leap")
        {
            // LEAPING LEFT
            if (leapLeft && !isGrounded) {
                if (transform.position.x > origXpos) transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            }
            // LEAPING RIGHT
            if (leapRight && !isGrounded) {
                if (transform.position.x < origXpos + 3) transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            }
            splitCam.transform.position = new Vector3(splitCam.transform.position.x, this.transform.position.y + 3, -10);
        }

        Reload();   // DELETE
    }
    
    private void FixedUpdate() 
    {
        if (sceneName == "Colour_Chaos" && manager.canPlay && !manager.timeUp) 
        {
            MOVE();
        }
        else if (sceneName == "Colour Chaos" && pw.canPlay) 
        {
            MOVE();
        }

        else if (sceneName == "Lava_Or_Leave_'Em" && manager.canPlay && !manager.timeUp && !isOut) 
        {
            MOVE();
        }
        else if (sceneName == "Lava Or Leave 'Em" && pw.canPlay && !isOut) 
        {
            MOVE();
        }

        else if (sceneName == "Spotlight-Fight" && manager.canPlay && !manager.timeUp && !isOut) 
        {
            if (!dashDirection.gameObject.activeSelf && !dashing) { MOVE(); }
        }
        else if (sceneName == "Spotlight Fight" && pw.canPlay && !isOut) 
        {
            if (!dashDirection.gameObject.activeSelf && !dashing) { MOVE(); }
        }

        else if (sceneName == "Pushy-Penguins" && manager.canPlay && !manager.timeUp && !isOut) 
        {
            MOVE();
        }
        else if (sceneName == "Pushy Penguins" && pw.canPlay && !isOut) 
        {
            MOVE();
        }

        else if (sceneName == "Fun_Run" && manager.canPlay && !manager.timeUp && !isOut) 
        {
            if (!dashDirection.gameObject.activeSelf && !dashing) { MOVE(); }
        }
        else if (sceneName == "Fun Run" && pw.canPlay && !isOut) 
        {
            if (!dashDirection.gameObject.activeSelf && !dashing) { MOVE(); }
        }

        else if (sceneName == "Shocking-Situation" && manager.canPlay && !manager.timeUp && !isOut )
        {
            MOVE();
        }
        else if (sceneName == "Shocking Situation" && pw.canPlay && !isOut) 
        {
            MOVE();
        }

        else if (sceneName == "Attack-On-Titan" && manager.canPlay && !manager.timeUp && !isOut )
        {
            if (!isTitan) MOVE();
            else ACCELERATE();
        }
        else if (sceneName == "Attack Of Titan" && pw.canPlay && !isOut) 
        {
            if (!isTitan) MOVE();
            else ACCELERATE();
        }

        else if (sceneName == "Flower-Shower" && manager.canPlay && !manager.timeUp && !isOut )
        {
            MOVE();
        }
        else if (sceneName == "Flower Shower" && pw.canPlay && !isOut) 
        {
            MOVE();
        }
        
        else if (sceneName == "Don't-Be-A-Zombie" && manager.canPlay && !manager.timeUp && !isOut )
        {
            MOVE();
        }
        else if (sceneName == "Don't Be A Zombie" && pw.canPlay && !isOut) 
        {
            MOVE();
        }
        
        if (sceneName == "Barrier_Bearers" && manager.canPlay && !manager.timeUp && !isOut )
        {
            MOVE();
            if (barrierOn) BARRIER();
        }
        else if (sceneName == "Barrier Bearers" && pw.canPlay && !isOut) 
        {
            MOVE();
            if (barrierOn) BARRIER();
        }
        
        else if (sceneName == "Plunder-Ground" && manager.canPlay && !manager.timeUp && !isOut )
        {
            MOVE();
            mask.transform.position = maskPos.position;
        }
        else if (sceneName == "Plunder Ground" && pw.canPlay && !pw.timeUp && !isOut )
        {
            MOVE();
            mask.transform.position = maskPos.position;
        }
        
        // BOSS BATTLE
        else if (manager != null)
        {
            if (manager.bossBattle)
            {
                MOVE();
                if (barrierOn) BARRIER();
                if (!shooting && !barrierOn) RESTORE_MANA();
            }
        }
    }

    // ******************************************************************************************************************** //
    // ************* PAUSED *************
    void Reload()
    {
        if (player.GetButtonDown("Minus") && playerID == 0 && pw == null) { SceneManager.LoadScene( sceneName ); }
    }

    public void RESUME()
    {
        Time.timeScale = 1;
        pauseUi.SetActive(false);
    }

    public void BACK_TO_SIDE_QUESTS()
    {
        Time.timeScale = 1;
        pauseUi.SetActive(false);
        SceneManager.LoadScene("3Quests");
    }

    public void RESTART()
    {
        if (pw != null) return;
        Time.timeScale = 1;
        pauseUi.SetActive(false);
        SceneManager.LoadScene( sceneName );
    }

    // ********************************************************** //
    // ******************** DEFAULT CONTROLS ******************** //

    public void UPDATE_POINTS()
    {
        if (manager != null) score.text = points.ToString();
        else                 scoreHead.text = points.ToString();
    }
    public void GOLD_PICKUP()
    {
        ts.A_SPAWN_FOUND();
        if (manager != null) coinPickup.Play();
    }

    void MOVE()
    {
        // FLIP CHARACTER WHEN MOVING RIGHT
        if (moveHorizontal > 0 && !cursorMode && !shooting)
        {
            character.transform.localScale = new Vector3(-scaleX, scaleX, scaleX);
            if (mask != null) { mask.transform.rotation = Quaternion.Euler(0,180,0); }
        }
        else if (moveHorizontal < 0 && !cursorMode && !shooting)
        {
            character.transform.localScale = new Vector3(scaleX, scaleX, scaleX);
            if (mask != null) { mask.transform.rotation = Quaternion.Euler(0,0,0);}
        }
        // Vector3 direction = new Vector3(moveHorizontal, moveVertical, 0);
        rb.MovePosition(transform.position + moveDir * moveSpeed * Time.fixedDeltaTime);
        // rb.velocity = direction * moveSpeed;

        // ANIMATION
        if (!cursorMode)
        {
            if (moveHorizontal != 0 || moveVertical != 0)
            {
                if (!hasMoved) hasMoved = true;
                _anim.SetBool("IsWalking", true);
                if (Mathf.Abs(moveHorizontal) > Mathf.Abs(moveVertical)) 
                { 
                    _anim.speed = moveSpeed * Mathf.Abs(moveHorizontal); 
                    // if (_anim.speed > 5)
                    movePower = moveSpeed * Mathf.Abs(moveHorizontal);
                }
                else 
                { 
                    _anim.speed = moveSpeed * Mathf.Abs(moveVertical); 
                    movePower = moveSpeed * Mathf.Abs(moveVertical);
                }
            }
            else { 
                _anim.SetBool("IsWalking", false); 
                _anim.speed = 1;
                movePower = 0;
            }
        }
    }

    void MOVEMENT()
    {
        moveHorizontal = player.GetAxis("Move Horizontal");
        moveVertical = player.GetAxis("Move Vertical");

        // FLIP CHARACTER WHEN MOVING RIGHT
        if (moveHorizontal > 0 && !cursorMode && !shooting)
        {
            character.transform.localScale = new Vector3(-scaleX, scaleX, scaleX);
        }
        else if (moveHorizontal < 0 && !cursorMode && !shooting)
        {
            character.transform.localScale = new Vector3(scaleX, scaleX, scaleX);
        }
        moveDir = new Vector3(moveHorizontal, moveVertical, 0);
        // rb.MovePosition(transform.position + direction * moveSpeed * Time.fixedDeltaTime);
        // rb.velocity = direction * moveSpeed;

        // // ANIMATION
        // if (!cursorMode)
        // {
        //     if (moveHorizontal != 0 || moveVertical != 0)
        //     {
        //         if (!hasMoved) hasMoved = true;
        //         _anim.SetBool("IsWalking", true);
        //         _anim.speed =  moveSpeed * (Mathf.Abs(moveHorizontal) + Mathf.Abs(moveVertical)) ;
        //         movePower = moveSpeed * (Mathf.Abs(moveHorizontal) + Mathf.Abs(moveVertical));
        //     }
        //     else { 
        //         _anim.SetBool("IsWalking", false); 
        //         _anim.speed = 1;
        //         movePower = 0;
        //     }
        // }
    }
    
    void TRANSLATION()
    {
        float moveHorizontal = player.GetAxis("Move Horizontal");
        float moveVertical = player.GetAxis("Move Vertical");


        // FLIP CHARACTER WHEN MOVING RIGHT
        if (moveHorizontal > 0 && !cursorMode)
        {
            character.transform.localScale = new Vector3(-scaleX, scaleX, scaleX);
        }
        else if (moveHorizontal < 0 && !cursorMode)
        {
            character.transform.localScale = new Vector3(scaleX, scaleX, scaleX);
        }
        Vector3 direction = new Vector3(moveHorizontal, moveVertical);
        //// rb.MovePosition(transform.position + direction * moveSpeed * Time.fixedDeltaTime);
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        // ANIMATION
        if (!cursorMode)
        {
            if (moveHorizontal != 0 || moveVertical != 0)
            {
                _anim.SetBool("IsWalking", true);
                _anim.speed =  moveSpeed * (Mathf.Abs(moveHorizontal) + Mathf.Abs(moveVertical)) ;
            }
            else { 
                _anim.SetBool("IsWalking", false); 
                _anim.speed = 1;
            }
        }
    }

    void BOUNDARIES()
    {
        if (transform.position.x < -8.8f) transform.position = new Vector3(-8.8f, transform.position.y);
        if (transform.position.x > 8.8f)  transform.position = new Vector3(8.8f, transform.position.y);
        if (transform.position.y > 5)  transform.position = new Vector3(transform.position.x, 5);
        if (transform.position.y < -5) transform.position = new Vector3(transform.position.x, -5);
    }
   
    void UPDATE_SCORE_UI()
    {
        if (manager != null) score.text = points.ToString();
        else                 scoreHead.text = points.ToString();
    }

    // ----------------------------------------------------------- //
    // --------------------- SNEAK AND SNORE --------------------- //
    
    void MOVEMENT_H()
    {
        float moveHorizontal = player.GetAxis("Move Horizontal");

        // FLIP CHARACTER WHEN MOVING RIGHT
        if (moveHorizontal > 0)
        {
            character.transform.localScale = new Vector3(-scaleX, scaleX, scaleX);
        }
        else if (moveHorizontal < 0)
        {
            character.transform.localScale = new Vector3(scaleX, scaleX, scaleX);
        }
        Vector3 direction = new Vector3(moveHorizontal, 0, 0);
        rb.MovePosition(transform.position + direction * moveSpeed * Time.fixedDeltaTime);
        // rb.velocity = direction * moveSpeed;

        if (moveHorizontal != 0)
        {
            _anim.SetBool("IsWalking", true);
            _anim.speed =  moveSpeed * (Mathf.Abs(moveHorizontal)) ;
        }
        else { 
            _anim.SetBool("IsWalking", false); 
            _anim.speed = 1;
        }
    }

    void CAMOFLAGE()
    {
        if (player.GetButtonDown("A"))
        {
            _anim.speed = 2;
            _anim.SetBool("isBuff", true);
        }
        if (player.GetButtonUp("A"))
        {
            _anim.SetBool("isBuff", false);
            _anim.SetTrigger("isDone");
            _collider.enabled = true;
        }
        if (_anim.GetCurrentAnimatorStateInfo(0).IsTag("invisible")) 
        { 
            _collider.enabled = false; 
        }
        else { _collider.enabled = true; }
    }

    // ----------------------------------------------------------- //
    // ---------------------- FOOD FESTIVAL ---------------------- //

    void EATING()
    {
        if (player.GetButtonDown("A"))
        {
            points++;
            if (manager != null) score.text = points.ToString();
            else                 scoreHead.text = points.ToString();

            if (points >= prevPoints) { foodPlate.MoreEaten(); prevPoints += 10; }

            if      (characterName == "Sweeterella") _anim.Play("Sweeterella_Eat", -1, 0);
            else if (characterName == "Felix")       _anim.Play("Felix_Eat", -1, 0);
            else if (characterName == "Jacob")       _anim.Play("Jacob_Eat_Anim", -1, 0);
            else if (characterName == "Mimi")        _anim.Play("Mimi_Eat_Anim", -1, 0);
            else if (characterName == "Maurice")     _anim.Play("Maurice_Eat_Anim", -1, 0);
            else if (characterName == "Laurel")      _anim.Play("Laurel_Eat_Anim", -1, 0);
            else if (characterName == "Pinkins")     _anim.Play("Pinkins_Eat_Anim", -1, 0);
            else if (characterName == "Thanatos")    _anim.Play("Thanatos_Eat_Anim", -1, 0);
            else if (characterName == "Charlotte")   _anim.Play("Charlotte_Eat", -1, 0);
        }
    }

    // ------------------------------------------------------------- //
    // ---------------------- CARD COLLECTORS ---------------------- //

    private IEnumerator CLICK()
    {
        float changeInPoints = points;
        cursorCollider.enabled = true;
        clicked = true;
        cursorSprite.color = new Color(1,1,1,0.5f);
        progressBar.SetActive(true);
        progressBar.GetComponent<Animator>().Play("Progress_Bar_Anim", -1, 0);
        

        yield return new WaitForSeconds(0.1f);
        cursorCollider.enabled = false;

        yield return new WaitForEndOfFrame();
        changeInPoints = points - changeInPoints;
        var obj = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
        TextMeshPro pointsScored = obj.gameObject.GetComponent<TextMeshPro>();
        
        if (changeInPoints > 0) {
            pointsScored.color = new Color(0,1,0);
            pointsScored.text = "+" + changeInPoints.ToString();
        }
        else if (changeInPoints == 0) {
            pointsScored.color = new Color(1,0,0); 
            pointsScored.text = "-" + changeInPoints.ToString();
        }
        else {
            pointsScored.color = new Color(1,0,0); 
            pointsScored.text = changeInPoints.ToString();
        }


        yield return new WaitForSeconds(1.9f);
        cursorSprite.color = new Color(1,1,1,1);
        clicked = false;
        progressBar.SetActive(false);
    }


    // ------------------------------------------------------------- //
    // ------------------------- LEAF LEAP ------------------------- //

    void LEAP()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        // splitCam.transform.position = new Vector3(splitCam.transform.position.x, this.transform.position.y + 3, -10);

        if (player.GetButtonDown("Left") && isGrounded)
        {
            rb.velocity = (Vector2.up * jumpSpeed);
            character.transform.localScale = new Vector3(scaleX, scaleX, scaleX);
            leapLeft    = true;
            leapRight   = false;
            isGrounded  = false;
        }
        if (player.GetButtonDown("Right") && isGrounded)
        {
            rb.velocity = (Vector2.up * jumpSpeed);
            character.transform.localScale = new Vector3(-scaleX, scaleX, scaleX);
            leapRight   = true;
            leapLeft    = false;
            isGrounded  = false;
        }
        
        // NEW LEAF
        if (transform.position.y >= topLeaf.transform.position.y && isGrounded && rb.velocity.y <= 0)
        {
            // SAME SIDE
            if (leafs.isRightLeaf[nleaf] == leafs.isRightLeaf[nleaf-1]) {
                var obj = Instantiate(leafPrefab, 
                    new Vector3(transform.position.x, transform.position.y + leafHeight), Quaternion.identity);
                // obj.transform.parent = instances.transform;
                if (leapRight) obj.GetComponent<SpriteRenderer>().flipX = true;
                if (manager != null) obj.transform.parent = manager.instances.transform;
                if (pw != null) obj.transform.parent = pw.transform;
                topLeaf = obj;
                nleaf++;
            }
            else if (leafs.isRightLeaf[nleaf]) {
                var obj = Instantiate(leafPrefab, 
                    new Vector3(transform.position.x + 3, transform.position.y + leafHeight), Quaternion.identity);
                // obj.transform.parent = instances.transform;
                if (manager != null) obj.transform.parent = manager.instances.transform;
                if (pw != null) obj.transform.parent = pw.transform;
                topLeaf = obj;
                obj.GetComponent<SpriteRenderer>().flipX = true;
                nleaf++;
            }
            else {
                var obj = Instantiate(leafPrefab,
                    new Vector3(transform.position.x - 3, transform.position.y + leafHeight), Quaternion.identity);
                // obj.transform.parent = instances.transform;
                if (manager != null) obj.transform.parent = manager.instances.transform;
                if (pw != null) obj.transform.parent = pw.transform;
                topLeaf = obj;
                nleaf++;
            }
        }
        
        // POINTS BASED ON HEIGHT
        points = (int) Mathf.Abs(transform.position.y - origYpos);
        if (manager != null) score.text = (Mathf.Abs(transform.position.y - origYpos)).ToString("F1") + "m";
        else scoreHead.text = (Mathf.Abs(transform.position.y - origYpos)).ToString("F1") + "m";
    }

    void SPAWN_FIRST_LEAF()
    {
        if (leafs.isRightLeaf[nleaf]) {
            var obj = Instantiate(leafPrefab, 
                new Vector3(transform.position.x + 3, transform.position.y + leafHeight), Quaternion.identity);
            // obj.transform.parent = instances.transform;
            obj.GetComponent<SpriteRenderer>().flipX = true;
            if (manager != null) obj.transform.parent = manager.instances.transform;
            if (pw != null) obj.transform.parent = pw.transform;
            topLeaf = obj;
            nleaf++;
        }
        else {
            var obj = Instantiate(leafPrefab, 
                new Vector3(transform.position.x, transform.position.y + leafHeight), Quaternion.identity);
            // obj.transform.parent = instances.transform;
            if (manager != null) obj.transform.parent = manager.instances.transform;
            if (pw != null) obj.transform.parent = pw.transform;
            topLeaf = obj;
            nleaf++;
        }
    }


    // ------------------------------------------------------------- //
    // ---------------------- LILYPAD LEAPERS ---------------------- //
    
    void LILYPAD()
    {
        splitCam.transform.position = new Vector3(splitCam.transform.position.x, this.transform.position.y + 3, -10);
        if (nPad < lp.pads.Length)
        {
            if (isCorrect && transform.position != lilypads[nPad].position)
            {
                // transform.position = lilypads[nPad].position;
                timer += moveSpeed * Time.deltaTime;
                this.transform.position = Vector3.Lerp(this.transform.position, lilypads[nPad].position, timer);

                if (transform.position == lilypads[nPad].position)
                {
                    timer = 0;
                    isCorrect = false;
                    nPad++;
                    if (nPad % 2 == 0) character.transform.localScale = new Vector3(scaleX,scaleX,scaleX);
                    else               character.transform.localScale = new Vector3(-scaleX,scaleX,scaleX);

                    if (nPad == lp.pads.Length) finalPos = new Vector3(transform.position.x-1,transform.position.y+3);
                }
            }
            else if (isWrong && transform.position != lilypads[nPad-2].position)
            {
                // transform.position = lilypads[nPad].position;
                timer += moveSpeed * Time.deltaTime;
                this.transform.position = Vector3.Lerp(this.transform.position, lilypads[nPad-2].position, timer);
                if (transform.position == lilypads[nPad-2].position)
                {
                    timer = 0;
                    isWrong = false;
                    nPad--;
                    if (nPad % 2 == 0) character.transform.localScale = new Vector3(scaleX,scaleX,scaleX);
                    else               character.transform.localScale = new Vector3(-scaleX,scaleX,scaleX);
                    
                }
            }
            else if (player.GetButtonDown("A") && !isCorrect && !isWrong)
            {
                if (lp.pads[nPad] == "A") isCorrect = true;
                else { if (nPad > 1)        isWrong   = true; BooBoo(); }
            }
            else if (player.GetButtonDown("B") && !isCorrect && !isWrong)
            {
                if (lp.pads[nPad] == "B") isCorrect = true;
                else { if (nPad > 1)        isWrong   = true; BooBoo(); }
            }
            else if (player.GetButtonDown("X") && !isCorrect && !isWrong)
            {
                if (lp.pads[nPad] == "X") isCorrect = true;
                else { if (nPad > 1)        isWrong   = true; BooBoo(); }
            }
            else if (player.GetButtonDown("Y") && !isCorrect && !isWrong)
            {
                if (lp.pads[nPad] == "Y") isCorrect = true;
                else { if (nPad > 1)        isWrong   = true; BooBoo(); }
            }
        }
        // FINISHED
        else if (transform.position != finalPos)
        {
            timer += moveSpeed * Time.deltaTime;
            this.transform.position = Vector3.Lerp(this.transform.position, finalPos, timer);

            if (transform.position == finalPos)
            {
                timer = 0;
                if (manager != null)
                {
                    points = controller.nPlayers - manager.nPlayersOut;
                    manager.nPlayersOut++;
                    DisplayRankPlacement("race");
                    manager.CheckIfEveyoneIsOut(1);
                }
                else if (pw != null)
                {
                    points = controller.nPlayers - pw.nPlayersOut;
                    pw.nPlayersOut++;
                    pw.CheckIfEveyoneIsOut(1);
                }
            }
        }
        

    }

    void BooBoo()
    {
        var sad = Instantiate(mistakeEmoji,
            new Vector3(transform.position.x, transform.position.y + 3), mistakeEmoji.transform.rotation);
        sad.transform.parent = instances.transform;
        Destroy(sad, 1);
    }


    // ------------------------------------------------------------- //
    // ----------------------- STOP WATCHERS ----------------------- //

    void StopWatch()
    {
        if (player.GetButtonDown("A") && !stageTimer.stopped)
        {
            stageTimer.StopTimer();
            if (manager != null)
            {
                manager.nPlayersOut++;
                manager.CheckIfEveyoneIsOut(0);
            }
            if (pw != null)
            {
                // points = Mathf.Abs( stageTimer.timer - pw.timeToStop );
                // stageTimer.ShowStoppedTime();
                // scoreHead.text = points.ToString("F2");
                pw.nPlayersOut++;
                pw.CheckIfEveyoneIsOut(0);
            }
        }
    }

    public void ShowTime()
    {
        // HAVE NOT STOPPED TIMER
        if (!stageTimer.stopped)
        {
            points = 99.99f;
            stageTimer.StopTimer();
        }
        else
        {
            if (manager != null) points = Mathf.Abs( stageTimer.timer - manager.timeToStop );
            if (pw != null) {
                points = Mathf.Abs( stageTimer.timer - pw.timeToStop );
                stageTimer.ShowStoppedTime();
                scoreHead.text = points.ToString("F2");
            }
        }
        score.text = points.ToString("F2");
        scoreHead.text = points.ToString("F2");
        stageTimer.ShowStoppedTime();
    }


    // ------------------------------------------------------------- //
    // ------------------------ SPEEDY SUMO ------------------------ //
    

    void GAIN_POINTS()
    {
        if (!outsideOfEffect)
        {
            points += Time.deltaTime;
            if (manager != null) score.text = points.ToString("F1");
            else                 scoreHead.text = points.ToString("F1");
        }
    }

    void LOSE_POINTS()
    {
        if (outsideOfEffect && !isOut)
        {
            points -= Time.deltaTime;
            if (manager != null) score.text = points.ToString("F1");
            else                 scoreHead.text = points.ToString("F1");
        }
        if (points <= 0 && !isOut)
        {
            isOut = true;
            points = manager.nPlayersOut - controller.nPlayers;
            if (manager != null) 
            {
                StartCoroutine( DelayElimCo() ); 
            }
            else if (pw != null) 
            {
                pw.nPlayersOut++;
                DisplayRankPlacement("elim");
                pw.CheckIfEveyoneIsOut(1);
            }
        }
    }

    void CHARGING_DIRECTION()
    {
        float inputZ = player.GetAxis("Move Horizontal");
        float inputX = player.GetAxis("Move Vertical");

        float angle = Mathf.Atan2(-inputZ, inputX) * Mathf.Rad2Deg; 
        if (angle != 0 && player.GetButton("A") && !beingKnocked) {
            _anim.SetBool("IsWalking", true);
            dashDirection.gameObject.SetActive(true); 
            dashDirection.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        else { dashDirection.gameObject.SetActive(false); }

        if (player.GetButton("A") && !dashing && !beingKnocked)
        {
            chargingParticle.SetActive(true);
            magicCircle.SetActive(true);
            if (chargePower < 25) { chargePower += 0.25f; _anim.speed = chargePower; }

            if (chargePower <= 10)        { dashDirection.sprite = dashPowerSprites[0]; }
            else if (chargePower <= 20)  { dashDirection.sprite = dashPowerSprites[1]; }
            else                        { dashDirection.sprite = dashPowerSprites[2]; }
        }
        if (player.GetButtonUp("A") && !dashing && !beingKnocked)
        {
            // Debug.Log("-- launch power = " + chargePower);
            chargingParticle.SetActive(false);
            magicCircle.SetActive(false);
            Vector3 direction = new Vector3(inputZ, inputX).normalized;
            chargeKb = chargePower;
            StartCoroutine( DashCo(0.5f, direction) );
        }
    }


    // ------------------------------------------------------------- //
    // ------------------------ COIN-VEYOR ------------------------- //

    void OPEN_BOX()
    {
        if (player.GetButtonDown("A") && boxToOpen != null)
        {
            float changeInPoints = coins;

            int opened = boxToOpen.OPEN_BOX();
            boxToOpen = null;
            if (opened < 0) 
            {
                opened = Mathf.Abs(opened);
                if (opened > coins) coins = 0;
                else                coins -= opened;
            }
            else { coins += opened; }

            changeInPoints = coins - changeInPoints;
            var obj = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
            TextMeshPro pointsScored = obj.gameObject.GetComponent<TextMeshPro>();
            
            if (changeInPoints > 0) {
                pointsScored.color = new Color(0,1,0);
                pointsScored.text = "+" + changeInPoints.ToString();
            }
            else if (changeInPoints == 0) {
                pointsScored.color = new Color(1,0,0); 
                pointsScored.text = "-" + changeInPoints.ToString();
            }
            else {
                pointsScored.color = new Color(1,0,0); 
                pointsScored.text = changeInPoints.ToString();
            }

            score.text = coins.ToString();
            scoreHead.text = coins.ToString();
        }
    }


    // ------------------------------------------------------------- //
    // ------------------------ STAMP BY ME ------------------------ //

    void STAMP()
    {
        if (player.GetButtonDown("A") && currentDoc != null && !justStamped)
        {
            int reward;
            stamper.Play("Stamper_Anim", -1, 0);
            float changeInPoints = points;
            currentDoc.STAMPED();
            StartCoroutine( RELOAD_STAMP() );
            if (docs[nDoc]) { reward = 1; }
            else            { reward = -3; }
            if (nDoc >= docs.Length - 1) { nDoc = 0; }
            else                         { nDoc++;   }
            currentDoc = null;
            
            var doc = Instantiate(docPrefab, transform.position, Quaternion.identity);
            doc.transform.parent = instances.transform;
            currentDoc = doc.GetComponent<Document>(); 
            if (docs[nDoc]) { currentDoc.goodDoc.SetActive(true); }
            else            { currentDoc.evilDoc.SetActive(true); }


            if (reward < 0) 
            {
                reward = Mathf.Abs(reward);
                if (reward > points) points = 0;
                else                 points -= reward;
            }
            else { points += reward; }

            // SHOW POINTS GAIN/LOST
            StartCoroutine( POINTS_GAINED(points-changeInPoints, true, 0.1f) );
        }
        else if (player.GetButtonDown("B") && currentDoc != null && !justStamped)
        {
            int reward;
            float changeInPoints = points;
            // bool negZero;
            currentDoc.DISCARD();
            StartCoroutine( RELOAD_STAMP() );
            if (docs[nDoc]) { reward = -1; }
            else            { reward = 1;  }
            if (nDoc >= docs.Length - 1) { nDoc = 0; }
            else                         { nDoc++;   }
            currentDoc = null;
            
            var doc = Instantiate(docPrefab, transform.position, Quaternion.identity);
            doc.transform.parent = instances.transform;
            currentDoc = doc.GetComponent<Document>(); 
            if (docs[nDoc]) { currentDoc.goodDoc.SetActive(true); }
            else            { currentDoc.evilDoc.SetActive(true); }


            if (reward < 0) 
            {
                reward = Mathf.Abs(reward);
                if (reward > points) points = 0;
                else                 points -= reward;
            }
            else { points += reward; }

            // SHOW POINTS GAIN/LOST
            StartCoroutine( POINTS_GAINED(points-changeInPoints, true, 0) );
        }
    }

    // if (negZero) = DO NOT DISPLAY IF GAINED ZERO POINTS (STILL OKAY TO LOSE ZERO POINTS)
    IEnumerator POINTS_GAINED(float score, bool negZero, float delay)
    {
        yield return new WaitForSeconds(delay);

        var obj = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
        TextMeshPro pointsScored = obj.gameObject.GetComponent<TextMeshPro>();
        
        if (score > 0) {
            pointsScored.color = new Color(0,1,0);
            pointsScored.text = "+" + score.ToString();
        }
        else if (score == 0 && negZero) {
            pointsScored.color = new Color(1,0,0); 
            pointsScored.text = "-" + score.ToString();
        }
        else if (score == 0 && !negZero) {
        }
        else {
            pointsScored.color = new Color(1,0,0); 
            pointsScored.text = score.ToString();
        }

        this.score.text = points.ToString();
        scoreHead.text = points.ToString();
    }

    IEnumerator GOLD_GAINED(float score, float delay)
    {
        yield return new WaitForSeconds(delay);

        var obj = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
        obj.transform.localScale /= 2;
        TextMeshPro pointsScored = obj.gameObject.GetComponent<TextMeshPro>();
        
        if (score > 0) {
            pointsScored.color = new Color(0,1,0);
            pointsScored.text = "+" + score.ToString();
        }
        else if (score == 0) {
            pointsScored.color = new Color(1,0,0); 
            pointsScored.text = "-" + score.ToString();
        }
        else {
            pointsScored.color = new Color(1,0,0); 
            pointsScored.text = score.ToString();
        }

        this.score.text = coins.ToString();
        scoreHead.text = coins.ToString();
    }

    IEnumerator RELOAD_STAMP()
    {
        justStamped = true;

        yield return new WaitForSeconds(0.1f);
        justStamped = false;
    }


    // -------------------------------------------------------------- //
    // ---------------------- ATTACK ON TITAN ----------------------- //

    IEnumerator TITANIZE()
    {
        if (isTitan) yield break;

        var eff = Instantiate(teleBlueEffect, transform.position, teleBlueEffect.transform.rotation);
        eff.transform.parent = this.transform; Destroy(eff, 0.6f);

        while (this.transform.localScale.y < titanFactor)
        {
            yield return new WaitForEndOfFrame();
            this.transform.localScale += new Vector3(0.1f,0.1f,0.1f);
        }
        isTitan = true;

        yield return new WaitForSeconds(3);
        isTitan = false;

        while (this.transform.localScale.y > oldSize)
        {
            yield return new WaitForEndOfFrame();
            this.transform.localScale -= new Vector3(0.1f,0.1f,0.1f);
        }

        yield return new WaitForEndOfFrame();
        timer = 0;
        rb.velocity = Vector2.zero;
        _anim.speed = moveSpeed;
    }

    void CHECK_SPELL()
    {
        if (currentCircle != null)
        {
            if (currentCircle.GET_SPELL())
            {
                var go = Instantiate(floatingSpellTextPrefab, transform.position + new Vector3(0,3), transform.rotation);
                SpriteRenderer spellImg = go.GetComponentInChildren<SpriteRenderer>();
                spellImg.sprite = titanSpellIcon;

                go.GetComponent<TextMeshPro>().text = "+1";
                Color top = new Color(0, 0.8f, 1);
                Color bot = new Color(0, 0.3f, 1);
                go.GetComponent<TextMeshPro>().colorGradient  = new VertexGradient(top, top, bot, bot);

                Destroy(currentCircle.gameObject);
                currentCircle = null;
                StartCoroutine(TITANIZE() );
            }
            else { Destroy(currentCircle.gameObject);  currentCircle = null; }
        }
    }

    void CALC_ACCELERATE()
    {
        moveHorizontal = player.GetAxis("Move Horizontal");
        moveVertical = player.GetAxis("Move Vertical");

        moveDir = new Vector3(moveHorizontal, moveVertical).normalized;
    }

    void ACCELERATE()
    {
        // FLIP CHARACTER WHEN MOVING RIGHT
        if (moveHorizontal > 0 && !cursorMode)
        {
            character.transform.localScale = new Vector3(-scaleX, scaleX, scaleX);
        }
        else if (moveHorizontal < 0 && !cursorMode)
        {
            character.transform.localScale = new Vector3(scaleX, scaleX, scaleX);
        }

        // DID NOT HIT A WALL
        if (rb.velocity.magnitude != 0) {_anim.speed = rb.velocity.magnitude; _anim.SetBool("IsWalking", true);}
        // HIT A WALL
        else {_anim.speed = 1; _anim.SetBool("IsWalking", false); }

        timer += Time.fixedDeltaTime;
        // 
        if (timer > accRate) { rb.AddForce(moveDir * accSpeed); }
        if (!isTitan) rb.velocity = Vector2.zero;
    }

    void SQUASHED()
    {
        if (sceneName == "Attack-On-Titan")
        {
            if (manager != null) points = manager.nPlayersOut - controller.nPlayers;
            _collider.enabled = false;
            isOut = true;
            float scaleY = character.transform.localScale.y * 0.8f;
            _anim.speed = 0;
            character.transform.localScale -= new Vector3(0,scaleY);
            StartCoroutine( DelayElimCo() );
        }
        else {
            _collider.enabled = false;
            isOut = true;
            float scaleY = character.transform.localScale.y * 0.8f;
            _anim.speed = 0;
            character.transform.localScale -= new Vector3(0,scaleY);
            if (pw != null) pw.nPlayersOut++;
            if (pw != null) pw.CheckIfEveyoneIsOut(1);
        }
    }


    // ------------------------------------------------------------ //
    // ------------------------- BARRIER -------------------------- //

    void BARRIER()
    {
        if (mp > 0)
        {
            barrierEffect.SetActive(true);
            moveSpeed = 2.5f;
            mpFill.transform.localScale -= new Vector3(0.004f,0);
            if (mp > 0) mp -= 0.4f;
            nMp.text = mp.ToString("F1") + "%";
        }
        else 
        {
            barrierEffect.SetActive(false);
            barrierOn = false;
            moveSpeed = 5;
            mp = 0;
            nMp.text = "0%";
        }
    }

    void SPAWN_PROJECTILES()
    {
        float inputX = player.GetAxis("Look Horizontal");
        float inputY = player.GetAxis("Look Vertical");

        if (inputX == 0 && inputY == 0) shooting = false;
        else { shooting = true; restorePrefab.SetActive(false); }

        if (inputX > 0) {
            character.transform.localScale = new Vector3(-scaleX, scaleX, scaleX);
        }
        else if (inputX < 0)
        {
            character.transform.localScale = new Vector3(scaleX, scaleX, scaleX);
        }

        if (nShots > 0 && canShootAgain) 
        {
            float angle = Mathf.Atan2(-inputX, inputY) * Mathf.Rad2Deg; 
            if (angle != 0) 
            {
                // Vector3 dir = new Vector3(inputX,inputY).normalized * 0.5f;
                SHOOT(angle);
                nShots = 0;
            }
        }
        else if (shot == null)
        {
            nShots = 1;
        }
    }

    void SHOOT(float angle)
    {
        StartCoroutine( RELOAD() );
        var obj = Instantiate(projectileEffect[characterN], transform.position + new Vector3(0,0.4f,-0.5f),
            Quaternion.Euler(0,0,angle));
        shot = obj.gameObject;
        Destroy(obj, 2);
    }

    void RESTORE_MANA()
    {
        if (movePower != 0 || player.GetButton("L") || player.GetButton("R") 
            || player.GetButton("ZR") || player.GetButton("ZL") || shooting || barrierEffect.activeSelf) 
        { 
            moveTimer = 0; 
            restorePrefab.SetActive(false);
        }
        else if (!barrierEffect.activeSelf && movePower <= 0) 
        {
            moveTimer += Time.fixedDeltaTime;
            if (moveTimer > 1) 
            {
                if (mpFill.transform.localScale.x < 1)
                {
                    if (!restorePrefab.activeSelf) restorePrefab.SetActive(true);
                    mpFill.transform.localScale += new Vector3(0.001f,0);
                    if (mp < 100) mp += 0.1f;
                    nMp.text = mp.ToString("F1") + "%";
                }
                else
                {
                    restorePrefab.SetActive(false);
                    mp = 100;
                    nMp.text =  "100%";
                }
            }
        }
    }

    IEnumerator RELOAD()
    {
        canShootAgain = false;
        yield return new WaitForSeconds(0.1f);
        canShootAgain = true;
    }

    // ------------------------------------------------------------- //
    // ------------------------- DARKNESS -------------------------- //

    void CHANGE_MASK()
    {
        if (sTimer < 0.2f) sTimer += Time.deltaTime;
        else
        {
            if (player.GetButtonDown("A"))
            {
                if      (nMask == 1)
                {
                    sTimer = 0;     nMask++;
                    mask.transform.localScale *= 2;
                    moveSpeed = 15;
                }
                else if (nMask == 2)
                {
                    sTimer = 0;     nMask++;
                    mask.transform.localScale *= 2;
                    moveSpeed = 7;
                }
                else if (nMask == 3)
                {
                    sTimer = 0;     nMask++;
                    mask.transform.localScale *= 2;
                    moveSpeed = 3;
                }
            }
            else if (player.GetButtonDown("B"))
            {
                if      (nMask == 4)
                {
                    sTimer = 0;     nMask--;
                    mask.transform.localScale /= 2;
                    moveSpeed = 7;
                }
                else if (nMask == 3)
                {
                    sTimer = 0;     nMask--;
                    mask.transform.localScale /= 2;
                    moveSpeed = 15;
                }
                else if (nMask == 2)
                {
                    sTimer = 0;     nMask--;
                    mask.transform.localScale /= 2;
                    moveSpeed = 22.5f;
                }
            }
        }
    }

    // ***************************************************************************** //
    // ***************************************************************************** //

    private void OnTriggerEnter2D(Collider2D other) 
    {
        // SNEAK AND SNORE
        if (sceneName == "Sneak_And_Snore")
        {
            if (other.tag == "Gold")
            {
                coins++;
                coinPickup.Play();
                score.text = coins.ToString();
                scoreHead.text = coins.ToString();
                Destroy(other.gameObject);
            }
            if (other.tag == "Safe")
            {
                outsideOfEffect = true;
                manager.nPlayersOut++;
                _anim.speed = 2.5f;
                moveSpeed = 2.5f;
                score.text = coins.ToString();
                score.color = new Color (0,1,0);
                scoreHead.text = coins.ToString();
                scoreHead.color = new Color (0,1,0);
                manager.PLAYER_WON_N_COINS(coins, name);
                manager.CheckIfEveyoneIsOut(0);
            }
        }
        // PRACTICE
        else if (sceneName == "Sneak And Snore")
        {
            if (other.tag == "Gold")
            {
                coins++;
                score.text = coins.ToString();
                scoreHead.text = coins.ToString();
                Destroy(other.gameObject);
            }
            if (other.tag == "Safe")
            {
                outsideOfEffect = true;
                pw.nPlayersOut++;
                _anim.speed = 2.5f;
                moveSpeed = 2.5f;
                score.text = coins.ToString();
                score.color = new Color (0,1,0);
                scoreHead.text = coins.ToString();
                scoreHead.color = new Color (0,1,0);
                pw.CheckIfEveyoneIsOut(0);
            }
        }


        else if (sceneName == "Lava_Or_Leave_'Em" || sceneName == "Lava Or Leave 'Em")
        {
            if (other.tag == "Hurtbox") {
                isOut = true;
                var dead = Instantiate(deathEffect, transform.position, Quaternion.identity);
                dead.transform.localScale *= 1.6f;
                Destroy(dead, 2);
                transform.position = new Vector3(transform.position.x, 50);
                if (manager != null) 
                {
                    StartCoroutine( DelayElimCo() );
                    dead.transform.parent = manager.transform;
                    points = manager.nPlayersOut - controller.nPlayers;
                    score.text = points.ToString();
                }
                else if (pw != null) 
                {
                    dead.GetComponent<AudioSource>().volume = 0;
                    dead.transform.parent = pw.transform;
                    pw.nPlayersOut++;
                    pw.CheckIfEveyoneIsOut(1);
                }
            }
        }
        
        else if (sceneName == "Pushy-Penguins" || sceneName == "Pushy Penguins")
        {
            if (other.tag == "Hurtbox" && !isOut) {
                isOut = true;
                // _collider.enabled = false;
                // Destroy(rb);
                // transform.position = new Vector3(-50, transform.position.y);
                if (manager != null) 
                {
                    StartCoroutine( DelayElimCo() );
                    points = manager.nPlayersOut - controller.nPlayers;
                    score.text = points.ToString();
                }
                else if (pw != null) 
                {
                    pw.nPlayersOut++;
                    pw.CheckIfEveyoneIsOut(1);
                }
            }
        }
        
        else if (sceneName == "Fun_Run" || sceneName == "Fun Run")
        {
            if (other.tag == "Respawn") 
            {
                if (manager != null) {
                    if (respawnPoint != other.gameObject && manager.canPlay) { checkpointSound.Play(); }
                }
                // NEW CHECKPOINT
                if (respawnPoint != other.gameObject)
                {
                    respawnPoint = other.gameObject;
                    var obj = Instantiate(checkpointEffect, transform.position + new Vector3(0,1), Quaternion.identity);
                    Destroy(obj, 0.5f);
                }
            }
            if (other.tag == "Hurtbox") 
            {
                StartCoroutine( OutOfBounds() );
            }
            if (other.tag == "Safe" && !isFinished) 
            {
                isFinished = true;
                respawnPoint = other.gameObject;
                var obj = Instantiate(checkpointEffect, transform.position + new Vector3(0,1), Quaternion.identity);
                Destroy(obj, 0.5f);
                if (manager != null)
                {
                    points = controller.nPlayers - manager.nPlayersOut;
                    manager.nPlayersOut++;
                    DisplayRankPlacement("race");
                    manager.CheckIfEveyoneIsOut(1);
                }
                else if (pw != null)
                {
                    points = controller.nPlayers - pw.nPlayersOut;
                    pw.nPlayersOut++;
                    pw.CheckIfEveyoneIsOut(1);
                }
            }
        }
        
        else if (sceneName == "Money_Belt" || sceneName == "Coin-veyor")
        {
            if (other.tag == "Gold")
            {
                boxToOpen = other.gameObject.GetComponent<Box>();
                // Debug.Log("found a box");
            }
        }

        else if (sceneName == "Shocking-Situation" || sceneName == "Shocking Situation" )
        {
            if (other.tag == "Hurtbox" && !isInvincible)
            {
                float changeInPoints = coins;
                if (manager != null) electrocuted.Play();

                if (2 > coins) coins = 0;
                else            coins -= 2;

                changeInPoints = coins - changeInPoints;
                var obj = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
                obj.transform.localScale /= 2;
                TextMeshPro pointsScored = obj.gameObject.GetComponent<TextMeshPro>();
                
                if (changeInPoints == 0) {
                    pointsScored.color = new Color(1,0,0); 
                    pointsScored.text = "-" + changeInPoints.ToString();
                }
                else {
                    pointsScored.color = new Color(1,0,0); 
                    pointsScored.text = changeInPoints.ToString();
                }

                score.text = coins.ToString();
                scoreHead.text = coins.ToString();
                StartCoroutine(Invincible());
            }
            if (other.tag == "Gold")
            {
                coins++;
                score.text = coins.ToString();
                scoreHead.text = coins.ToString();
                if (manager != null) coinPickup.Play();
                Destroy(other.gameObject);
                StartCoroutine( GOLD_GAINED(1, 0) );
            }
        }

        else if (sceneName == "Attack-On-Titan" || sceneName == "Attack Of Titan")
        {
            if (other.tag == "Node") { 
                currentCircle = other.GetComponent<FreeSpellNode>();
                currentCircle.HIGHLIGHT(1);
            }
        }
        
        else if (sceneName == "Flower-Shower" || sceneName == "Flower Shower")
        {
            if (manager != null)
            {
                if (!manager.timeUp)
                {
                    if (other.tag == "Safe") 
                    { 
                        points++;
                        onePointSound.Play();
                        score.text = points.ToString();
                        scoreHead.text = points.ToString();
                        Destroy(other.gameObject);
                    }
                    else if (other.tag == "Gold")
                    { 
                        points += 3;
                        threePointSound.Play();
                        score.text = points.ToString();
                        scoreHead.text = points.ToString();
                        Destroy(other.gameObject);
                    }
                }
            }
            else
            {
                if (other.tag == "Safe") 
                { 
                    points++;
                    score.text = points.ToString();
                    scoreHead.text = points.ToString();
                    Destroy(other.gameObject);
                }
                else if (other.tag == "Gold")
                { 
                    points += 3;
                    score.text = points.ToString();
                    scoreHead.text = points.ToString();
                    Destroy(other.gameObject);
                }
            }
        }
   
        else if (sceneName == "Barrier_Bearers" || sceneName == "Barrier Bearers")
        {
            if (other.tag == "Hurtbox" && !isOut)
            {
                Destroy(other.transform.parent.gameObject);
                if (!barrierEffect.activeSelf)
                {
                    // WINNER CANNOT BE ELIMINATED
                    if (manager != null) { if (manager.timeUp) return; }
                    var eff = Instantiate(teleportEffect, transform.position, teleportEffect.transform.rotation);
                    eff.transform.localScale *= 0.6f;
                    Destroy(eff, 1.5f);
                    transform.position += new Vector3(0,500);
                    isOut = true; _collider.enabled = false;
                    if (manager != null) {
                        if (manager.timeUp) return; 
                        points = manager.nPlayersOut - controller.nPlayers;
                        StartCoroutine( DelayElimCo() );
                    }
                    else {
                        pw.nPlayersOut++; 
                        pw.CheckIfEveyoneIsOut(1);
                    }
                }
            }
            else if (other.tag == "Special" && !isOut)
            {
                if (!barrierEffect.activeSelf)
                {
                    if (manager != null) { if (manager.timeUp) return; }
                    var eff = Instantiate(teleportEffect, transform.position, teleportEffect.transform.rotation);
                    eff.transform.localScale *= 0.6f;
                    Destroy(eff, 1.5f);
                    transform.position += new Vector3(0,500);
                    isOut = true; _collider.enabled = false;
                    if (manager != null) {
                        if (manager.timeUp) return;
                        points = manager.nPlayersOut - controller.nPlayers;
                        StartCoroutine( DelayElimCo() );
                    }
                    else {
                        pw.nPlayersOut++; 
                        pw.CheckIfEveyoneIsOut(1);
                    }
                }
            }
        }
    
        else if (manager != null && manager.bossBattle)
        {
            if (manager.bossBattle && other.tag == "Hurtbox" && !manager.timeUp && !isOut)
            {
                Destroy(other.transform.parent.gameObject);
                if (!barrierOn && !isInvincible) StartCoroutine( Damaged() );
            }
            if (manager.bossBattle && other.tag == "Special" && !manager.timeUp && !isOut)
            {
                if (!barrierOn && !isInvincible) StartCoroutine( Damaged() );
            }
        }
    }

    // COLLIDE WITH OTHER PLAYERS
    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (bouncePhysics && !isTitan)
        {
            if (sceneName == "Don't-Be-A-Zombie" || sceneName == "Don't Be A Zombie")
            {
                if (other.gameObject.tag == "Enemy" && !isZombie)
                {
                    moveSpeed = 3.5f;
                    rb.velocity = Vector2.zero;
                    isZombie = true;
                    this.gameObject.tag = "Enemy";
                    if (manager != null) {
                        if (manager.timeUp) return;
                        points = manager.nPlayersOut - controller.nPlayers;
                        StartCoroutine( DelayElimCo() );
                    }
                    else {
                        pw.nPlayersOut++; pw.CheckIfEveyoneIsOut(1);
                    }

                    if (other.gameObject.TryGetComponent(out Zombie zom))
                    {
                        zom.CAPTURED_A_HUMAN(this.gameObject);
                    }
                    else
                    {
                        CentralZombieSpawner hub = GameObject.Find("Central Spawner").GetComponent<CentralZombieSpawner>();
                        hub.PLAYER_CAPTURED(this.gameObject);
                    }

                    // TURN GREEN
                    foreach (Transform child in character.transform)  
                    {  
                        if (child.name != "Shadow") {
                            if (child.TryGetComponent(out SpriteRenderer cs)) { cs.color = new Color(0,0.75f,0); }
                        }
                        foreach (Transform grandChild in child)
                        {
                            if (grandChild.name != "Shadow") {
                                if (grandChild.TryGetComponent(out SpriteRenderer gs)) { gs.color = new Color(0,0.75f,0); }
                            }
                        }
                    }
                }
            }
            if (other.gameObject.tag == "Player")
            {
                if (sceneName == "Spotlight-Fight" || sceneName == "Spotlight Fight")
                {
                    if (!dashing) {
                        MinigameControls opponent = other.gameObject.GetComponent<MinigameControls>();
                        StartCoroutine( opponent.KnockBackCo(
                            knockbackDuration, knockbackPower * movePower, this.transform, 0.1f) );
                    }
                    else {

                        MinigameControls opponent = other.gameObject.GetComponent<MinigameControls>();
                        Debug.Log("== " + knockbackPower + ", " + chargePower + ", " + _anim.speed);
                        StartCoroutine( opponent.KnockBackCo(
                            knockbackDuration, knockbackPower * chargeKb, this.transform, chargeKb * 0.1f) );
                    }
                }
                else if (sceneName == "Attack-On-Titan" || sceneName == "Attack Of Titan")
                {
                    MinigameControls opponent = other.gameObject.GetComponent<MinigameControls>();
                    // COLLIDE WITH REGULAR OPPONENTS
                    if (!opponent.isTitan)
                    {
                        StartCoroutine( opponent.KnockBackCo(
                            knockbackDuration, knockbackPower * movePower, this.transform, 0.1f) );
                    }
                    // HIT BY TITAN
                    else 
                    {
                        SQUASHED();
                    }
                }
                
                else if (sceneName == "Fun_Run" || sceneName == "Fun Run")
                {
                    if (!dashing) {
                        MinigameControls opponent = other.gameObject.GetComponent<MinigameControls>();
                        StartCoroutine( opponent.KnockBackCo(
                            knockbackDuration, knockbackPower * movePower, this.transform, 0.1f) );
                    }
                    else {
                        MinigameControls opponent = other.gameObject.GetComponent<MinigameControls>();
                        Debug.Log("== " + knockbackPower + ", " + chargePower + ", " + _anim.speed);
                        StartCoroutine( opponent.KnockBackCo(
                            knockbackDuration, knockbackPower * chargeKb, this.transform, chargeKb * 0.1f) );
                    }
                }
                else 
                {
                    MinigameControls opponent = other.gameObject.GetComponent<MinigameControls>();
                    StartCoroutine( opponent.KnockBackCo(
                        knockbackDuration, knockbackPower * movePower, this.transform, 0.1f) );
                }
            }

            if (manager != null)
            {
                if (manager.bossBattle && other.gameObject.tag == "Enemy")
                {
                    StartCoroutine( KnockBackCo( knockbackDuration, 3.5f, other.transform, 0.1f) );
                }
            }
        }
        else if (!bouncePhysics)
        {
            if (sceneName == "Plunder Ground" && other.gameObject.tag == "Player")
            {
                Collider2D col = other.gameObject.GetComponent<Collider2D>();
                Physics2D.IgnoreCollision(col, _collider, true);
            }
            else if (sceneName == "Plunder-Ground" && other.gameObject.tag == "Player")
            {
                Collider2D col = other.gameObject.GetComponent<Collider2D>();
                Physics2D.IgnoreCollision(col, _collider, true);
            }
        }
        if (sceneName == "Attack-On-Titan" && other.gameObject.tag == "Safe" && isTitan)
        {
            canAcc = false;
            rb.velocity = Vector2.zero;
            timer = 0;
        }
        else if (sceneName == "Attack Of Titan" && other.gameObject.tag == "Safe" && isTitan)
        {
            canAcc = false;
            rb.velocity = Vector2.zero;
            timer = 0;
        }
        if (manager != null && manager.bossBattle && sceneName == "Dojo" && playerID == 0) {
            if (other.gameObject.tag == "Obstacle") {
                challenge.SetActive(true);
                bossScene = other.gameObject.name;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if (manager != null && manager.bossBattle && sceneName == "Dojo" && playerID == 0) {
            if (other.gameObject.tag == "Obstacle") {
                challenge.SetActive(false);
                // bossScene = other.gameObject.name;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        // SNEAK AND SNORE
        if (sceneName == "Sneak_And_Snore")
        {
            if (other.tag == "Hurtbox" && !crystalised.activeSelf && !outsideOfEffect) {
                crystalised.SetActive(true);
                manager.nPlayersOut++;
                _anim.speed = 0;
                manager.CheckIfEveyoneIsOut(0);
            }
            if (other.tag == "Safe")
            {
                _anim.speed = 2.5f;
                moveSpeed = 2.5f;
                transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            }
        }
        else if (sceneName == "Sneak And Snore")
        {
            if (other.tag == "Hurtbox" && !crystalised.activeSelf && !outsideOfEffect) {
                crystalised.SetActive(true);
                pw.nPlayersOut++;
                _anim.speed = 0;
                pw.CheckIfEveyoneIsOut(0);
            }
        }
    
        // COLOUR CHAOS
        else if (sceneName == "Colour_Chaos")
        {
            if (other.tag == "Hurtbox") {
                // WINNER CANNOT BE ELIMINATED
                if (manager != null) { if (manager.timeUp) return; }
                var eff = Instantiate(teleportEffect, transform.position, teleportEffect.transform.rotation);
                Destroy(eff, 1.5f);
                transform.position = new Vector3(5, transform.position.y);
                var obj = Instantiate(teleportEffect, transform.position, teleportEffect.transform.rotation);
                Destroy(obj, 1.5f); obj.gameObject.transform.parent = instances.transform;
                points = manager.nPlayersOut - controller.nPlayers;
                score.text = points.ToString();
                StartCoroutine( DelayElimCo() );
                // manager.nPlayersOut++;
                // manager.CheckIfEveyoneIsOut(1);
            }
        }
        else if (sceneName == "Colour Chaos")
        {
            if (other.tag == "Hurtbox") {
                // WINNER CANNOT BE ELIMINATED
                if (manager != null) { if (manager.timeUp) return; }
                var eff = Instantiate(teleportEffect, transform.position, teleportEffect.transform.rotation);
                Destroy(eff, 1.5f);
                transform.position = new Vector3(5, transform.position.y);
                var obj = Instantiate(teleportEffect, transform.position, teleportEffect.transform.rotation);
                Destroy(obj, 1.5f); obj.gameObject.transform.parent = instances.transform;
                pw.nPlayersOut++;
                pw.CheckIfEveyoneIsOut(1);
            }
        }

        else if (sceneName == "Spotlight-Fight" || sceneName == "Spotlight Fight" )
        {
            if (other.tag == "Safe")
            {
                points += Time.deltaTime;
                score.color = new Color (0,1,0);
                score.text = points.ToString("F1");
                scoreHead.color = new Color (0,1,0);
                scoreHead.text = points.ToString("F1");
            }
        }

        else if (sceneName == "Pushy-Penguins" || sceneName == "Pushy Penguins")
        {
            if (other.tag == "Hurtbox") {
                // transform.position = new Vector3(-50, transform.position.y);
                transform.Translate(Vector3.down * 5 * Time.fixedDeltaTime);
                transform.Translate(Vector3.left* 5 * Time.fixedDeltaTime);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (sceneName == "Spotlight-Fight" || sceneName == "Spotlight Fight")
        {
            if (other.tag == "Safe")
            {
                score.color = new Color (1,1,1);
                score.text = points.ToString("F1");
                scoreHead.color = new Color (1,1,1);
                scoreHead.text = points.ToString("F1");
            }
        }
        else if (sceneName == "Attack-On-Titan" || sceneName == "Attack Of Titan")
        {
            if (other.tag == "Node") { 
                currentCircle.HIGHLIGHT(-1);
                currentCircle = null;
            }
        }
    }


    private IEnumerator OutOfBounds()
    {
        isOut = true;   // CAN'T MOVE
        _collider.enabled = false;
        character.SetActive(false);

        yield return new WaitForSeconds(1.5f);
        transform.position = respawnPoint.transform.position + new Vector3(Random.Range(-6f,6f),0);
        _collider.enabled = true;
        gameObject.SetActive(true);
        isOut = false;   // CAN MOVE
        character.SetActive(true);

    }

    private IEnumerator DelayElimCo()
    {
        yield return new WaitForSeconds(0.05f);
        if (manager.timeUp) { yield break; }
        manager.nPlayersOut++;
        DisplayRankPlacement("elim");
        manager.CheckIfEveyoneIsOut(1);
    }
    
    private IEnumerator IS_EVERYONE_OUT()
    {
        yield return new WaitForSeconds(0.05f);
        if (manager.timeUp) { yield break; }
        manager.nPlayersOut++;
        // DisplayRankPlacement("elim");
        manager.CheckIfEveyoneIsOut(0);
    }

    public IEnumerator KnockBackCo(float knockbackDuration, float knockbackPower, Transform opponent, float chargeDelay)
    {
        float timer = 0;
        beingKnocked = true;
        _anim.speed = 5;
        if (chargingParticle != null) chargingParticle.SetActive(false); chargePower = 0; magicCircle.SetActive(false);
        yield return new WaitForEndOfFrame();
        if (dashing) { beingKnocked = false; yield break;}
        Debug.Log(name + " received kb of = " + knockbackPower);

        while (knockbackDuration > timer)
        {
            timer += Time.fixedDeltaTime;
            Vector2 direction = ( opponent.transform.position - this.transform.position ).normalized;
            rb.AddForce(-direction * knockbackPower, ForceMode2D.Impulse);
        }

        yield return new WaitForSeconds(knockbackDuration);
        // chargeKb = 0;
        rb.velocity = Vector2.zero;
        if (chargeDelay > 0.1f)
        {
            var diz = Instantiate(dizzyEffect, transform.position + new Vector3(0,2), dizzyEffect.transform.rotation);
            diz.transform.parent = this.transform;
            Destroy(diz, chargeDelay);
        }

        yield return new WaitForSeconds(chargeDelay);
        beingKnocked = false;
    }

    private IEnumerator DashCo(float dashDuration, Vector3 dashDirection)
    {
        float timer = 0;
        yield return new WaitForEndOfFrame();
        dashing = true;

        while (dashDuration > timer)
        {
            timer += Time.fixedDeltaTime;
            rb.velocity = dashDirection * chargePower * 4f;
        }

        yield return new WaitForSeconds(knockbackDuration);
        dashing     = false;
        chargePower = 0;
        rb.velocity = Vector2.zero;
        chargeKb = 0;
    }

    private IEnumerator Invincible()
    {
        Debug.Log("INVINCIBLE");
        isInvincible = true;
        moveSpeed = 0;
        foreach (Transform child in character.transform)  
        {  
            if (child.name != "Shadow") {
                if (child.TryGetComponent(out SpriteRenderer cs)) { cs.color = new Color(1,0,0,0.4f); }
            }
            foreach (Transform grandChild in child)
            {
                if (grandChild.name != "Shadow") {
                    if (grandChild.TryGetComponent(out SpriteRenderer gs)) { gs.color = new Color(1,0,0,0.4f); }
                }
            }
        }
        yield return new WaitForSeconds(0.5f);
        moveSpeed = 4;

        yield return new WaitForSeconds(1.5f);
        Debug.Log("NO MORE");
        isInvincible = false;
        foreach (Transform child in character.transform)  
        {  
            if (child.name != "Shadow") {
                if (child.TryGetComponent(out SpriteRenderer cs)) { cs.color = new Color(1,1,1,1); }
            }
            foreach (Transform grandChild in child)
            {
                if (grandChild.name != "Shadow") {
                    if (grandChild.TryGetComponent(out SpriteRenderer gs)) { gs.color = new Color(1,1,1,1); }
                }
            }
        }
    }

    private IEnumerator Damaged()
    {
        isInvincible = true;
        foreach (Transform child in character.transform)  
        {  
            if (child.name != "Shadow") {
                if (child.TryGetComponent(out SpriteRenderer cs)) { cs.color = new Color(1,0,0); }
            }
            foreach (Transform grandChild in child)
            {
                if (grandChild.name != "Shadow") {
                    if (grandChild.TryGetComponent(out SpriteRenderer gs)) { gs.color = new Color(1,0,0); }
                }
            }
        }

        if (health > 0) health--;
        hp.sprite = hearts[health];

        if (health <= 0)
        {
            _collider.enabled = false;
            if (manager != null) {
                if (manager.timeUp) yield break; 
                isOut = true;
                transform.position += new Vector3(0,500);
                points = manager.nPlayersOut - controller.nPlayers;
                StartCoroutine( IS_EVERYONE_OUT() );
            }
            else {
                pw.nPlayersOut++; 
                pw.CheckIfEveyoneIsOut(0);
            }
        }
        else 
        {
            var obj = Instantiate(hurtPrefab, transform.position + new Vector3(0,0.5f), hurtPrefab.transform.rotation);
            Destroy(obj.gameObject, 2);
        }

        yield return new WaitForSeconds(1);

        foreach (Transform child in character.transform)  
        {  
            if (child.name != "Shadow") {
                if (child.TryGetComponent(out SpriteRenderer cs)) { cs.color = new Color(1,1,1,1); }
            }
            foreach (Transform grandChild in child)
            {
                if (grandChild.name != "Shadow") {
                    if (grandChild.TryGetComponent(out SpriteRenderer gs)) { gs.color = new Color(1,1,1,1); }
                }
            }
        }
        isInvincible = false;
    }


    //  elimMode = last man standing (last -> first = 0)
    //     raceMode = race placement    (first -> last = 0) 
    //     scoreMode = rank based on score 
    private void DisplayRankPlacement(string gameStyle)
    {
        if (gameStyle == "elim")
        {
            for ( int rank = controller.nPlayers ; rank >= 0 ; rank-- )
            {
                // ** last -> first (controller.nPlayers -> 0)
                if (Mathf.Abs(points) == rank)
                {
                    placeBubble.SetActive(true);
                    placetext.text = ( (rank) + CardinalValue(rank, "elim") ).ToString();
                    break;
                }
            }
        }
        else if (gameStyle == "race")
        {
            for ( int rank=0 ; rank<=controller.nPlayers ; rank++ )
            {
                // ** first -> last (controller.nPlayers -> 0)
                if (Mathf.Abs(points) == rank)
                {
                    placeBubble.SetActive(true);
                    int nth = controller.nPlayers - rank;
                    placetext.text = ( (nth+1) + CardinalValue(nth+1, "race") ).ToString();
                    break;
                }
            }
        }
    }
    public void DisplayRankPlacement_MANAGER(int rank)
    {
        placeBubble.SetActive(true);
        placetext.text = ( (rank+1) + CardinalValue(rank, "") ).ToString();
    }
    private string CardinalValue(int n, string gameStyle)
    {
        if (gameStyle == "elim")
        {
            switch (n)
            {
                case 0 : return "st";
                case 1 : return "st";   // SHOULDN'T REACH
                case 2 : return "nd";
                case 3 : return "rd";
                default : return "th";
            }
        }
        else if (gameStyle == "race")
        {
            if (n == 1 || n == 0)         return "st";
            if (n == 2)                   return "nd";
            if (n == 3)                   return "rd";
            return "th";
        }
        else 
        {
            if (n == 0)         return "st";
            if (n == 1)         return "nd";
            if (n == 2)         return "rd";
            return "th";
        }
    }

    public void COIN_MINIGAME()
    {
        // score.text = coins.ToString();
        // score.color = new Color (0,1,0);
        // scoreHead.text = coins.ToString();
        // scoreHead.color = new Color (0,1,0);
        points = coins;
        manager.PLAYER_WON_N_COINS(coins, name);
    }

}
