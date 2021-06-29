using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
// Imported assets
using Rewired;
using Rewired.Integration.UnityUI;
using BeautifulTransitions.Scripts.Transitions.Components.Camera.AbstractClasses;
using BeautifulTransitions.Scripts.Transitions.TransitionSteps;
using BeautifulTransitions.Scripts.Transitions.Components.Camera;

public class PathFollower : MonoBehaviour
{
    // GLOBAL VARIABLES
    [Header("Data to save across Scenes")]
    [SerializeField] private int playerID;
    [SerializeField] private Player player;
    public List<PlayerPrevData> data;
    private List<PlayerBuffData> stat;
    
    //! DELETE
    //// public Node[] _currentPath;                    // CURRENT NODE(PATH)    
    //// public int _currentNode;                       // INDEX OF THE CURRENT SPACE ON NODE(PATH)  
    //// public Vector3 _currentPositionHolder;         // POSITION OF THE CURRENT SPACE 
    private Vector3 asidePos1;
    private Vector3 asidePos2;
    private Vector3 asidePos3;
    private Vector3 asidePos4;
    private Vector3 asidePos5;
    private Vector3 asidePos6;
    private Vector3 asidePos7;
    private Vector3 asidePos8;


    [Header("Player Character")]
    [SerializeField] public string characterName;
    private float lScale;
    public  Animator _animator;
    private GameObject character;
    [SerializeField] private GameObject[] characters;   // ** inspector
    [SerializeField] private GameObject[] heads;        // ** inspector
    

    // CUSTOM PATHS 
    [SerializeField] public Node currentNode;
    [SerializeField] public Node nextNode;
    //! -------- DELETE --------
    //// private GameObject  _path0;
    //// private Node[]      _nodes0;
    //// private GameObject  _path1;
    //// private Node[]      _nodes1;
    //// private GameObject  _path2;
    //// private Node[]      _nodes2;
    //// private GameObject  _path3;
    //// private Node[]      _nodes3;
    //// private GameObject  _path4;
    //// private Node[]      _nodes4;
    //// private GameObject  _path5;
    //// private Node[]      _nodes5;
    //// private GameObject  _path6;
    //// private Node[]      _nodes6;
    //// private GameObject  _path7;
    //// private Node[]      _nodes7;
    //// private GameObject  _path8;
    //// private Node[]      _nodes8;
    //// private GameObject  _path9;
    //// private Node[]      _nodes9;
    //// private GameObject  _path10;
    //// private Node[]      _nodes10;
    //// private GameObject  _path11;
    //// private Node[]      _nodes11;
    //// private GameObject  _path12;
    //// private Node[]      _nodes12;
    //// private GameObject  _path13;
    //// private Node[]      _nodes13;
    //// private GameObject  _path14;
    //// private Node[]      _nodes14;
    //// private GameObject  _path15;
    //// private Node[]      _nodes15;
    //// private GameObject  _path16;
    //// private Node[]      _nodes16;
    //// private GameObject  _path17;
    //// private Node[]      _nodes17;
    //// private GameObject  _path18;
    //// private Node[]      _nodes18;
    //// private GameObject  _path19;
    //// private Node[]      _nodes19;
    //// private GameObject  _path20;
    //// private Node[]      _nodes20;
    //! -------- DELETE --------


    [Header("Movement Related")]
    [SerializeField] private bool noMovementLoss;   //todo DEBUGGING ONLY
    [SerializeField] public TextMeshProUGUI nMovesLeft; // UI ABOVE PLAYER SHOWING N MOVES LEFT
    [SerializeField] private Animator movesTextAnim; // ** INSPECTOR
    [SerializeField] private Text _mapMoves;
    [SerializeField] private GameObject setting;
    [SerializeField] private Slider pSpeed;
    public int _movesRemaining;
    private bool moveDecremented;

    private float _moveSpeed = 5f;                 // SPEED TO MOVE TO NEXT SPACE (7.5f)
    private float _timer;

    private GameManager manager;
    public Camera _cam;           // PLAYER TRACKING CAM
    private float camSizeLarge = 17.5f;
    private float camSizeNormal = 7.5f;
    [SerializeField] private float camSpeed = 15;                   // CAMERA PANNING SPEED ON VIEW MAP MODE
    [SerializeField] private float maxCamSpeed = 60;                   // CAMERA PANNING SPEED ON VIEW MAP MODE
    [SerializeField] private float minCamSpeed = 15;                   // CAMERA PANNING SPEED ON VIEW MAP MODE
    private float transitionTime = 1;
    [SerializeField] private GameObject _choice;    // VISUAL INDICATING WHICH WAY TO GO

    private BoxCollider2D _collider;
    [SerializeField] private GameObject    hurtBox;
    private string _currentScene;                   // FOR PATH FORKS (BASED ON MAPS)


    // ********************** PLAYER STATES ********************** //
    private bool isPlayerTurn;              // (FIRST STATE) THIS PLAYER'S TURN
    private bool isReadyToMove;             // (NEXT STATE)  FINISHED ROLLING DICE
    private bool isRollingDice;             //   ROLLING DICE
    private bool isCastingSpell;            //   CASTING SPELL
    private bool isViewingMap;              //   VIEWING MAP AT CROSSROADS | AT START OF TURN
    private bool isAtCentre;                // PLAYER AT CENTRE OF NODE
    public  bool isAtFork;                  // (OPT. STATE)  SPLIT IN PATH
    public  bool isAtShop;                  // (OPT. STATE)  AT SPELL STORE
    public  bool isAtFree;                  // (OPT. STATE)  AT FREE SPELL NODE
    public  bool isAtPotion;                  // (OPT. STATE)  AT ITEM STORE
    public  bool isAtMagicOrb;             // (OPT. STATE)  AT MAGIC ORB
    public  bool isAtSpecial;               // (OPT. STATE)  AT SPECIAL (END OF MAP [MULTI-EVENTS] )
    public  bool isBoat;                    // MAKE SURE BOAT HAPPENING OCCURS ONCE
    private bool isLanded;                  // (NEXT STATE)  NO MOVEMENT LEFT
    private bool hasPurchased;              
    public  bool isPayingSomeone;           // LANDED ON A TRAP MUST PAY SOMEONE
    public  bool beingPayed;                // PLAYER RECEIVING GOLD FROM OPPONENT
    private bool gotAsidePos;                   // MOVED ASIDE AFTER 
    private bool readyToMovedAside;         // (NEXT STATE)  START TO MOVE ASIDE
    private bool isAside;
    public  bool diceRolled;                // excludes movement from spells


    [Header("PLAYER GAME DATA")]
    [SerializeField] private AudioSource coinPickup;
    [SerializeField] private AudioSource coinLoss;
    [SerializeField] private AudioSource orbPickup;
    [SerializeField] private AudioSource orbLoss;
    [SerializeField] private AudioSource orbStolen;
    [SerializeField] private AudioSource gotAnOrb;
    [SerializeField] private AudioSource spellPickup;
    [SerializeField] private AudioSource manaUsed;
    [SerializeField] private GameObject layout;
    public int coins;
    public int orbs;   // MARIO PARTY STARS
    public TextMeshProUGUI ranking;



    // ** PLAYER STATUS/BUFFS (TO UPDATE TO CONTROLLER)
    [SerializeField] private Slider[] movebars;
    public bool playerSlowed    = false;
    [SerializeField] private GameObject slowed;
    public bool playerBarrier   = false;
    [SerializeField] private GameObject barrier;
    public bool playerMove15    = false;
    public bool playerRange2    = false;
    public bool playerExtraBuy  = false;
    [SerializeField] private GameObject powerup;  //   RAINBOW SPARKLE EFFECT
    [SerializeField] private AudioSource powerupSound;  


    [Header("Prefabs to Spawn")]
    [SerializeField] private GameObject floatingCoinTextPrefab;
    [SerializeField] private GameObject floatingOrbTextPrefab;
    [SerializeField] private GameObject floatingManaTextPrefab;
    [SerializeField] private GameObject floatingSpellTextPrefab;
    [SerializeField] private GameObject confettiPrefab;
    [SerializeField] private Text[] textBox;
    private GameController controller;


    [Header("Player Choices At Start Of Turn")]
    [SerializeField] private Image startUi;
    private Animator startUiAnim;
    [SerializeField] private Sprite[] starts;

    [SerializeField] private Image buttonMove;
    [SerializeField] private Image buttonSpells;
    [SerializeField] private Image buttonMap;

    [SerializeField] private Slider moveBar;
    [SerializeField] private ProgressBar radialBar;
    private Image radialCircle;     // ** SCRIPT
    [SerializeField] private Sprite[]     radialCircles;
    private int buttonIndex = 0;



    [Header("Button Mapping Image")]
    [SerializeField] private Image mapMapping;
    [SerializeField] private Image mapViewmap;
    [SerializeField] private Image mapCastSpell;


    [Header("Direction Input")]
    [SerializeField] private Image arrowLeft;   // 1
    [SerializeField] private Image arrowRight;  // 2
    [SerializeField] private Image arrowUp;     // 3
    [SerializeField] private Image arrowDown;   // 4
    private Node.Directions arrowDirection;


    [Header("Spells")]
    [SerializeField] public Image[] spellSlots;
    private int maxNoOfSpells = 3;
    private int nSpells = 0;
    [SerializeField] private GrimoireMenu grimoireUI;
    [SerializeField] public List<SpellType> spells;
    [SerializeField] public Slider grimoireBar;
    [SerializeField] private TextMeshProUGUI grimoireLeft;
    public  int spellMpCost;
    private int spellIndex;
    [SerializeField] public Slider mpBar;
    [SerializeField] private TextMeshProUGUI mpLeft;
    [SerializeField] private TrapSpell    spellTrapTarget;
    [SerializeField] private EffectSpell  spellEffectTarget;
    [SerializeField] private EffectSpell  spellSpecialEffectTarget; // EFFECT SINGLE PLAYER
    [SerializeField] private GameObject   lockedOn;
    [SerializeField] private GameObject areaOfEffect;
    [SerializeField] private GameObject blueSpellCirclePrefab;
    [SerializeField] private GameObject blueBuffPrefab;
    [SerializeField] private GameObject greenSpellCirclePrefab;
    [SerializeField] private GameObject greenBuffPrefab;

    // ************************* SPELL STATES ************************* //
    private bool trapSpellActive;               //   CASTING TRAP SPELL
    private bool effectSpellActive;             //   CASTING EFFECT SPELL
    private bool specialSpellActive;             //   CASTING SPECIAL SPELL
    private bool pseudoMove;
    public bool stealMode;
    [SerializeField] private GameObject stealingAura;
    [SerializeField] private GameObject greenTeleportEffect;



    [Header("Shopping")]
    [SerializeField] private GameObject shopUI;     // ALL
    [SerializeField] private GameObject shopKeeperUI; // STAGE 1
    [SerializeField] private GameObject shopSpellUI;  // STAGE 2
    [SerializeField] private GameObject shopItemUI;   // STAGE 2
    [SerializeField] private GameObject shopItemUIcontent;   // STAGE 3
    [SerializeField] private GameObject shopItemUIdesc;   // STAGE 3
    [SerializeField] private Image      shopKeeperImg;// SELLER
    [SerializeField] private TextMeshProUGUI sellerGreetings;
    [SerializeField] private TextMeshProUGUI purchaseLeftText;
    [SerializeField] private TextMeshProUGUI purchaseLeftText2;
    private float bgMusicVol = 0.2f;

    [SerializeField] private GameObject discardSpellUI;   // GRIMOIRE FULL
    [SerializeField] private Image[] discardSpellImg;
    [SerializeField] private TextMeshProUGUI spellDesc;
    [SerializeField] private GameObject spellDescUI;
    private Sprite fourthSpell;  // HOLDER
    private GameObject highlightedSpell;
    private GameObject currentSelected;
    private int spellToGain;        // RANDOM SPELL FROM CONTROLLER
    private bool fromGoodSpell;
    private int shopSeller;
    public bool shop1;
    public bool shop2;
    public bool shop3;
    public bool shop4;
    public int  nPurchaseLeft;
    public int  maxNPurchases;
    [SerializeField] private Sprite[] shopKeepers;
    [SerializeField] private ShopMenu   playerPurchase;
    [SerializeField] private ShopMenu   potionShop;
    [SerializeField] private GameObject rewiredEventSystem;


    [Header("Rewired")]
    [SerializeField] private RewiredStandaloneInputModule[] rInputs;


    [Header("Beautiful Transitions")]
    public FadeCamera beaut;


    [Header("Text Related")]
    [SerializeField] private GameObject notEnoughCoins;
    [SerializeField] private GameObject buyMagicOrb;


    [Header("Shogun Seaport")]
    [SerializeField] private GameObject goldBoat;
    [SerializeField] private TextMeshProUGUI goldtext;
    [SerializeField] private GameObject evilBoat;
    [SerializeField] private TextMeshProUGUI eviltext;
    private int nPrompt;
    [SerializeField] private GameObject magicOrbUI;
    [SerializeField] private Image magicOrbUIPanel;


    [Header("Shogun Seaport")]
    [SerializeField] private bool losingAll;


    [Header("Images Spell")]
    [SerializeField] public Sprite spellSlotEmpty;
    [SerializeField] public Sprite spellNone;
    [SerializeField] public Sprite spellTrap10;
    [SerializeField] public Sprite spellTrap20;
    [SerializeField] public Sprite spellTrapOrb;
    [SerializeField] public Sprite spellEffect10;
    [SerializeField] public Sprite spellEffectMana3;
    [SerializeField] public Sprite spellEffectSpell1;
    [SerializeField] public Sprite spellEffectSlow1;
    [SerializeField] public Sprite spellEffectSwap;
    [SerializeField] public Sprite spellMoveDash5;
    [SerializeField] public Sprite spellMoveDash8;
    [SerializeField] public Sprite spellMoveSlow;
    [SerializeField] public Sprite spellMoveSlowgo;
    [SerializeField] public Sprite spellMoveBarrier;
    [SerializeField] public Sprite spellMoveSteal;
    [SerializeField] public Sprite spellMoveOrb;
    private bool slowDice;


    // ---------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        switch (name)
        {
            case "Player_1" : playerID = 0; break;
            case "Player_2" : playerID = 1; break;
            case "Player_3" : playerID = 2; break;
            case "Player_4" : playerID = 3; break;
            case "Player_5" : playerID = 4; break;
            case "Player_6" : playerID = 5; break;
            case "Player_7" : playerID = 6; break;
            case "Player_8" : playerID = 7; break;
        }
        player = ReInput.players.GetPlayer(playerID);
        spells = new List<SpellType>();

        controller = GameObject.Find("Game_Controller").GetComponent<GameController>();
        manager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _currentScene = SceneManager.GetActiveScene().name;
        magicOrbUI.SetActive(false);

        _collider = this.transform.GetComponent<BoxCollider2D>();
        _collider.enabled = false;

        // CHANGE CHARACTER
        switch (name)
        {
            case "Player_1" : characterName = controller.characterName1; break;
            case "Player_2" : characterName = controller.characterName2; break;
            case "Player_3" : characterName = controller.characterName3; break;
            case "Player_4" : characterName = controller.characterName4; break;
            case "Player_5" : characterName = controller.characterName5; break;
            case "Player_6" : characterName = controller.characterName6; break;
            case "Player_7" : characterName = controller.characterName7; break;
            case "Player_8" : characterName = controller.characterName8; break;
        }
        // CHARACTER MOVER
        for (int i=0 ; i<characters.Length ; i++) {
            if (characterName == characters[i].name) {
                var obj = Instantiate(characters[i], transform.position, Quaternion.identity, this.transform); 
                character = obj.gameObject; 
                obj.transform.parent = this.transform;  
                _animator = obj.GetComponent<Animator>();
                break;
            }
            if (i == characters.Length - 1) {
                Debug.LogError("ERROR : Have not assign character to name");
            }
        }
        // CHARACTER UI HEAD
        for (int i=0 ; i<heads.Length ; i++) {
            if (heads[i].name.Contains(characterName)) {
                heads[i].SetActive(true);
                break;
            }
            if (i == characters.Length - 1) {
                Debug.LogError("ERROR : Have not assign character to name");
            }
        }

        if (character != null) {
            character.transform.localScale *= 2;
            lScale      = character.transform.localScale.x;
        }
        
        if (rInputs != null)
        {
            foreach (RewiredStandaloneInputModule rInput in rInputs)
            {
                rInput.RewiredInputManager = GameObject.Find("Rewired_Input_Manager").GetComponent<InputManager>();
                if (rInput.RewiredPlayerIds != null) rInput.RewiredPlayerIds[0] = playerID;
            }
        }

        startUi.gameObject.SetActive(false);
        startUiAnim = startUi.GetComponent<Animator>();
        if (startUiAnim == null) Debug.LogError("COULDN'T GET START CHARACTER CIRCLE ANIMATOR");
        shopUI.SetActive(false);
        buttonMove.gameObject.SetActive(false);
        buttonSpells.gameObject.SetActive(false);
        buttonMap.gameObject.SetActive(false);
        // moveBar.gameObject.SetActive(false);
        radialBar.gameObject.SetActive(false);
        radialCircle = radialBar.GetComponent<Image>();

        // VISUAL BUTTON INDICATORS AT FORK
        arrowLeft.gameObject.SetActive(false);
        arrowRight.gameObject.SetActive(false);
        arrowUp.gameObject.SetActive(false);
        arrowDown.gameObject.SetActive(false);

        grimoireUI.gameObject.SetActive(false);
        areaOfEffect.SetActive(false);
        powerup.SetActive(false);

