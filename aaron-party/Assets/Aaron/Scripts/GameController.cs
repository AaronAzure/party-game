using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public List<int> charactersChosen;  // ENSURE NO TWO PLAYERS HAVE THE SAME CHARACTER
    public int nPlayers;
    public int nReady;
    [Tooltip("Current turn")] public int turnNumber;  // WHAT TURN [1,2,...]
    public float pSpeed = 7.5f;       // ** PATHFOLLOWER SPEED
    public bool minigameMode;          // ** SET BY PLAYER 1 (LobbyControls) {true = in minigame tent}
    public string questToPlay;


    [Header("Debugging")]
    public bool skipSideQuest;  //* DEBUGGING ONLY, INSPECTOR
    public bool slowerDice;  //* DEBUGGING ONLY, INSPECTOR
    public bool infiniteMovement;  //* DEBUGGING ONLY, INSPECTOR
    public bool playLatestQuest;  //* DEBUGGING ONLY, INSPECTOR


    [Header("Board Settings")]
    public int maxTurns = 15;       // ** SET BY PLAYER 1 (LobbyControls)
    public bool isCasual = false;   // ** SET BY PLAYER 1 (LobbyControls) {true = casual | false = competitive}
    public bool easy;
    public bool norm = true;
    public bool hard;
    public bool isSetOrder = true;    // ** SET BY PLAYER 1 (LobbyControls) {true = set player order | false = flex player order}


    [Header("Player Data")]
    public string characterName1;
    public string characterName2;
    public string characterName3;
    public string characterName4;
    public string characterName5;
    public string characterName6;
    public string characterName7;
    public string characterName8;
    private string[] characterNames;

    private List<PlayerPrevData>[] playersData;
    public List<PlayerPrevData> p1;
    public List<PlayerPrevData> p2;
    public List<PlayerPrevData> p3;
    public List<PlayerPrevData> p4;
    public List<PlayerPrevData> p5;
    public List<PlayerPrevData> p6;
    public List<PlayerPrevData> p7;
    public List<PlayerPrevData> p8;

    public List<List<PlayerBuffData>> buffDatas;


    public List<SpellType> spells1;
    public List<SpellType> spells2;
    public List<SpellType> spells3;
    public List<SpellType> spells4;
    public List<SpellType> spells5;
    public List<SpellType> spells6;
    public List<SpellType> spells7;
    public List<SpellType> spells8;

    public List<SpellType> randomSpells;
    public List<SpellType> randomGoodSpells;
    public List<SpellType> spellBook;


    public int[] playerOrder;
    public bool hasStarted = false;
    private List<NodeSpace> changedSpaces;
    private int currentMagicOrbIndex = -1;
    private List<NodeSpace> magicOrbSpace;


    [Header("Crystal Caverns")]
    [SerializeField] private Camera[] cavingInCamera;   // CRYSTAL CAVERN SPECIFIC CAMERAS
    [SerializeField] private GameObject collapsePrefab;   // 
    private List<NodeSpace> cavedInSpaces;
    private List<CavedSpace> canBeCavedSpaces;
    private HashSet<int> nCavedSection;


    [Header("Magic Orb")]
    [SerializeField] public Camera orbCam;
    [SerializeField] private GameObject aaronOrb;   // THE NEXT MAGIC ORB LOCATION
    private float camSpeed;
    private bool showingNewMagicOrb;
    private Node camNode;
    private string playerToResume;
    private bool firstMagicOrbShown;
    public bool starting;

    private string boardSceneName;
    private string introSceneName;
    public List<Quest> quests;
    public List<string> noList;


    // *** BONUS ORBS *** //
    public int[] richOrb;
    public int[] questOrb;
    public int[] trapOrb;
    public int[] eventOrb;
    public int[] blueOrb;
    public int[] redOrb;
    public int[] slowOrb;
    public int[] shopOrb;
    public int turnMultiplier = 1;


    public List<int> allGold;
    public List<int> allOrb;


    // -----------------------------------------------------------------------------------
    // GAME SETUP = NUMBER OF PLAYERS
    void Start()
    {
        camSpeed = Time.deltaTime;
        charactersChosen = new List<int>();

        turnNumber = 1;
        DontDestroyOnLoad(this);
        Screen.SetResolution(1980, 1090, Screen.fullScreen);

        // SPACES THAT HAVE BEEN TRANSFORMED INTO TRAPS
        changedSpaces = new List<NodeSpace>();
        cavedInSpaces = new List<NodeSpace>();

        // SPACES THAT ARE MAGIC ORB NODES
        magicOrbSpace = new List<NodeSpace>();

        quests = new List<Quest>();
        noList = new List<string>();
        RESET_QUESTS();

        // PREVIOUS GAME SETTINGS (MINIGAME NOT TO INCLUDE)
        for (int i=0 ; i<quests.Count ; i++) {
            if (PlayerPrefs.HasKey( quests[i].questMini )) {
                ADD_TO_NO_LIST(quests[i].questMini);
            }
        }

        spells1 = new List<SpellType>();
        spells2 = new List<SpellType>();
        spells3 = new List<SpellType>();
        spells4 = new List<SpellType>();
        spells5 = new List<SpellType>();
        spells6 = new List<SpellType>();
        spells7 = new List<SpellType>();
        spells8 = new List<SpellType>();

        p1 = new List<PlayerPrevData>();
        p2 = new List<PlayerPrevData>();
        p3 = new List<PlayerPrevData>();
        p4 = new List<PlayerPrevData>();
        p5 = new List<PlayerPrevData>();
        p6 = new List<PlayerPrevData>();
        p7 = new List<PlayerPrevData>();
        p8 = new List<PlayerPrevData>();

        randomSpells   = new List<SpellType>();
        randomGoodSpells   = new List<SpellType>();
        spellBook   = new List<SpellType>();
        FILL_RANDOM_SPELLS();
    }

    private void FILL_RANDOM_SPELLS()
    {
        randomSpells.Add( new SpellType("Spell_Trap_10", 1, "Trap") );

        randomSpells.Add( new SpellType("Spell_Effect_10", 2, "Effect") );
        randomSpells.Add( new SpellType("Spell_Effect_Spell_1", 2, "Effect") );
        randomSpells.Add( new SpellType("Spell_Effect_Slow_1", 2, "Effect") );

        randomSpells.Add( new SpellType("Spell_Move_Slowgo", 2, "Move") );
        randomSpells.Add( new SpellType("Spell_Move_Barrier", 2, "Move") );
        randomSpells.Add( new SpellType("Spell_Move_Dash_5", 2, "Move") );


        // randomGoodSpells.Add( new SpellType("Spell_Trap_10", 1, "Trap") );
        randomGoodSpells.Add( new SpellType("Spell_Trap_20", 2, "Trap") );
        randomGoodSpells.Add( new SpellType("Spell_Trap_Orb", 3, "Trap") );

        randomGoodSpells.Add( new SpellType("Spell_Effect_10", 2, "Effect") );
        randomGoodSpells.Add( new SpellType("Spell_Effect_Spell_1", 2, "Effect") );
        randomGoodSpells.Add( new SpellType("Spell_Effect_Slow_1", 2, "Effect") );
        randomGoodSpells.Add( new SpellType("Spell_Effect_Swap", 3, "Effect") );

        // randomGoodSpells.Add( new SpellType("Spell_Move_Slowgo", 2, "Move") );
        // randomGoodSpells.Add( new SpellType("Spell_Move_Dash_5", 2, "Move") );

        randomGoodSpells.Add( new SpellType("Spell_Move_Barrier", 2, "Move") );
        randomGoodSpells.Add( new SpellType("Spell_Move_Dash_8", 3, "Move") );
        randomGoodSpells.Add( new SpellType("Spell_Move_Slow", 3, "Move") );
        randomGoodSpells.Add( new SpellType("Spell_Move_Steal", 3, "Move") );
        randomGoodSpells.Add( new SpellType("Spell_Move_Orb", 3, "Move") );


        spellBook.Add( new SpellType("Spell_Trap_10", 1, "Trap") );
        spellBook.Add( new SpellType("Spell_Trap_10", 1, "Trap") );
        spellBook.Add( new SpellType("Spell_Trap_10", 1, "Trap") );

        spellBook.Add( new SpellType("Spell_Trap_20", 2, "Trap") );
        spellBook.Add( new SpellType("Spell_Trap_20", 2, "Trap") );

        spellBook.Add( new SpellType("Spell_Trap_Orb", 3, "Trap") );

        spellBook.Add( new SpellType("Spell_Effect_10", 2, "Effect") );
        spellBook.Add( new SpellType("Spell_Effect_10", 2, "Effect") );
        spellBook.Add( new SpellType("Spell_Effect_Spell_1", 2, "Effect") );
        spellBook.Add( new SpellType("Spell_Effect_Spell_1", 2, "Effect") );
        spellBook.Add( new SpellType("Spell_Effect_Slow_1", 2, "Effect") );
        spellBook.Add( new SpellType("Spell_Effect_Slow_1", 2, "Effect") );
        spellBook.Add( new SpellType("Spell_Move_Barrier", 2, "Move") );
        spellBook.Add( new SpellType("Spell_Move_Barrier", 2, "Move") );

        spellBook.Add( new SpellType("Spell_Move_Dash_5", 2, "Move") );
        spellBook.Add( new SpellType("Spell_Move_Dash_8", 3, "Move") );
        spellBook.Add( new SpellType("Spell_Move_Slowgo", 2, "Move") );
        spellBook.Add( new SpellType("Spell_Move_Slow", 3, "Move") );
        spellBook.Add( new SpellType("Spell_Move_Orb", 3, "Move") );
    }

    public void SET_PLAYER_CHARACTER(int playerNumber, string newCharacterName)
    {
        switch (playerNumber)
        {
            case 0:     characterName1 = newCharacterName; break;
            case 1:     characterName2 = newCharacterName; break;
            case 2:     characterName3 = newCharacterName; break;
            case 3:     characterName4 = newCharacterName; break;
            case 4:     characterName5 = newCharacterName; break;
            case 5:     characterName6 = newCharacterName; break;
            case 6:     characterName7 = newCharacterName; break;
            case 7:     characterName8 = newCharacterName; break;
        }
    }

    // **** AFTER DETERMINING NO. OF PLAYERS **** //
    public void PLAYER_SETUP_FINISHED()
    {
        if (nPlayers >= 1 && nPlayers == nReady)
        {
            if (buffDatas == null) {
                buffDatas = new List<List<PlayerBuffData>>();
            }
            for (int i=0 ; i<nPlayers ; i++) {
                buffDatas.Add( new List<PlayerBuffData>() ); 
                buffDatas[i].Add (new PlayerBuffData(false,false,false,false,false) );
            }
            playerOrder     = new int[nPlayers];
            for (int i=0 ; i<playerOrder.Length ; i++)
            {
                playerOrder[i] = i;
            }
            richOrb         = new int[nPlayers];
            questOrb        = new int[nPlayers];
            blueOrb         = new int[nPlayers];
            redOrb          = new int[nPlayers];
            shopOrb         = new int[nPlayers];
            eventOrb        = new int[nPlayers];
            slowOrb         = new int[nPlayers];
            p1.Add(new PlayerPrevData("Path_0", 0, new Vector3(0,0), new Vector3(0,0), 10, 0, 5, 0, false));
            p2.Add(new PlayerPrevData("Path_0", 0, new Vector3(0,0), new Vector3(0,0), 10, 0, 5, 0, false));
            p3.Add(new PlayerPrevData("Path_0", 0, new Vector3(0,0), new Vector3(0,0), 10, 0, 5, 0, false));
            p4.Add(new PlayerPrevData("Path_0", 0, new Vector3(0,0), new Vector3(0,0), 10, 0, 5, 0, false));
            p5.Add(new PlayerPrevData("Path_0", 0, new Vector3(0,0), new Vector3(0,0), 10, 0, 5, 0, false));
            p6.Add(new PlayerPrevData("Path_0", 0, new Vector3(0,0), new Vector3(0,0), 10, 0, 5, 0, false));
            p7.Add(new PlayerPrevData("Path_0", 0, new Vector3(0,0), new Vector3(0,0), 10, 0, 5, 0, false));
            p8.Add(new PlayerPrevData("Path_0", 0, new Vector3(0,0), new Vector3(0,0), 10, 0, 5, 0, false));
            SceneManager.LoadScene("2Lobby");
        }
    }

    public bool ALL_PLAYERS_READY()
    {
        if (nPlayers > 1 && nPlayers == nReady) { Debug.Log("-- ALL PLAYERS ARE READY"); return true; }
        else    { Debug.Log("-- NOT ALL PLAYERS READY YET"); return false; }
    }


    public void RESET_QUESTS()
    {
        quests.Add( new Quest("Sneak And Snore", "Sneak_And_Snore") );      // 0
        quests.Add( new Quest("Feast-ival", "Food_Festival") );
        quests.Add( new Quest("Colour Chaos", "Colour_Chaos") );
        quests.Add( new Quest("Card Collectors", "Card_Collectors") );      // 3
        quests.Add( new Quest("Leaf Leap", "Leaf_Leap") );
        quests.Add( new Quest("Lava Or Leave 'Em", "Lava_Or_Leave_'Em") );
        quests.Add( new Quest("Lilypad Leapers", "Lilypad_Leapers") );      // 6
        quests.Add( new Quest("Stop Watchers", "Stop_Watchers") );
        quests.Add( new Quest("Spotlight Fight", "Spotlight-Fight") );
        quests.Add( new Quest("Pushy Penguins", "Pushy-Penguins") );        // 9
        quests.Add( new Quest("Fun Run", "Fun_Run") );
        quests.Add( new Quest("Coin-veyor", "Money_Belt") );
        quests.Add( new Quest("Stamp By Me", "Stamp-By-Me") );              // 12
        quests.Add( new Quest("Shocking Situation", "Shocking-Situation") );
        quests.Add( new Quest("Attack Of Titan", "Attack-On-Titan") );
        quests.Add( new Quest("Flower Shower", "Flower-Shower") );          // 15
        quests.Add( new Quest("Don't Be A Zombie", "Don't-Be-A-Zombie") );
        quests.Add( new Quest("Barrier Bearers", "Barrier_Bearers") );
        quests.Add( new Quest("Plunder Ground", "Plunder-Ground") );
        quests.Add( new Quest("Pinpoint The Endpoint", "Pinpoint-The-Endpoint") );
        quests.Add( new Quest("Camo Cutters", "Camo-Cutters") );
        quests.Add( new Quest("County Bounty", "County-Bounty") );
        quests.Add( new Quest("Slay The Shades", "Slay-The-Shades") );
        // CATCHY TUNES
        // TIDAL FOOLS
        // SPEEDY SERVERS
        // DARKNESS MAZE
        // BARRIER BEARERS

    }

    public void SET_AVAILABLE_QUEST()
    {
        foreach (string miniName in noList)
        {
            foreach(Quest quest in quests)
            {
                if (quest.questMini == miniName)
                {
                    quests.Remove(quest);
                    Debug.Log("-- side quest removed");
                    break;
                }
            }
        }
    }

    public void RESET_BEST_QUESTS()
    {
        quests.Add( new Quest("Colour Chaos", "Colour_Chaos") );
        quests.Add( new Quest("Leaf Leap", "Leaf_Leap") );
        quests.Add( new Quest("Lava Or Leave 'Em", "Lava_Or_Leave_'Em") );
        quests.Add( new Quest("Spotlight Fight", "Spotlight-Fight") );
        quests.Add( new Quest("Pushy Penguins", "Pushy-Penguins") );        
        quests.Add( new Quest("Fun Run", "Fun_Run") );
        quests.Add( new Quest("Coin-veyor", "Money_Belt") );
        quests.Add( new Quest("Shocking Situation", "Shocking-Situation") );
        quests.Add( new Quest("Attack Of Titan", "Attack-On-Titan") );
        quests.Add( new Quest("Flower Shower", "Flower-Shower") );          
    }
    
    // QUEST(S) NOT TO PLAY/SELECTED
    public bool ADD_TO_NO_LIST(string questName)
    {
        // PLAYABLE
        foreach (string alreadyIn in noList)
        {
            if (alreadyIn == questName)
            {
                noList.Remove(questName);
                if (PlayerPrefs.HasKey(questName)) PlayerPrefs.DeleteKey(questName);
                return true;
            }
        }
        // NON-PLAYABLE
        noList.Add(questName);
        if (!PlayerPrefs.HasKey(questName)) PlayerPrefs.SetString(questName, questName);

        return false;
    }


    // ************ MAP SELECTION ************ //
    public void LOAD_BOARD(string boardName)
    {  
        SET_AVAILABLE_QUEST();
        if (boardName == "Crystal_Caverns" || boardName == "Crystal_Caverns Intro" ) {
            boardSceneName = "Crystal_Caverns";
            introSceneName = "Crystal_Caverns Intro";
        }
        else if (boardName == "Shogun_Seaport" || boardName == "Shogun_Seaport Intro" ) {
            boardSceneName = "Shogun_Seaport";
            introSceneName = "Shogun_Seaport Intro";
        }
        else if (boardName == "Plasma_Palace" || boardName == "Plasma_Palace Intro" ) {
            boardSceneName = "Plasma_Palace";
            introSceneName = "Plasma_Palace Intro";
        }
        else Debug.LogError("HAVEN'T ADDED MAP TO - GameController.LOAD_BOARD() ");
        SceneManager.LoadScene(boardName); 
    }
    public void LOAD_GAME_BOARD() { if(boardSceneName != null)  SceneManager.LoadScene(boardSceneName); }
    public void LOAD_CUTAWAY()      { if(introSceneName != null)  SceneManager.LoadScene(introSceneName); }

    public void LOAD_MINIGAMES_BOARD()
    {
        minigameMode = true;
        SceneManager.LoadScene("3Quests");
    }

    public void LOAD_CRYSTAL_CAVERNS()  {  SceneManager.LoadScene("Crystal_Caverns"); }

    public void Testing() { SceneManager.LoadScene("2Lobby"); }

    // -----------------------------------------------------------------------------------

    public void GAME_START() {  hasStarted = true; }

    public void NEXT_TURN() { turnNumber++; }



    /* ---------------------------- */
    /* ----- ACCESSOR METHODS ----- */
    
    public List<SpellType> GET_SPELLS(int player)
    {
        switch (player)
        {
            case 0:     return (spells1);
            case 1:     return (spells2);
            case 2:     return (spells3);
            case 3:     return (spells4);
            case 4:     return (spells5);
            case 5:     return (spells6);
            case 6:     return (spells7);
            case 7:     return (spells8);
            default:    return null;
        }
    }
    
    public List<PlayerPrevData> GET_PLAYER_DATA(int playerID)
    {
        switch (playerID)
        {
            case 0 : return p1;
            case 1 : return p2;
            case 2 : return p3;
            case 3 : return p4;
            case 4 : return p5;
            case 5 : return p6;
            case 6 : return p7;
            case 7 : return p8;
        }
        return null;
    }

    public int GET_PLAYER_GOLD(int playerID)
    {
        switch (playerID)
        {
            case 0 : return p1[0].coins;
            case 1 : return p2[0].coins;
            case 2 : return p3[0].coins;
            case 3 : return p4[0].coins;
            case 4 : return p5[0].coins;
            case 5 : return p6[0].coins;
            case 6 : return p7[0].coins;
            case 7 : return p8[0].coins;
        }
        Debug.LogError("controller - NON-EXISTANT PLAYER");
        return 0;
    }
    public Vector3 GET_PLAYER_POS(int playerID)
    {
        switch (playerID)
        {
            case 0 : return p1[0].posAside;
            case 1 : return p2[0].posAside;
            case 2 : return p3[0].posAside;
            case 3 : return p4[0].posAside;
            case 4 : return p5[0].posAside;
            case 5 : return p6[0].posAside;
            case 6 : return p7[0].posAside;
            case 7 : return p8[0].posAside;
        }
        Debug.LogError("controller - NON-EXISTANT PLAYER");
        return (new Vector3(0,0,0));
    }

    /* --------------------------- */
    /* ----- MUTATOR METHODS ----- */

    public void SET_PLAYER_DATA(int playerID, string newPath, int newNode,
            Vector3 newPos, Vector3 newAside, int newCoins, int newOrbs, int newMP)
    {
        switch (playerID)
        {
            case 0 : { Debug.Log("Player " + (playerID+1) + " updating info");
                p1.Clear(); p1.Add(new PlayerPrevData(newPath,newNode,newPos,newAside,newCoins,newOrbs,newMP,0,false)); break;
            }
            case 1 : { Debug.Log("Player " + (playerID+1) + " updating info");
                p2.Clear(); p2.Add(new PlayerPrevData(newPath,newNode,newPos,newAside,newCoins,newOrbs,newMP,0,false)); break;
            }
            case 2 : { Debug.Log("Player " + (playerID+1) + " updating info");
                p3.Clear(); p3.Add(new PlayerPrevData(newPath,newNode,newPos,newAside,newCoins,newOrbs,newMP,0,false)); break;
            }
            case 3 : { Debug.Log("Player " + (playerID+1) + " updating info");
                p4.Clear(); p4.Add(new PlayerPrevData(newPath,newNode,newPos,newAside,newCoins,newOrbs,newMP,0,false)); break;
            }
            case 4 : { Debug.Log("Player " + (playerID+1) + " updating info");
                p5.Clear(); p5.Add(new PlayerPrevData(newPath,newNode,newPos,newAside,newCoins,newOrbs,newMP,0,false)); break;
            }
            case 5 : { Debug.Log("Player " + (playerID+1) + " updating info");
                p6.Clear(); p6.Add(new PlayerPrevData(newPath,newNode,newPos,newAside,newCoins,newOrbs,newMP,0,false)); break;
            }
            case 6 : { Debug.Log("Player " + (playerID+1) + " updating info");
                p7.Clear(); p7.Add(new PlayerPrevData(newPath,newNode,newPos,newAside,newCoins,newOrbs,newMP,0,false)); break;
            }
            case 7 : { Debug.Log("Player " + (playerID+1) + " updating info");
                p8.Clear(); p8.Add(new PlayerPrevData(newPath,newNode,newPos,newAside,newCoins,newOrbs,newMP,0,false)); break;
            }
        }
    }


    public void SET_PLAYER_BUFFS(int playerID, bool newSlowed, bool newBarrier, bool newMove15, bool newRange2, bool newExtraBuy)
    {
        buffDatas[playerID][0].slowed   = newSlowed;
        buffDatas[playerID][0].barrier  = newBarrier;
        buffDatas[playerID][0].move15   = newMove15;
        buffDatas[playerID][0].range2   = newRange2;
        buffDatas[playerID][0].extraBuy = newExtraBuy;
    }

    public void SET_SPELLS(int player, List<SpellType> newSpells)
    {
        switch (player)
        {
            case 0:     { spells1.Clear();   foreach (SpellType sp in newSpells) { spells1.Add( sp ); }   break; }
            case 1:     { spells2.Clear();   foreach (SpellType sp in newSpells) { spells2.Add( sp ); }   break; }
            case 2:     { spells3.Clear();   foreach (SpellType sp in newSpells) { spells3.Add( sp ); }   break; }
            case 3:     { spells4.Clear();   foreach (SpellType sp in newSpells) { spells4.Add( sp ); }   break; }
            case 4:     { spells5.Clear();   foreach (SpellType sp in newSpells) { spells5.Add( sp ); }   break; }
            case 5:     { spells6.Clear();   foreach (SpellType sp in newSpells) { spells6.Add( sp ); }   break; }
            case 6:     { spells7.Clear();   foreach (SpellType sp in newSpells) { spells7.Add( sp ); }   break; }
            case 7:     { spells8.Clear();   foreach (SpellType sp in newSpells) { spells8.Add( sp ); }   break; }
            default:    Debug.LogError("SETTING SPELLS FAILED"); break;
        }
    }


    /* -------------------------- */
    /* ------- TURN STUFF ------- */

    public IEnumerator NEXT_ORB_SPACE(string player)
    {
        PathFollower p = GameObject.Find(player).GetComponent<PathFollower>();
        StartCoroutine( p.PLAYER_CAM_OFF(0) );
        orbCam.gameObject.SetActive(true);
        playerToResume = player;    // GLOBAL

        yield return new WaitForSeconds(0.5f);

        // camNode = GameObject.Find("/" + magicOrbSpace[currentMagicOrbIndex].parentPath + "/" + magicOrbSpace[currentMagicOrbIndex].childNode).GetComponent<Node>();
        showingNewMagicOrb = true;

    }

    private IEnumerator RESUME_PLAYER()
    {
        yield return new WaitForSeconds(1);

        PathFollower p = GameObject.Find(playerToResume).GetComponent<PathFollower>();
        orbCam.gameObject.SetActive(false);
        aaronOrb.SetActive(false);
        ResetOrbCam();
        p.PLAYER_CAM_ON();

        yield return new WaitForSeconds(1);

        p.RESUME_PLAYER_TURN();
    }
    public IEnumerator START_THE_GAME(bool isSeaport)
    {
        if (!isSeaport)
        {
            yield return new WaitForSeconds(1);
            aaronOrb.SetActive(false);
            
            GameManager manager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
            manager.p1Start();

            yield return new WaitForSeconds(1);
            orbCam.gameObject.SetActive(false);
            ResetOrbCam();
        }
        else
        {
            yield return new WaitForSeconds(1.5f);
            GameManager manager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
            StartCoroutine( manager.CAMERA_TRANSITION(0,1,false) );
            

            yield return new WaitForSeconds(1);
            aaronOrb.SetActive(false);
            orbCam.gameObject.SetActive(false);
            ResetOrbCam();

        }
        // StartCoroutine( manager.CAMERA_TRANSITION() );
    }
    
    // RESET "orbCam" TO ORIGINAL POS
    private void ResetOrbCam()
    {
        orbCam.orthographicSize     = 10;
        camSpeed                    = Time.deltaTime;
    }

    // MOVE CAMERA TO SHOW WHERE THE MAGIC ORB SPACE IS
    void Update() 
    {
        // SHOW THE FIRST MAGIC ORB
        if (!firstMagicOrbShown && starting)
        {
            // AFTER MOVING TO NEXT MAGIC ORB SPACE, SHRINK CAM (2)
            if (orbCam.transform.position.x - camNode.transform.position.x <= 0.01f &&
                orbCam.transform.position.x - camNode.transform.position.x >= -0.01f)
            {
                orbCam.orthographicSize -= 0.05f;

                if (orbCam.orthographicSize <= 7.5f) 
                {
                    firstMagicOrbShown = true;
                    StartCoroutine( START_THE_GAME(false) );
                }
            }
            else {  // GROW CAM, THEN MOVE TO NEXT MAGIC ORB SPACE (1)
                orbCam.transform.position = 
                    Vector3.Lerp(
                        orbCam.transform.position, 
                        new Vector3(camNode.transform.position.x, camNode.transform.position.y, orbCam.transform.position.z), 
                        camSpeed);
                camSpeed *= 1.01f;
                aaronOrb.SetActive(true);
            }
        }

        // SHOWING ALL CONSECUTIVE MAGIC ORB SPACES
        if (showingNewMagicOrb)
        {
            
            if (orbCam.transform.position.x - camNode.transform.position.x <= 0.01f &&
                orbCam.transform.position.x - camNode.transform.position.x >= -0.01f)
            {
                orbCam.orthographicSize -= 0.05f;

                // AT MAGIC ORB LOCATION (3)
                if (orbCam.orthographicSize <= 7.5f) 
                {
                    showingNewMagicOrb = false;
                    StartCoroutine( RESUME_PLAYER() );
                }
            }
            else
            {
                // GROWING CAM (1)
                if (orbCam.orthographicSize < 10)
                {
                    orbCam.orthographicSize += 0.05f;
                    aaronOrb.SetActive(true);
                }
                // MOVING TO NEW MAGIC ORB (2)
                else 
                {
                    orbCam.transform.position = 
                        Vector3.Lerp(
                            orbCam.transform.position, 
                            new Vector3(camNode.transform.position.x, camNode.transform.position.y, orbCam.transform.position.z), 
                            camSpeed);
                    camSpeed *= 1.01f;
                }
            }
        }
    }



    /* ------------------------------ */
    /* - TRANSFORMED SPACES (TRAPS) - */


    // FILL LIST WITH THE NODE(S) THAT HAVE BEEN TURN INTO TRAPS
    public void NewTrap(string parentName, string childName, Sprite spaceType)
    {
        foreach (NodeSpace node in changedSpaces)
        {
            if (node.parentPath == parentName && node.childNode == childName) { changedSpaces.Remove(node); break; }
        }
        changedSpaces.Add( new NodeSpace(parentName, childName, spaceType) );
    }
    public void NewBoulder(string parentName, string childName, Sprite spaceType)
    {
        foreach (NodeSpace node in cavedInSpaces)
        {
            if (node.parentPath == parentName && node.childNode == childName) { cavedInSpaces.Remove(node); break; }
        }
        cavedInSpaces.Add( new NodeSpace(parentName, childName, spaceType) );
    }
    public void RemoveBoulder(string parentName, string childName)
    {
        foreach (NodeSpace node in cavedInSpaces)
        {
            if (node.parentPath == parentName && node.childNode == childName) { cavedInSpaces.Remove(node); break; }
        }
    }

    public void ResetAllTraps()
    {
        //* ALL SPELL TRAPS
        foreach(NodeSpace space in changedSpaces)
        {
            //// Debug.Log("Parent name = " + space.parentPath + ", Child name = " + space.childNode + ", Renderer = " + space.nodeType);
            Node node = GameObject.Find(space.parentPath + "/" + space.childNode).GetComponent<Node>();
            node.CHANGE_SPACE_TYPE(space.nodeType);
        }
        //* ALL CAVED IN SPOTS
        foreach(NodeSpace space in cavedInSpaces)
        {
            Node node = GameObject.Find(space.parentPath + "/" + space.childNode).GetComponent<Node>();
            node.CHANGE_SPACE_TYPE(space.nodeType);
        }
    }


    // todo --------------------------------------------------------------- */
    // todo ---------------------- MAGIC ORB RELATED ---------------------- */



    // FILL LIST WITH THE NODE(S) THAT ARE MAGIC ORB SPACES 
    public void NewOrbSpace(string parentName, string childName, Sprite spaceType)
    {
        magicOrbSpace.Add( new NodeSpace(parentName, childName, spaceType) );
    }

    // START OF NEXT TURN, RETAIN THE NODE THAT HAS THE PREVIOUSLY ACTIVE MAGIC ORB SPACE
    public void ResetCurrentMagicOrb()
    {
        int r = currentMagicOrbIndex;
        // Node node = GameObject.Find("/PATHS/" + magicOrbSpace[r].parentPath + "/" + magicOrbSpace[r].childNode).GetComponent<Node>();
        Node node = GameObject.Find(magicOrbSpace[r].parentPath + "/" + magicOrbSpace[r].childNode).GetComponent<Node>();
        camNode = node;
        node.TURN_INTO_ORB_SPACE();
    }


    // CHOOSE NEW NODE FOR MAGIC ORB SPACE
    public void CHOOSE_MAGIC_ORB_SPACE(string key)
    {
        int r = Random.Range(0, magicOrbSpace.Count);

        // DO NOT PICK THE SAME MAGIC ORB SPACE AFTER PURCHASE
        while (currentMagicOrbIndex == r) {
            r = Random.Range(0, magicOrbSpace.Count);
        }
        if (!firstMagicOrbShown) { 
            r = 0; 
            Node[] magicOrbs = new Node[magicOrbSpace.Count];
            // DESIGNATED FIRST MAGIC ORB SPACE
            for (int i=0 ; i<magicOrbs.Length ; i++) {
                magicOrbs[i] = GameObject.Find(magicOrbSpace[i].parentPath + "/" + magicOrbSpace[i].childNode).GetComponent<Node>();
                if (magicOrbs[i].firstMagicOrb) {
                    Debug.Log("  FIRST MAGIC ORB LOCATION FOUND!!!");
                    r = i; break;
                }
            }
        }
        currentMagicOrbIndex = r;

        // Debug.Log(magicOrbSpace[r].parentPath + "  -  " + magicOrbSpace[r].childNode);
        // Node node = GameObject.Find("/PATHS/" + magicOrbSpace[r].parentPath + "/" + magicOrbSpace[r].childNode).GetComponent<Node>();
        Node node = GameObject.Find(magicOrbSpace[r].parentPath + "/" + magicOrbSpace[r].childNode).GetComponent<Node>();
        camNode = node;
        node.TURN_INTO_ORB_SPACE();
        
        //  GO BACK TO PLAYER CAM
        if (key != "")
        {
            StartCoroutine( NEXT_ORB_SPACE(key) );
        }
        // 
        else 
        {
            orbCam.gameObject.SetActive(true);
            starting = true;
        }
    }

    // THE SPACE WITH THE MAGIC ORB DISAPPEARS
    public void MagicOrbBoughtController()
    {
        int r = currentMagicOrbIndex;
        Node node = GameObject.Find(magicOrbSpace[r].parentPath + "/" + magicOrbSpace[r].childNode).GetComponent<Node>();
        camNode = node;
        node.MAGIC_ORB_BOUGHT();
    }


    /* ------------------------------------------------------------------ */
    /* ------------------------ CRSYTAL CAVERNS ------------------------- */

    // FILL LIST WITH NODE(S) THAT CAN BE BOULDERED (CAVED IN)
    public void NewCavedInSpace(string parentName, string childName, int cavedSection)
    {
        if (canBeCavedSpaces == null) canBeCavedSpaces = new List<CavedSpace>();
        canBeCavedSpaces.Add( new CavedSpace(parentName, childName, cavedSection) );
    }


    public IEnumerator CAVING_IN()
    {
        for (int i=0 ; i<cavingInCamera.Length ; i++) {
            Debug.Log("Caving in camera " + (i+1));
            cavingInCamera[i].gameObject.SetActive(true);

            // GET THE POSSIBLE CAVE IN SPOTS IN CAMERA AREA
            List<Node> nodes = new List<Node>();
            foreach (var possibleCavedInSpace in canBeCavedSpaces) {
                if (possibleCavedInSpace.cavedSection == i) {
                    string pathToObject = possibleCavedInSpace.parentPath + "/" + possibleCavedInSpace.childNode;
                    nodes.Add( GameObject.Find(pathToObject).GetComponent<Node>() );
                }
            }

            //* UNBLOCK ALREADY BLOCKED, AND BLOCK A NEW SPOT
            yield return new WaitForSeconds(1f);

            // UNBLOCK OLD SPOT (IF EXISTS)
            int blocked = -1;
            for (int j=0 ; j<nodes.Count ; j++) {
                if (nodes[j].IS_BLOCKED()) {
                    Debug.LogError("--    " + j + " is blocked");
                    blocked = j; 
                    nodes[blocked].UNBLOCK();
                    break;
                }
            }

            int r = blocked;
            // NONE BLOCKED
            if (blocked < 0) {
                r = Random.Range(0, nodes.Count);
            }
            // ALREADY BLOCKED
            else {
                while (r == blocked) {
                    r = Random.Range(0, nodes.Count);
                }
            }

            // BLOCK NEW SPOT
            Debug.Log("      r  =  " + r + ",    nodes.Count  =  " + nodes.Count);
            if (nodes[r] != null) {
                nodes[r].BLOCK(); 
                var quake = Instantiate(collapsePrefab, nodes[r].transform.position, Quaternion.identity);
                Destroy(quake, 3);
            }
            
            yield return new WaitForSeconds(2f);
            nodes.Clear();
            cavingInCamera[i].gameObject.SetActive(false);
        }
    }

    private int IS_THERE_ALREADY_ONE_CAVED_IN(List<Node> nodes)
    {
        for (int i=0 ; i<nodes.Count ; i++) {
            if (nodes[i].IS_BLOCKED()) return i;
        }
        Debug.Log("    NONE BLOCKED!!!");
        return -1;
    }

    /* ---------------------------------------------------------------- */
    /* ---------------------------- SPELLS ---------------------------- */

    public void AddSpell(string player, string newSpellName, int newSpellCost, string newSpellKind)
    {
        switch (player)
        {
            case "Player_1":   spells1.Add( new SpellType(newSpellName, newSpellCost, newSpellKind));    break;
            case "Player_2":   spells2.Add( new SpellType(newSpellName, newSpellCost, newSpellKind));    break;
            case "Player_3":   spells3.Add( new SpellType(newSpellName, newSpellCost, newSpellKind));    break;
            case "Player_4":   spells4.Add( new SpellType(newSpellName, newSpellCost, newSpellKind));    break;
            case "Player_5":   spells5.Add( new SpellType(newSpellName, newSpellCost, newSpellKind));    break;
            case "Player_6":   spells6.Add( new SpellType(newSpellName, newSpellCost, newSpellKind));    break;
            case "Player_7":   spells7.Add( new SpellType(newSpellName, newSpellCost, newSpellKind));    break;
            case "Player_8":   spells8.Add( new SpellType(newSpellName, newSpellCost, newSpellKind));    break;
        }
    }


    /* ------------------------------------------------------------------ */
    /* ---------------------------- MINIGAME ---------------------------- */

    public void LOAD_MINIGAME()
    {
        SceneManager.LoadScene("Overlay"); // ** DEFAULT ** //

        // TESTING MINIGAMES BEFORE ADDING TO quests (List<>)
        // SceneManager.LoadScene("Darkness"); 
        // SceneManager.LoadScene("Colour_Chaos"); 
    }

    public void QUICK_PLAY()
    {
        SceneManager.LoadScene( quests[quests.Count - 1].questReal ); 
    }

    public void MINIGAME_PRIZE(int coin1, int coin2, int coin3, int coin4, int coin5, int coin6, int coin7, int coin8)
    {
        // Debug.Log("-- showing prize money");
        if (nPlayers >= 1)    { p1[0].coins += coin1;   questOrb[0] += coin1; }
        if (nPlayers >= 2)    { p2[0].coins += coin2;   questOrb[1] += coin2; }
        if (nPlayers >= 3)    { p3[0].coins += coin3;   questOrb[2] += coin3; }
        if (nPlayers >= 4)    { p4[0].coins += coin4;   questOrb[3] += coin4; }
        if (nPlayers >= 5)    { p5[0].coins += coin5;   questOrb[4] += coin5; }
        if (nPlayers >= 6)    { p6[0].coins += coin6;   questOrb[5] += coin6; }
        if (nPlayers >= 7)    { p7[0].coins += coin7;   questOrb[6] += coin7; }
        if (nPlayers >= 8)    { p8[0].coins += coin8;   questOrb[7] += coin8; }
    }


    
    /* ------------------------------------------------------------------ */
    /* ----------------------------- FINALE ----------------------------- */

    // PATHFOLLOWER WITH (playerID) CALLS AND RETURNS RANK OF PLAYER
    // lower = winning, higher = losing
    public int MY_RANKING(int playerID)
    {
        int[] arr = new int[nPlayers];
        int[] pid = new int[nPlayers];

        for (int i=0 ; i<nPlayers ; i++) { pid[i] = i; }

        // RANGE OF SCORES/POINTS
        if (nPlayers > 0 && p1.Count > 0)  {  arr[0] += p1[0].coins;   arr[0] += (p1[0].orbs * 1000);  }
        if (nPlayers > 1 && p2.Count > 0)  {  arr[1] += p2[0].coins;   arr[1] += (p2[0].orbs * 1000);  }
        if (nPlayers > 2 && p3.Count > 0)  {  arr[2] += p3[0].coins;   arr[2] += (p3[0].orbs * 1000);  }
        if (nPlayers > 3 && p4.Count > 0)  {  arr[3] += p4[0].coins;   arr[3] += (p4[0].orbs * 1000);  }
        if (nPlayers > 4 && p5.Count > 0)  {  arr[4] += p5[0].coins;   arr[4] += (p5[0].orbs * 1000);  }
        if (nPlayers > 5 && p6.Count > 0)  {  arr[5] += p6[0].coins;   arr[5] += (p6[0].orbs * 1000);  }
        if (nPlayers > 6 && p7.Count > 0)  {  arr[6] += p7[0].coins;   arr[6] += (p7[0].orbs * 1000);  }
        if (nPlayers > 7 && p8.Count > 0)  {  arr[7] += p8[0].coins;   arr[7] += (p8[0].orbs * 1000);  }

        /// SORT EVERYONE'S SCORES (DESCENDING)
        for ( int i=0 ; i<arr.Length ; i++ )
        {
            for ( int j=0 ; j<arr.Length ; j++ )
            {
                if (arr[i] > arr[j])
                {
                    int temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;

                    int tempID = pid[i];
                    pid[i] = pid[j];
                    pid[j] = tempID;
                }
            }
        }

        // debugging
        string test = "";
        for (int i=0 ; i<arr.Length ; i++) { test += arr[i] + " ,"; }
        Debug.Log(test);

        
        // FIRST -> LAST
        for ( int i=0 ; i<arr.Length ; i++ )  
        { 
            if (playerID == pid[i])  { Debug.Log("Player " + playerID + " is in " + CARDINAL_RANK(i)); return i; } 
        }
        Debug.Log("Player " + playerID + " is NOT FOUND!!");
        return 9;
    }

    // RETURN RANKIING (MORE COMPLICATED VER OF [ MY_RANKING() ] )
    public int DISPLAY_PLAYER_RANKINGS(int playerID)
    {
        int[] arr = new int[nPlayers];
        int[] unsorted = new int[nPlayers];
        int[] pid = new int[nPlayers];

        for (int i=0 ; i<nPlayers ; i++) { pid[i] = i; }

        // RANGE OF SCORES/POINTS
        if (p1.Count > 0 && nPlayers > 0)  {  arr[0] += p1[0].coins; arr[0] += (p1[0].orbs * 1000);  unsorted[0] = arr[0];  }
        if (p2.Count > 0 && nPlayers > 1)  {  arr[1] += p2[0].coins; arr[1] += (p2[0].orbs * 1000);  unsorted[1] = arr[1];  }
        if (p3.Count > 0 && nPlayers > 2)  {  arr[2] += p3[0].coins; arr[2] += (p3[0].orbs * 1000);  unsorted[2] = arr[2];  }
        if (p4.Count > 0 && nPlayers > 3)  {  arr[3] += p4[0].coins; arr[3] += (p4[0].orbs * 1000);  unsorted[3] = arr[3];  }
        if (p5.Count > 0 && nPlayers > 4)  {  arr[4] += p5[0].coins; arr[4] += (p5[0].orbs * 1000);  unsorted[4] = arr[4];  }
        if (p6.Count > 0 && nPlayers > 5)  {  arr[5] += p6[0].coins; arr[5] += (p6[0].orbs * 1000);  unsorted[5] = arr[5];  }
        if (p7.Count > 0 && nPlayers > 6)  {  arr[6] += p7[0].coins; arr[6] += (p7[0].orbs * 1000);  unsorted[6] = arr[6];  }
        if (p8.Count > 0 && nPlayers > 7)  {  arr[7] += p8[0].coins; arr[7] += (p8[0].orbs * 1000);  unsorted[7] = arr[7];  }

        /// SORT EVERYONE'S SCORES (DESCENDING)
        for ( int i=0 ; i<arr.Length ; i++ )
        {
            for ( int j=0 ; j<arr.Length ; j++ )
            {
                if (arr[i] > arr[j])
                {
                    int temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;

                    int tempID = pid[i];
                    pid[i] = pid[j];
                    pid[j] = tempID;
                }
            }
        }

        
        // FIRST -> LAST
        for ( int i=0 ; i<arr.Length ; i++ )
        {
            // IF THE SCORE IS THE SAME IS AS THE PREVIOUS SCORE, THEN SKIP
            if (i != 0) if (arr[i] == arr[i-1]) continue;

            // ALL PLAYERS WHO HAD THAT SCORE
            for ( int j=0 ; j<nPlayers ; j++)
            {
                if (arr[i] == (unsorted[playerID]))
                {
                    return i;
                }
            }
        }
        return 9;
    }

    public string THE_WINNER()
    {
        // List<ImportantData>[] playersData = new List<ImportantData>[nPlayers];
        float[]  arr   = new float[nPlayers];
        string[] names = new string[nPlayers];

        if (p1.Count > 0 && nPlayers > 0) {
            arr[0] += p1[0].coins;
            arr[0] += (p1[0].orbs * 1000);
            names[0] = characterName1;
        }
        if (p2.Count > 0 && nPlayers > 1) {
            arr[1] += p2[0].coins;
            arr[1] += (p2[0].orbs * 1000);
            names[1] = characterName2;
        }
        if (p3.Count > 0 && nPlayers > 2) {
            arr[2] += p3[0].coins;
            arr[2] += (p3[0].orbs * 1000);
            names[2] = characterName3;
        }
        if (p4.Count > 0 && nPlayers > 3) {
            arr[3] += p4[0].coins;
            arr[3] += (p4[0].orbs * 1000);
            names[3] = characterName4;
        }
        if (p5.Count > 0 && nPlayers > 4) {
            arr[4] += p5[0].coins;
            arr[4] += (p5[0].orbs * 1000);
            names[4] = characterName5;
        }
        if (p6.Count > 0 && nPlayers > 5) {
            arr[5] += p6[0].coins;
            arr[5] += (p6[0].orbs * 1000);
            names[5] = characterName6;
        }
        if (p7.Count > 0 && nPlayers > 6) {
            arr[6] += p7[0].coins;
            arr[6] += (p7[0].orbs * 1000);
            names[6] = characterName7;
        }
        if (p8.Count > 0 && nPlayers > 7) {
            arr[7] += p8[0].coins;
            arr[7] += (p8[0].orbs * 1000);
            names[7] = characterName8;
        }

        // SORT THE HIGHEST POINT VALUES (ASCENDING ORDER)
        for ( int i=0 ; i<arr.Length ; i++ )
        {
            for ( int j=0 ; j<arr.Length ; j++ )
            {
                if (arr[i] < arr[j])
                {
                    float  temp     = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                    string nameTemp = names[i];
                    names[i] = names[j];
                    names[j] = nameTemp;
                }
            }
        }

        return names[nPlayers-1];
    }
    public List<int> THE_LOSER()
    {
        int[] arr = new int[nPlayers];  // SCORE
        int[] pid = new int[nPlayers];  // player ID

        if (p1.Count > 0 && nPlayers > 0) {
            arr[0] += p1[0].coins;
            arr[0] += (p1[0].orbs * 1000);
            pid[0] = 0;
        }
        if (p2.Count > 0 && nPlayers > 1) {
            arr[1] += p2[0].coins;
            arr[1] += (p2[0].orbs * 1000);
            pid[1] = 1;
        }
        if (p3.Count > 0 && nPlayers > 2) {
            arr[2] += p3[0].coins;
            arr[2] += (p3[0].orbs * 1000);
            pid[2] = 2;
        }
        if (p4.Count > 0 && nPlayers > 3) {
            arr[3] += p4[0].coins;
            arr[3] += (p4[0].orbs * 1000);
            pid[3] = 3;
        }
        if (p5.Count > 0 && nPlayers > 4) {
            arr[4] += p5[0].coins;
            arr[4] += (p5[0].orbs * 1000);
            pid[4] = 4;
        }
        if (p6.Count > 0 && nPlayers > 5) {
            arr[5] += p6[0].coins;
            arr[5] += (p6[0].orbs * 1000);
            pid[5] = 5;
        }
        if (p7.Count > 0 && nPlayers > 6) {
            arr[6] += p7[0].coins;
            arr[6] += (p7[0].orbs * 1000);
            pid[6] = 6;
        }
        if (p8.Count > 0 && nPlayers > 7) {
            arr[7] += p8[0].coins;
            arr[7] += (p8[0].orbs * 1000);
            pid[7] = 7;
        }
        // SORT THE HIGHEST POINT VALUES (DESCENDING ORDER)
        for ( int i=0 ; i<arr.Length ; i++ )
        {
            for ( int j=0 ; j<arr.Length ; j++ )
            {
                if (arr[i] > arr[j])
                {
                    int temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                    int itemp = pid[i];
                    pid[i] = pid[j];
                    pid[j] = itemp;
                }
            }
        }

        // PLAYER(S) WITH THE HIGHEST SCORE
        List<int> losing = new List<int>();
        for (int i=0 ; i<arr.Length ; i++) { if (arr[nPlayers-1] == arr[i]) { losing.Add( pid[i] ); } }

        return (losing);
    }

    public void RICH_ORB_UPDATE(int x)
    {
        if (richOrb == null)    { richOrb = new int[nPlayers]; }

        switch (x)
        {
            case 0 :    if (p1[0].coins > richOrb[x])   richOrb[x] = p1[0].coins; break;
            case 1 :    if (p2[0].coins > richOrb[x])   richOrb[x] = p2[0].coins; break;
            case 2 :    if (p3[0].coins > richOrb[x])   richOrb[x] = p3[0].coins; break;
            case 3 :    if (p4[0].coins > richOrb[x])   richOrb[x] = p4[0].coins; break;
            case 4 :    if (p5[0].coins > richOrb[x])   richOrb[x] = p5[0].coins; break;
            case 5 :    if (p6[0].coins > richOrb[x])   richOrb[x] = p6[0].coins; break;
            case 6 :    if (p7[0].coins > richOrb[x])   richOrb[x] = p7[0].coins; break;
            case 7 :    if (p8[0].coins > richOrb[x])   richOrb[x] = p8[0].coins; break;
        }
        string rich = "-- rich update  ";
        for (int i=0 ; i<nPlayers ; i++)
        {
            if (i==0)   rich += p1[0].coins.ToString() + ", ";
            if (i==1)   rich += p2[0].coins.ToString() + ", ";
            if (i==2)   rich += p3[0].coins.ToString() + ", ";
            if (i==3)   rich += p4[0].coins.ToString() + ", ";
            if (i==4)   rich += p5[0].coins.ToString() + ", ";
            if (i==5)   rich += p6[0].coins.ToString() + ", ";
            if (i==6)   rich += p7[0].coins.ToString() + ", ";
            if (i==7)   rich += p8[0].coins.ToString() + ", ";   
        }

        // Debug.Log(rich); //! DELETE
    }

    public void EVENT_ORB_UPDATE(int playerIndex)
    {
        if (eventOrb == null)    { eventOrb = new int[nPlayers]; }
        if (eventOrb.Length > playerIndex) eventOrb[playerIndex]++;
        else Debug.LogError("ERROR event space update FAIL");
    }

    public void RED_ORB_UPDATE(int playerIndex)
    {
        if (redOrb == null)    { redOrb = new int[nPlayers]; }
        if (redOrb.Length > playerIndex) redOrb[playerIndex]++;
        else Debug.LogError("ERROR red space update FAIL");
    }
    
    public void SLOW_ORB_UPDATE(int playerIndex, int amount)
    {
        if (slowOrb == null)    { slowOrb = new int[nPlayers]; }
        slowOrb[playerIndex] += amount;
    }
    
    public void SHOP_ORB_UPDATE(int playerIndex, int amount)
    {
        if (shopOrb == null)    { shopOrb = new int[nPlayers]; }
        shopOrb[playerIndex] += amount;
    }

    private void ALL_PLAYER_DATA()
    {
        if (playersData == null)    playersData = new List<PlayerPrevData>[nPlayers];
        for ( int i=0 ; i<nPlayers ; i++ )
        {
            switch (i)
            {
                case 0 :    playersData[i] = p1; break;
                case 1 :    playersData[i] = p2; break;
                case 2 :    playersData[i] = p3; break;
                case 3 :    playersData[i] = p4; break;
                case 4 :    playersData[i] = p5; break;
                case 5 :    playersData[i] = p6; break;
                case 6 :    playersData[i] = p7; break;
                case 7 :    playersData[i] = p8; break;
            }
        }
    }

    public void Finalise_Data()
    {
        allGold = new List<int>();
        if (nPlayers >= 1)  allGold.Add( p1[0].coins );
        if (nPlayers >= 2)  allGold.Add( p2[0].coins );
        if (nPlayers >= 3)  allGold.Add( p3[0].coins );
        if (nPlayers >= 4)  allGold.Add( p4[0].coins );
        if (nPlayers >= 5)  allGold.Add( p5[0].coins );
        if (nPlayers >= 6)  allGold.Add( p6[0].coins );
        if (nPlayers >= 7)  allGold.Add( p7[0].coins );
        if (nPlayers >= 8)  allGold.Add( p8[0].coins );
        allOrb  = new List<int>();
        if (nPlayers >= 1)  allOrb.Add( p1[0].orbs );
        if (nPlayers >= 2)  allOrb.Add( p2[0].orbs );
        if (nPlayers >= 3)  allOrb.Add( p3[0].orbs );
        if (nPlayers >= 4)  allOrb.Add( p4[0].orbs );
        if (nPlayers >= 5)  allOrb.Add( p5[0].orbs );
        if (nPlayers >= 6)  allOrb.Add( p6[0].orbs );
        if (nPlayers >= 7)  allOrb.Add( p7[0].orbs );
        if (nPlayers >= 8)  allOrb.Add( p8[0].orbs );
        trapOrb   = new int[nPlayers];
        foreach (NodeSpace trap in changedSpaces)
        {
            if (nPlayers >= 1 && trap.nodeType.name.Contains(characterName1)) { trapOrb[0]++;   continue; }
            if (nPlayers >= 2 && trap.nodeType.name.Contains(characterName2)) { trapOrb[1]++;   continue; }
            if (nPlayers >= 3 && trap.nodeType.name.Contains(characterName3)) { trapOrb[2]++;   continue; }
            if (nPlayers >= 4 && trap.nodeType.name.Contains(characterName4)) { trapOrb[3]++;   continue; }
            if (nPlayers >= 5 && trap.nodeType.name.Contains(characterName5)) { trapOrb[4]++;   continue; }
            if (nPlayers >= 6 && trap.nodeType.name.Contains(characterName6)) { trapOrb[5]++;   continue; }
            if (nPlayers >= 7 && trap.nodeType.name.Contains(characterName7)) { trapOrb[6]++;   continue; }
            if (nPlayers >= 8 && trap.nodeType.name.Contains(characterName8)) { trapOrb[7]++;   continue; }
        }
    }

    public (List<int>, int) CALCULATE_RICH_WINNER()
    {
        int[] arr = new int[nPlayers];  // SCORE
        int[] pid = new int[nPlayers];  // player ID

        for ( int i=0 ; i<nPlayers ; i++ ) { arr[i] = richOrb[i]; }
        for ( int i=0 ; i<nPlayers ; i++ ) { pid[i] = i; }

        // SORT THE HIGHEST POINT VALUES (ASCENDING ORDER)
        for ( int i=0 ; i<arr.Length ; i++ )
        {
            for ( int j=0 ; j<arr.Length ; j++ )
            {
                if (arr[i] > arr[j])
                {
                    int temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                    int itemp = pid[i];
                    pid[i] = pid[j];
                    pid[j] = itemp;
                }
            }
        }

        string rich = "-- richWinner update  ";
        for (int i=0 ; i<nPlayers ; i++) { rich += arr[i].ToString() + ", "; }
        Debug.Log(rich);

        // PLAYER(S) WITH THE HIGHEST SCORE
        List<int> richest = new List<int>();
        for (int i=0 ; i<arr.Length ; i++) { if (arr[0] == arr[i]) { richest.Add( pid[i] ); } }

        return (richest, arr[0]);
    }
    public (List<int>, int) CALCULATE_EVENT_WINNER()
    {
        if (eventOrb == null)    { eventOrb = new int[nPlayers]; }

        int[] arr = new int[nPlayers];  // SCORE
        int[] pid = new int[nPlayers];  // player ID

        for ( int i=0 ; i<nPlayers ; i++ ) { arr[i] = eventOrb[i]; }
        for ( int i=0 ; i<nPlayers ; i++ ) { pid[i] = i; }

        // SORT THE HIGHEST POINT VALUES (ASCENDING ORDER)
        for ( int i=0 ; i<arr.Length ; i++ )
        {
            for ( int j=0 ; j<arr.Length ; j++ )
            {
                if (arr[i] > arr[j])
                {
                    int temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                    int itemp = pid[i];
                    pid[i] = pid[j];
                    pid[j] = itemp;
                }
            }
        }

        string rich = "-- eventWinner update  ";
        for (int i=0 ; i<nPlayers ; i++) { rich += arr[i].ToString() + ", "; }
        Debug.Log(rich);
        
        // PLAYER(S) WITH THE HIGHEST SCORE
        List<int> eventest = new List<int>();
        for (int i=0 ; i<arr.Length ; i++) { if (arr[0] == arr[i]) { eventest.Add( pid[i] ); } }

        return (eventest, arr[0]);
    }
    public (List<int>, int) CALCULATE_RED_WINNER()
    {
        if (redOrb == null)    { redOrb = new int[nPlayers]; }

        int[] arr = new int[nPlayers];  // SCORE
        int[] pid = new int[nPlayers];  // player ID

        for ( int i=0 ; i<nPlayers ; i++ ) { arr[i] = redOrb[i]; }
        for ( int i=0 ; i<nPlayers ; i++ ) { pid[i] = i; }

        // SORT THE HIGHEST POINT VALUES (ASCENDING ORDER)
        for ( int i=0 ; i<arr.Length ; i++ )
        {
            for ( int j=0 ; j<arr.Length ; j++ )
            {
                if (arr[i] > arr[j])
                {
                    int temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                    int itemp = pid[i];
                    pid[i] = pid[j];
                    pid[j] = itemp;
                }
            }
        }

        string rich = "-- redWinner update  ";
        for (int i=0 ; i<nPlayers ; i++) { rich += arr[i].ToString() + ", "; }
        Debug.Log(rich);
        
        // PLAYER(S) WITH THE HIGHEST SCORE
        List<int> redest = new List<int>();
        for (int i=0 ; i<arr.Length ; i++) { if (arr[0] == arr[i]) { redest.Add( pid[i] ); } }

        return (redest, arr[0]);
    }
    public (List<int>, int) CALCULATE_SLOW_WINNER()
    {
        if (slowOrb == null)    { slowOrb = new int[nPlayers]; }

        int[] arr = new int[nPlayers];  // SCORE
        int[] pid = new int[nPlayers];  // player ID

        for ( int i=0 ; i<nPlayers ; i++ ) { arr[i] = slowOrb[i]; }
        for ( int i=0 ; i<nPlayers ; i++ ) { pid[i] = i; }

        // SORT THE HIGHEST POINT VALUES (ASCENDING ORDER)
        for ( int i=0 ; i<arr.Length ; i++ )
        {
            for ( int j=0 ; j<arr.Length ; j++ )
            {
                if (arr[i] < arr[j])
                {
                    int temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                    int itemp = pid[i];
                    pid[i] = pid[j];
                    pid[j] = itemp;
                }
            }
        }

        string rich = "-- slowWinner update  ";
        for (int i=0 ; i<nPlayers ; i++) { rich += arr[i].ToString() + ", "; }
        Debug.Log(rich);
        
        // PLAYER(S) WITH THE HIGHEST SCORE
        List<int> slowest = new List<int>();
        for (int i=0 ; i<arr.Length ; i++) { if (arr[0] == arr[i]) { slowest.Add( pid[i] ); } }

        return (slowest, arr[0]);
    }
    public (List<int>, int) CALCULATE_SHOP_WINNER()
    {
        if (shopOrb == null)    { shopOrb = new int[nPlayers]; }

        int[] arr = new int[nPlayers];  // SCORE
        int[] pid = new int[nPlayers];  // player ID

        for ( int i=0 ; i<nPlayers ; i++ ) { arr[i] = shopOrb[i]; }
        for ( int i=0 ; i<nPlayers ; i++ ) { pid[i] = i; }

        // SORT THE HIGHEST POINT VALUES (ASCENDING ORDER)
        for ( int i=0 ; i<arr.Length ; i++ )
        {
            for ( int j=0 ; j<arr.Length ; j++ )
            {
                if (arr[i] > arr[j])
                {
                    int temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                    int itemp = pid[i];
                    pid[i] = pid[j];
                    pid[j] = itemp;
                }
            }
        }

        string rich = "-- fastWinner update  ";
        for (int i=0 ; i<nPlayers ; i++) { rich += arr[i].ToString() + ", "; }
        Debug.Log(rich);
        
        // PLAYER(S) WITH THE HIGHEST SCORE
        List<int> fastest = new List<int>();
        for (int i=0 ; i<arr.Length ; i++) { if (arr[0] == arr[i]) { fastest.Add( pid[i] ); } }

        return (fastest, arr[0]);
    }
    public (List<int>, int) CALCULATE_TRAP_WINNER()
    {
        int[] arr = new int[nPlayers];  // SCORE
        int[] pid = new int[nPlayers];  // player ID
        trapOrb   = new int[nPlayers];

        foreach (NodeSpace trap in changedSpaces)
        {
            if (trap.nodeType.name.Contains(characterName1)) { arr[0]++;   continue; }
            if (trap.nodeType.name.Contains(characterName2)) { arr[1]++;   continue; }
            if (trap.nodeType.name.Contains(characterName3)) { arr[2]++;   continue; }
            if (trap.nodeType.name.Contains(characterName4)) { arr[3]++;   continue; }
            if (trap.nodeType.name.Contains(characterName5)) { arr[4]++;   continue; }
            if (trap.nodeType.name.Contains(characterName6)) { arr[5]++;   continue; }
            if (trap.nodeType.name.Contains(characterName7)) { arr[6]++;   continue; }
            if (trap.nodeType.name.Contains(characterName8)) { arr[7]++;   continue; }
        }

        for ( int i=0 ; i<nPlayers ; i++)
        {
            if (i == 0) { Debug.Log("Trap Bonus : " + characterName1 + ", nTraps " + arr[i]); continue; }
            if (i == 1) { Debug.Log("Trap Bonus : " + characterName2 + ", nTraps " + arr[i]);   continue; }
            if (i == 2) { Debug.Log("Trap Bonus : " + characterName3 + ", nTraps " + arr[i]);   continue; }
            if (i == 3) { Debug.Log("Trap Bonus : " + characterName4 + ", nTraps " + arr[i]);   continue; }
            if (i == 4) { Debug.Log("Trap Bonus : " + characterName5 + ", nTraps " + arr[i]);   continue; }
            if (i == 5) { Debug.Log("Trap Bonus : " + characterName6 + ", nTraps " + arr[i]);   continue; }
            if (i == 6) { Debug.Log("Trap Bonus : " + characterName7 + ", nTraps " + arr[i]);   continue; }
            if (i == 7) { Debug.Log("Trap Bonus : " + characterName8 + ", nTraps " + arr[i]);   continue; }
        }
        //// for ( int i=0 ; i<nPlayers ; i++ ) { arr[i] = eventOrb[i]; }
        for ( int i=0 ; i<nPlayers ; i++ ) { pid[i] = i; }

        // SORT THE HIGHEST POINT VALUES (ASCENDING ORDER)
        for ( int i=0 ; i<arr.Length ; i++ )
        {
            for ( int j=0 ; j<arr.Length ; j++ )
            {
                if (arr[i] > arr[j])
                {
                    int temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                    int itemp = pid[i];
                    pid[i] = pid[j];
                    pid[j] = itemp;
                }
            }
        }

        string rich = "-- eventWinner update  ";
        for (int i=0 ; i<nPlayers ; i++) { rich += arr[i].ToString() + ", "; }
        Debug.Log(rich);
        
        // PLAYER(S) WITH THE HIGHEST SCORE
        List<int> richest = new List<int>();
        for (int i=0 ; i<arr.Length ; i++) { if (arr[0] == arr[i]) { richest.Add( pid[i] ); } }

        return (richest, arr[0]);
    }

    public void BONUS_PRIZE(int index)
    {
        switch (index)
        {
            case 0 : p1[0].orbs++; break;
            case 1 : p2[0].orbs++; break;
            case 2 : p3[0].orbs++; break;
            case 3 : p4[0].orbs++; break;
            case 4 : p5[0].orbs++; break;
            case 5 : p6[0].orbs++; break;
            case 6 : p7[0].orbs++; break;
            case 7 : p8[0].orbs++; break;
        }
    }

    public string ID_TO_NAME(int index)
    {
        switch (index)
        {
            case 0 : return (characterName1);
            case 1 : return (characterName2);
            case 2 : return (characterName3);
            case 3 : return (characterName4);
            case 4 : return (characterName5);
            case 5 : return (characterName6);
            case 6 : return (characterName7);
            case 7 : return (characterName8);
        }
        return null;
    }
    public int NAME_TO_ID(string name)
    {
        if      (name == characterName1) return 0;
        else if (name == characterName2) return 1;
        else if (name == characterName3) return 2;
        else if (name == characterName4) return 3;
        else if (name == characterName5) return 4;
        else if (name == characterName6) return 5;
        else if (name == characterName7) return 6;
        else if (name == characterName8) return 7;
        return 0;
    }

    public string CARDINAL_RANK(int xth)
    {
        switch (xth)
        {
            case 0:   return "1st";
            case 1:   return "2nd";
            case 2:   return "3rd";
            case 3:   return "4th";
            case 4:   return "5th";
            case 5:   return "6th";
            case 6:   return "7th";
            case 7:   return "8th";
        }
        return "0rd";
    }



}


