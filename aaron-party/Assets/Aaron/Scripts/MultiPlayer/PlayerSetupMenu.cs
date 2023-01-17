using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class PlayerSetupMenu : MonoBehaviour
{
    // GLOBAL VARS
    private GameController controller;
    private int nOfPlayersSelected;
    [SerializeField] private TextMeshProUGUI titleText;


    [SerializeField] private TextMeshProUGUI bottomText;
    [SerializeField] private GameObject _ready;
    [SerializeField] private Image      background;
    [SerializeField] private int        characterIndex = 0;
    [SerializeField] private Image selectedCharacter;
    [SerializeField] private Sprite[] characters;
    private Color alreadySelectedColour;


    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject readyPanel; 
    [SerializeField] private Button     readyFirstButton;

    [SerializeField] private int playerID;
    [SerializeField] private Player player;
    
    private bool joinedGame;
    private bool isReady;
    private bool finishedSetup;
    private TextMeshProUGUI instruction;

    // ------------------------------------------------------------------------

    void Start() 
    {
        if (GameObject.Find("Instruction (TMP)") != null) instruction = GameObject.Find("Instruction (TMP)").GetComponent<TextMeshProUGUI>();
        alreadySelectedColour = new Color(0.3f,0.3f,0.3f,0.5f);
        player = ReInput.players.GetPlayer(playerID);
        controller = GameObject.Find("Game_Controller").GetComponent<GameController>();
    }

    void Update()
    {
        // CANCEL SELECTION OR READY
        if (player.GetButtonDown("B"))
        {
            CANCEL();
        }

        if (nOfPlayersSelected != controller.charactersChosen.Count && joinedGame && !isReady)
        {
            nOfPlayersSelected = (int) controller.charactersChosen.Count;
            SOMEONE_SELECTED_A_CHARACTER();
        }
        // JOIN GAME
        if (!joinedGame) 
        {
            if (player.GetButtonDown("A")) { PLAYER_JOINED(); }
        }
        // CHOOSING A CHARACTER
        else if (!isReady)
        {
            CHOOSING_CHARACTER();
        }
        // CHOSE A CHARACTER
        else if (!finishedSetup)
        {
            DONE();
        }
        // PRESSED THE READY BUTTON
        else
        {
            FINISHED();
        }
    }


    // JOIN GAME
    public void PLAYER_JOINED()
    {
        joinedGame = true;

        controller.nPlayers++;
        characterIndex = playerID;

        titleText.text = "Player " + (playerID + 1);
        background.color = new Color(1, 0.7f, 0, 1);
        selectedCharacter.gameObject.SetActive(true);
        CURRENT_SELECTED_CHARACTER();
    }

    private void CANCEL()
    {
        if (finishedSetup)
        {
            _ready.SetActive(false);
            controller.nReady--;
            finishedSetup = false;
            isReady = false;
            menuPanel.gameObject.SetActive(true);
            bottomText.gameObject.SetActive(true);
            readyPanel.SetActive(false);
            for (int i=0 ; i<controller.charactersChosen.Count ; i++) 
            {
                if (controller.charactersChosen[i] == this.characterIndex)
                {
                    controller.charactersChosen.Remove(controller.charactersChosen[i]);
                    break;
                }
            }
            if (controller.ALL_PLAYERS_READY()) {
                PRESS_START_TO_PLAY();
            }
            else {
                PLEASE_SELECT_YOUR_CHARACTER();
            }
        }
        else if (joinedGame)
        {
            joinedGame = false;

            controller.nPlayers--;

            titleText.text = "Press A to Join";
            background.color = new Color(0.8f, 0.8f, 0.8f, 1);
            selectedCharacter.gameObject.SetActive(false);
            bottomText.text = "Name";
        }
    }

    private void CHOOSING_CHARACTER()
    {
        if (player.GetButtonDown("A") && selectedCharacter.color != alreadySelectedColour) 
        {
            isReady = true;
            menuPanel.gameObject.SetActive(false);
            bottomText.gameObject.SetActive(false);
            // IS_READY();
            DONE();

            controller.charactersChosen.Add(characterIndex);
        }
        else if (player.GetButtonDown("Left")) 
        {  
            if (characterIndex == 0) characterIndex = characters.Length - 1;
            else                     characterIndex--;
            CURRENT_SELECTED_CHARACTER();
        }
        else if (player.GetButtonDown("Right")) 
        {  
            if (characterIndex == characters.Length - 1) characterIndex = 0;
            else                                         characterIndex++;
            CURRENT_SELECTED_CHARACTER();
        }
    }

    private void CURRENT_SELECTED_CHARACTER()
    {
        selectedCharacter.sprite = characters[characterIndex];
        bottomText.text = characters[characterIndex].name;
        selectedCharacter.color = new Color(1,1,1,1);
        // CHECK IF ANY CHARACTER HAS ALREADY BEEN CHOSEN
        foreach (int notAvailable in controller.charactersChosen) 
        {
            if (characterIndex == notAvailable) 
            {
                selectedCharacter.color = alreadySelectedColour;
                break;
            }
        }
        
        // CHANGE BACKGROUND COLOR BASED ON CHARACTER
        switch (selectedCharacter.sprite.name)  
        {
            case "Sweeterella" :    background.color = new Color(1, 0.7f, 0, 1); break;
            case "Laurel" :         background.color = new Color(0.9716981f, 0.8611333f, 0.7104397f, 1); break;
            case "Mimi" :           background.color = new Color(0.75f, 0.9f, 0.4f, 1); break;
            case "Thanatos" :       background.color = new Color(1f, 0.3f, 0.3f, 1); break;
            case "Jacob" :          background.color = new Color(0.6f, 0.9f, 1f, 1); break;
            case "Maurice" :        background.color = new Color(0.3f, 0.3f, 0.3f, 1); break;
            case "Pinkins" :        background.color = new Color(1, 0.5f, 1, 1); break;
            case "Felix" :          background.color = new Color(0.75f, 0.4f, 0.2f, 1); break;
            case "Charlotte" :      background.color = new Color(0.6f, 0.1f, 0.8f, 1); break;
        }
    }

    private void SOMEONE_SELECTED_A_CHARACTER()
    {
        selectedCharacter.color = new Color(1,1,1,1);
        foreach (int notAvailable in controller.charactersChosen) 
        {
            if (characterIndex == notAvailable) 
            {
                selectedCharacter.color = alreadySelectedColour;
                break;
            }
        }
    }

    private void IS_READY()
    {
        readyFirstButton.Select();
        readyPanel.SetActive(true);
    }

    // READY = CHARACTER SELECTED
    public void DONE()
    {
        // if (player.GetButtonDown("A")) 
        // {
            _ready.SetActive(true);
            readyFirstButton.gameObject.SetActive(false);
            controller.nReady++;
            finishedSetup = true;
            if (controller.ALL_PLAYERS_READY()) {
                PRESS_START_TO_PLAY();
            }
            else {
                PLEASE_SELECT_YOUR_CHARACTER();
            }
            switch (name)
            {
                case "Setup_Panel (0)" : controller.characterName1 = characters[characterIndex].name; break;
                case "Setup_Panel (1)" : controller.characterName2 = characters[characterIndex].name; break;
                case "Setup_Panel (2)" : controller.characterName3 = characters[characterIndex].name; break;
                case "Setup_Panel (3)" : controller.characterName4 = characters[characterIndex].name; break;
                case "Setup_Panel (4)" : controller.characterName5 = characters[characterIndex].name; break;
                case "Setup_Panel (5)" : controller.characterName6 = characters[characterIndex].name; break;
                case "Setup_Panel (6)" : controller.characterName7 = characters[characterIndex].name; break;
                case "Setup_Panel (7)" : controller.characterName8 = characters[characterIndex].name; break;
            }
        // }
    }

    private void FINISHED()
    {
        if (player.GetButtonDown("Start"))
        {
            controller.PLAYER_SETUP_FINISHED();
        }
    }

    private void PRESS_START_TO_PLAY() {
        if (instruction != null) instruction.text = "Press Start!";
    }
    
    private void PLEASE_SELECT_YOUR_CHARACTER() {
        if (instruction != null) instruction.text = "Please select your character";
    }

}