        //! DELETE
        if (true)
        {
            // if (GameObject.Find("Path_0") != null){
            //     _path0 = GameObject.Find("Path_0");
            //     _nodes0 = _path0.GetComponentsInChildren<Node>();
            // }
            // if (GameObject.Find("Path_1") != null){
            //     _path1 = GameObject.Find("Path_1");
            //     _nodes1 = _path1.GetComponentsInChildren<Node>();
            // }
            // if (GameObject.Find("Path_2") != null){
            //     _path2 = GameObject.Find("Path_2");
            //     _nodes2 = _path2.GetComponentsInChildren<Node>();
            // }
            // if (GameObject.Find("Path_3") != null){
            //     _path3 = GameObject.Find("Path_3");
            //     _nodes3 = _path3.GetComponentsInChildren<Node>();
            // }
            // if (GameObject.Find("Path_4") != null){
            //     _path4 = GameObject.Find("Path_4");
            //     _nodes4 = _path4.GetComponentsInChildren<Node>();
            // }
            // if (GameObject.Find("Path_5") != null){
            //     _path5 = GameObject.Find("Path_5");
            //     _nodes5 = _path5.GetComponentsInChildren<Node>();
            // }
            // if (GameObject.Find("Path_6") != null){
            //     _path6 = GameObject.Find("Path_6");
            //     _nodes6 = _path6.GetComponentsInChildren<Node>();
            // }
            // if (GameObject.Find("Path_7") != null){
            //     _path7 = GameObject.Find("Path_7");
            //     _nodes7 = _path7.GetComponentsInChildren<Node>();
            // }
            // if (GameObject.Find("Path_8") != null){
            //     _path8 = GameObject.Find("Path_8");
            //     _nodes8 = _path8.GetComponentsInChildren<Node>();
            // }
            // if (GameObject.Find("Path_9") != null){
            //     _path9 = GameObject.Find("Path_9");
            //     _nodes9 = _path9.GetComponentsInChildren<Node>();
            // }
            // if (GameObject.Find("Path_10") != null){
            //     _path10 = GameObject.Find("Path_10");
            //     _nodes10 = _path10.GetComponentsInChildren<Node>();
            // }
            // if (GameObject.Find("Path_11") != null){
            //     _path11 = GameObject.Find("Path_11");
            //     _nodes11 = _path11.GetComponentsInChildren<Node>();
            // }
            // if (GameObject.Find("Path_12") != null){
            //     _path12 = GameObject.Find("Path_12");
            //     _nodes12 = _path12.GetComponentsInChildren<Node>();
            // }
            // if (GameObject.Find("Path_13") != null){
            //     _path13 = GameObject.Find("Path_13");
            //     _nodes13 = _path13.GetComponentsInChildren<Node>();
            // }
            // if (GameObject.Find("Path_14") != null){
            //     _path14 = GameObject.Find("Path_14");
            //     _nodes14 = _path14.GetComponentsInChildren<Node>();
            // }
            // if (GameObject.Find("Path_15") != null){
            //     _path15 = GameObject.Find("Path_15");
            //     _nodes15 = _path15.GetComponentsInChildren<Node>();
            // }
            // if (GameObject.Find("Path_16") != null){
            //     _path16 = GameObject.Find("Path_16");
            //     _nodes16 = _path16.GetComponentsInChildren<Node>();
            // }
            // if (GameObject.Find("Path_17") != null){
            //     _path17 = GameObject.Find("Path_17");
            //     _nodes17 = _path17.GetComponentsInChildren<Node>();
            // }
            // if (GameObject.Find("Path_18") != null){
            //     _path18 = GameObject.Find("Path_18");
            //     _nodes18 = _path18.GetComponentsInChildren<Node>();
            // }
            // if (GameObject.Find("Path_19") != null){
            //     _path19 = GameObject.Find("Path_19");
            //     _nodes19 = _path19.GetComponentsInChildren<Node>();
            // }
            // if (GameObject.Find("Path_20") != null){
            //     _path20 = GameObject.Find("Path_20");
            //     _nodes20 = _path20.GetComponentsInChildren<Node>();
            // }
        }

        // TURN 2+
        if (controller.hasStarted)
        {
            // GET PREVIOUS VALUES (LAST SCENE) FROM GAME_CONTROLLER
            data = controller.GET_PLAYER_DATA(playerID);

            string newPath  = data[0].path;
            currentNode = GameObject.Find(newPath).GetComponent<Node>();
            transform.position  = data[0].posAside;
            mpBar.value         = data[0].mp;
            coins               = data[0].coins;
            orbs                = data[0].orbs;

            if (controller.playerOrder[0] == playerID) { transform.position = data[0].pos; }

            // STATUS EFFECTS / ITEMS FROM LAST TURN
            playerSlowed    = controller.buffDatas[playerID][0].slowed;
            playerBarrier   = controller.buffDatas[playerID][0].barrier;
            playerMove15    = controller.buffDatas[playerID][0].move15;
            playerRange2    = controller.buffDatas[playerID][0].range2;
            playerExtraBuy  = controller.buffDatas[playerID][0].extraBuy;

            if (playerSlowed) { 
                slowed.SetActive(true); slowed.transform.parent = character.transform; 
                foreach (Transform child in character.transform)  
                {  
                    if (child.name != "Shadow")
                    {
                        if (child.TryGetComponent(out SpriteRenderer cs)) { cs.color = new Color(0,0.5f,1); }
                    }
                    foreach (Transform grandChild in child)
                    {
                        if (grandChild.name != "Shadow")
                        {
                            if (grandChild.TryGetComponent(out SpriteRenderer gs)) { gs.color = new Color(0,0.5f,1); }
                        }
                    }
                }
            }
            if (playerBarrier) { 
                barrier.SetActive(true); hurtBox.SetActive(false); 
                barrier.transform.parent = character.transform;  
            }
            if (playerExtraBuy) maxNPurchases = 3;
            else                maxNPurchases = 1;
            if (playerRange2)   areaOfEffect.transform.localScale *= 2;
        }
        // TURN 1 ONLY
        else
        {
            // GET PATHS
            if (GameObject.Find("New-Node") != null) {
                currentNode = GameObject.Find("New-Node").GetComponent<Node>();
            }

            mpBar.value = mpBar.maxValue;
            coins = 10; // TO BE CHANGED // DELETE
            orbs = 0;
            _collider.enabled = true;
            maxNPurchases = 1;
        }
        
        // UPDATE SPELL BOOK (GRIMOIRE)
        switch (name) {
            case "Player_1" : 
                foreach (SpellType sp in controller.spells1) { spells.Add(sp); }    break;
            case "Player_2" : 
                foreach (SpellType sp in controller.spells2) { spells.Add(sp); }    break;
            case "Player_3" : 
                foreach (SpellType sp in controller.spells3) { spells.Add(sp); }    break;
            case "Player_4" : 
                foreach (SpellType sp in controller.spells4) { spells.Add(sp); }    break;
            case "Player_5" : 
                foreach (SpellType sp in controller.spells5) { spells.Add(sp); }    break;
            case "Player_6" : 
                foreach (SpellType sp in controller.spells6) { spells.Add(sp); }    break;
            case "Player_7" : 
                foreach (SpellType sp in controller.spells7) { spells.Add(sp); }    break;
            case "Player_8" : 
                foreach (SpellType sp in controller.spells8) { spells.Add(sp); }    break;
        }
        UPDATE_SPELLS_UI();

        RESET_PLAYER_UI();
        mpLeft.text = mpBar.value + "/" + mpBar.maxValue;