// ************************************************************************************************
// ************************************************************************************************

public class PlayerPrevData
{
    public string path;     // PLAYER'S CURRENT PATH
    public int    node;    // PLAYER'S CURRENT POSITION IN PATH (NODE)
    public Vector3 pos;    // LAST VECTOR 3 POSITION
    public Vector3 posAside;   // PLAYER'S ASIDE POSITION
    public int     coins;
    public int     orbs;
    public int     mp;
    public int     prize;   // GOLD WON FROM MINIGAME (QUESTS)
    public bool    firstPlace;   // CAME FIRST IN MINIGAME (QUESTS)

    public PlayerPrevData(string newPath, int newNode, Vector3 newPos, 
            Vector3 newPosAside, int newCoins, int newOrbs, int newMp, int newPrize, bool first)
    {
        path        = newPath;
        node        = newNode;  //! DELETE
        pos         = newPos;
        posAside    = newPosAside;
        coins       = newCoins;
        orbs        = newOrbs;
        mp          = newMp;
        prize       = newPrize;
        firstPlace  = first;
    }
}


public class PlayerBuffData
{
    public bool slowed;
    public bool barrier;
    public bool move15;
    public bool range2;
    public bool extraBuy;


    public PlayerBuffData(bool newSlowed, bool newBarrier, bool newMove15, bool newRange2, bool newExtraBuy)
    {
        slowed  = newSlowed;
        barrier = newBarrier;
        move15  = newMove15;
        range2  = newRange2;
        extraBuy = newExtraBuy;
    }
}


public class ImportantData
{
    public int orbs;
    public int golds;

    public ImportantData(int newOrbs, int newGolds)
    {
        orbs    = newOrbs;
        golds   = newGolds;
    }
}


public class Quest
{
    public string questMini;    // PREVIEW PRACTICE SCENE (MINI)
    public string questReal;    // ACTUAL SCENE

    // ** TYPE OF MINIGAME ( COIN / BATTLE / FREE FOR ALL )

    public Quest(string newMini, string newReal)
    {
        questMini = newMini;
        questReal = newReal;
    }
}