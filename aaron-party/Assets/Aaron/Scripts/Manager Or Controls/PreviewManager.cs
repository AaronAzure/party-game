using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class PreviewManager : MonoBehaviour
{
    public bool canPlay;
    public bool timeUp;
    public int nPlayersOut = 0;
    private int c1 = 0;
    private int c2 = 0;
    private int c3 = 0;
    private int c4 = 0;
    private int c5 = 0;
    private int c6 = 0;
    private int c7 = 0;
    private int c8 = 0;

    [SerializeField] private Image blackScreen;
    private float transitionTime = 0.5f;
    public MinigameControls[] players;             // PLAYERS GAMEOBJECT
    [SerializeField] private MinigameControls spawnPlayers;
    [SerializeField] private MinigameControls spawnCursors;
    [SerializeField] private Transform[] spawnPos;
    [SerializeField] private Transform _A;
    [SerializeField] private Transform _B;
    private string sceneName;

    private GameController controller;
    private int timer;

    // *** UNIQUE *** //
    public float timeToStop;
    [SerializeField] private MultipleTargetCamera mtCam;
    [SerializeField] private GameObject whereTheBall;


    // ** EVERY NEW MINIGAME ADJUST TIMER
    void Start()
    {
        controller = GameObject.Find("Game_Controller").GetComponent<GameController>();
        sceneName = gameObject.scene.name;
        players = new MinigameControls[controller.nPlayers];

        // TIMER MAP-SPECIFIC
        if (sceneName == "Sneak And Snore") 
        {
            timer = 30;
            SpawnPlayers(1);
        }
        else if (sceneName == "Feast-ival") 
        {
            timer = 10;
            SpawnPlayers(2);
        }
        else if (sceneName == "Colour Chaos") 
        {
            timer = 90;
            SpawnPlayers_CIRCLE(1.2f, 2.25f);
        }
        else if (sceneName == "Card Collectors") 
        {
            timer = 30;
            SpawnPlayers(1);
        }
        else if (sceneName == "Leaf Leap") 
        {
            timer = 30;
            SpawnPlayers_POINTS(1);
        }
        else if (sceneName == "Lava Or Leave 'Em") 
        {
            timer = 45;
            SpawnPlayers(1);
        }
        else if (sceneName == "Lilypad Leapers") 
        {
            timer = 45;
            SpawnPlayers_POINTS(1);
        }
        else if (sceneName == "Stop Watchers") 
        {
            timer = 30;
            SpawnPlayers(1.5f);
            // timeUI.SetActive(false);
        }
        else if (sceneName == "Spotlight Fight") 
        {
            timer = 30;
            SpawnPlayers_CIRCLE(1, 3);
        }
        else if (sceneName == "Pushy Penguins") 
        {
            timer = 45;
            SpawnPlayers(1.2f);
        }
        else if (sceneName == "Fun Run") 
        {
            timer = 60;
            SpawnPlayers(1.5f);
        }
        else if (sceneName == "Coin-veyor") 
        {
            timer = 25;
            SpawnPlayers_POINTS(1);
        }
        else if (sceneName == "Stamp By Me") 
        {
            timer = 25;
            SpawnPlayers_POINTS(1);
        }
        else if (sceneName == "Shocking Situation") 
        {
            timer = 20;
            SpawnPlayers_CIRCLE(0.75f, 2);
        }
        else if (sceneName == "Attack Of Titan") 
        {
            timer = 60;
            SpawnPlayers_CIRCLE(0.75f, 2);
        }
        else if (sceneName == "Flower Shower") 
        {
            timer = 30;
            SpawnPlayers_CIRCLE(0.75f, 2);
        }
        else if (sceneName == "Don't Be A Zombie") 
        {
            SpawnPlayers_CIRCLE(1.25f, 3);
            timer = 60;
            SetupMultiTargetCam();
        }
        else if (sceneName == "Barrier Bearers") 
        {
            timer = 60;
            SpawnPlayers_CIRCLE(0.75f, 2);
        }
        else if (sceneName == "Plunder Ground") 
        {
            timer = 30;
            SpawnPlayers_CIRCLE(1, 7.5f);
            SetupMultiTargetCam();
        }
        else if (sceneName == "Pinpoint The Endpoint") 
        {
            timer = 11;
            SpawnPlayers(1);
        }
        else if (sceneName == "Camo Cutters") 
        {
            timer = 45;
            SpawnPlayers_CIRCLE(1, 2);
        }

        // timerText.text = timer.ToString();
        blackScreen.gameObject.SetActive(true);
        blackScreen.CrossFadeAlpha(0f, transitionTime, false);  // FADE OUT

        canPlay = true;
        StartCoroutine( StartMinigame() );
    }

    private void SpawnPlayers(float ratio)
    {
        if (controller.nPlayers < 5)
        {
            for ( int i=0 ; i<controller.nPlayers ; i++ )
            {
                var player = Instantiate(spawnPlayers, 
                    Vector3.Lerp(_A.position, _B.position, (float) (i+1)/(controller.nPlayers+1) ), Quaternion.identity);
                player.transform.parent = this.gameObject.transform;
                player.playerID = i; 
                player.transform.localScale *= ratio;
                player.name = "Player_" + (i+1); 
                player.pw = this.gameObject.GetComponent<PreviewManager>();
                player.sceneName = this.sceneName;

                players[i] = player;
                if (controller.easy && (sceneName == "Fun Run") )
                {
                    player.transform.position -= new Vector3(0,150,0);
                }
                if (controller.hard && (sceneName == "Fun Run") )
                {
                    player.transform.position += new Vector3(0,150,0);
                }
            }
        }
        else
        {
            for ( int i=0 ; i<controller.nPlayers ; i++ )
            {
                var player = Instantiate(spawnPlayers, 
                    Vector3.Lerp(_A.position, _B.position, (float) (i)/(controller.nPlayers-1) ), Quaternion.identity);
                player.transform.parent = this.gameObject.transform;
                player.playerID = i; 
                player.transform.localScale *= ratio;
                player.name = "Player_" + (i+1); 
                player.pw = this.gameObject.GetComponent<PreviewManager>();
                player.sceneName = this.sceneName;

                players[i] = player;
                if (controller.easy && (sceneName == "Fun Run") )
                {
                    player.transform.position -= new Vector3(0,150,0);
                }
                if (controller.hard && (sceneName == "Fun Run") )
                {
                    player.transform.position += new Vector3(0,150,0);
                }
            }
        }
    }

    // EACH INDIVIDUAL SPAWN POINT
    private void SpawnPlayers_POINTS(float ratio)
    {
        for ( int i=0 ; i<controller.nPlayers ; i++ )
        {
            var player = Instantiate(spawnPlayers, spawnPos[i].position, Quaternion.identity);
            player.transform.parent = this.gameObject.transform;
            player.transform.localScale *= ratio;
            player.playerID = i; 
            player.name = "Player_" + (i+1); 
            player.pw = this.gameObject.GetComponent<PreviewManager>();;
            player.sceneName = this.sceneName;
        }
    }

    private void SpawnPlayers_CIRCLE(float ratio, float radius)
    {
        for ( int i=0 ; i<controller.nPlayers ; i++ )
        {
            /* Distance around the circle */  
            var radians = 2 * Mathf.PI / controller.nPlayers * i;
            
            /* Get the vector direction */ 
            var vertical = Mathf.Sin(radians);
            var horizontal = Mathf.Cos(radians); 
            
            var spawnDir = new Vector3 (-horizontal, vertical);
            
            /* Get the spawn position */ 
            Vector3 spawnPos = _A.transform.position + (spawnDir * radius); // Radius is just the distance away from the point
            
            /* Now spawn */
            var player = Instantiate(spawnPlayers, spawnPos , Quaternion.identity);
            player.transform.parent = this.gameObject.transform;
            player.transform.localScale *= ratio;
            player.playerID = i; 
            player.name = "Player_" + (i+1); 
            player.pw = this.gameObject.GetComponent<PreviewManager>();;
            player.sceneName = this.sceneName;

            players[i] = player;
        }
    }

    private void SpawnCursors(float ratio)
    {
        if (controller.nPlayers < 5)
        {
            for ( int i=0 ; i<controller.nPlayers ; i++ )
            {
                var player = Instantiate(spawnPlayers, 
                    Vector3.Lerp(_A.position, _B.position, (float) (i+1)/(controller.nPlayers+1) ), Quaternion.identity);
                player.transform.parent = this.gameObject.transform;
                player.playerID = i; 
                player.transform.localScale *= ratio;
                player.name = "Player_" + (i+1); 
                player.pw = this.gameObject.GetComponent<PreviewManager>();
                player.cursorMode = true;
                player.sceneName = this.sceneName;
            }
        }
        else
        {
            for ( int i=0 ; i<controller.nPlayers ; i++ )
            {
                var player = Instantiate(spawnCursors, 
                    Vector3.Lerp(_A.position, _B.position, (float) (i)/(controller.nPlayers-1) ), Quaternion.identity);
                player.transform.parent = this.gameObject.transform;
                player.playerID = i; 
                player.transform.localScale *= ratio;
                player.name = "Player_" + (i+1); 
                player.pw = this.gameObject.GetComponent<PreviewManager>();
                player.cursorMode = true;
                player.sceneName = this.sceneName;
            }
        }
    }

    private void SpawnCursors_CIRCLE(float ratio, float radius)
    {
        for ( int i=0 ; i<controller.nPlayers ; i++ )
        {
            /* Distance around the circle */  
            var radians = 2 * Mathf.PI / controller.nPlayers * i;
            
            /* Get the vector direction */ 
            var vertical = Mathf.Sin(radians);
            var horizontal = Mathf.Cos(radians); 
            
            var spawnDir = new Vector3 (-horizontal, vertical);
            
            /* Get the spawn position */ 
            Vector3 spawnPos = _A.transform.position + (spawnDir * radius); // Radius is just the distance away from the point
            
            /* Now spawn */
            var player = Instantiate(spawnCursors, spawnPos , Quaternion.identity);
            player.transform.parent = this.gameObject.transform;
            player.transform.localScale *= ratio;
            player.playerID = i; 
            player.name = "Player_" + (i+1); 
            player.pw = this.gameObject.GetComponent<PreviewManager>();;
            player.cursorMode = true;
            player.sceneName = this.sceneName;
        }
    }

    private void SetupMultiTargetCam()
    {
        mtCam.offset = new Vector3(8, -1.7f, -10);
        if (mtCam.targets == null) mtCam.targets = new List<Transform>();
        mtCam.minZoom *= 1.64f;
        mtCam.maxZoom *= 1.64f;
        for (int i=0 ; i<players.Length ; i++)
        {
            mtCam.targets.Add( players[i].transform );
        }
        if (sceneName == "Plunder Ground")
        {
            mtCam.offset = new Vector3(11.75f, -3, -10);
        }
    }

    // TIMER DECREMENTS
    private IEnumerator StartMinigame()
    {
        yield return new WaitForSeconds(1);
        timer--;
        //// Debug.Log("-- timer = " + timer);

        if (timer > 0) { StartCoroutine( StartMinigame() ); }
        else if (sceneName == "Pinpoint The Endpoint")
        {
            StartCoroutine( EventGameOver() );
        }
        // TIMES UP
        else           { StartCoroutine( RELOAD() ); }
    }


    public void CheckIfEveyoneIsOut(int x)
    {
        if (nPlayersOut >= controller.nPlayers - x)     
        { 
            if (sceneName == "Stop Watchers") { StartCoroutine( EventGameOver() ); }
            else    { StartCoroutine( RELOAD() ); }
        }
    }

    private IEnumerator EventGameOver()
    {
        if (sceneName == "Stop Watchers")
        {
            for (int i=0 ;i<players.Length ; i++)
            {
                players[i].ShowTime();
            }
            CLOSEST_TIME();

            yield return new WaitForSeconds(1);
            StartCoroutine( RELOAD() );
        }
        else if (sceneName == "Pinpoint The Endpoint")
        {
            if (whereTheBall != null) whereTheBall.SetActive(true);

            yield return new WaitForSeconds(2);
            // if (whereTheBall != null) whereTheBall.SetActive(false);

            yield return new WaitForSeconds(5);
            canPlay = false;
            MagicBall mb = GameObject.Find("BALL").GetComponent<MagicBall>();
            mb.CHECK_DISTANCE();

            yield return new WaitForSeconds(3);
            StartCoroutine( RELOAD() );
        }

    }

    private void CLOSEST_TIME()
    {
        float[] arr = new float[controller.nPlayers];
        for (int i=0 ; i<arr.Length ; i++) { arr[i] = players[i].points; }

        // SORT BY DESCENDING ORDER
        for (int i = 0; i < arr.Length; i++)            
        {
            for (int j = 0; j < arr.Length; j++)    
            {
                if (arr[i] > arr[j])       
                {
                    float tmp   = arr[j];
                    arr[i]      = arr[j];   
                    arr[j]      = tmp;    
                }
            }
        }

        // LOWEST SCORE WINS
        for (int i=0 ; i<arr.Length ; i++)
        {
            if (arr[ arr.Length-1 ] == players[i].points) { players[i].scoreHead.color = new Color(0,1,0); }
        }

    }


    // RELOAD PRACTICE MINIGAME
    private IEnumerator RELOAD()
    {
        if (timeUp) { yield break; }
        timeUp = true;
        if (sceneName == "Spotlight Fight") {
            // REMOVE ALL SPOTLIGHTS IN MINIGAME
            GameObject[] spotlights = GameObject.FindGameObjectsWithTag("Safe");
            foreach (GameObject light in spotlights) { Destroy(light.gameObject); }
        }

        yield return new WaitForSeconds(1);
        blackScreen.CrossFadeAlpha(1, transitionTime, false);

        yield return new WaitForSeconds(transitionTime);
        SceneManager.UnloadSceneAsync(sceneName);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }
}
