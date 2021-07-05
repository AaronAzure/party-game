using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PreviewOverlay : MonoBehaviour
{
    private GameController controller;      // TO BE SET
    private List<Quest> quests; // FROM CONTROLLER
    private string SceneMiniName;
    private string SceneRealName;
    [SerializeField] private Image blackScreen;    // INSPECTOR
    [SerializeField] private AudioSource bgMusic;    // INSPECTOR
    [SerializeField] private TextMeshProUGUI readyText;
    private float transitionTime = 0.5f;

    // LOAD GAME CONDITIONS
    public int nReady = 0;

    [Header("Quest Description Related Data")]
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI desc;
    
    [SerializeField] private GameObject      map1;
    [SerializeField] private Image           pic1;
    [SerializeField] private TextMeshProUGUI txt1;
    [SerializeField] private GameObject      map2;
    [SerializeField] private Image           pic2;
    [SerializeField] private TextMeshProUGUI txt2;
    [SerializeField] private GameObject      map3;
    [SerializeField] private Image           pic3;
    [SerializeField] private TextMeshProUGUI txt3;

    [Header("Picture Sprites")]
    [SerializeField] private Sprite buttonEast;
    [SerializeField] private Sprite buttonWest;
    [SerializeField] private Sprite buttonNorth;
    [SerializeField] private Sprite buttonSouth;
    [SerializeField] private Sprite buttonAny;
    [SerializeField] private Sprite stickHorizontal;
    [SerializeField] private Sprite stickVertical;
    [SerializeField] private Sprite stickEverything;



    // -----------------------------------------------------------------------------------------------

    private void Start() 
    {
        controller = GameObject.Find("Game_Controller").GetComponent<GameController>();
        // if (controller.quests == null) { controller.RESET_QUESTS(); }
        if (controller.quests.Count == 0) { 
            controller.RESET_QUESTS(); 
            controller.SET_AVAILABLE_QUEST();
        }

        int questIndex = 0; // ** DEFAULT ** //

        // MINIGAME MODE 
        if (controller.minigameMode)
        {
            Debug.Log("is minigame mode");
            for (int i=0 ; i<controller.quests.Count ; i++)
            {
                if (controller.quests[i].questMini == controller.questToPlay)
                {
                    Debug.Log("playing minigame " + controller.quests[i].questMini);
                    questIndex = i;
                    break;
                }
                if (i == controller.quests.Count - 1) {Debug.LogError("COULDN'T FIND QUEST");}
            }
        }
        // BOARD MODE
        else
        {
            questIndex = Random.Range(0, controller.quests.Count);  
        }
        
        if (controller.playLatestQuest) questIndex = controller.quests.Count - 1;  // ** RECENTLY ADDED MINIGAME ** //

        SceneMiniName  = controller.quests[questIndex].questMini;
        SceneRealName  = controller.quests[questIndex].questReal;
        if (!controller.minigameMode) controller.quests.RemoveAt(questIndex); // DON'T PLAY THE SAME MINIGAME AGAIN
        title.text = SceneMiniName;

        blackScreen.gameObject.SetActive(true);
        blackScreen.CrossFadeAlpha(0f, transitionTime, false);
        SceneManager.LoadSceneAsync(SceneMiniName, LoadSceneMode.Additive);

        MINIGAME_DESCRIPTION();
    }

    private void MINIGAME_DESCRIPTION()
    {
        // MINIGAME DESCRIPTION AND CONTROLS
        map2.SetActive(false);
        map3.SetActive(false);
        switch (SceneMiniName)
        {
            case "Sneak And Snore" : 
                desc.text = "Collect as many Gold as possible, then head for the exit.\n";
                desc.text += "Use your invisibility spell before the beast AWAKES!"; 
                // NUMBER OF CONTROLS NEEDED
                map1.SetActive(true);
                pic1.sprite = stickHorizontal;
                txt1.text = "Move";
                map2.SetActive(true);
                pic2.sprite = buttonEast;
                txt2.text = "Turn invisible (hold)";
                break;
            case "Feast-ival" : 
                desc.text = "Eat as much dumplings before time runs out.\nRepeatedly press A to eat dumplings.";
                map1.SetActive(true);
                pic1.sprite = buttonEast;
                txt1.text = "Eat Dumpling";
                break;
            case "Colour Chaos" : 
                desc.text = "Avoid standing on the coloured tile that the host Aaron calls out.\n";
                desc.text += "Otherwise, you will be eliminated.";
                map1.SetActive(true);
                pic1.sprite = stickEverything;
                txt1.text = "Move";
                break;
            case "Card Collectors" : 
                desc.text = "Collect as many points from the moving cards as possible.\n";
                desc.text += "You can collect multiple cards at once.\n";
                desc.text += "There is a cooldown before you can another card.";
                map1.SetActive(true);
                pic1.sprite = stickEverything;
                txt1.text = "Move";
                map2.SetActive(true);
                pic2.sprite = buttonEast;
                txt2.text = "Collect card(s)";
                break;
            case "Leaf Leap" : 
                desc.text = "Whoever climbs the beanstalk the highest wins.\n";
                desc.text += "Jump left or right onto the branching leaves.";
                map1.SetActive(true);
                pic1.sprite = stickHorizontal;
                txt1.text = "Leap left/right";
                break;
            case "Lava Or Leave 'Em" : 
                desc.text = "Last man standing rules.\n";
                desc.text += "Evasive maneuver around the lava guardians.\n";
                desc.text += "Avoid the blazing tracks they leave behind.\n";
                desc.text += "And do not run out of bounds.\n";
                map1.SetActive(true);
                pic1.sprite = stickEverything;
                txt1.text = "Move";
                break;
            case "Lilypad Leapers" : 
                desc.text = "Be the first to reach the treasure island by leaping across lilypads.\n";
                desc.text += "Press the indicated button to leap left to right.\n";
                desc.text += "If you press the wrong button, you will leap to the wrong lilypad.\n";
                map1.SetActive(true);
                pic1.sprite = buttonAny;
                txt1.text = "Leap to next lilypad";
                break;
            case "Stop Watchers" : 
                desc.text = "Stop your timer as close to the specified time as possible.\n";
                desc.text += "The timer display disappears after a while.\n";
                map1.SetActive(true);
                pic1.sprite = buttonEast;
                txt1.text = "Stop timer";
                break;
            case "Spotlight Fight" : 
                desc.text = "Whoever gets the most spotlight wins.\n";
                desc.text += "Stay in the spotlight for as long as possible.\n";
                desc.text += "Charge and aim to dash into your opponents and send them flying.\n";
                map1.SetActive(true);
                pic1.sprite = stickEverything;
                txt1.text = "Move / Direction of dash";
                map2.SetActive(true);
                pic2.sprite = buttonEast;
                txt2.text = "Charge acceleration";
                break;
            case "Pushy Penguins" : 
                desc.text = "Do not let the penguins push you off the cliff (on the left).\n";
                desc.text += "Last man standing wins.\n";
                map1.SetActive(true);
                pic1.sprite = stickEverything;
                txt1.text = "Move";
                break;
            case "Fun Run" : 
                desc.text = "Race to finish line at the top.\n";
                desc.text += "Avoid obstacles and avoid falling off the course.\n";
                map1.SetActive(true);
                pic1.sprite = stickEverything;
                txt1.text = "Move / Direction of dash";
                map2.SetActive(true);
                pic2.sprite = buttonEast;
                txt2.text = "Charge acceleration";
                break;
            case "Coin-veyor" : 
                desc.text = "Open the boxes to reveal its contents.\n";
                desc.text += "Gain gold equal to the number of gold inside each box you open.\n";
                desc.text += "You lose three gold for every box containing a bomb you open.\n";
                map1.SetActive(true);
                pic1.sprite = buttonEast;
                txt1.text = "Open box";
                break;
            case "Stamp By Me" : 
                desc.text = "Stamp and approve documents, but do not discard them.\n";
                desc.text += "Discard all malicious documents, but do not stamp and approve them.\n";
                map1.SetActive(true);
                pic1.sprite = buttonEast;
                txt1.text = "Stamp paper";
                map2.SetActive(true);
                pic2.sprite = buttonSouth;
                txt2.text = "Discard paper";
                break;
            case "Shocking Situation" : 
                desc.text = "Race around the room and collect as many gold as possible.\n";
                desc.text += "Avoid the electricty coursing through the room.\n";
                desc.text += "More gold will appear when then are none left.\n";
                map1.SetActive(true);
                pic1.sprite = stickEverything;
                txt1.text = "Move";
                break;
            case "Attack Of Titan" : 
                desc.text = "Activate the magic circle and hope you acquire the titanization spell.\n";
                desc.text += "If you are the titan, run and crush your opponents.\n";
                desc.text += "Otherwise, evade the titan until the spell wears off.\n";
                map1.SetActive(true);
                pic1.sprite = stickEverything;
                txt1.text = "Move";
                map2.SetActive(true);
                pic2.sprite = buttonEast;
                txt2.text = "Activate magic circle";
                break;
            case "Flower Shower" : 
                desc.text = "Gather the most flower falling from the sky to win.\n";
                desc.text += "Gold flowers are worth <b>three points</b>.\n";
                map1.SetActive(true);
                pic1.sprite = stickEverything;
                txt1.text = "Move";
                break;
            case "Don't Be A Zombie" : 
                desc.text = "Run away from the zombies. Last man standing wins!\n";
                desc.text += "If you turn into a zombie, you can infect other humans.\n";
                map1.SetActive(true);
                pic1.sprite = stickEverything;
                txt1.text = "Move";
                break;
            case "Barrier Bearers" : 
                desc.text = "Evade all spells cast by the great Wee-zard Aaron!\n";
                desc.text += "Hold A and consume mana to cast barrier,\n";
                desc.text += "which protects yourself from all attacks.\n";
                // desc.text += "Stay stationary to eventually and slowly restore mana.\n";
                map1.SetActive(true);
                pic1.sprite = stickEverything;
                txt1.text = "Move";
                map2.SetActive(true);
                pic2.sprite = buttonEast;
                txt2.text = "Cast Barrier Spell";
                break;
            case "Plunder Ground" : 
                desc.text = "Traverse through the ground to find buried gems.\n";
                desc.text += "Collect the most gem to win!\n";
                desc.text += "Press A or B to alter your field of vision and speed.\n";
                // desc.text += "Stay stationary to eventually and slowly restore mana.\n";
                map1.SetActive(true);
                pic1.sprite = stickEverything;
                txt1.text = "Move";
                map2.SetActive(true);
                pic2.sprite = buttonEast;
                txt2.text = "Increase field of vision & decrease speed";
                map3.SetActive(true);
                pic3.sprite = buttonSouth;
                txt3.text = "Decrease field of vision & increase speed";
                break;
            case "Pinpoint The Endpoint" : 
                desc.text = "Follow the ball. The ball will turn invisble after a while.\n";
                desc.text += "Predict where the ball will end up.\n";
                desc.text += "The closest player to the ball wins!\n";
                map1.SetActive(true);
                pic1.sprite = stickEverything;
                txt1.text = "Move";
                break;
            case "Camo Cutters" : 
                desc.text = "Last man standing rules.\n";
                desc.text += "Avoid the magical saw blades,\n";
                desc.text += "which can camoflage in its own colour.\n";
                map1.SetActive(true);
                pic1.sprite = stickEverything;
                txt1.text = "Move";
                break;
            case "County Bounty" : 
                desc.text = "Count how many foxes there are.\n";
                desc.text += "Whoever is the closest to or gets the correct answer wins!";
                // desc.text += "which can camoflage in its own colour.\n";
                map1.SetActive(true);
                pic1.sprite = buttonEast;
                txt1.text = "Increase count";
                map2.SetActive(true);
                pic2.sprite = buttonSouth;
                txt2.text = "Decrease count";
                map3.SetActive(true);
                pic3.sprite = buttonNorth;
                txt3.text = "Hide/Display your answer";
                break;
            default : Debug.LogError("MINIGAME NAME INCORRECT IN PrviewManager  (" + SceneMiniName + ")"); break;
        }
    }


    public void CHECK_IF_CAN_BEGIN_GAME()
    {
        if (nReady >= controller.nPlayers) { StartCoroutine( LOAD_MINIGAME() ); }
    }

    private IEnumerator LOAD_MINIGAME()
    {
        readyText.text = "Starting Side Quest!";
        yield return new WaitForSeconds(1);
        blackScreen.CrossFadeAlpha(1, transitionTime, false);
        if (bgMusic != null)
        {
            while (bgMusic.volume > 0)
            {
                yield return new WaitForSeconds(0.1f);
                bgMusic.volume -= 0.01f;
            }
        }

        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(SceneRealName);
    }

}
