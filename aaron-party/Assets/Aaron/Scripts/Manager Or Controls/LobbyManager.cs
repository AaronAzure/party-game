using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    private GameController controller;
    [SerializeField] private Image blackScreen;
    [SerializeField] private float transitionTime = 0.5f;
    [SerializeField] private LobbyControls spawnPlayers; 
    [SerializeField] private Transform _A;
    [SerializeField] private Transform _B;
    [SerializeField] private MultipleTargetCamera mtCam;
    [SerializeField] private AudioSource bgMusic;
    [SerializeField] private GameObject aaronPrefab;


    public GameObject p1;
    public GameObject p2;
    public GameObject p3;
    public GameObject p4;
    public GameObject p5;
    public GameObject p6;
    public GameObject p7;
    public GameObject p8;
    


    void Start()
    {
        controller = GameObject.Find("Game_Controller").GetComponent<GameController>();
        blackScreen.gameObject.SetActive(true);
        blackScreen.CrossFadeAlpha(0f, transitionTime, false);  // FADE OUT
        
        SpawnPlayers();
    }

    private void SpawnPlayers()
    {
        for ( int i=0 ; i<controller.nPlayers ; i++ )
        {
            var player = Instantiate(spawnPlayers, 
                    Vector3.Lerp(_A.position, _B.position, (float) (i+1)/(controller.nPlayers+1) ), Quaternion.identity);
            player.playerID = i; 
            player.name = "Player_" + (i+1); 
            player.aaron = aaronPrefab;
            // Debug.Log("-- " + player.name + " ADDED (" + player.playerID + ")");
            if (mtCam != null) { mtCam.targets.Add(player.transform); }
            if (i == 0) player.multipleTargetCamera = mtCam;
        }
    }

    public void YouAllTooFar()
    {
        // if (p2 != null) p2.gameObject.transform.position = p1.transform.position;
        // if (p3 != null) p3.gameObject.transform.position = p1.transform.position;
        // if (p4 != null) p4.gameObject.transform.position = p1.transform.position;
        // if (p5 != null) p5.gameObject.transform.position = p1.transform.position;
        // if (p6 != null) p6.gameObject.transform.position = p1.transform.position;
        // if (p7 != null) p7.gameObject.transform.position = p1.transform.position;
        // if (p8 != null) p8.gameObject.transform.position = p1.transform.position;
    }

    public IEnumerator FADE(string boardName)
    {
        blackScreen.CrossFadeAlpha(1, transitionTime, false);  // FADE OUT
        if (bgMusic != null)
        {
            while (bgMusic.volume > 0) 
            {
                yield return new WaitForSeconds(0.1f);
                bgMusic.volume -= 0.01f;
            }
        }
        controller.LOAD_BOARD(boardName);
    }
    
    public IEnumerator PLAY_MINIGAMES()
    {
        blackScreen.CrossFadeAlpha(1, transitionTime, false);  // FADE OUT
        controller.minigameMode = true;
        if (bgMusic != null)
        {
            while (bgMusic.volume > 0) 
            {
                yield return new WaitForSeconds(0.1f);
                bgMusic.volume -= 0.01f;
            }
        }
        controller.LOAD_MINIGAMES_BOARD();
    }
}
