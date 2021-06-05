using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Rewired;

public class MainMenu : MonoBehaviour
{
    private int menuButtonIndex = 0;
    private GameController controller;

    [Header("Buttons")]
    [SerializeField] private Image[] buttons;
    private string sceneName;
    private float alpha = 0.6f;

    private Player player;

    // ----------------------------------------------------------------------------

    private void Start()
    {
        sceneName =  SceneManager.GetActiveScene().name;
        if (controller == null)
        {
            controller = GameObject.Find("Game_Controller").GetComponent<GameController>();
        }

        for (int i=0; i<buttons.Length; i++) { buttons[i].color = new Color(1, 1, 1, alpha); }
        buttons[menuButtonIndex].color = new Color(0.6f, 1, 1, 1);

        player = ReInput.players.GetPlayer(0);   // ONLY PLAYER 1 CAN CONTROL
    }

    void Update()
    {
        if (player.GetButtonDown("A") && controller.nPlayers > 1) 
        {
            controller.LOAD_CRYSTAL_CAVERNS();
        }

        // DELETE
        if (player.GetButtonDown("Minus"))
        {
            controller.Testing();
        }
    }

    // void OnMoveLeft()
    // {
    //     if (sceneName == "MainMenu")
    //     {
    //         // PREV BUTTON IS UNHIGHLIGHTED
    //         buttons[menuButtonIndex].color = new Color(1, 1, 1, alpha);

    //         menuButtonIndex--;
    //         if (menuButtonIndex < 0) { menuButtonIndex = buttons.Length - 1; }

    //         // NEW BUTTON IS HIGHLIGHTED
    //         buttons[menuButtonIndex].color = new Color(0.6f, 1, 1, 1);

    //     }
    //     else if (sceneName == "MapSelection")
    //     {
    //         // PREV BUTTON IS UNHIGHLIGHTED
    //         buttons[menuButtonIndex].color = new Color(1, 1, 1, alpha);

    //         menuButtonIndex--;
    //         if (menuButtonIndex < 0) { menuButtonIndex = buttons.Length - 1; }

    //         // NEW BUTTON IS HIGHLIGHTED
    //         buttons[menuButtonIndex].color = new Color(0.6f, 1, 1, 1);
    //     }
        
    // }
    // void OnMoveRight()
    // {
    //     if (sceneName == "MainMenu")
    //     {
    //         // PREV BUTTON IS UNHIGHLIGHTED
    //         buttons[menuButtonIndex].color = new Color(1, 1, 1, alpha);

    //         menuButtonIndex++;
    //         if (menuButtonIndex >= buttons.Length) { menuButtonIndex = 0; }

    //         // NEW BUTTON IS HIGHLIGHTED
    //         buttons[menuButtonIndex].color = new Color(0.6f, 1, 1, 1);
    //     }
    //     else if (sceneName == "MapSelection")
    //     {
    //         // PREV BUTTON IS UNHIGHLIGHTED
    //         buttons[menuButtonIndex].color = new Color(1, 1, 1, alpha);

    //         menuButtonIndex++;
    //         if (menuButtonIndex >= buttons.Length) { menuButtonIndex = 0; }

    //         // NEW BUTTON IS HIGHLIGHTED
    //         buttons[menuButtonIndex].color = new Color(0.6f, 1, 1, 1);
    //     }


    // }
    // void OnSelect()
    // {
    //     if (sceneName == "MainMenu")
    //     {
    //         int nPlayers = (menuButtonIndex + 1) * 2;
    //         controller.LOAD_N_PLAYERS(nPlayers);
    //     }
    //     else if (sceneName == "MapSelection")
    //     {
    //         controller.LOAD_CRYSTAL_CAVERNS();
    //     }
        
    // }
    // void OnCancel()
    // {
    //     Debug.Log("  CANCELED");
    // }

}
