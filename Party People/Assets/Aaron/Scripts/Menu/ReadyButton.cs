using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class ReadyButton : MonoBehaviour
{
    [SerializeField] private int playerID;  // INSPECTOR
    [SerializeField] private Player player;
    [SerializeField] private Image _square;
    private GameController controller;      // FIND | GET COMPONENT
    private PreviewOverlay _over;      // FIND | GET COMPONENT
    private string characterName;

    [Header("Character Squares")]
    [SerializeField] private Sprite[] squares;
    [SerializeField] private Sprite felixS;
    [SerializeField] private Sprite jacobS;
    [SerializeField] private Sprite laurelS;
    [SerializeField] private Sprite mauriceS;
    [SerializeField] private Sprite mimiS;
    [SerializeField] private Sprite pinkinsS;
    [SerializeField] private Sprite sweeterellaS;
    [SerializeField] private Sprite thanatosS;

    [Header("Inspector")]
    [SerializeField] private GameObject readyText;
    [SerializeField] private ReadyButton[] others;



    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("Game_Controller").GetComponent<GameController>();
        _over      = GameObject.Find("Preview_Overlay").GetComponent<PreviewOverlay>();

        if (playerID >= controller.nPlayers) {  this.gameObject.SetActive(false);  }

        player = ReInput.players.GetPlayer(playerID);
        _square.color = new Color(1,1,1,0.3f);

        // CHANGE CHARACTER
        switch (name)
        {
            case "SQUARES_READY" :     characterName = controller.characterName1;   break;
            case "SQUARES_READY (1)" : characterName = controller.characterName2;    break;
            case "SQUARES_READY (2)" : characterName = controller.characterName3;    break;
            case "SQUARES_READY (3)" : characterName = controller.characterName4;    break;
            case "SQUARES_READY (4)" : characterName = controller.characterName5;    break;
            case "SQUARES_READY (5)" : characterName = controller.characterName6;    break;
            case "SQUARES_READY (6)" : characterName = controller.characterName7;    break;
            case "SQUARES_READY (7)" : characterName = controller.characterName8;    break;
        }
        for (int i=0 ; i<squares.Length ; i++) {
            if (squares[i].name.Contains(characterName)) {
                _square.sprite = squares[i];
                break;
            }
            if (i == squares.Length - 1) { Debug.LogError("ERROR : Have not assign character to name (" + characterName + ")"); }
        }
        // switch (characterName)
        // {
        //     case "Felix" :          _square.sprite = felixS;        break;
        //     case "Jacob" :          _square.sprite = jacobS;        break;
        //     case "Laurel" :         _square.sprite = laurelS;       break;
        //     case "Maurice" :        _square.sprite = mauriceS;      break;
        //     case "Mimi" :           _square.sprite = mimiS;         break;
        //     case "Pinkins" :        _square.sprite = pinkinsS;      break;
        //     case "Sweeterella" :    _square.sprite = sweeterellaS;  break;
        //     case "Thanatos" :       _square.sprite = thanatosS;     break;
        //     case "" : break;
        //     default :       Debug.LogError("ERROR : Have not assign character to name (" + characterName + ")"); break;
        // }
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetButtonDown("Start") && !readyText.activeSelf)
        {
            readyText.SetActive(true);
            _square.color = new Color(1,1,1,1);
            _over.nReady++;
            _over.CHECK_IF_CAN_BEGIN_GAME();
        }
        if (playerID == 0 && player.GetButtonDown("Home")) {
            foreach (ReadyButton button in others) {
                button.READY_UP();
            }
        }
    }

    public void READY_UP() {
        if (!readyText.activeSelf)
        {
            readyText.SetActive(true);
            _square.color = new Color(1,1,1,1);
            _over.nReady++;
            _over.CHECK_IF_CAN_BEGIN_GAME();
        }
    }
}