        _choice.SetActive(false);
        nMovesLeft.gameObject.SetActive(false);
        camSpeed = minCamSpeed;
    }


    // START THE CAMERA BEFORE THE START OF THE FIRST TURN (NO ABRUPT TRANSTIONS)
    public void BEGIN() { _cam.gameObject.SetActive(true); }


    // ******************************************************************************************************* //
    // ******************************************************************************************************* //
    // *************************************** USER-INPUT FUNCTIONS ****************************************** //
    void Update()
    {
        if (isPlayerTurn)
        {
            // if (player.GetButtonDown("Start"))
            // {
            //     // TURNS ON
            //     if (!setting.activeSelf) { this.pSpeed.value = controller.pSpeed; }
            //     setting.SetActive(!setting.activeSelf);
            //     // TURNS OFF
            //     if (!setting.activeSelf) { controller.pSpeed = this.pSpeed.value; }
            // }

            // if (setting.activeSelf)
            // {

            // }

            // CHARACTER START
            if (startUi.gameObject.activeSelf)
            {
                READY_TO_START();
            }

            // DISCARDING SPELL(S)
            else if (discardSpellUI.activeSelf)
            {
                currentSelected = EventSystem.current.currentSelectedGameObject;
                if (highlightedSpell == null || highlightedSpell != currentSelected)
                {
                    highlightedSpell = currentSelected;
                    spellDesc.text = highlightedSpell.GetComponent<Spell>()._desc;
                }
            }

            // USED MOVE SPELL DASH
            else if (pseudoMove && _movesRemaining > 0 && isReadyToMove) 
            {
                MOVE_PLAYER();
                UPDATE_MOVEMENT(_movesRemaining);
            }
            else if (pseudoMove && _movesRemaining == 0 && isReadyToMove) { 
                pseudoMove = false; 
                isReadyToMove = false; 
                _animator.SetBool("IsWalking", false);
                _animator.speed = 1;

                nMovesLeft.gameObject.SetActive(false);

                buttonMove.gameObject.SetActive(true);
                buttonSpells.gameObject.SetActive(true);
                buttonMap.gameObject.SetActive(true);
                manager.UNHIDE_UI();
            }

            // ****************** STANDARD ****************** //

            // HAVE NOT ROLLED DICE
            else if (!isReadyToMove && !pseudoMove) 
            {
                UserStartMenu();
            }
            
            // THE PLAYER HAS ROLLED DICE, AND IS MOVING TO A NEW SPACE
            // ** FORKS, SHOPS, MAGIC ORB HAPPEN IN MOVE_PLAYER() ** //
            else if (_movesRemaining > 0 && isReadyToMove && !pseudoMove)
            {
                MOVE_PLAYER();
                UPDATE_MOVEMENT(_movesRemaining);
            }
            // PLAYER HAS ROLLED DICE AND CAN NO LONGER MOVE
            else if (!isLanded && isReadyToMove && !isPayingSomeone && !pseudoMove)
            {
                SPACE_END();
            }
            // IF PLAYER HAS RECIEVED EFFECT FROM SPACE, THEN MOVE ASIDE TO AVOID OTHER PLAYERS ON THEIR TURNS
            else if (readyToMovedAside && isReadyToMove && !isAside && !pseudoMove)
            {
                MOVE_ASIDE();
            }
        }

        textBox[0].text = coins.ToString();
        textBox[1].text = orbs.ToString();
    }
    
    // ******************************************************************************************************* //
    // ******************************************************************************************************* //
    // ********************************** ROLL DICE | CAST SPELL | VIEW MAP ********************************** //
    
    /* -- USER INPUT -- */
    private void UserStartMenu()
    {
        // START MENU
        if (!isRollingDice && !isCastingSpell && !isViewingMap && !grimoireUI.gameObject.activeSelf && !diceRolled)
        {
            // SCROLLING THROUGH OPTIONS
            if (player.GetButtonDown("Up")) {
                buttonIndex--;
                if (buttonIndex < 0) { buttonIndex = 2; }
                START_MENU_BUTTON_HIGHLIGHT();
            }
            else if (player.GetButtonDown("Down")) {
                buttonIndex++;
                if (buttonIndex >= 3) { buttonIndex = 0; }
                START_MENU_BUTTON_HIGHLIGHT();
            }
            // SELECTED AN OPTION (BUTTON SELECT)
            else if (player.GetButtonDown("A")) 
            {
                if (buttonIndex == 0)       // ROLL DICE
                {
                    buttonMove.gameObject.SetActive(false);
                    buttonSpells.gameObject.SetActive(false);
                    buttonMap.gameObject.SetActive(false);

                    radialBar.gameObject.SetActive(true);
                    radialBar.maxValue = 360;
                    radialBar.value = Random.Range(radialBar.minValue, radialBar.maxValue);
                    if      (playerSlowed) { radialCircle.sprite = radialCircles[0]; }
                    else if (playerMove15) { radialCircle.sprite = radialCircles[2]; }
                    else                   { radialCircle.sprite = radialCircles[1]; }

                    isRollingDice = true;
                }
                else if (buttonIndex == 1)  // SEE SPELL BOOK
                {
                    buttonMove.gameObject.SetActive(false);
                    buttonSpells.gameObject.SetActive(false);
                    buttonMap.gameObject.SetActive(false);
                    hurtBox.SetActive(false);

                    grimoireUI.gameObject.SetActive(true);
                    grimoireUI.CHECK_EMPTY_GRIMOIRE();
                    grimoireBar.value = mpBar.value;
                    grimoireLeft.text = grimoireBar.value + "/" + grimoireBar.maxValue;
                    manager.HIDE_UI();
                }
                else if (buttonIndex == 2)  // VIEW MAP
                {
                    buttonMove.gameObject.SetActive(false);
                    buttonSpells.gameObject.SetActive(false);
                    buttonMap.gameObject.SetActive(false);

                    SHOW_NODE_DISTANCE();   // DISPLAY SPACES AWAY
                    mapMapping.gameObject.SetActive(true);
                    isViewingMap = true;

                    _cam.orthographicSize = camSizeLarge;
                    manager.HIDE_UI();
                }
            }
            // VIEW MAP SHORTCUT
            else if (player.GetButtonDown("X"))
            {
                SHOW_NODE_DISTANCE();   // DISPLAY SPACES AWAY

                buttonMove.gameObject.SetActive(false);
                buttonSpells.gameObject.SetActive(false);
                buttonMap.gameObject.SetActive(false);

                mapMapping.gameObject.SetActive(true);
                isViewingMap = true;

                _cam.orthographicSize = camSizeLarge;
                manager.HIDE_UI();
            }
        }
        // ROLLING DICE
        else if (isRollingDice && !diceRolled) 
        {
            if (slowDice)           radialBar.value += 1.5f;
            else if (!diceRolled)   radialBar.value += Random.Range(6f,72f);
            // else if (!diceRolled)   radialBar.value += 2;   // TODO : DELETE (DEBUG)
            if (radialBar.value >= radialBar.maxValue) radialBar.value -= radialBar.maxValue;
            if (radialBar.value >= radialBar.maxValue) radialBar.value = 0; // ** NOT GONNA ROLL A ONE

            if (player.GetButtonDown("A"))
            {
                diceRolled = true;
                nMovesLeft.gameObject.SetActive(true);
                movesTextAnim.SetTrigger("diceRolled");
                if (radialBar.value >= radialBar.maxValue) radialBar.value = 0;
                StartCoroutine(DICE_ROLLED());
            }
            // CANCEL ROLLING DICE
            else if (player.GetButtonDown("B"))
            {
                buttonMove.gameObject.SetActive(true);
                buttonSpells.gameObject.SetActive(true);
                buttonMap.gameObject.SetActive(true);
                radialBar.gameObject.SetActive(false);
                isRollingDice = false;
            }
        }
        // READING GRIMOIRE
        else if (grimoireUI.gameObject.activeSelf)
        {
            // CANCEL READING GRIMOIRE
            if (player.GetButtonDown("B"))
            {
                buttonMove.gameObject.SetActive(true);
                buttonSpells.gameObject.SetActive(true);
                buttonMap.gameObject.SetActive(true);
                hurtBox.SetActive(true);

                grimoireUI.gameObject.SetActive(false);
                manager.UNHIDE_UI();

                isCastingSpell = false;
            }
        }
        // CASTING SPELL
        else if (isCastingSpell)
        {
            UserCastSpell();
        }
        // VIEWING MAP
        else if (isViewingMap)
        {
            UserViewMap();
        }
    }

    private void UserViewMap()
    {
        // CANCEL VIEWING MAP
        if (player.GetButtonDown("B"))
        {
            if (diceRolled && !isAtShop || diceRolled && !isAtPotion || diceRolled && !isAtMagicOrb) { 
                nMovesLeft.gameObject.SetActive(true); 
            }
            for (int i=0 ; i<currentNode.nexts.Length ; i++) {
                currentNode.nexts[i].node.GetComponent<Node>().HIDE_MOVEMENT();
            }

            mapMapping.gameObject.SetActive(false);
            if (!isAtShop) isViewingMap = false;

            _cam.orthographicSize = camSizeNormal;
            _cam.transform.position = 
                new Vector3(this.transform.position.x, this.transform.position.y, _cam.transform.position.z);
            manager.UNHIDE_UI();

            if (isAtSpecial)
            {
                _cam.orthographicSize = 20;
                _cam.transform.position += new Vector3(0,16.25f);
            }

            if (isAtFork && _movesRemaining > 0)
            {
                SHOW_ARROWS();
                _mapMoves.text = "";
            }
            else if (isAtShop)
            {
                nMovesLeft.gameObject.SetActive(false);
                manager.HIDE_UI();
                shopUI.SetActive(true);
                shopKeeperUI.SetActive(true);
                isViewingMap = false;
            }
            else if (isAtMagicOrb)
            {
                nMovesLeft.gameObject.SetActive(false);
                manager.HIDE_UI();
                magicOrbUI.SetActive(true);
            }
            else 
            {
                buttonMove.gameObject.SetActive(true);
                buttonSpells.gameObject.SetActive(true);
                buttonMap.gameObject.SetActive(true);
                _cam.orthographicSize = camSizeNormal;
            }
        }
        // SPEED UP CAMERA SCROLL SPEED
        if (player.GetButtonDown("X")) { camSpeed = maxCamSpeed; }
        if (player.GetButtonUp("X"))   { camSpeed = minCamSpeed;}

        float moveHorizontal = player.GetAxis("Move Horizontal");
        float moveVertical   = player.GetAxis("Move Vertical");
        Vector3 direction = new Vector3(moveHorizontal, moveVertical, 0);
        _cam.transform.Translate(direction * camSpeed * Time.deltaTime);    
    }

    private void UserCastSpell()
    {
        // TRAP SPELLS
        if (trapSpellActive)
        {
            // CAST SPELL
            if (player.GetButtonDown("A"))
            {
                spellTrapTarget.PLAYER_CAST_TRAP();
            }

            // CANCEL CASTING SPELL
            if (player.GetButtonDown("B"))
            {
                trapSpellActive = false;
                // mapMapping.gameObject.SetActive(false);
                mapCastSpell.gameObject.SetActive(false);
                isViewingMap = false;
                spellTrapTarget.transform.position = new Vector3(this.transform.position.x,
                    this.transform.position.y, this.transform.position.z);
                isCastingSpell = false;
                spellTrapTarget.gameObject.SetActive(false);
                areaOfEffect.SetActive(false);

                _cam.orthographicSize = camSizeNormal;
                _cam.transform.position = 
                    new Vector3(this.transform.position.x, this.transform.position.y, _cam.transform.position.z);
                // _cam.transform.localPosition = new Vector3(0, 0, -30f);

                buttonMove.gameObject.SetActive(false);
                buttonSpells.gameObject.SetActive(false);
                buttonMap.gameObject.SetActive(false);

                grimoireUI.gameObject.SetActive(true);
                grimoireUI.CHECK_EMPTY_GRIMOIRE();
                grimoireBar.value = mpBar.value;
                grimoireLeft.text = grimoireBar.value + "/" + grimoireBar.maxValue;
            }
            // SPEED UP CAMERA SCROLL SPEED
            if (player.GetButtonDown("X")) { camSpeed = maxCamSpeed; }
            if (player.GetButtonUp("X"))   { camSpeed = minCamSpeed;}

            float moveHorizontal = player.GetAxis("Move Horizontal");
            float moveVertical   = player.GetAxis("Move Vertical");
            Vector3 direction = new Vector3(moveHorizontal, moveVertical, 0);
            _cam.transform.Translate(direction * camSpeed * Time.deltaTime);
            spellTrapTarget.gameObject.transform.Translate(direction * camSpeed * Time.deltaTime);
        }
        // EFFECT SPELLS
        else if (effectSpellActive)
        {
            // CAST SPELL
            if (player.GetButtonDown("A"))
            {
                spellEffectTarget.PLAYER_CAST_EFFECT();
            }

            // CANCEL CASTING SPELL
            if (player.GetButtonDown("B"))
            {
                effectSpellActive = false;
                // mapMapping.gameObject.SetActive(false);
                mapCastSpell.gameObject.SetActive(false);
                isViewingMap = false;
                spellEffectTarget.transform.position = new Vector3(this.transform.position.x,
                    this.transform.position.y, this.transform.position.z);
                isCastingSpell = false;
                spellEffectTarget.gameObject.SetActive(false);
                areaOfEffect.SetActive(false);

                _cam.orthographicSize = camSizeNormal;
                _cam.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -30f);

                buttonMove.gameObject.SetActive(false);
                buttonSpells.gameObject.SetActive(false);
                buttonMap.gameObject.SetActive(false);

                grimoireUI.gameObject.SetActive(true);
                grimoireUI.CHECK_EMPTY_GRIMOIRE();
                grimoireBar.value = mpBar.value;
                grimoireLeft.text = grimoireBar.value + "/" + grimoireBar.maxValue;
            }
            // SPEED UP CAMERA SCROLL SPEED
            if (player.GetButtonDown("X")) { camSpeed = maxCamSpeed; }
            if (player.GetButtonUp("X"))   { camSpeed = minCamSpeed;}

            float moveHorizontal = player.GetAxis("Move Horizontal");
            float moveVertical   = player.GetAxis("Move Vertical");
            Vector3 direction = new Vector3(moveHorizontal, moveVertical, 0);
            _cam.transform.Translate(direction * camSpeed * Time.deltaTime);
            spellEffectTarget.gameObject.transform.Translate(direction * camSpeed * Time.deltaTime);
        }
        // EFFECT SPECIAL SPELLS (TELEPORT)
        else if (specialSpellActive)
        {
            // CAST SPELL
            if (player.GetButtonDown("A"))
            {
                spellSpecialEffectTarget.PLAYER_CAST_SPECIAL();
            }

            // CANCEL CASTING SPELL
            if (player.GetButtonDown("B"))
            {
                specialSpellActive = false;
                // mapMapping.gameObject.SetActive(false);
                mapCastSpell.gameObject.SetActive(false);
                isViewingMap = false;
                spellSpecialEffectTarget.transform.position = new Vector3(this.transform.position.x,
                    this.transform.position.y, this.transform.position.z);
                isCastingSpell = false;
                spellSpecialEffectTarget.gameObject.SetActive(false);
                areaOfEffect.SetActive(false);

                _cam.orthographicSize = camSizeNormal;
                _cam.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -30f);

                buttonMove.gameObject.SetActive(false);
                buttonSpells.gameObject.SetActive(false);
                buttonMap.gameObject.SetActive(false);

                grimoireUI.gameObject.SetActive(true);
                grimoireUI.CHECK_EMPTY_GRIMOIRE();
                grimoireBar.value = mpBar.value;
                grimoireLeft.text = grimoireBar.value + "/" + grimoireBar.maxValue;
            }
            // SPEED UP CAMERA SCROLL SPEED
            if (player.GetButtonDown("X")) { camSpeed = maxCamSpeed; }
            if (player.GetButtonUp("X"))   { camSpeed = minCamSpeed;}

            float moveHorizontal = player.GetAxis("Move Horizontal");
            float moveVertical   = player.GetAxis("Move Vertical");
            Vector3 direction = new Vector3(moveHorizontal, moveVertical, 0);
            _cam.transform.Translate(direction * camSpeed * Time.deltaTime);
            spellSpecialEffectTarget.gameObject.transform.Translate(direction * camSpeed * Time.deltaTime);
        }
    }

    private void START_MENU_BUTTON_HIGHLIGHT()
    {
        // BUTTON HIGHLIGHT
        if (buttonIndex == 0)
        {
            buttonMove.color = new Color(1, 1, 1, 1);
            buttonSpells.color = new Color(1, 1, 1, 0.2f);
            buttonMap.color = new Color(1, 1, 1, 0.2f);
        }
        else if (buttonIndex == 1)
        {
            buttonMove.color = new Color(1, 1, 1, 0.2f);
            buttonSpells.color = new Color(1, 1, 1, 1);
            buttonMap.color = new Color(1, 1, 1, 0.2f);
        }
        else if (buttonIndex == 2)
        {
            buttonMove.color = new Color(1, 1, 1, 0.2f);
            buttonSpells.color = new Color(1, 1, 1, 0.2f);
            buttonMap.color = new Color(1, 1, 1, 1);
        }
    }

    void READY_TO_START()
    {
        if ((player.GetButtonDown("A") || player.GetButtonDown("B") || player.GetButtonDown("X") || player.GetButtonDown("Y"))
            && startUiAnim.GetCurrentAnimatorStateInfo(0).IsName("Character_Circle_Anim"))
        {
            // startUiAnim.SetTrigger("Ready");
            startUiAnim.Play("Character_Circle_End_Anim", -1, 0);
            StartCoroutine( START_CIRCLE_OFF() );
            buttonMove.gameObject.SetActive(true);
            buttonSpells.gameObject.SetActive(true);
            buttonMap.gameObject.SetActive(true);
        }
    }

    IEnumerator START_CIRCLE_OFF()
    {
        float sixth = 1f/6f;
        yield return new WaitForSeconds(sixth);
        startUi.gameObject.SetActive(false);
    }

    // ******************************************************************************************************* //
    // ******************** MOVING FROM NODE TO NODE | AT FORKS | AT SHOPS | AT MAGIC ORB ******************** //

    private void MOVE_PLAYER()
    {
        //* ONLY CHECK MAP AT FORK
        if (isViewingMap)
        {
            UserViewMap();
        }
        //* THERE IS A FORK IN THE PATH (SPLIT IN PATH)
        else if (isAtFork) {
            UserPathForkMenu();
        }
        //* AT SHOP SPACE
        else if (isAtShop) 
        {
            // GET SHOP KEEPER
            if (shopUI.activeSelf == false && !isViewingMap) { 
                nMovesLeft.gameObject.SetActive(false); 
                _mapMoves.text = _movesRemaining + " Moves Left";
                _animator.speed = 1;
                _animator.SetBool("IsWalking", false);
                manager.HIDE_UI();
                shopUI.SetActive(true); 
                if (!shopKeeperUI.activeSelf && !shopSpellUI.activeSelf)
                {
                    shopKeeperUI.SetActive(true); shopSpellUI.SetActive(false);
                }
                // SET SELLER SPRITE
                switch (shopSeller)
                {
                    case 0 :  
                        shopKeeperImg.sprite = shopKeepers[0]; sellerGreetings.text = "WELCOME to my humble store!";
                        break;
                    case 1 :  
                        shopKeeperImg.sprite = shopKeepers[1]; sellerGreetings.text = "How may I help you today.";
                        break;
                    case 2 :  
                        shopKeeperImg.sprite = shopKeepers[2]; sellerGreetings.text = "Salutations.";
                        break;
                    case 3 :  
                        shopKeeperImg.sprite = shopKeepers[3]; 
                        sellerGreetings.text = "One shall nev'r knoweth at which hour the tide shalt turneth.";
                        shop4 = true;
                        break;
                }
            }
            // AFTER GETTING SHOP KEEPER //* User Input *//
            else { UserAtShop(); }
        }
        //* AT BOAT SPACE
        else if (isAtSpecial)
        {
            if (goldBoat.activeSelf) { if (player.GetAnyButtonDown()) { nPrompt++; NEXT_TEXT(); } }
            if (evilBoat.activeSelf) { if (player.GetAnyButtonDown()) { nPrompt++; NEXT_TEXT(); } }
        }
        //* AT MAGIC ORB SPACE
        else if (isAtMagicOrb && currentNode.magicOrb.activeSelf) 
        {
            if (_animator.GetBool("IsWalking"))
            {
                _animator.speed = 1;
                _animator.SetBool("IsWalking", false);
            }
            if (!magicOrbUI.activeSelf && !isViewingMap) { 
                magicOrbUI.SetActive(true); 
                nMovesLeft.gameObject.SetActive(false);
                _mapMoves.text = _movesRemaining + " Moves Left";
                manager.HIDE_UI();
                if (coins < 20) { magicOrbUIPanel.color = new Color(1,0.1f,0.1f); }
            }
            UserAtMagicOrb();
        }
        //* RECIEVING FREE SPELL, DO NOTHING AND LET ANIMATION PLAY
        else if (isAtFree) {}
        //* GET THE NEXT NODE
        else if (nextNode == null && _movesRemaining > 0) {
            // SINGULAR PATH
            if (currentNode.nexts[0] != null && currentNode.nexts.Length <= 1) {
                nextNode = currentNode.nexts[0].node.GetComponent<Node>();
                PATH_BLOCKED_MOVE_TO_ALTERNATE();
                WHERE_TO_FACE();
            }
            // PATH SPLITS/FORKS
            else {
                AT_FORK();
                WHERE_TO_FACE();
            }
        }
        //* MOVEMENT && SPECIAL SPACE EVENTS
        else {
            // SOUND EFFECT
            if (nextNode.DOES_SPACE_DECREASE_MOVEMENT() && 
                Mathf.Abs(Vector2.Distance(this.transform.position, nextNode.transform.position)) < 0.5f) {
                    currentNode.PlaySound();   
            }
            // TODO - COMMENT OUT FOR INFINITE MOVEMENT - TODO //
            if (_movesRemaining != 1) {
                if (nextNode.DOES_SPACE_DECREASE_MOVEMENT() && !noMovementLoss && !moveDecremented &&
                    Mathf.Abs(Vector2.Distance(this.transform.position, nextNode.transform.position)) < 0.5f) {
                        moveDecremented = true;
                        _movesRemaining--;
                }
            }
            // MOVING TO NEXT NODE
            if (this.transform.position != nextNode.transform.position) {
                if (!_animator.GetBool("IsWalking")) {
                    _animator.SetBool("IsWalking", true);
                    _animator.speed = _moveSpeed;
                }
                _timer += _moveSpeed * Time.deltaTime;
                this.transform.position = Vector3.Lerp(this.transform.position, nextNode.transform.position, _timer);
            }
            // REACHED NEXT NODE
            else if (nextNode != null && this.transform.position == nextNode.transform.position) {
                // if (currentNode.DOES_SPACE_DECREASE_MOVEMENT()) currentNode.PlaySound();    // SOUND EFFECT
                currentNode = nextNode;
                nextNode = null;
                _timer = 0;

                // todo SPECIAL NON-DECREMENTING SPACES
                if (!currentNode.DOES_SPACE_DECREASE_MOVEMENT()) {
                    //* IF IS A FREE SPELL SPACE, THEN RECEIVE FREE SPELL
                    if (currentNode.TYPE_OF_SPECIAL_SPACE("free")) {
                        isAtFree = true;
                        StartCoroutine( GAIN_FREE_SPELL() );
                    }
                    //* SHOP/POTION SPACE
                    else if (currentNode.TYPE_OF_SPECIAL_SPACE("shop") || currentNode.TYPE_OF_SPECIAL_SPACE("potion")) {
                        shopSeller = currentNode.whoIsTheSeller;
                        isAtShop = true;
                        RESET_PURCHASES();
                        PURCHASES_LEFT();
                    }
                    //* MAGIC ORB SPACE
                    else if (currentNode.TYPE_OF_SPECIAL_SPACE("orb"))  // BUY A MAGIC ORB
                    {
                        isAtMagicOrb = true;
                    }
                    //* SPECIAL SPACE
                    else if (currentNode.TYPE_OF_SPECIAL_SPACE("spec"))  // BUY A MAGIC ORB
                    {
                        if (!isBoat) MANAGER_EVENT();
                    }
                }

                // TODO - COMMENT OUT FOR INFINITE MOVEMENT - TODO //
                // FINISHED MOVING
                if (_movesRemaining == 1 && !moveDecremented) {
                    if (currentNode.DOES_SPACE_DECREASE_MOVEMENT() && !noMovementLoss) {
                        moveDecremented = true;
                        _movesRemaining--;  
                    }
                }
                // HAVE NOT FINISHED MOVING
                else 
                {
                    // SINGULAR PATH
                    if (currentNode.nexts.Length <= 1) {
                        if (currentNode.nexts[0] != null) {
                            nextNode = currentNode.nexts[0].node.GetComponent<Node>();
                            PATH_BLOCKED_MOVE_TO_ALTERNATE();
                        }
                        if (_movesRemaining > 0) WHERE_TO_FACE();
                    }
                    // PATH SPLITS/FORKS
                    else {
                        isAtFork = true;
                        _animator.SetBool("IsWalking", false);
                        _animator.speed = 1;
                        SHOW_ARROWS();
                    }
                    moveDecremented = false;
                }
            }
        }

        if (true) {     //! DELETE
        // // ONLY CHECK MAP AT FORK
        // if (isViewingMap)
        // {
        //     UserViewMap();
        // }
        // // AT PATH FORK (SPLIT IN PATH)
        // else if (isAtFork)
        // {
        //     UserPathForkMenu();
        // }
        // // AT SHOP SPACE
        // else if (isAtShop && isAtCentre) 
        // {
        //     if (shopUI.activeSelf == false && !isViewingMap) { 
        //         nMovesLeft.gameObject.SetActive(false); 
        //         _mapMoves.text = _movesRemaining + " Moves Left";
        //         _animator.speed = 1;
        //         _animator.SetBool("IsWalking", false);
        //         manager.HIDE_UI();
        //         shopUI.SetActive(true); 
        //         if (!shopKeeperUI.activeSelf && !shopSpellUI.activeSelf)
        //         {
        //             shopKeeperUI.SetActive(true); shopSpellUI.SetActive(false);
        //         }
        //         // SET SELLER SPRITE
        //         if      (shop1) { shopKeeperImg.sprite = shopKeepers[0]; sellerGreetings.text = "WELCOME to my humble store!"; }
        //         else if (shop2) { shopKeeperImg.sprite = shopKeepers[1]; sellerGreetings.text = "How may I help you today."; }
        //         else if (shop3) { shopKeeperImg.sprite = shopKeepers[2]; sellerGreetings.text = "Salutations."; }
        //         else if (shop4) { shopKeeperImg.sprite = shopKeepers[3]; 
        //             sellerGreetings.text = "Tomorrow's yesterday is undoubtedly unpredictable."; }
        //     }
        //     //* User Input *//
        //     else { UserAtShop(); }
        // }
        // // AT END OF Shogun_Seaport MAP
        // else if (isAtSpecial && isAtCentre)
        // {
        //     if (goldBoat.activeSelf) { if (player.GetAnyButtonDown()) { nPrompt++; NEXT_TEXT(); } }
        //     if (evilBoat.activeSelf) { if (player.GetAnyButtonDown()) { nPrompt++; NEXT_TEXT(); } }
        // }
        // // AT MAGIC ORB SPACE
        // else if (isAtMagicOrb && isAtCentre) 
        // {
        //     if (_animator.GetBool("IsWalking"))
        //     {
        //         _animator.speed = 1;
        //         _animator.SetBool("IsWalking", false);
        //     }
        //     if (!magicOrbUI.activeSelf && !isViewingMap) { 
        //         magicOrbUI.SetActive(true); 
        //         nMovesLeft.gameObject.SetActive(false);
        //         _mapMoves.text = _movesRemaining + " Moves Left";
        //         manager.HIDE_UI();
        //         if (coins < 20) { magicOrbUIPanel.color = new Color(1,0.1f,0.1f); }
        //     }
        //     UserAtMagicOrb();
        // }
        // //
        // else if (isAtFree) 
        // {

        // }

        // // NOT ON ANY SPACE (STILL MOVING TO NEXT SPACE)
        // else if (this.transform.position != _currentPositionHolder)
        // {
        //     _timer += _moveSpeed * Time.deltaTime;
        //     this.transform.position = Vector3.Lerp(this.transform.position, _currentPositionHolder, _timer);
        //     if (isAtCentre) isAtCentre = false;

        //     // THE PLAYER HAS MOVED A SPACE (DECREMENT MOVEMENT DISPLAY)
        //     if (inContactWithNode && _movesRemaining != 1)
        //     {
        //         if (!_animator.GetBool("IsWalking")) {
        //             _animator.SetBool("IsWalking", true);
        //             _animator.speed = _moveSpeed;
        //         }
        //         if(!player.GetButton("Y")) _movesRemaining--;  //! COMMENT OUT FOR INFINITE MOVEMENT ** //
        //         // _movesRemaining--;
        //         inContactWithNode = false;
        //     }
        //     // LAST MOVEMENT LEFT
        //     else if (inContactWithNode && this.transform.position == _currentPositionHolder && _movesRemaining == 1) 
        //     {
        //         if(!player.GetButton("Y")) _movesRemaining--;  //! COMMENT OUT FOR INFINITE MOVEMENT ** //
        //         // _movesRemaining--; 
        //         inContactWithNode = false;
        //     }

        // }
        // // LOOKS BETTER WHEN STANDING IN THE MIDDLE AT SHOP / MAGIC ORB
        // else if (this.transform.position == _currentPositionHolder && !isAtCentre)
        // {
        //     isAtCentre = true;
        // }
        // // MOVE TO NEXT NODE ON PATH
        // else if (_currentNode < _currentPath.Length)        // REACHED A NODE ON THE PATH
        // {
        //     _currentNode++;
        //     CHECK_NODE();
        // }
        // // NEXT NODE IS EITHER A SPLIT PATH OR A CONJOINING PATH
        // else if (_currentNode == _currentPath.Length)       // REACHED THE END OF A PATH
        // {
        //     if (CHECK_FORK())
        //     {
        //         _animator.SetBool("IsWalking", false);
        //         _animator.speed = 1;
        //         _choice.SetActive(true);
        //         isAtFork = true;
        //         FORK_ARROW_VISUAL();
        //     }
        // }
        }
    }

    // todo ---------------- NODE/PATHING RELATED ---------------- *//

    // AT FORK, MULTIPLE NEXT NODES
    private void AT_FORK()
    {
        isAtFork = true;
        // if (_movesRemaining > 0) { SHOW_ARROWS(); }
        SHOW_ARROWS();
    }

    //** WHICH PATH TO TAKE IN FORKS
    private void CHOOSING_PATH_AT_FORK()
    {
        if (!isViewingMap && isAtFork) {
            for (int i=0 ; i<currentNode.nexts.Length ; i++) {
                if (arrowDirection == currentNode.nexts[i].direction) {
                    nextNode = currentNode.nexts[i].node.GetComponent<Node>();
                    PATH_BLOCKED_MOVE_TO_ALTERNATE();

                    isAtFork = false;
                    _animator.SetBool("IsWalking", true);
                    _animator.speed = _moveSpeed;

                    // HIDE ARROW VISUALS
                    arrowDirection      = Node.Directions.none;
                    arrowLeft.color     = new Color(1, 0.7f, 0, 0.4f);
                    arrowRight.color    = new Color(1, 0.7f, 0, 0.4f);
                    arrowUp.color       = new Color(1, 0.7f, 0, 0.4f);
                    arrowDown.color     = new Color(1, 0.7f, 0, 0.4f);

                    arrowLeft.gameObject.SetActive(false);
                    arrowRight.gameObject.SetActive(false);
                    arrowUp.gameObject.SetActive(false);
                    arrowDown.gameObject.SetActive(false);
                    WHERE_TO_FACE();
                    break;
                }
            }
        }
        // CHOOSE A PATH (BASED ON MAP) //! DELETE
        if (!isViewingMap && isAtFork) {
        // /* --------------------------------------------------------------- */
        // /* ----------------- "CRYSTAL CAVERNS" MAP ----------------------- */
        //     if (_currentScene == "Crystal_Caverns")
        //     {
        //         // END OF PATH 0
        //         if (_currentPath == _nodes0)
        //         {
        //             // MOVE TO PATH 1
        //             if (arrowDirection == _down) // CHANGE DIR INPUT
        //             {
        //                 _choice.SetActive(false);
        //                 _currentNode = 0;       // CHANGE SPACE IN PATH
        //                 _currentPath = _nodes1; // CHANGE PATH
        //                 CHECK_NODE();
        //             }
        //             // MOVE TO PATH 4
        //             else if (arrowDirection == _up) // CHANGE DIR INPUT
        //             {
        //                 _choice.SetActive(false);
        //                 _currentNode = 0;       // CHANGE SPACE IN PATH
        //                 _currentPath = _nodes4; // CHANGE PATH
        //                 CHECK_NODE();
        //             }
        //         }
        //         // END OF PATH 1
        //         else if (_currentPath == _nodes1)
        //         {
        //             // MOVE TO PATH 3
        //             if (arrowDirection == _up)   // CHANGE DIR INPUT
        //             {
        //                 _choice.SetActive(false);
        //                 _currentNode = 0;       // CHANGE SPACE IN PATH
        //                 _currentPath = _nodes3; // CHANGE PATH
        //                 CHECK_NODE();
        //             }
        //             // MOVE TO PATH 2
        //             else if (arrowDirection == _down) // CHANGE DIR INPUT
        //             {
        //                 _choice.SetActive(false);
        //                 _currentNode = 0;       // CHANGE SPACE IN PATH
        //                 _currentPath = _nodes2; // CHANGE PATH
        //                 CHECK_NODE();
        //             }
        //         }
        //         // END OF PATH 4
        //         else if (_currentPath == _nodes4)
        //         {
        //             // MOVE TO PATH 5
        //             if (arrowDirection == _left) // CHANGE DIR INPUT
        //             {
        //                 _choice.SetActive(false);
        //                 _currentNode = 0;       // CHANGE SPACE IN PATH
        //                 _currentPath = _nodes5; // CHANGE PATH
        //                 CHECK_NODE();
        //             }
        //             // MOVE TO PATH 8
        //             else if (arrowDirection == _right) // CHANGE DIR INPUT
        //             {
        //                 _choice.SetActive(false);
        //                 _currentNode = 0;       // CHANGE SPACE IN PATH
        //                 _currentPath = _nodes8; // CHANGE PATH
        //                 CHECK_NODE();
        //             }
        //         }
        //         // END OF PATH 5
        //         else if (_currentPath == _nodes5) 
        //         {
        //             // MOVE TO PATH 6
        //             if (arrowDirection == _left)   // CHANGE DIR INPUT
        //             {
        //                 _choice.SetActive(false);
        //                 _currentNode = 0;       // CHANGE SPACE IN PATH
        //                 _currentPath = _nodes6; // CHANGE PATH
        //                 CHECK_NODE();
        //             }
        //             // MOVE TO PATH 7
        //             else if (arrowDirection == _right) // CHANGE DIR INPUT
        //             {
        //                 _choice.SetActive(false);
        //                 _currentNode = 0;       // CHANGE SPACE IN PATH
        //                 _currentPath = _nodes7; // CHANGE PATH
        //                 CHECK_NODE();
        //             }
        //         }
        //         // END OF PATH 8
        //         else if (_currentPath == _nodes8)
        //         {
        //             // MOVE TO PATH 9
        //             if (arrowDirection == _up) // CHANGE DIR INPUT
        //             {
        //                 _choice.SetActive(false);
        //                 _currentNode = 0;       // CHANGE SPACE IN PATH
        //                 _currentPath = _nodes9; // CHANGE PATH
        //                 CHECK_NODE();
        //             }
        //             // MOVE TO PATH 12
        //             else if (arrowDirection == _down) // CHANGE DIR INPUT
        //             {
        //                 _choice.SetActive(false);
        //                 _currentNode = 0;       // CHANGE SPACE IN PATH
        //                 _currentPath = _nodes12; // CHANGE PATH
        //                 CHECK_NODE();
        //             }
        //         }
        //         // END OF PATH 9
        //         else if (_currentPath == _nodes9)
        //         {
        //             // MOVE TO PATH 10
        //             if (arrowDirection == _left) // CHANGE DIR INPUT
        //             {
        //                 _choice.SetActive(false);
        //                 _currentNode = 0;       // CHANGE SPACE IN PATH
        //                 _currentPath = _nodes10; // CHANGE PATH
        //                 CHECK_NODE();
        //             }
        //             // MOVE TO PATH 11
        //             else if (arrowDirection == _down) // CHANGE DIR INPUT
        //             {
        //                 _choice.SetActive(false);
        //                 _currentNode = 0;       // CHANGE SPACE IN PATH
        //                 _currentPath = _nodes11; // CHANGE PATH
        //                 CHECK_NODE();
        //             }
        //             // MOVE TO PATH 16
        //             else if (arrowDirection == _right) // CHANGE DIR INPUT
        //             {
        //                 _choice.SetActive(false);
        //                 _currentNode = 0;       // CHANGE SPACE IN PATH
        //                 _currentPath = _nodes16; // CHANGE PATH
        //                 CHECK_NODE();
        //             }
        //         }
        //         // END OF PATH 12
        //         else if (_currentPath == _nodes12)
        //         {
        //             // MOVE TO PATH 15
        //             if (arrowDirection == _left) // CHANGE DIR INPUT
        //             {
        //                 _choice.SetActive(false);
        //                 _currentNode = 0;       // CHANGE SPACE IN PATH
        //                 _currentPath = _nodes15; // CHANGE PATH
        //                 CHECK_NODE();
        //             }
        //             // MOVE TO PATH 13
        //             else if (arrowDirection == _right) // CHANGE DIR INPUT
        //             {
        //                 _choice.SetActive(false);
        //                 _currentNode = 0;       // CHANGE SPACE IN PATH
        //                 _currentPath = _nodes13; // CHANGE PATH
        //                 CHECK_NODE();
        //             }
        //         }

        //         isAtFork = false;
        //         _animator.SetBool("IsWalking", true);
        //         _animator.speed = _moveSpeed;
        //     }
        // /* ----------------- "CRYSTAL CAVERNS" MAP ----------------------- */
        // /* --------------------------------------------------------------- */
       
       
        // /* --------------------------------------------------------------- */
        // /* ------------------ "SHOGUN SEAPORT" MAP ----------------------- */
        //     if (_currentScene == "Shogun_Seaport")
        //     {
        //         // END OF PATH 0
        //         if (_currentPath == _nodes0)
        //         {
        //             if (isBoat) isBoat = false;
        //             // MOVE TO PATH 1
        //             if (arrowDirection == _up) // CHANGE DIR INPUT
        //             {
        //                 _choice.SetActive(false);
        //                 _currentNode = 0;       // CHANGE SPACE IN PATH
        //                 _currentPath = _nodes1; // CHANGE PATH
        //                 CHECK_NODE();
        //             }
        //             // MOVE TO PATH 2
        //             else if (arrowDirection == _left) // CHANGE DIR INPUT
        //             {
        //                 _choice.SetActive(false);
        //                 _currentNode = 0;       // CHANGE SPACE IN PATH
        //                 _currentPath = _nodes2; // CHANGE PATH
        //                 CHECK_NODE();
        //             }
        //         }
        //         // END OF PATH 3
        //         else if (_currentPath == _nodes3)
        //         {
        //             // MOVE TO PATH 4
        //             if (arrowDirection == _right)   // CHANGE DIR INPUT
        //             {
        //                 _choice.SetActive(false);
        //                 _currentNode = 0;       // CHANGE SPACE IN PATH
        //                 _currentPath = _nodes4; // CHANGE PATH
        //                 CHECK_NODE();
        //             }
        //             // MOVE TO PATH 5
        //             else if (arrowDirection == _left) // CHANGE DIR INPUT
        //             {
        //                 _choice.SetActive(false);
        //                 _currentNode = 0;       // CHANGE SPACE IN PATH
        //                 _currentPath = _nodes5; // CHANGE PATH
        //                 CHECK_NODE();
        //             }
        //             // MOVE TO PATH 6
        //             else if (arrowDirection == _up) // CHANGE DIR INPUT
        //             {
        //                 _choice.SetActive(false);
        //                 _currentNode = 0;       // CHANGE SPACE IN PATH
        //                 _currentPath = _nodes6; // CHANGE PATH
        //                 CHECK_NODE();
        //             }
        //         }
        //         // END OF PATH 4
        //         else if (_currentPath == _nodes4)
        //         {
        //             // MOVE TO PATH 7
        //             if (arrowDirection == _left) // CHANGE DIR INPUT
        //             {
        //                 _choice.SetActive(false);
        //                 _currentNode = 0;       // CHANGE SPACE IN PATH
        //                 _currentPath = _nodes7; // CHANGE PATH
        //                 CHECK_NODE();
        //             }
        //             // MOVE TO PATH 8
        //             else if (arrowDirection == _up) // CHANGE DIR INPUT
        //             {
        //                 _choice.SetActive(false);
        //                 _currentNode = 0;       // CHANGE SPACE IN PATH
        //                 _currentPath = _nodes8; // CHANGE PATH
        //                 CHECK_NODE();
        //             }
        //         }
        //         // END OF PATH 9
        //         else if (_currentPath == _nodes9) 
        //         {
        //             // MOVE TO PATH 10
        //             if (arrowDirection == _left)   // CHANGE DIR INPUT
        //             {
        //                 _choice.SetActive(false);
        //                 _currentNode = 0;       // CHANGE SPACE IN PATH
        //                 _currentPath = _nodes10; // CHANGE PATH
        //                 CHECK_NODE();
        //             }
        //             // MOVE TO PATH 11
        //             else if (arrowDirection == _right) // CHANGE DIR INPUT
        //             {
        //                 _choice.SetActive(false);
        //                 _currentNode = 0;       // CHANGE SPACE IN PATH
        //                 _currentPath = _nodes11; // CHANGE PATH
        //                 CHECK_NODE();
        //             }
        //         }
                
        //         isAtFork = false;
        //         _animator.SetBool("IsWalking", true);
        //         _animator.speed = _moveSpeed;
        //     }
        // /* ------------------ "SHOGUN SEAPORT" MAP ----------------------- */
        // /* --------------------------------------------------------------- */



        //     // RESUME SOUND EFFECTS & HIDE ARROW VISUALS
        //     if (!isAtFork)
        //     {
        //         arrowDirection      = 0;
        //         arrowLeft.color     = new Color(1, 0.7f, 0, 0.4f);
        //         arrowRight.color    = new Color(1, 0.7f, 0, 0.4f);
        //         arrowUp.color       = new Color(1, 0.7f, 0, 0.4f);
        //         arrowDown.color     = new Color(1, 0.7f, 0, 0.4f);

        //         arrowLeft.gameObject.SetActive(false);
        //         arrowRight.gameObject.SetActive(false);
        //         arrowUp.gameObject.SetActive(false);
        //         arrowDown.gameObject.SetActive(false);
        //     }
        }
    }

    // IF NEXTNODE IS BLOCKED (BOULDER), THEN GO TO ALTERNATE NODE
    private void PATH_BLOCKED_MOVE_TO_ALTERNATE()
    {
        if (nextNode.IS_BLOCKED()) {
            nextNode = currentNode.nexts[0].alternative.GetComponent<Node>();
        }
    }

    // FLIP CHARACTER BASED ON DIRECTION MOVEMENT
    private void WHERE_TO_FACE()
    {
        if (nextNode != null) {
            // FACE RIGHT
            if (nextNode.transform.position.x > transform.position.x) { 
                character.transform.localScale = new Vector3(-lScale, lScale, lScale); 
            }
            // FACE LEFT (STANDARD)
            else  { 
                character.transform.localScale = new Vector3(lScale, lScale, lScale); 
            }
        }
    }

    private void SHOW_ARROWS() 
    {
        for (int i=0 ; i<currentNode.nexts.Length ; i++) {
            if (currentNode.nexts[i].direction == Node.Directions.left && 
                !currentNode.nexts[i].node.GetComponent<Node>().IS_BLOCKED())  
            { 
                arrowLeft.gameObject.SetActive(true); continue; 
            }
            if (currentNode.nexts[i].direction == Node.Directions.right && 
                !currentNode.nexts[i].node.GetComponent<Node>().IS_BLOCKED()) 
            { 
                arrowRight.gameObject.SetActive(true); continue; 
            }
            if (currentNode.nexts[i].direction == Node.Directions.up && 
                !currentNode.nexts[i].node.GetComponent<Node>().IS_BLOCKED())  
            { 
                arrowUp.gameObject.SetActive(true); continue; 
            }
            if (currentNode.nexts[i].direction == Node.Directions.down && 
                !currentNode.nexts[i].node.GetComponent<Node>().IS_BLOCKED())  
            { 
                arrowDown.gameObject.SetActive(true); continue; 
            }
        }
    }

    // DISPLAY SPACES AWAY
    private void SHOW_NODE_DISTANCE()
    {
        for (int i=0 ; i<currentNode.nexts.Length ; i++) {
            currentNode.nexts[i].node.GetComponent<Node>().DISPLAY_MOVEMENT(1, _movesRemaining);
        }
    }
    
    // SPACE PLAYER LANDS ON WITH NO MOVEMENT REMAINING (RECEIVE GOLD/EFFECT)
    private void SPACE_END()
    {
        isLanded = true;
        _animator.SetBool("IsWalking", false);
        _animator.speed = 1;
        
        if (_movesRemaining == 0 && isPlayerTurn && nMovesLeft.gameObject.activeSelf) {
            StartCoroutine(fadeMovesLeft());
        }

        if (currentNode.IS_GREEN())  { controller.EVENT_ORB_UPDATE(playerID); }
        // LANDED ON BLUE | RED | EVENT (SO FAR)
        if (currentNode.SPACE_LANDED()) {
            int coinsGained = currentNode.COINS_RECEIVED_FROM_SPACE();
            if (currentNode.IS_BLUE())   { controller.blueOrb[playerID]++; }
            if (currentNode.IS_RED())    { controller.RED_ORB_UPDATE(playerID); }
            StartCoroutine( UPDATE_PLAYER_COINS(coinsGained * controller.turnMultiplier) );
        }
        // GAIN SPELL FROM SPACE
        else if (currentNode.IS_SPELL()) {
            StartCoroutine( GAIN_RANDOM_SPELL() );
        }
        
        // todo - EVENT RELATED
        // LANDED ON EVENT (caving in)
        else if (currentNode.IS_BOULDER_EVENT()) {
            if (SceneManager.GetActiveScene().name == "Crystal_Caverns")
            {
                StartCoroutine( PATHFOLLOWER_CAVING_IN() );
            }
        }
        // LANDED ON EVENT (changing boat)
        else if (currentNode.IS_BOAT()) {
            if (SceneManager.GetActiveScene().name == "Shogun_Seaport")
            {
                StartCoroutine( HAPPEN_NEW_BOAT() );
            }
        }
        // LANDED ON EVENT (rotate laser)
        else if (currentNode.IS_ROTATE()) {
            if (SceneManager.GetActiveScene().name == "Plasma_Palace")
            {
                StartCoroutine( PATHFOLLOWER_TURRET_ROTATE() );
            }
        }
        // LANDED ON EVENT (laser speed up countdown)
        else if (currentNode.IS_SPEED_UP()) {
            if (SceneManager.GetActiveScene().name == "Plasma_Palace")
            {
                StartCoroutine( PATHFOLLOWER_TURRET_SPEED_UP() );
            }
        }
       
       
        // LANDED ON ORB TRAP
        else if (currentNode.IS_ORB_TRAP()) {
            Debug.Log("LANDED ON ORB TRAP");
            int onesTrap = currentNode.ORB_TRAP_SPACE_COST(characterName);
            if (onesTrap == 5)
            {
                Debug.Log("-- " + onesTrap);
                StartCoroutine( UPDATE_PLAYER_COINS(onesTrap * controller.turnMultiplier) );
            } 
            else 
            {
                Debug.Log("ORB BEING STOLEN");
                if (!playerBarrier) StartCoroutine( ORB_STOLEN(-1) );
                else { StartCoroutine( UPDATE_PLAYER_COINS(0) ); }
            }
        }
        // LANDED ON COIN TRAP
        else {
            int price = currentNode.TRAP_SPACE_COST(characterName);
            if (price > 0) // OWN TRAP
            {
                StartCoroutine( UPDATE_PLAYER_COINS(price * controller.turnMultiplier) );
            }
            else            // OPPONENT'S TRAP
            {
                if (!playerBarrier) {
                    isPayingSomeone = true;
                    StartCoroutine( UPDATE_PLAYER_COINS(price) );
                }
                else { Debug.Log("PROTECTED"); StartCoroutine( UPDATE_PLAYER_COINS(0) ); }
            }
        }
    }
    

    // todo -------------------------------------------------------- *//


    private void UserPathForkMenu()
    {
        if (isAtFork && !isViewingMap) mapViewmap.gameObject.SetActive(true);
        if (player.GetButtonDown("X"))
        {
            SHOW_NODE_DISTANCE();   // DISPLAY SPACES AWAY

            mapViewmap.gameObject.SetActive(false);
            _cam.orthographicSize = camSizeLarge;
            isViewingMap = true;
            mapMapping.gameObject.SetActive(true);

            arrowLeft.gameObject.SetActive(false);
            arrowRight.gameObject.SetActive(false);
            arrowUp.gameObject.SetActive(false);
            arrowDown.gameObject.SetActive(false);
            nMovesLeft.gameObject.SetActive(false); 
            manager.HIDE_UI();

            if (_movesRemaining > 0) { _mapMoves.text = _movesRemaining + " Moves Left"; }
        }
        else if (player.GetButtonDown("A"))
        {
            mapViewmap.gameObject.SetActive(false);
            CHOOSING_PATH_AT_FORK();
        }
        else if (player.GetButtonDown("Up")    && arrowUp.IsActive())
        {
            arrowDirection      = Node.Directions.up;
            arrowLeft.color     = new Color(1, 0.7f, 0, 0.4f);
            arrowRight.color    = new Color(1, 0.7f, 0, 0.4f);
            arrowUp.color       = new Color(1, 0.7f, 0, 1);
            arrowDown.color     = new Color(1, 0.7f, 0, 0.4f);
        }
        else if (player.GetButtonDown("Down")  && arrowDown.IsActive())
        {
            arrowDirection      = Node.Directions.down;
            arrowLeft.color     = new Color(1, 0.7f, 0, 0.4f);
            arrowRight.color    = new Color(1, 0.7f, 0, 0.4f);
            arrowUp.color       = new Color(1, 0.7f, 0, 0.4f);
            arrowDown.color     = new Color(1, 0.7f, 0, 1);
        }
        else if (player.GetButtonDown("Left")  && arrowLeft.IsActive())
        {
            arrowDirection      = Node.Directions.left;
            arrowLeft.color     = new Color(1, 0.7f, 0, 1);
            arrowRight.color    = new Color(1, 0.7f, 0, 0.4f);
            arrowUp.color       = new Color(1, 0.7f, 0, 0.4f);
            arrowDown.color     = new Color(1, 0.7f, 0, 0.4f);
        }
        else if (player.GetButtonDown("Right") && arrowRight.IsActive())
        {
            arrowDirection      = Node.Directions.right;
            arrowLeft.color     = new Color(1, 0.7f, 0, 0.4f);
            arrowRight.color    = new Color(1, 0.7f, 0, 1);
            arrowUp.color       = new Color(1, 0.7f, 0, 0.4f);
            arrowDown.color     = new Color(1, 0.7f, 0, 0.4f);
        }
    }

    private void UserAtShop()
    {
        // TALKING TO SELLER (INTRO)
        if (shopKeeperUI.activeSelf && !shopSpellUI.activeSelf && !potionShop.confirmObj.activeSelf
            && !playerPurchase.confirmObj.activeSelf && !playerPurchase.discardObj.activeSelf)
        {
            // TAKE A LOOK AT STOCK
            if      (player.GetButtonDown("A")) { 
                if (!shop4) { shopKeeperUI.SetActive(false); shopSpellUI.SetActive(true); } 
                else        { shopKeeperUI.SetActive(false); shopItemUI.SetActive(true); }
            }
            // VIEW MAP
            else if (player.GetButtonDown("X")) { 
                if (!shop4) { shopKeeperUI.SetActive(false); shopSpellUI.SetActive(false); }
                else        { shopKeeperUI.SetActive(false); shopItemUI.SetActive(false); }
                SHOW_NODE_DISTANCE();   // DISPLAY SPACES AWAY
                isViewingMap = true;  _cam.orthographicSize = camSizeLarge;
                mapMapping.gameObject.SetActive(true);
            }
            // ** LEAVE SHOP
            else if (shopKeeperUI.activeSelf && !isViewingMap && player.GetButtonDown("B")) { 
                nMovesLeft.gameObject.SetActive(true); 
                _cam.orthographicSize = camSizeNormal;
                RESET_CAM();
                _mapMoves.text = "";
                manager.UNHIDE_UI();
                shopUI.SetActive(false);
                isAtShop = false;
                shop1 = false; shop2 = false; shop3 = false; shop4 = false;
                _animator.SetBool("IsWalking", true);
                _animator.speed = _moveSpeed;
                if (isAtSpecial)
                {
                    // manager.FADE_TO_BLACK();
                    PLAYER_CAM_OFF(0);
                    StartCoroutine( manager.MANAGER_BOAT_ALTERNATE(this) );
                }
            }
        }
        // BUYING SPELLS ()
        else if (!shopKeeperUI.activeSelf && shopSpellUI.activeSelf && !potionShop.confirmObj.activeSelf
            && !playerPurchase.confirmObj.activeSelf && !playerPurchase.discardObj.activeSelf
            || !shopKeeperUI.activeSelf && shopItemUI.activeSelf && !potionShop.confirmObj.activeSelf
            && !playerPurchase.confirmObj.activeSelf && !playerPurchase.discardObj.activeSelf)
        {
            if (player.GetButtonDown("B"))
            {
                if (!shop4) { shopSpellUI.SetActive(false); shopKeeperUI.SetActive(true); }
                else        { shopItemUI.SetActive(false);  shopKeeperUI.SetActive(true); }
            }
        }
        // CONFIRM TO BUY SPELL
        else if (playerPurchase.confirmObj.activeSelf && !shopKeeperUI.activeSelf && shopSpellUI.activeSelf) 
        {
            if      (player.GetButtonDown("A")) { playerPurchase.OnYes(); }
            else if (player.GetButtonDown("B")) { playerPurchase.OnNo();  }
        }
        // CONFIRM TO BUY ITEM/POTION
        else if (potionShop.confirmObj.activeSelf && !shopKeeperUI.activeSelf && shopItemUI.activeSelf) 
        {
            if      (player.GetButtonDown("A")) { potionShop.OnYes(); }
            else if (player.GetButtonDown("B")) { potionShop.OnNo();  }
        }
    }

    private void UserAtMagicOrb()
    {
        if (player.GetButtonDown("A") && coins >= 20)
        {
            magicOrbUI.SetActive(false);
            _mapMoves.text = "";

            StartCoroutine(UPDATE_PLAYER_COINS(-20));
            StartCoroutine(BUYING_MAGIC_ORB()); // MUSIC
        }
        // VIEW MAP
        else if (player.GetButtonDown("X")) { 
            magicOrbUI.SetActive(false);
            isViewingMap = true;  
            _cam.orthographicSize = camSizeLarge;
            mapMapping.gameObject.SetActive(true);
        }
        else if (player.GetButtonDown("B"))
        {
            isAtMagicOrb = false;
            _mapMoves.text = "";
            nMovesLeft.gameObject.SetActive(true); 
            manager.UNHIDE_UI();
            magicOrbUI.SetActive(false);
        }
    }


    // TEXT SHOWING MOVES REMAINING
    private void UPDATE_MOVEMENT(int movesLeft) { nMovesLeft.text = movesLeft.ToString(); }

    // MOVE PLAYER ASIDE FOR VISUAL INDICATION OF WHO'S ON WHAT SPACE
    private void MOVE_ASIDE()
    {
        if (!gotAsidePos)   // CALL ONCE
        {
            _timer = 0;
            // // float scaleFactor = 1.3f;
            // // float trig1 = 0.5f * scaleFactor;
            // // float trig2 = 0.866f * scaleFactor;
            float playerX = transform.position.x;
            float playerY = transform.position.y;
            float sin45 = 0.707f * 1.5f;
            asidePos1 = new Vector3(playerX - 1.5f,     playerY + 0);
            asidePos2 = new Vector3(playerX - sin45,    playerY + sin45);
            asidePos3 = new Vector3(playerX + 0,        playerY + 1.5f);
            asidePos4 = new Vector3(playerX + sin45,    playerY + sin45);
            asidePos5 = new Vector3(playerX + 1.5f,     playerY - 0);
            asidePos6 = new Vector3(playerX + sin45,    playerY - sin45 - 0.75f);
            asidePos7 = new Vector3(playerX - 0,        playerY - 1.5f - 0.75f);
            asidePos8 = new Vector3(playerX - sin45,    playerY - sin45 - 0.75f);
            gotAsidePos = true;
        }

        _moveSpeed = 7.5f;
        _timer += _moveSpeed * Time.deltaTime;
        // if (transform.localScale.x > 0.25f) {
        //     this.transform.localScale -= new Vector3(0.025f, 0.025f, 0.025f);    // STRINK
        // }

        // MOVE TO ASIDE POSITION
        switch (name)
        {
            case "Player_1":
                if (this.transform.position != asidePos1) {
                    this.transform.position = Vector3.Lerp(this.transform.position, asidePos1, _timer); }
                break;
            case "Player_2":
                if (this.transform.position != asidePos2) {
                    this.transform.position = Vector3.Lerp(this.transform.position, asidePos2, _timer); }
                break;
            case "Player_3":
                if (this.transform.position != asidePos3) {
                    this.transform.position = Vector3.Lerp(this.transform.position, asidePos3, _timer); }
                break;
            case "Player_4":
                if (this.transform.position != asidePos4) {
                    this.transform.position = Vector3.Lerp(this.transform.position, asidePos4, _timer); }
                break;
            case "Player_5":
                if (this.transform.position != asidePos5) {
                    this.transform.position = Vector3.Lerp(this.transform.position, asidePos5, _timer); }
                break;
            case "Player_6":
                if (this.transform.position != asidePos6) {
                    this.transform.position = Vector3.Lerp(this.transform.position, asidePos6, _timer); }
                break;
            case "Player_7":
                if (this.transform.position != asidePos7) {
                    this.transform.position = Vector3.Lerp(this.transform.position, asidePos7, _timer); }
                break;
            case "Player_8":
                if (this.transform.position != asidePos8) {
                    this.transform.position = Vector3.Lerp(this.transform.position, asidePos8, _timer); }
                break;
        }

        // END TURN
        switch (name)
        {
            case "Player_1":
                if (this.transform.position == asidePos1) { TURN_FINISHED(); }     break;
            case "Player_2":
                if (this.transform.position == asidePos2) { TURN_FINISHED(); }     break;
            case "Player_3":
                if (this.transform.position == asidePos3) { TURN_FINISHED(); }     break;
            case "Player_4":
                if (this.transform.position == asidePos4) { TURN_FINISHED(); }     break;
            case "Player_5":
                if (this.transform.position == asidePos5) { TURN_FINISHED(); }     break;
            case "Player_6":
                if (this.transform.position == asidePos6) { TURN_FINISHED(); }     break;
            case "Player_7":
                if (this.transform.position == asidePos7) { TURN_FINISHED(); }     break;
            case "Player_8":
                if (this.transform.position == asidePos8) { TURN_FINISHED(); }     break;
        }
    }



    /* --------------------------------------------- */
    /* ------------ PLAYER'S TURN SETUP ------------ */


    // CAMERA FOCUSES ON PLAYER, WAIT BEFORE STARTING TURN
    public IEnumerator YOUR_TURN()
    {
        // CAMERA FOCUSES ON PLAYER
        _cam.gameObject.SetActive(true);

        // MOVE BACK (UNASIDE)
        if (controller.hasStarted) { transform.position = data[0].pos; }
        manager.CHECK_RANKINGS();
        manager.UNHIDE_UI();
        // manager.FADE_FROM_BLACK();
        
        // LOSE BARRIER
        if (playerBarrier) {
            playerBarrier = false;
            controller.buffDatas[playerID][0].barrier = false;
            barrier.SetActive(false);
            hurtBox.SetActive(true);
        }

        yield return new WaitForSeconds(1);

        // PLAYER CHOICES
        startUi.gameObject.SetActive(true);
        foreach (Sprite characterSprite in starts)
        {
            if (characterSprite.name.Contains(characterName))
            {
                startUi.sprite = characterSprite; break;
            }
        }
        isPlayerTurn = true;           // THIS PLAYER'S TURN

        // RESTORE 1 MP (EVERY ODD TURNS)     ( COMPETITIVE )
        if (mpBar.value < 5 && controller.turnNumber % 2 == 1 && !controller.isCasual) 
        { 
            mpBar.value += 1; 
            var go = Instantiate(floatingManaTextPrefab, transform.position + new Vector3(0,3), transform.rotation);
            go.GetComponent<TextMeshPro>().text    = "+1";
        }
        // RESTORE 1 MP (EVERY TURN)        ( CASUAL )
        else if (mpBar.value < 5 && controller.isCasual) 
        { 
            mpBar.value += 1; 
            var go = Instantiate(floatingManaTextPrefab, transform.position + new Vector3(0,3), transform.rotation);
            go.GetComponent<TextMeshPro>().text    = "+1";
        }
        mpLeft.text = mpBar.value + "/" + mpBar.maxValue;

        UPDATE_MOVEMENT(_movesRemaining);
    }
    
    // ******** STORE INFORMATION ABOUT PLAYER TO GAME_CONTROLLER 
    public void UPDATE_INFORMATION(bool updateSpells)
    {
        // STORE DATA ON THE PATH THE PLAYER IS CURRENTLY ON
        string newPath = currentNode.transform.parent.name + "/" + currentNode.name;

        // SET ALL VALUES OF PLAYER
        controller.SET_PLAYER_DATA(playerID, newPath, 0, currentNode.transform.position, transform.position,
            coins, orbs, (int) mpBar.value);

        // STORE DATA ON THE CURRENT PLAYER'S SPELLS
        if (updateSpells) { controller.SET_SPELLS (playerID, spells); }
        else { manager.CHECK_RANKINGS(); }

        // STATUS EFFECTS
        controller.SET_PLAYER_BUFFS(playerID, playerSlowed, playerBarrier, playerMove15, playerRange2, playerExtraBuy);

        controller.RICH_ORB_UPDATE(playerID);
    }

    // CAMERA CHANGES, WAIT BEFORE ENDING TURN
    private void TURN_FINISHED()
    {
        manager.CHECK_RANKINGS();
        isAside = true;
        _animator.SetBool("IsWalking", false);
        isPlayerTurn = false;
        isReadyToMove = false;

        nMovesLeft.gameObject.SetActive(false);
        buttonMove.gameObject.SetActive(false);
        buttonSpells.gameObject.SetActive(false);
        buttonMap.gameObject.SetActive(false);

        if (playerSlowed) {
            playerSlowed = false;
            foreach (Transform child in character.transform)  
            {  
                if (child.name != "Shadow") {
                    if (child.TryGetComponent(out SpriteRenderer cs)) { cs.color = new Color(1,1,1); }
                }
                foreach (Transform grandChild in child)
                {
                    if (grandChild.name != "Shadow") {
                        if (grandChild.TryGetComponent(out SpriteRenderer gs)) { gs.color = new Color(1,1,1); }
                    }
                }
            }
        }
        if (stealMode)  {stealMode = false; stealingAura.SetActive(false);}
        if (!playerBarrier) { hurtBox.SetActive(true); }
        StartCoroutine( PLAYER_CAM_OFF(transitionTime) );
        if (!isPayingSomeone) { manager.StartCoroutine(manager.INCREMENT_TURN()); }
    }

    public void PLAYER_CAM_ON()
    {
        _cam.gameObject.SetActive(true);    // CAM TURNS ON
    }
    public IEnumerator PLAYER_CAM_OFF(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        _cam.gameObject.SetActive(false);   // CAM TURNS OFF
    }

    // ENABLE COLLIDER = SOUND EFFECT FOR WALKING OVER NODE
    IEnumerator WALK_OVER_SOUND_ON()
    {
        yield return new WaitForSeconds(0.05f);
        _collider.enabled = true;
    }

    public void HIDE_DATA_CANVAS() 
    {
        layout.SetActive(false);
    }

    public void UNHIDE_DATA_CANVAS() 
    {
        layout.SetActive(true);
    }


    /* --------------------------------------------------------- */
    /* ------------ DURING THE START OF PLAYER TURN ------------ */


    // AFTER ROLLING DICE, DELAY THEN MOVE PLAYER
    IEnumerator DICE_ROLLED()
    {
        if (!pseudoMove) {
            // int nMoves = (int)(moveBar.value + 10) / 10;
            int nMoves;
            if      (playerSlowed) { nMoves = (int)(radialBar.value) / ((int)(radialBar.maxValue) / 5); }
            else if (playerMove15) { nMoves = (int)(radialBar.value) / ((int)(radialBar.maxValue) / 15); }
            else                   { nMoves = (int)(radialBar.value) / ((int)(radialBar.maxValue) / 10); }
            _movesRemaining = (nMoves+1);
            controller.SLOW_ORB_UPDATE(playerID, _movesRemaining);
            radialBar.GetComponent<Animator>().speed = 0;
        }
        nMovesLeft.text = _movesRemaining.ToString();
        grimoireUI.gameObject.SetActive(false);

        if (!pseudoMove) yield return new WaitForSeconds(1.6f);
        // moveBar.gameObject.SetActive(false);
        radialBar.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.1f);
        _animator.SetBool("IsWalking", true);
        _animator.speed = _moveSpeed;
        isReadyToMove = true;

        WHERE_TO_FACE();
    }

    private void RESET_PLAYER_UI()
    {
        RectTransform rt = layout.transform.GetComponent<RectTransform>();

    // WHAT IS PLAYER ORDER IN TURN ROTATION
        int xth = 0;
        for (int i=0 ; i<controller.playerOrder.Length ; i++)
        {
            if (playerID == controller.playerOrder[i])
            {
                xth = i; break;
            }
        }

        rt.localPosition -= new Vector3(0, (55*xth), 0);
    }


    public IEnumerator STEAL_COINS(int n, PathFollower p)
    {
        n = Mathf.Abs(n);
        
        int tempGold = coins;
        var go = Instantiate(floatingCoinTextPrefab, transform.position + new Vector3(0,3), transform.rotation);

        // NOT ENOUGH COINS TO LOSE
        if (coins < n) {
            go.GetComponent<TextMeshPro>().text    = "-" + coins.ToString();
            Color top = new Color(1, 0.7f, 0);
            Color bot = new Color(1, 0.35f,0);
            go.GetComponent<TextMeshPro>().colorGradient  = new VertexGradient(top, top, bot, bot);
            StartCoroutine( p.UPDATE_PLAYER_COINS(coins) );
        }
        else {
            go.GetComponent<TextMeshPro>().text    = "-" +  n.ToString();
            Color top = new Color(1, 0.15f, 0.2f);
            Color bot = new Color(0.8f, 0, 0.05f);
            go.GetComponent<TextMeshPro>().colorGradient  = new VertexGradient(top, top, bot, bot);
            StartCoroutine( p.UPDATE_PLAYER_COINS(n) );
        }
        // LOSE COINS
        for (int i = 0; i < n; i++)
        {
            if (coins <= 0) { break; }  // NOT MORE GOLD TO LOSE

            if (n<11) { yield return new WaitForSeconds(0.1f); }
            else { yield return new WaitForSeconds(0.02f); }
            coinLoss.Play();
            coins--;
        }
    }

    // GAIN OR LOSE COINS
    public IEnumerator UPDATE_PLAYER_COINS(int n)
    {
        // FADE VISUAL NUMBER OF MOVES LEFT
        if (_movesRemaining == 0 && isPlayerTurn && nMovesLeft.gameObject.activeSelf) {
            StartCoroutine(fadeMovesLeft());
        }
        if (isAtSpecial && orbStolen.isPlaying) { orbStolen.Stop(); }
        int tempGold = coins;
        var go = Instantiate(floatingCoinTextPrefab, transform.position + new Vector3(0,3), transform.rotation);

        // GAIN COINS
        if (n > 0)
        {
            go.GetComponent<TextMeshPro>().text    = "+" + n.ToString();
            Color top = new Color(0, 0.8f, 1);
            Color bot = new Color(0, 0.3f, 1);
            go.GetComponent<TextMeshPro>().colorGradient  = new VertexGradient(top, top, bot, bot);
            // GAIN COINS
            for (int i = 0; i < n; i++)
            {
                if (coins >= 999) { break; }

                if (n<11) { yield return new WaitForSeconds(0.1f); }
                else if (n >= 11 && n<101) { yield return new WaitForSeconds(0.02f); }
                coinPickup.Play();
                coins++;
            }
        }
        else if (n == 0) { Destroy(go); }
        // LOSE COINS
        else
        {
            // NOT ENOUGH COINS TO LOSE
            if (coins < -n ) {
                go.GetComponent<TextMeshPro>().text    = "-" + coins.ToString();
                Color top = new Color(1, 0.7f, 0);
                Color bot = new Color(1, 0.35f,0);
                go.GetComponent<TextMeshPro>().colorGradient  = new VertexGradient(top, top, bot, bot);
            }
            else {
                go.GetComponent<TextMeshPro>().text    = n.ToString();
                Color top = new Color(1, 0.15f, 0.2f);
                Color bot = new Color(0.8f, 0, 0.05f);
                go.GetComponent<TextMeshPro>().colorGradient  = new VertexGradient(top, top, bot, bot);
            }
            // LOSE COINS
            for (int i = 0; i > n; i--)
            {
                if (coins <= 0) { break; }

                if (n<11) { yield return new WaitForSeconds(0.1f); }
                else { yield return new WaitForSeconds(0.02f); }
                coinLoss.Play();
                coins--;
            }
        }


        // PAY OPPONENT AFTER LOSING COINS
        if (isPayingSomeone && tempGold != 0) {
            if (n < 0) { n = Mathf.Abs(n); } // make positive

            yield return new WaitForSeconds(1);
            StartCoroutine( PLAYER_CAM_OFF(0) );
            string toWhom = currentNode.WHOS_TRAP();
            if ( tempGold > n ) { // MORE THAN ENOUGH TO PAY BACK
                manager.PAYING_SOMEONE(toWhom, n);
            }
            else {              // PAY WHAT YOU HAVE
                manager.PAYING_SOMEONE(toWhom, tempGold);
            }
        }
        // MOVE PLAYER TO THE SIDE
        if (_movesRemaining == 0 && isPlayerTurn) {
            yield return new WaitForSeconds(0.5f);
            readyToMovedAside = true;
        }
        // OTHER PLAYER RECIEVING COINS INCREMENTS TURN ORDER
        if (beingPayed) { 
            beingPayed = false; StartCoroutine( PLAYER_CAM_OFF(transitionTime) ); 
            manager.StartCoroutine(manager.INCREMENT_TURN()); 
        }
        
        /* shogun seaport */
        if (isAtSpecial && nPrompt > 0)
        {
            if (orbStolen.isPlaying) {
                while (orbStolen.volume > 0)
                {
                    yield return new WaitForEndOfFrame();
                    orbStolen.volume -= 0.01f;
                }
            }
            PLAYER_CAM_OFF(0);
            StartCoroutine( manager.MANAGER_BOAT_ALTERNATE(this) );
        }
        UPDATE_INFORMATION(false);
    }

    // LOSE COINS WITHOUT FROM EFFECTS OR BOARD
    public IEnumerator LOSE_COINS(int n)
    {
        int tempGold = coins;
        var go = Instantiate(floatingCoinTextPrefab, transform.position + new Vector3(0,3), transform.rotation);
        // NOT ENOUGH COINS TO LOSE
        if (coins < Mathf.Abs(n) ) {
            go.GetComponent<TextMeshPro>().text    = "-" + coins.ToString();
            Color top = new Color(1, 0.7f, 0);
            Color bot = new Color(1, 0.35f,0);
            go.GetComponent<TextMeshPro>().colorGradient  = new VertexGradient(top, top, bot, bot);
        }
        else {
            go.GetComponent<TextMeshPro>().text    = n.ToString();
            Color top = new Color(1, 0.15f, 0.2f);
            Color bot = new Color(0.8f, 0, 0.05f);
            go.GetComponent<TextMeshPro>().colorGradient  = new VertexGradient(top, top, bot, bot);
        }
        // LOSE COINS
        for (int i = 0; i > n; i--)
        {
            if (coins <= 0) { break; }

            if (n<11) { yield return new WaitForSeconds(0.1f); }
            else { yield return new WaitForSeconds(0.02f); }
            coinLoss.Play();
            coins--;
        }
        manager.CHECK_RANKINGS();
    }
    public IEnumerator LOSE_ALL_COINS()
    {
        if (!losingAll) {losingAll = true;}
        else yield break;

        int n = -coins;
        int tempGold = coins;
        var go = Instantiate(floatingCoinTextPrefab, transform.position + new Vector3(0,3), transform.rotation);
        // NOT ENOUGH COINS TO LOSE
        if (coins < Mathf.Abs(n) ) {
            go.GetComponent<TextMeshPro>().text    = "-" + coins.ToString();
            Color top = new Color(1, 0.7f, 0);
            Color bot = new Color(1, 0.35f,0);
            go.GetComponent<TextMeshPro>().colorGradient  = new VertexGradient(top, top, bot, bot);
        }
        else {
            go.GetComponent<TextMeshPro>().text    = n.ToString();
            Color top = new Color(1, 0.15f, 0.2f);
            Color bot = new Color(0.8f, 0, 0.05f);
            go.GetComponent<TextMeshPro>().colorGradient  = new VertexGradient(top, top, bot, bot);
        }
        // LOSE COINS
        for (int i = 0; i > n; i--)
        {
            if (coins <= 0) { break; }
            if (n < 125) yield return new WaitForSeconds(0.01f);
            coinLoss.Play();
            coins--;
        }
        UPDATE_INFORMATION(false);
        manager.CHECK_RANKINGS();
    }
    public void LOSE_MP(int n)
    {
        n = Mathf.Abs(n);
        var go = Instantiate(floatingManaTextPrefab, transform.position + new Vector3(0,3), transform.rotation);
        if (mpBar.value >= n) 
        { 
            mpBar.value -= n; 
            go.GetComponent<TextMeshPro>().text    = "-" + n;
            Color top = new Color(1, 0.15f, 0.2f);
            Color bot = new Color(0.8f, 0, 0.05f);
            go.GetComponent<TextMeshPro>().colorGradient  = new VertexGradient(top, top, bot, bot);
        }
        // NOT ENOUGH MP TO LOSE
        else {
            go.GetComponent<TextMeshPro>().text    = "-" + mpBar.value;
            mpBar.value = 0;
            Color top = new Color(1, 0.7f, 0);
            Color bot = new Color(1, 0.35f,0);
            go.GetComponent<TextMeshPro>().colorGradient  = new VertexGradient(top, top, bot, bot);
        }
        mpLeft.text = mpBar.value + "/" + mpBar.maxValue;
    }
    public IEnumerator LOSE_SPELL(int n)
    {
        n = Mathf.Abs(n);
        
        for ( int i=0 ; i<n ; i++ )
        {
            if (spells.Count > 0) {
                var go = Instantiate(floatingSpellTextPrefab, transform.position + new Vector3(0,3), transform.rotation);
                SpriteRenderer spellImg = go.GetComponentInChildren<SpriteRenderer>();
                int rng = Random.Range(0,spells.Count);
                switch (spells[rng].spellName)
                {
                    case "Spell_Trap_10" :          spellImg.sprite = spellTrap10;   break;
                    case "Spell_Trap_20" :          spellImg.sprite = spellTrap20;   break;
                    case "Spell_Trap_Orb" :         spellImg.sprite = spellTrapOrb;  break;
                    
                    case "Spell_Effect_10" :        spellImg.sprite = spellEffect10; break;
                    case "Spell_Effect_Mana_3" :    spellImg.sprite = spellEffectMana3; break;
                    case "Spell_Effect_Spell_1" :   spellImg.sprite = spellEffectSpell1; break;
                    case "Spell_Effect_Slow_1" :    spellImg.sprite = spellEffectSlow1; break;
                    case "Spell_Effect_Swap" :      spellImg.sprite = spellEffectSwap; break;
                    
                    case "Spell_Move_Dash_5" :      spellImg.sprite = spellMoveDash5; break;
                    case "Spell_Move_Dash_8" :      spellImg.sprite = spellMoveDash8; break;
                    case "Spell_Move_Slow" :        spellImg.sprite = spellMoveSlow; break;
                    case "Spell_Move_Slowgo" :      spellImg.sprite = spellMoveSlowgo; break;
                    case "Spell_Move_Steal" :       spellImg.sprite = spellMoveSteal; break;
                    case "Spell_Move_Barrier" :     spellImg.sprite = spellMoveBarrier; break;
                    case "Spell_Move_Orb" :         spellImg.sprite = spellMoveOrb; break;
                    default :      break;
                }
                go.GetComponent<TextMeshPro>().text = "-1";
                Color top = new Color(1, 0.15f, 0.2f);
                Color bot = new Color(0.8f, 0, 0.05f);
                go.GetComponent<TextMeshPro>().colorGradient  = new VertexGradient(top, top, bot, bot);
                spells.RemoveAt( rng );
                yield return new WaitForSeconds(0.5f);
            }
        } 

        UPDATE_SPELLS_UI();
    }

    public IEnumerator BOUGHT_AN_ORB(int n)
    {
        var eff = Instantiate(confettiPrefab, transform.position, transform.rotation);
        Destroy(eff, 1);
        var go = Instantiate(floatingOrbTextPrefab, transform.position + new Vector3(0,3), transform.rotation);
        if (!isAtShop) controller.MagicOrbBoughtController();
        if (n >= 0)
        {
            go.GetComponent<TextMeshPro>().text    = "+" + n.ToString();
            Color top = new Color(0, 0.8f, 1);
            Color bot = new Color(0, 0.3f, 1);
            go.GetComponent<TextMeshPro>().colorGradient  = new VertexGradient(top, top, bot, bot);
            // GAIN ORBS
            for (int i = 0; i < n; i++)
            {
                if (n<11) { yield return new WaitForSeconds(0.1f); }
                else { yield return new WaitForSeconds(0.02f); }
                orbPickup.Play();
                orbs++;
            }
        }
        else
        {
            go.GetComponent<TextMeshPro>().text    = n.ToString();
            Color top = new Color(1, 0.15f, 0.2f);
            Color bot = new Color(0.8f, 0, 0.05f);
            go.GetComponent<TextMeshPro>().colorGradient  = new VertexGradient(top, top, bot, bot);
            // LOSE ORBS
            for (int i = 0; i > n; i--)
            {
                if (n<11) { yield return new WaitForSeconds(0.1f); }
                else { yield return new WaitForSeconds(0.02f); }
                coinLoss.Play();    // CHANGE
                orbs--;
            }
        }
        manager.CHECK_RANKINGS();

        yield return new WaitForSeconds(1.5f);
        AudioSource bgMusic = GameObject.Find("BACKGROUND_MUSIC").GetComponent<AudioSource>();
        bgMusic.volume = bgMusicVol;
        controller.CHOOSE_MAGIC_ORB_SPACE(name);
            
        if (isAtMagicOrb) { nMovesLeft.gameObject.SetActive(true); }
        manager.UNHIDE_UI();
        isAtMagicOrb = false;
        _animator.speed = _moveSpeed;
        _animator.SetBool("IsWalking", true);
        hasPurchased = false;
    }
    

    public IEnumerator ORB_STOLEN(int n)
    {
        bool haveOrb = false;
        if (orbs > 0) {haveOrb = true;}
        var go = Instantiate(floatingOrbTextPrefab, transform.position + new Vector3(0,3), transform.rotation);
            
        // NO ORBS TO LOSE
        if (!haveOrb) {
            go.GetComponent<TextMeshPro>().text    = "-0";
            Color top = new Color(1, 0.7f, 0);
            Color bot = new Color(1, 0.35f,0);
            go.GetComponent<TextMeshPro>().colorGradient  = new VertexGradient(top, top, bot, bot);
            if (_movesRemaining == 0 && isPlayerTurn) {
                yield return new WaitForSeconds(0.5f);
                readyToMovedAside = true;
            }
        }
        else {
            go.GetComponent<TextMeshPro>().text    = n.ToString();
            Color top = new Color(1, 0.15f, 0.2f);
            Color bot = new Color(0.8f, 0, 0.05f);
            go.GetComponent<TextMeshPro>().colorGradient  = new VertexGradient(top, top, bot, bot);
            // LOSE ORB(S)
            for (int i = 0; i > n; i--)
            {
                if (n<11) { yield return new WaitForSeconds(0.1f); }
                else { yield return new WaitForSeconds(0.02f); }
                orbLoss.Play();    
                orbs--;
            }
            // PAY OPPONENT AFTER LOSING ORB(S)
            if (n < 0) { n = Mathf.Abs(n); } // make positive

            yield return new WaitForSeconds(1);
            StartCoroutine( PLAYER_CAM_OFF(0) );
            string toWhom = currentNode.WHOS_TRAP();
            manager.STEALING_ORB(toWhom, n);
        }
        manager.CHECK_RANKINGS();
    }
    
    public IEnumerator ORB_GAINED(int n)
    {
        var go = Instantiate(floatingOrbTextPrefab, transform.position + new Vector3(0,3), transform.rotation);
            
        go.GetComponent<TextMeshPro>().text    = "+" + n.ToString();
        Color top = new Color(0, 0.8f, 1);
        Color bot = new Color(0, 0.3f, 1);
        go.GetComponent<TextMeshPro>().colorGradient  = new VertexGradient(top, top, bot, bot);
        // GAIN ORB(S)
        for (int i = 0; i < n; i++)
        {
            if (n<11) { yield return new WaitForSeconds(0.1f); }
            else { yield return new WaitForSeconds(0.02f); }
            orbPickup.Play();    
            orbs++;
        }

        if (beingPayed) { 
            yield return new WaitForSeconds(1);
            beingPayed = false; StartCoroutine( PLAYER_CAM_OFF(0.5f) ); 
            manager.StartCoroutine(manager.INCREMENT_TURN()); 
        }
    }
    
    private IEnumerator ORB_LOST_FROM_BOAT(int n)
    {
        if (orbStolen.isPlaying) orbStolen.Stop();

        n = Mathf.Abs(n);
        var go = Instantiate(floatingOrbTextPrefab, transform.position + new Vector3(0,3), transform.rotation);
            
        go.GetComponent<TextMeshPro>().text    = "-" + n.ToString();
        Color top = new Color(1, 0.15f, 0.2f);
        Color bot = new Color(0.8f, 0, 0.05f);
        go.GetComponent<TextMeshPro>().colorGradient  = new VertexGradient(top, top, bot, bot);
        // LOSE ORB(S)
        for (int i = 0; i < n; i++)
        {
            if (n<11) { yield return new WaitForSeconds(0.1f); }
            else { yield return new WaitForSeconds(0.02f); }
            orbLoss.Play();    
            orbs--;
        }
        
        if (isAtSpecial && nPrompt > 0)
        {
            PLAYER_CAM_OFF(0);
            StartCoroutine( manager.MANAGER_BOAT_ALTERNATE(this) );
        }
        manager.CHECK_RANKINGS();
    }

    // DESTROY THE VISUAL REPRESENTATION OF MOVES LEFT
    private IEnumerator fadeMovesLeft()
    {
        yield return new WaitForSeconds(0.5f);
        nMovesLeft.gameObject.SetActive(false);
    }

    /* ------------------------------------------------------------ */
    /* ----------------------- BUYING STUFF ----------------------- */

    // DOUBLE PURCHASES IN LAST 5 TURNS
    public void RESET_PURCHASES() 
    { 
        if (shop4)  { nPurchaseLeft = controller.turnMultiplier; }
        else        { nPurchaseLeft = maxNPurchases * controller.turnMultiplier;  }
    }

    public void SHOP_ORB_UPDATE(int amount)
    {
        controller.SHOP_ORB_UPDATE(playerID, amount);
    }

    public IEnumerator BUYING_SPELL(int cost, string newSpellName, int newSpellCost, string newSpellKind)
    {
        coins -= cost;
        nPurchaseLeft--;
        purchaseLeftText.text = "Purchase Left: " + nPurchaseLeft;
        if (spells == null) { spells = new List<SpellType>(); }
        spells.Add( new SpellType(newSpellName, newSpellCost, newSpellKind) );
        UPDATE_SPELLS_UI();

        // LOSE COINS
        for (int i = 0; i < cost; i++)
        {
            if (cost<11) { yield return new WaitForSeconds(0.1f); }
            else { yield return new WaitForSeconds(0.02f); }
            coinLoss.Play();
        }
        manager.CHECK_RANKINGS();
    }
    public IEnumerator BUYING_ITEM(int cost, bool isMagicOrb)
    {
        // coins -= cost;
        isPlayerTurn = false;
        shopItemUIcontent.SetActive(false);
        shopItemUIdesc.SetActive(false);
        nPurchaseLeft--;
        purchaseLeftText2.text = "Purchase Left: " + nPurchaseLeft;


        // LOSE COINS
        for (int i = 0; i < cost; i++)
        {
            yield return new WaitForSeconds(0.02f);
            coins--;
            coinLoss.Play();
        }
        if (isMagicOrb)
        {
            StartCoroutine( BUYING_MAGIC_ORB_FROM_SHOP() );
        }
        if (!isMagicOrb) {
            powerup.SetActive(true); powerupSound.Play();
        }
        yield return new WaitForSeconds(3.9f);
        if (!isMagicOrb) {
            while (powerupSound.volume > 0)
            {
                yield return new WaitForEndOfFrame();
                powerupSound.volume -= 0.01f;
            }
            powerup.SetActive(false); powerupSound.Stop();
            shopItemUIcontent.SetActive(true);
            shopItemUIdesc.SetActive(true);
            isPlayerTurn = true;
        }
        manager.CHECK_RANKINGS();
    }

    public void PURCHASES_LEFT()
    {
        purchaseLeftText.text = "Purchase Left: " + nPurchaseLeft;
        purchaseLeftText2.text = "Purchase Left: " + nPurchaseLeft;
    }

    public void FINISHED_SHOPPING()
    {
        isAtShop = false;
        shopUI.SetActive(false);
        _mapMoves.text = "";

        nMovesLeft.gameObject.SetActive(true); 
        manager.UNHIDE_UI();
    }

    private IEnumerator BUYING_MAGIC_ORB()
    {
        // _animator.SetBool("IsWalking", false);
        _animator.speed = 1;
        isPlayerTurn = false;

        yield return new WaitForSeconds(2.5f);  // PAYING MONEY TIME
        AudioSource bgMusic = GameObject.Find("BACKGROUND_MUSIC").GetComponent<AudioSource>();
        bgMusicVol = bgMusic.volume;
        bgMusic.volume = 0;
        gotAnOrb.Play();

        yield return new WaitForSeconds(4);
        StartCoroutine( BOUGHT_AN_ORB(1) );
        
    }

    public IEnumerator BUYING_MAGIC_ORB_FROM_SHOP()
    {
        // _animator.SetBool("IsWalking", false);
        _animator.speed = 1;
        isPlayerTurn = false;

        yield return new WaitForEndOfFrame();
        AudioSource bgMusic = GameObject.Find("BACKGROUND_MUSIC").GetComponent<AudioSource>();
        bgMusicVol = bgMusic.volume;
        bgMusic.volume = 0;
        gotAnOrb.Play();

        yield return new WaitForSeconds(4);
        var eff = Instantiate(confettiPrefab, transform.position, transform.rotation);
        Destroy(eff, 1);
        var go = Instantiate(floatingOrbTextPrefab, transform.position + new Vector3(0,3), transform.rotation);
        int n = 1;
        if (n >= 0)
        {
            go.GetComponent<TextMeshPro>().text    = "+" + n.ToString();
            Color top = new Color(0, 0.8f, 1);
            Color bot = new Color(0, 0.3f, 1);
            go.GetComponent<TextMeshPro>().colorGradient  = new VertexGradient(top, top, bot, bot);
            // GAIN ORBS
            for (int i = 0; i < n; i++)
            {
                if (n<11) { yield return new WaitForSeconds(0.1f); }
                else { yield return new WaitForSeconds(0.02f); }
                orbPickup.Play();
                orbs++;
            }
        }
        manager.CHECK_RANKINGS();

        yield return new WaitForSeconds(1.5f);
        if (shop4) { 
            shopItemUIcontent.SetActive(true);
            shopItemUIdesc.SetActive(true); 
        }
        isPlayerTurn = true;
        bgMusic.volume = bgMusicVol;

        // gold boat
        if (isAtSpecial && nPrompt > 0)
        {
            PLAYER_CAM_OFF(0);
            StartCoroutine( manager.MANAGER_BOAT_ALTERNATE(this) );
        }
    }

    public void RESUME_PLAYER_TURN()
    {
        isPlayerTurn = true;
        if (!diceRolled)
        {
            buttonMove.gameObject.SetActive(true);
            buttonSpells.gameObject.SetActive(true);
            buttonMap.gameObject.SetActive(true);
            _cam.orthographicSize = camSizeNormal;
        }
    }


    /* ---------------------------------------------------------------- */
    /* ------------------------ SPELLS RELATED ------------------------ */

    private IEnumerator GAIN_RANDOM_SPELL()
    {
        // GAIN SPELL 
        if (spells.Count < 3)
        {
            spellPickup.Play();
            yield return new WaitForSeconds(0.1f);

            int rng = Random.Range(0,controller.randomSpells.Count);
            if (spells == null) { spells = new List<SpellType>(); }
            spells.Add( controller.randomSpells[rng] );
            UPDATE_SPELLS_UI();

            SPELL_IMG_INSTANTIATE(controller.randomSpells[rng].spellName, true);

            // MOVE PLAYER TO THE SIDE
            if (_movesRemaining == 0 && isPlayerTurn) {
                yield return new WaitForSeconds(0.5f);
                readyToMovedAside = true;
            }
            UPDATE_INFORMATION(true);
        }
        // GAIN SPELL, FULL INVENTORY MUST DISCARD A SPELL
        else {
            spellPickup.Play();
            yield return new WaitForSeconds(0.1f);

            discardSpellUI.SetActive(true);
            spellDescUI.SetActive(true);
            manager.HIDE_UI();
            for (int i=0 ; i<spells.Count ; i++) { discardSpellImg[i].sprite = spellSlots[i].sprite; }
            int rng = Random.Range(0,controller.randomSpells.Count);
            spellToGain = rng; fromGoodSpell = false;

            SPELL_IMG_INSTANTIATE(controller.randomSpells[rng].spellName, true);
            
        }
    }
    public IEnumerator GAIN_FREE_SPELL()
    {
        // CASUAL (RANDOM SPELL)
        if (controller.isCasual)
        {
            // GAIN SPELL 
            if (spells.Count < 3)
            {
                spellPickup.Play();
                yield return new WaitForSeconds(0.1f);
                if (spells == null) { spells = new List<SpellType>(); }

                int rank = controller.MY_RANKING(playerID);
                float loserTier = ( (float) controller.nPlayers)/2f;
                Debug.Log("--- player_"+(playerID+1)+" rank is "+rank);
                int rng; 
                // LOSING = GET BETTER SPELLS
                if (rank >= loserTier && controller.turnNumber >= 3)
                {
                    Debug.Log("GAINED A GOOD SPELL");
                    rng = Random.Range(0,controller.randomGoodSpells.Count);
                    spells.Add( controller.randomGoodSpells[rng] );
                    SPELL_IMG_INSTANTIATE(controller.randomGoodSpells[rng].spellName, true);
                }
            // ************************************************************************
                // WINNING = GET AVG SPELLS
                else 
                {
                    rng = Random.Range(0,controller.randomSpells.Count);
                    spells.Add( controller.randomSpells[rng] );
                    SPELL_IMG_INSTANTIATE(controller.randomSpells[rng].spellName, true);
                }

                UPDATE_SPELLS_UI();

                yield return new WaitForSeconds(0.5f);
                isAtFree = false;
                UPDATE_INFORMATION(true);
            }
        // **************************************************************************************************
            // GAIN SPELL, FULL INVENTORY MUST DISCARD A SPELL
            else 
            {
                spellPickup.Play();
                yield return new WaitForSeconds(0.1f);

                discardSpellUI.SetActive(true);
                spellDescUI.SetActive(true);
                manager.HIDE_UI();
                for (int i=0 ; i<spells.Count ; i++) { 
                    discardSpellImg[i].sprite = spellSlots[i].sprite; 
                    discardSpellImg[i].name = spellSlots[i].name;
                }

                int rank = controller.MY_RANKING(playerID);
                float loserTier = ( (float) controller.nPlayers)/2f;
                int rng; 
                // LOSING = GET BETTER SPELLS
                if (rank >= loserTier && controller.turnNumber > 2)
                {
                    Debug.Log("GAINED A GOOD SPELL");
                    rng = Random.Range(0,controller.randomGoodSpells.Count);
                    SPELL_IMG_INSTANTIATE(controller.randomGoodSpells[rng].spellName, true);
                    spellToGain = rng; fromGoodSpell = true;
                }
            // ************************************************************************
                // WINNING = GET AVG SPELLS
                else 
                {
                    rng = Random.Range(0,controller.randomSpells.Count);
                    SPELL_IMG_INSTANTIATE(controller.randomSpells[rng].spellName, true);
                    spellToGain = rng; fromGoodSpell = false;
                }
            }
        }
    // ************************************************************************
        // COMPETITIVE (FIXED SPELL)
        else 
        {
            // GAIN SPELL 
            if (spells.Count < 3)
            {
                spellPickup.Play();
                yield return new WaitForSeconds(0.1f);

                if (spells == null) { spells = new List<SpellType>(); }
                spells.Add( new SpellType("Spell_Trap_10", 1, "Trap") );
                UPDATE_SPELLS_UI();

                var go = Instantiate(floatingSpellTextPrefab, transform.position + new Vector3(0,3), transform.rotation);
                SpriteRenderer spellImg = go.GetComponentInChildren<SpriteRenderer>();

                spellImg.sprite = spellTrap10;

                go.GetComponent<TextMeshPro>().text = "+1";
                Color top = new Color(0, 0.8f, 1);
                Color bot = new Color(0, 0.3f, 1);
                go.GetComponent<TextMeshPro>().colorGradient  = new VertexGradient(top, top, bot, bot);

                yield return new WaitForSeconds(0.5f);
                isAtFree = false;
                UPDATE_INFORMATION(true);
            }
        // ************************************************************************
            // GAIN SPELL, FULL INVENTORY MUST DISCARD A SPELL
            else 
            {
                spellPickup.Play();
                yield return new WaitForSeconds(0.1f);

                discardSpellUI.SetActive(true);
                spellDescUI.SetActive(true);
                manager.HIDE_UI();
                for (int i=0 ; i<spells.Count ; i++) { discardSpellImg[i].sprite = spellSlots[i].sprite; }
                SPELL_IMG_INSTANTIATE("Spell_Trap_10", true);
                spellToGain = 0; fromGoodSpell = false;
            }
        }
    }

    void SPELL_IMG_INSTANTIATE(string spellName, bool gained)
    {
        // AVG SPELLS
        var go = Instantiate(floatingSpellTextPrefab, transform.position + new Vector3(0,3), transform.rotation);
        SpriteRenderer spellImg = go.GetComponentInChildren<SpriteRenderer>();

        if (gained)
        {
            switch (spellName)
            {
                case "Spell_Trap_10" :          spellImg.sprite = spellTrap10;   break;
                case "Spell_Trap_20" :          spellImg.sprite = spellTrap20;   break;
                case "Spell_Trap_Orb" :         spellImg.sprite = spellTrapOrb;  break;

                case "Spell_Effect_10" :        spellImg.sprite = spellEffect10; break;
                case "Spell_Effect_Mana_3" :    spellImg.sprite = spellEffectMana3; break;
                case "Spell_Effect_Spell_1" :   spellImg.sprite = spellEffectSpell1; break;
                case "Spell_Effect_Slow_1" :    spellImg.sprite = spellEffectSlow1; break;
                case "Spell_Effect_Swap" :      spellImg.sprite = spellEffectSwap; break;

                case "Spell_Move_Dash_5" :      spellImg.sprite = spellMoveDash5; break;
                case "Spell_Move_Dash_8" :      spellImg.sprite = spellMoveDash8; break;
                case "Spell_Move_Slow" :        spellImg.sprite = spellMoveSlow; break;
                case "Spell_Move_Slowgo" :      spellImg.sprite = spellMoveSlowgo; break;
                case "Spell_Move_Steal" :       spellImg.sprite = spellMoveSteal; break;
                case "Spell_Move_Barrier" :     spellImg.sprite = spellMoveBarrier; break;
                case "Spell_Move_Orb" :         spellImg.sprite = spellMoveOrb; break;
                default :      break;
            }

            go.GetComponent<TextMeshPro>().text = "+1";
            Color top = new Color(0, 0.8f, 1);
            Color bot = new Color(0, 0.3f, 1);
            go.GetComponent<TextMeshPro>().colorGradient  = new VertexGradient(top, top, bot, bot);

            switch (spellName)
            {
                case "Spell_Trap_10" :          discardSpellImg[3].sprite = spellTrap10;   break;
                case "Spell_Trap_20" :          discardSpellImg[3].sprite = spellTrap20;   break;
                case "Spell_Trap_Orb" :         discardSpellImg[3].sprite = spellTrapOrb;  break;

                case "Spell_Effect_10" :        discardSpellImg[3].sprite = spellEffect10; break;
                case "Spell_Effect_Mana_3" :    discardSpellImg[3].sprite = spellEffectMana3; break;
                case "Spell_Effect_Spell_1" :   discardSpellImg[3].sprite = spellEffectSpell1; break;
                case "Spell_Effect_Slow_1" :    discardSpellImg[3].sprite = spellEffectSlow1; break;
                case "Spell_Effect_Swap" :      discardSpellImg[3].sprite = spellEffectSwap; break;

                case "Spell_Move_Dash_5" :      discardSpellImg[3].sprite = spellMoveDash5; break;
                case "Spell_Move_Dash_8" :      discardSpellImg[3].sprite = spellMoveDash8; break;
                case "Spell_Move_Slow" :        discardSpellImg[3].sprite = spellMoveSlow; break;
                case "Spell_Move_Slowgo" :      discardSpellImg[3].sprite = spellMoveSlowgo; break;
                case "Spell_Move_Steal" :       discardSpellImg[3].sprite = spellMoveSteal; break;
                case "Spell_Move_Barrier" :     discardSpellImg[3].sprite = spellMoveBarrier; break;
                case "Spell_Move_Orb" :         discardSpellImg[3].sprite = spellMoveOrb; break;
                default :      break;
            }
            discardSpellImg[3].name = spellName;
        }
        else
        {
            spellImg.sprite = fourthSpell;
            switch (spellName)
            {
                case "Spell_Trap_10" :          spellImg.sprite = spellTrap10;   break;
                case "Spell_Trap_20" :          spellImg.sprite = spellTrap20;   break;
                case "Spell_Trap_Orb" :         spellImg.sprite = spellTrapOrb;   break;

                case "Spell_Effect_10" :        spellImg.sprite = spellEffect10;   break;
                case "Spell_Effect_Mana_3" :    spellImg.sprite = spellEffectMana3;   break;
                case "Spell_Effect_Spell_1" :   spellImg.sprite = spellEffectSpell1;   break;
                case "Spell_Effect_Slow_1" :    spellImg.sprite = spellEffectSlow1;   break;
                case "Spell_Effect_Swap" :      spellImg.sprite = spellEffectSwap;   break;

                case "Spell_Move_Dash_5" :      spellImg.sprite = spellMoveDash5;   break;
                case "Spell_Move_Dash_8" :      spellImg.sprite = spellMoveDash8;   break;
                case "Spell_Move_Slow" :        spellImg.sprite = spellMoveSlow;   break;
                case "Spell_Move_Slowgo" :      spellImg.sprite = spellMoveSlowgo;   break;
                case "Spell_Move_Steal" :       spellImg.sprite = spellMoveSteal;   break;
                case "Spell_Move_Barrier" :     spellImg.sprite = spellMoveBarrier;   break;
                case "Spell_Move_Orb" :         spellImg.sprite = spellMoveOrb;   break;
                default :      break;
            }

            go.GetComponent<TextMeshPro>().text = "-1";
            Color top = new Color(1, 0.15f, 0.2f);
            Color bot = new Color(0.8f, 0, 0.05f);
            go.GetComponent<TextMeshPro>().colorGradient  = new VertexGradient(top, top, bot, bot);
            
            switch (spellName)
            {
                case "Spell_Trap_10" :          discardSpellImg[3].sprite = spellTrap10;   break;
                case "Spell_Trap_20" :          discardSpellImg[3].sprite = spellTrap20;   break;
                case "Spell_Trap_Orb" :         discardSpellImg[3].sprite = spellTrapOrb;  break;

                case "Spell_Effect_10" :        discardSpellImg[3].sprite = spellEffect10; break;
                case "Spell_Effect_Mana_3" :    discardSpellImg[3].sprite = spellEffectMana3; break;
                case "Spell_Effect_Spell_1" :   discardSpellImg[3].sprite = spellEffectSpell1; break;
                case "Spell_Effect_Slow_1" :    discardSpellImg[3].sprite = spellEffectSlow1; break;
                case "Spell_Effect_Swap" :      discardSpellImg[3].sprite = spellEffectSwap; break;

                case "Spell_Move_Dash_5" :      discardSpellImg[3].sprite = spellMoveDash5; break;
                case "Spell_Move_Dash_8" :      discardSpellImg[3].sprite = spellMoveDash8; break;
                case "Spell_Move_Slow" :        discardSpellImg[3].sprite = spellMoveSlow; break;
                case "Spell_Move_Slowgo" :      discardSpellImg[3].sprite = spellMoveSlowgo; break;
                case "Spell_Move_Steal" :       discardSpellImg[3].sprite = spellMoveSteal; break;
                case "Spell_Move_Barrier" :     discardSpellImg[3].sprite = spellMoveBarrier; break;
                case "Spell_Move_Orb" :         discardSpellImg[3].sprite = spellMoveOrb; break;
                default :      break;
            }
            discardSpellImg[3].name = spellName;
        }
        
        switch (spellName)
        {
            case "Spell_Trap_10" :          fourthSpell = spellTrap10;   break;
            case "Spell_Trap_20" :          fourthSpell = spellTrap20;   break;
            case "Spell_Trap_Orb" :         fourthSpell = spellTrapOrb;   break;

            case "Spell_Effect_10" :        fourthSpell = spellEffect10;   break;
            case "Spell_Effect_Mana_3" :    fourthSpell = spellEffectMana3;   break;
            case "Spell_Effect_Spell_1" :   fourthSpell = spellEffectSpell1;   break;
            case "Spell_Effect_Slow_1" :    fourthSpell = spellEffectSlow1;   break;
            case "Spell_Effect_Swap" :      fourthSpell = spellEffectSwap;   break;

            case "Spell_Move_Dash_5" :      fourthSpell = spellMoveDash5;   break;
            case "Spell_Move_Dash_8" :      fourthSpell = spellMoveDash8;   break;
            case "Spell_Move_Slow" :        fourthSpell = spellMoveSlow;   break;
            case "Spell_Move_Slowgo" :      fourthSpell = spellMoveSlowgo;   break;
            case "Spell_Move_Steal" :       fourthSpell = spellMoveSteal;   break;
            case "Spell_Move_Barrier" :     fourthSpell = spellMoveBarrier;   break;
            case "Spell_Move_Orb" :         fourthSpell = spellMoveOrb;   break;
            default :      break;
        }

        REFRESH_SPELLS();
    }
    void DISCARD_NEW_SPELL()
    {
        var go = Instantiate(floatingSpellTextPrefab, transform.position + new Vector3(0,3), transform.rotation);
        SpriteRenderer spellImg = go.GetComponentInChildren<SpriteRenderer>();
        spellImg.sprite = fourthSpell;
            
        go.GetComponent<TextMeshPro>().text = "-1";
        Color top = new Color(1, 0.15f, 0.2f);
        Color bot = new Color(0.8f, 0, 0.05f);
        go.GetComponent<TextMeshPro>().colorGradient  = new VertexGradient(top, top, bot, bot);
        UPDATE_SPELLS_UI();
    }
    void REFRESH_SPELLS()
    {
        for (int i=0 ; i<spellSlots.Length ; i++)
        {
            discardSpellImg[i].name = spellSlots[i].name;
            grimoireUI._slots[i].name = spellSlots[i].name;
        }
        foreach (Image discardSlot in discardSpellImg)
        {
            discardSlot.GetComponent<Spell>().enabled = false;
            discardSlot.GetComponent<Spell>().enabled = true;
        }
        foreach (Button grimoireSlot in grimoireUI._slots)
        {
            grimoireSlot.GetComponent<Spell>().enabled = false;
            grimoireSlot.GetComponent<Spell>().enabled = true;
        }
    }

    // ** CALLED BY BUTTON
    public void DISCARD_THIS(int index)
    {
        // GOOD SPELL
        if (fromGoodSpell)
        {
            // DISCARD OLD SPELL
            if (index < 3)
            {
                SPELL_IMG_INSTANTIATE(spells[index].spellName, false);
                spells.RemoveAt(index);
                spells.Add( controller.randomGoodSpells[spellToGain] );
                UPDATE_SPELLS_UI();
            }
            // DISCARD NEW SPELL
            else 
            {
                DISCARD_NEW_SPELL();
            }
        }
        else 
        {
            // DISCARD OLD SPELL
            if (index < 3)
            {
                SPELL_IMG_INSTANTIATE(spells[index].spellName, false);
                spells.RemoveAt(index);
                spells.Add( controller.randomSpells[spellToGain] );
                UPDATE_SPELLS_UI();
            }
            // DISCARD NEW SPELL
            else 
            {
                DISCARD_NEW_SPELL();
            }
        }

        discardSpellUI.SetActive(false);
        spellDescUI.SetActive(false);
        highlightedSpell = null;
        manager.UNHIDE_UI();

        StartCoroutine( DoneDiscarding() );
    }

    private IEnumerator DoneDiscarding()
    {
        // MOVE PLAYER TO THE SIDE
        if (_movesRemaining == 0 && isPlayerTurn) {
            yield return new WaitForSeconds(0.5f);
            readyToMovedAside = true;
        }
        isAtFree = false;
        UPDATE_INFORMATION(true);
    }

    // CASTED SPELL
    public void USE_MP(int mpCost)
    {
        if (mpBar.value >= mpCost)
        {
            mpBar.value -= mpCost;
            mpLeft.text = mpBar.value + "/" + mpBar.maxValue;
        }
        manaUsed.Play();
        spells.RemoveAt(spellIndex);
        UPDATE_SPELLS_UI();

        spellTrapTarget.gameObject.SetActive(false);
        spellEffectTarget.gameObject.SetActive(false);
        // areaOfEffect.SetActive(false);
    }

    public void UPDATE_SPELLS_UI()
    {
        // UPDATE UI TOP RIGHT CORNER
        foreach (SpellType sp in spells)
        {
            switch (sp.spellName) {
                case "Spell_Trap_10":       spellSlots[nSpells].sprite = spellTrap10; spellSlots[nSpells].name =    "Spell_Trap_10"; break;
                case "Spell_Trap_20":       spellSlots[nSpells].sprite = spellTrap20; spellSlots[nSpells].name =    "Spell_Trap_20"; break;
                case "Spell_Trap_Orb":      spellSlots[nSpells].sprite = spellTrapOrb; spellSlots[nSpells].name =   "Spell_Trap_Orb"; break;
                
                case "Spell_Effect_10":     spellSlots[nSpells].sprite = spellEffect10; spellSlots[nSpells].name =  "Spell_Effect_10"; break;
                case "Spell_Effect_Mana_3": spellSlots[nSpells].sprite = spellEffectMana3; spellSlots[nSpells].name =  "Spell_Effect_Mana_3"; break;
                case "Spell_Effect_Spell_1": spellSlots[nSpells].sprite = spellEffectSpell1; spellSlots[nSpells].name =  "Spell_Effect_Spell_1"; break;
                case "Spell_Effect_Slow_1": spellSlots[nSpells].sprite = spellEffectSlow1; spellSlots[nSpells].name =  "Spell_Effect_Slow_1"; break;
                case "Spell_Effect_Swap":   spellSlots[nSpells].sprite = spellEffectSwap; spellSlots[nSpells].name =  "Spell_Effect_Swap"; break;
                
                case "Spell_Move_Dash_5":   spellSlots[nSpells].sprite = spellMoveDash5; spellSlots[nSpells].name =  "Spell_Move_Dash_5"; break;
                case "Spell_Move_Dash_8":   spellSlots[nSpells].sprite = spellMoveDash8; spellSlots[nSpells].name =  "Spell_Move_Dash_8"; break;
                case "Spell_Move_Slow":     spellSlots[nSpells].sprite = spellMoveSlow; spellSlots[nSpells].name =  "Spell_Move_Slow"; break;
                case "Spell_Move_Slowgo":   spellSlots[nSpells].sprite = spellMoveSlowgo; spellSlots[nSpells].name =  "Spell_Move_Slowgo"; break;
                case "Spell_Move_Steal":    spellSlots[nSpells].sprite = spellMoveSteal; spellSlots[nSpells].name =  "Spell_Move_Steal"; break;
                case "Spell_Move_Barrier":  spellSlots[nSpells].sprite = spellMoveBarrier; spellSlots[nSpells].name =  "Spell_Move_Barrier"; break;
                case "Spell_Move_Orb":      spellSlots[nSpells].sprite = spellMoveOrb; spellSlots[nSpells].name =  "Spell_Move_Orb"; break;
                default :                   spellSlots[nSpells].sprite = spellSlotEmpty; spellSlots[nSpells].name = ""; break;
            }
            nSpells++;
        }
        nSpells = 0;

        // UPDATE IN GRIMOIRE
        for ( int i=0 ; i < maxNoOfSpells ; i++)
        {
            // UP TO MAX SPELLS
            if (i >= spells.Count) {
                grimoireUI._slots[i].GetComponent<Image>().sprite = spellNone;
                spellSlots[i].sprite = spellSlotEmpty;
                grimoireUI._slots[i].interactable = false; 
                continue;
            }
            
            // UP TO CURRENT NUMBER OF SPELLS
            grimoireUI._slots[i].name = spells[i].spellName;
            grimoireUI._slots[i].interactable = true; 
            switch (spells[i].spellName)
            {
                case "Spell_Trap_10" :          grimoireUI._slots[i].GetComponent<Image>().sprite = spellTrap10;   break;
                case "Spell_Trap_20" :          grimoireUI._slots[i].GetComponent<Image>().sprite = spellTrap20;   break;
                case "Spell_Trap_Orb" :         grimoireUI._slots[i].GetComponent<Image>().sprite = spellTrapOrb;  break;
                
                case "Spell_Effect_10" :        grimoireUI._slots[i].GetComponent<Image>().sprite = spellEffect10; break;
                case "Spell_Effect_Mana_3" :    grimoireUI._slots[i].GetComponent<Image>().sprite = spellEffectMana3; break;
                case "Spell_Effect_Spell_1" :   grimoireUI._slots[i].GetComponent<Image>().sprite = spellEffectSpell1; break;
                case "Spell_Effect_Slow_1" :    grimoireUI._slots[i].GetComponent<Image>().sprite = spellEffectSlow1; break;
                case "Spell_Effect_Swap" :      grimoireUI._slots[i].GetComponent<Image>().sprite = spellEffectSwap; break;
                
                case "Spell_Move_Dash_5" :      grimoireUI._slots[i].GetComponent<Image>().sprite = spellMoveDash5; break;
                case "Spell_Move_Dash_8" :      grimoireUI._slots[i].GetComponent<Image>().sprite = spellMoveDash8; break;
                case "Spell_Move_Slow" :        grimoireUI._slots[i].GetComponent<Image>().sprite = spellMoveSlow; break;
                case "Spell_Move_Slowgo" :      grimoireUI._slots[i].GetComponent<Image>().sprite = spellMoveSlowgo; break;
                case "Spell_Move_Steal" :       grimoireUI._slots[i].GetComponent<Image>().sprite = spellMoveSteal; break;
                case "Spell_Move_Barrier" :     grimoireUI._slots[i].GetComponent<Image>().sprite = spellMoveBarrier; break;
                case "Spell_Move_Orb" :         grimoireUI._slots[i].GetComponent<Image>().sprite = spellMoveOrb; break;
                default :                 
                    grimoireUI._slots[i].GetComponent<Image>().sprite = spellNone;
                    grimoireUI._slots[i].interactable = false; 
                    break;
            }
        }
        REFRESH_SPELLS();
    }

    // ** CALLED BY BUTTON ON GRIMOIRE ** //
    public void CAST_TRAP_SPELL(int index)
    {
        spellMpCost = spells[index].spellCost;
        spellIndex  = index;
        if (spellMpCost > mpBar.value) return;  // NOT ENOUGH MP

        grimoireUI.gameObject.SetActive(false);

        // CAST ON SELF AND END CASTING
        if (spells[index].spellKind == "Move") {
            switch (spells[index].spellName) {
                case "Spell_Move_Dash_5" : 
                    pseudoMove = true;
                    StartCoroutine(ANIMATE_MAGIC_CIRCLE_MOVE(4));   
                    return;
                case "Spell_Move_Dash_8" : 
                    pseudoMove = true;
                    StartCoroutine(ANIMATE_MAGIC_CIRCLE_MOVE(8));   
                    return;
                case "Spell_Move_Slow" : 
                    StartCoroutine(ANIMATE_MAGIC_CIRCLE(""));   
                    slowDice = true;
                    return;
                case "Spell_Move_Slowgo" : 
                    StartCoroutine(ANIMATE_MAGIC_CIRCLE(""));   
                    slowDice = true;
                    playerSlowed = true;
                    return;
                case "Spell_Move_Barrier" : 
                    StartCoroutine(ANIMATE_MAGIC_CIRCLE("barrier"));   
                    return;
                case "Spell_Move_Steal" : 
                    StartCoroutine(ANIMATE_MAGIC_CIRCLE("steal"));   
                    return;
                case "Spell_Move_Orb" : 
                    StartCoroutine( CHANGE_ORB_SPACE() );
                    return;
                default : Debug.LogError("Move Spell -- NO MATCHES"); return;
            }
        }

        isViewingMap = true;
        isCastingSpell = true;
        if (!mapCastSpell.gameObject.activeSelf) mapCastSpell.gameObject.SetActive(true);

        if (spells[index].spellKind == "Trap")  { 
            trapSpellActive = true;
            spellTrapTarget.gameObject.SetActive(true); 
            spellTrapTarget.spellName = spells[index].spellName;
        }
        else if (spells[index].spellKind == "Effect" && spells[index].spellName != "Spell_Effect_Swap")  { 
            effectSpellActive = true;
            spellEffectTarget.gameObject.SetActive(true); 
            spellEffectTarget.specialEffect = false;
            spellEffectTarget.spellName = spells[index].spellName;
        }
        else if (spells[index].spellKind == "Effect" && spells[index].spellName == "Spell_Effect_Swap")  {
            specialSpellActive = true;
            spellSpecialEffectTarget.gameObject.SetActive(true); 
            spellSpecialEffectTarget.specialEffect = true; 
            spellSpecialEffectTarget.spellName = spells[index].spellName;
        }

        areaOfEffect.SetActive(true);
        _cam.orthographicSize = camSizeLarge;
    }

    public void LOCKED_ON() { lockedOn.SetActive(true); }
    public void LOCKED_OFF() { lockedOn.SetActive(false); }

    // PSEUDO MOVES
    private IEnumerator ANIMATE_MAGIC_CIRCLE_MOVE(int moves)
    {
        yield return new WaitForEndOfFrame();
        USE_MP(spellMpCost); 
        grimoireUI.gameObject.SetActive(false);
        var mCircle = Instantiate(blueSpellCirclePrefab, transform.position, blueSpellCirclePrefab.transform.rotation);

        yield return new WaitForSeconds(1);
        Destroy(mCircle.gameObject);
        var buff = Instantiate(blueBuffPrefab, transform.position, blueBuffPrefab.transform.rotation);

        yield return new WaitForSeconds(1);
        Destroy(buff.gameObject);

        pseudoMove = true;   
        _movesRemaining = moves;  
        controller.SLOW_ORB_UPDATE(playerID, _movesRemaining);
        nMovesLeft.gameObject.SetActive(true);  StartCoroutine(DICE_ROLLED()); 
    }

    // MOVE RELATED SPELLS
    private IEnumerator ANIMATE_MAGIC_CIRCLE(string effect)
    {
        yield return new WaitForEndOfFrame();
        USE_MP(spellMpCost); 
        grimoireUI.gameObject.SetActive(false);
        var mCircle = Instantiate(blueSpellCirclePrefab, transform.position, blueSpellCirclePrefab.transform.rotation);

        yield return new WaitForSeconds(1);
        Destroy(mCircle.gameObject);
        var buff = Instantiate(blueBuffPrefab, transform.position, blueBuffPrefab.transform.rotation);

        yield return new WaitForSeconds(1);
        Destroy(buff.gameObject);
        buttonMove.gameObject.SetActive(true);
        buttonSpells.gameObject.SetActive(true);
        buttonMap.gameObject.SetActive(true);
        _cam.orthographicSize = camSizeNormal;

        if (effect == "barrier") {
            playerBarrier = true;
            barrier.SetActive(true);
            barrier.transform.parent = character.transform;
            hurtBox.SetActive(false);
        }
        else if (effect == "steal") {
            stealingAura.SetActive(true);
            hurtBox.SetActive(false);
            stealingAura.transform.parent = character.transform;
            stealMode = true;
        }
    }

    private IEnumerator CHANGE_ORB_SPACE()
    {
        yield return new WaitForEndOfFrame();
        USE_MP(spellMpCost); 
        grimoireUI.gameObject.SetActive(false);
        var mCircle = Instantiate(greenSpellCirclePrefab, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(0.6f);
        Destroy(mCircle.gameObject);
        var buff = Instantiate(greenBuffPrefab, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(0.6f);
        Destroy(buff.gameObject);
        isPlayerTurn = false;
        if (SceneManager.GetActiveScene().name == "Crystal_Caverns")
        {
            controller.MagicOrbBoughtController();
            controller.CHOOSE_MAGIC_ORB_SPACE(name);
        }
        else 
        {
            PLAYER_CAM_OFF(0);
            StartCoroutine( manager.MANAGER_BOAT_ALTERNATE(this) );
        }
    }

    // FREEZING SPELL
    public void PLAYER_SLOWED()
    {
        playerSlowed = true;
        slowed.SetActive(true); 
        
        foreach (Transform child in character.transform)  
        {  
            if (child.name != "Shadow")
            {
                if (child.TryGetComponent(out SpriteRenderer cs)) { cs.color = new Color(0,0.5f,1); }
            }
            foreach (Transform grandChild in child)
            {
                if (grandChild.name != "Shadow")
                {
                    if (grandChild.TryGetComponent(out SpriteRenderer gs)) { gs.color = new Color(0,0.5f,1); }
                }
            }
        }
        slowed.transform.parent = character.transform; 
    }

    // TELEPORTATION
    public void SET_NEW_PATH(string newPath)
    {
        currentNode = GameObject.Find(newPath).GetComponent<Node>();
    }

    public void RESET_TARGET_SPELL_CAM()
    {
        spellSpecialEffectTarget.transform.position = new Vector3(this.transform.position.x,
                this.transform.position.y, this.transform.position.z);
        _cam.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -30f);
        var obj = Instantiate(greenTeleportEffect, transform.position, greenTeleportEffect.transform.rotation);
        Destroy(obj, 1.5f);
    }

    public void DISPLAY_PLAYER_RANKINGS(int xth)
    {
        switch (xth)
        {
            case 0:   ranking.text = "1<sup>st";   break;
            case 1:   ranking.text = "2<sup>nd";   break;
            case 2:   ranking.text = "3<sup>rd";   break;
            case 3:   ranking.text = "4<sup>th";   break;
            case 4:   ranking.text = "5<sup>th";   break;
            case 5:   ranking.text = "6<sup>th";   break;
            case 6:   ranking.text = "7<sup>th";   break;
            case 7:   ranking.text = "8<sup>th";   break;
        }
    }

    


    // todo : HAPPENING \ EVENT SPACES

    IEnumerator PATHFOLLOWER_CAVING_IN()
    {
        manager.SCREEN_TRANSITION("Happen_Transition", 0);
        
        yield return new WaitForSeconds(1);
        StartCoroutine( manager.MANAGER_CAVING_IN(this) );
        _cam.gameObject.SetActive(false);   // CAM TURNS OFF
    }

    IEnumerator HAPPEN_NEW_BOAT()
    {
        manager.SCREEN_TRANSITION("Happen_Transition", 0);
        
        yield return new WaitForSeconds(1);
        StartCoroutine( manager.MANAGER_BOAT_ALTERNATE(this) );
        _cam.gameObject.SetActive(false);   // CAM TURNS OFF
    }

    IEnumerator PATHFOLLOWER_TURRET_ROTATE()
    {
        manager.SCREEN_TRANSITION("Happen_Transition", 0);
        
        yield return new WaitForSeconds(1);
        manager.MANAGER_TURRET_ROTATE(this);
        _cam.gameObject.SetActive(false);   // CAM TURNS OFF
    }
    
    IEnumerator PATHFOLLOWER_TURRET_SPEED_UP()
    {
        manager.SCREEN_TRANSITION("Happen_Transition", 0);
        
        yield return new WaitForSeconds(1);
        manager.MANAGER_TURRET_SPEED_UP(this);
        _cam.gameObject.SetActive(false);   // CAM TURNS OFF
    }



    // PLAYER MOVED TO SPECIAL SPACE
    public void MANAGER_EVENT()
    {
        if (isAtSpecial) return;
        isAtSpecial = true;
        isBoat = true;
        _animator.SetBool("IsWalking", false);
        _animator.speed = 1;
        _cam.orthographicSize = camSizeLarge;
        string boat = manager.WHICH_BOAT_IS_IT();

        switch (boat)
        {
            case "Norm" : 
                isAtShop = true; shop4 = true; nPurchaseLeft = controller.turnMultiplier; 
                PURCHASES_LEFT(); StartCoroutine(CAM_TO_BOAT());
                break;
            case "Gold" : 
                goldBoat.SetActive(true); manager.HIDE_UI(); NEXT_TEXT();
                StartCoroutine(CAM_TO_BOAT());
                break;
            case "Evil" :
                evilBoat.SetActive(true); manager.HIDE_UI(); NEXT_TEXT();
                StartCoroutine(CAM_TO_BOAT());
                break;
        }

        // call manager based on currentBoat returns string
    }
   
    // REACHED SPECIAL SPACE IN SHOGUN SEAPORT
    private IEnumerator CAM_TO_BOAT()
    {
        while (_cam.orthographicSize < 20)
        {
            yield return new WaitForEndOfFrame();
            _cam.orthographicSize += 0.1f;
            _cam.transform.position += new Vector3(0,0.25f);
        }
    }

    private void NEXT_TEXT()
    {
        if (goldBoat.activeSelf)
        {
            switch (nPrompt)
            {
                case 0 :  goldtext.text = "How kind of you to see me off on my voyage.";  break;
                case 1 :  goldtext.text = "Here is a little gift.";  break;
                case 2 :  nPrompt++; goldBoat.SetActive(false); StartCoroutine( BUYING_MAGIC_ORB_FROM_SHOP() );  break;
            }
        }
        else if (evilBoat.activeSelf)
        {
            switch (nPrompt)
            {
                case 0 :  eviltext.text = "Today is yer unlucky day.";  break;
                case 1 :  eviltext.text = "Ye shall be spared, if ye forks over yer booty.";  break;
                case 2 :  
                    if (orbs > 0) 
                    {
                        nPrompt = 10; evilBoat.SetActive(false); StartCoroutine( EVIL_BOAT("orb") );  break;
                    }
                    else if (coins > 0)
                    {
                        nPrompt = 10; evilBoat.SetActive(false); StartCoroutine( EVIL_BOAT("coin") ); break;
                    }
                    else 
                    {
                        eviltext.text = "WHAT! YE HAS NOTHING!?"; break;
                    }
                case 4 :  eviltext.text = "Very well, I pity ye, so here"; break;
                case 5 : nPrompt = 10; evilBoat.SetActive(false); StartCoroutine( UPDATE_PLAYER_COINS(30) ); break;
            }
        }
    }

    void RESET_CAM()
    {
        _cam.orthographicSize = camSizeNormal;
        _cam.transform.position = 
            new Vector3(this.transform.position.x, this.transform.position.y, _cam.transform.position.z);
    }

    public void LEAVE_PORT()
    {
        RESET_CAM();
        isAtSpecial = false; manager.UNHIDE_UI(); isPlayerTurn = true;
        isBoat = false;
        nPrompt = 0;
        if (!diceRolled && _movesRemaining > 0)
        {
            _animator.SetBool("IsWalking", true);
            _animator.speed = _moveSpeed;
        }
        else if (!diceRolled)
        {
            RESUME_PLAYER_TURN();
        }
    }


    private IEnumerator EVIL_BOAT(string whatIsLost)
    {
        if (whatIsLost == "orb")
        {
            _animator.SetBool("IsWalking", false);
            _animator.speed = 1;
            isPlayerTurn = false;

            yield return new WaitForEndOfFrame();
            AudioSource bgMusic = GameObject.Find("BACKGROUND_MUSIC").GetComponent<AudioSource>();
            bgMusicVol = bgMusic.volume;
            bgMusic.volume = 0;
            orbStolen.Play();

            yield return new WaitForSeconds(4);
            while (orbStolen.volume > 0)
            {
                yield return new WaitForEndOfFrame();
                orbStolen.volume -= 0.01f;
            }
            bgMusic.volume = bgMusicVol;
            StartCoroutine( ORB_LOST_FROM_BOAT(-1) );
        }
        else
        {
            _animator.SetBool("IsWalking", false);
            _animator.speed = 1;
            isPlayerTurn = false;

            yield return new WaitForEndOfFrame();
            AudioSource bgMusic = GameObject.Find("BACKGROUND_MUSIC").GetComponent<AudioSource>();
            bgMusicVol = bgMusic.volume;
            bgMusic.volume = 0;
            orbStolen.Play();

            yield return new WaitForSeconds(4);
            while (orbStolen.volume > 0)
            {
                yield return new WaitForEndOfFrame();
                orbStolen.volume -= 0.01f;
            }
            bgMusic.volume = bgMusicVol;
            StartCoroutine( UPDATE_PLAYER_COINS(-20) );
        }
    }
}
