using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MinigameWinner : MonoBehaviour
{
    [SerializeField] private AudioSource bgMusic;
    [SerializeField] private Image       blackScreen;
    [SerializeField] private float transitionTime = 0.5f;

    private void Start() {
        StartCoroutine( BackToBoard() );
    }


    private IEnumerator BackToBoard()
    {
        blackScreen.CrossFadeAlpha(0, transitionTime, false);
        GameController controller = GameObject.Find("Game_Controller").GetComponent<GameController>();
        for ( int i=0 ; i<controller.nPlayers ; i++)    { controller.RICH_ORB_UPDATE(i); }

        yield return new WaitForSeconds(6 + transitionTime);
        blackScreen.CrossFadeAlpha(1, transitionTime, false);
        while (bgMusic.volume > 0) {
            yield return new WaitForSeconds(0.1f);
            bgMusic.volume -= 0.01f;
        }

        yield return new WaitForSeconds(transitionTime);
        
        // FIVE TURNS LEFT
        if (controller.turnNumber == controller.maxTurns - 4) {
            controller.LOAD_CUTAWAY();
        }
        // NORMAL TURN
        else if (controller.turnNumber <= controller.maxTurns) {
            string mySavedScene = PlayerPrefs.GetString("sceneName");
            SceneManager.LoadScene(mySavedScene);
        }

    }
}
