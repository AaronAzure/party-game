using System.Collections;
using System.Collections.Generic;
using Rewired;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CharacterHolder : MonoBehaviour
{
    private Player player;
    public int playerID;
    public float moveSpeed;
    public float minSpeed;
    public float maxSpeed;
    public Rigidbody2D rb;
    public bool movable;

    [SerializeField] private GameObject[] characters;
    [SerializeField] private GameObject _Felix;
    [SerializeField] private GameObject _Jacob;
    [SerializeField] private GameObject _Laurel;
    [SerializeField] private GameObject _Maurice;
    [SerializeField] private GameObject _Mimi;
    [SerializeField] private GameObject _Pinkins;
    [SerializeField] private GameObject _Sweeterella;
    [SerializeField] private GameObject _Thanatos;
    [SerializeField] private GameObject _Charlotte;


    private GameController controller;
    private Animator anim;
    private string characterName;
    private GameObject character;
    private float scaleX;
    public bool thisIsTheWinner;
    public bool gameWinner;
 
    public IntroManager intro; // SCRIPT ONLY (IntroManager)
    public bool         introMode;
    private MagicCard   selectedCard; // ** BY RUN-TIME
    private bool        cardCollected;
    [SerializeField] private TextMeshPro rankUI;    // ** INSPECTOR
    [SerializeField] private GameObject  orbsUI;    // ** INSPECTOR
    [SerializeField] private TextMeshPro orbs;      // ** INSPECTOR
    [SerializeField] private GameObject  goldUI;    // ** INSPECTOR
    [SerializeField] private TextMeshPro gold;      // ** INSPECTOR
    [SerializeField] private TextMeshPro ranking;   // ** INSPECTOR
    [SerializeField] private GameObject floatingCoinTextPrefab;   // ** INSPECTOR
    [SerializeField] private AudioSource coinPickup;   // ** INSPECTOR
    public PlayerPrevData data;

    void Start()
    {
        if (GameObject.Find("Game_Controller") != null) {
            controller = GameObject.Find("Game_Controller").GetComponent<GameController>();
        }
        player = ReInput.players.GetPlayer(playerID);
        maxSpeed = moveSpeed * 4;
        moveSpeed = minSpeed;
        if (movable) rb = GetComponent<Rigidbody2D>();

        if (!thisIsTheWinner)
        {
            switch (name)
            {
                case "Player_1" : characterName = controller.characterName1;    break;
                case "Player_2" : characterName = controller.characterName2;    break;
                case "Player_3" : characterName = controller.characterName3;    break;
                case "Player_4" : characterName = controller.characterName4;    break;
                case "Player_5" : characterName = controller.characterName5;    break;
                case "Player_6" : characterName = controller.characterName6;    break;
                case "Player_7" : characterName = controller.characterName7;    break;
                case "Player_8" : characterName = controller.characterName8;    break;
            }
            if (true)
            {
                for (int i=0 ; i<characters.Length ; i++) {
                    if (characterName == characters[i].name) {
                        var obj = Instantiate(characters[i], transform.position, Quaternion.identity, this.transform); 
                        character = obj.gameObject; obj.transform.parent = this.transform;  anim = obj.GetComponent<Animator>();
                        break;
                    }
                    if (i == characters.Length - 1) {
                        Debug.LogError("ERROR : Have not assign character to name (" + characterName + ")");
                    }
                }
                scaleX = character.transform.localScale.x;
            }
        }
        else { 
            string winnerName = controller.THE_WINNER(); 
            if (winnerName == "Felix") {
                    var obj = Instantiate(_Felix, transform.position, Quaternion.identity, this.transform); 
                    character = obj.gameObject;     anim = obj.GetComponent<Animator>();
                }
            else if (winnerName == "Jacob") {
                var obj = Instantiate(_Jacob, transform.position, Quaternion.identity, this.transform); 
                character = obj.gameObject;     anim = obj.GetComponent<Animator>();
            }
            else if (winnerName == "Laurel") {
                var obj = Instantiate(_Laurel, transform.position, Quaternion.identity, this.transform); 
                character = obj.gameObject;     anim = obj.GetComponent<Animator>();
            }
            else if (winnerName == "Maurice") {
                var obj = Instantiate(_Maurice, transform.position, Quaternion.identity, this.transform); 
                character = obj.gameObject;     anim = obj.GetComponent<Animator>();
            }
            else if (winnerName == "Mimi") {
                var obj = Instantiate(_Mimi, transform.position, Quaternion.identity, this.transform); 
                character = obj.gameObject;     anim = obj.GetComponent<Animator>();
            }
            else if (winnerName == "Pinkins") {
                var obj = Instantiate(_Pinkins, transform.position, Quaternion.identity, this.transform); 
                character = obj.gameObject;     anim = obj.GetComponent<Animator>();
            }
            else if (winnerName == "Sweeterella") {
                var obj = Instantiate(_Sweeterella, transform.position, Quaternion.identity, this.transform); 
                character = obj.gameObject;     anim = obj.GetComponent<Animator>();
            }
            else if (winnerName == "Thanatos") {
                var obj = Instantiate(_Thanatos, transform.position, Quaternion.identity, this.transform); 
                character = obj.gameObject;     anim = obj.GetComponent<Animator>();
            }
            else if (winnerName == "Charlotte") {
                var obj = Instantiate(_Charlotte, transform.position, Quaternion.identity, this.transform); 
                character = obj.gameObject;     anim = obj.GetComponent<Animator>();
            }
            else {
                Debug.LogError("ERROR : Have not assign character to name");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (movable) { MOVEMENT(); SPEED(); }
        if (introMode) { COLLECT_CARD(); }
        // if (goldUI.activeSelf) { RESTART(); }
    }

    void SPEED()
    {
        if (player.GetButton("X"))  { moveSpeed = maxSpeed; }
        else                        { moveSpeed = minSpeed;}
    }


    void MOVEMENT()
    {
        float moveHorizontal = player.GetAxis("Move Horizontal");
        float moveVertical = player.GetAxis("Move Vertical");


        // FLIP CHARACTER WHEN MOVING RIGHT
        if (moveHorizontal > 0 )
        {
            character.transform.localScale = new Vector3(-scaleX, scaleX, scaleX);
        }
        else if (moveHorizontal < 0)
        {
            character.transform.localScale = new Vector3(scaleX, scaleX, scaleX);
        }
        Vector3 direction = new Vector3(moveHorizontal, moveVertical);
        rb.MovePosition(transform.position + direction * moveSpeed * Time.fixedDeltaTime);

        // ANIMATION
        if (moveHorizontal != 0 || moveVertical != 0)
        {
            anim.SetBool("IsWalking", true);
            if (Mathf.Abs(moveHorizontal) > Mathf.Abs(moveVertical)) 
            { 
                anim.speed = Mathf.Clamp(moveSpeed * Mathf.Abs(moveHorizontal), 0, 5); 
            }
            else 
            { 
                anim.speed = Mathf.Clamp(moveSpeed * Mathf.Abs(moveVertical), 0, 5); 
                // anim.speed =  moveSpeed * (Mathf.Abs(moveHorizontal) + Mathf.Abs(moveVertical)) ;
            }
        }
        else { 
            anim.SetBool("IsWalking", false); 
            anim.speed = 1;
        }
    }

    void COLLECT_CARD()
    {
        if (selectedCard != null)
        {
            if (player.GetButtonDown("A") && !selectedCard.collected && !cardCollected)
            {
                cardCollected = true;
                int placement = selectedCard.turnRank;
                selectedCard.COLLECT();
                rankUI.gameObject.SetActive(true);
                rankUI.GetComponent<RectTransform>().localPosition = new Vector3(0, 4.5f);
                rankUI.text = selectedCard.rank.text;
                selectedCard.gameObject.SetActive(false);

                intro.A_PLAYER_HAS_COLLECTED_A_CARD(playerID, placement);
            }
        }
    }

    public void GET_PLAYER_DATA()
    {
        data = controller.GET_PLAYER_DATA(playerID);
        gold.text = data.coins.ToString();
        orbs.text = data.orbs.ToString();
        goldUI.SetActive(true);
        orbsUI.SetActive(true);

        int xth = controller.DISPLAY_PLAYER_RANKINGS(playerID);
        ranking.gameObject.SetActive(true);
        switch (xth)
        {
            case 0:   ranking.text = "<#ECC233>1<sup>st";  break;
            case 1:   ranking.text = "2<sup>nd";   break;
            case 2:   ranking.text = "3<sup>rd";   break;
            case 3:   ranking.text = "4<sup>th";   break;
            case 4:   ranking.text = "5<sup>th";   break;
            case 5:   ranking.text = "6<sup>th";   break;
            case 6:   ranking.text = "7<sup>th";   break;
            case 7:   ranking.text = "8<sup>th";   break;
            default : ranking.text = "DQ";   break;
        }
    }
    public void UPDATE_PLAYER_DATA()
    {
        // data = controller.GET_PLAYER_DATA(playerID);
        gold.text = data.coins.ToString();
        orbs.text = data.orbs.ToString();
        goldUI.SetActive(true);
        orbsUI.SetActive(true);

        int xth = controller.DISPLAY_PLAYER_RANKINGS(playerID);
        ranking.gameObject.SetActive(true);
        switch (xth)
        {
            case 0:   ranking.text = "<#ECC233>1<sup>st";  break;
            case 1:   ranking.text = "2<sup>nd";   break;
            case 2:   ranking.text = "3<sup>rd";   break;
            case 3:   ranking.text = "4<sup>th";   break;
            case 4:   ranking.text = "5<sup>th";   break;
            case 5:   ranking.text = "6<sup>th";   break;
            case 6:   ranking.text = "7<sup>th";   break;
            case 7:   ranking.text = "8<sup>th";   break;
            default : ranking.text = "DQ";   break;
        }
    }


    public IEnumerator UPDATE_PLAYER_COINS(int n)
    {
        Debug.Log("UPDATING!");
        int coins = controller.GET_PLAYER_GOLD(playerID);

        int tempGold = coins;
        var go = Instantiate(floatingCoinTextPrefab, 
            new Vector3(transform.position.x, transform.position.y + 3, transform.position.z),
            transform.rotation);

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
                UPDATE_PLAYER_DATA();
                controller.playerData[playerID].coins++;
                //! DELETE
                // if (playerID == 0) controller.p1[0].coins++;
                // if (playerID == 1) controller.p2[0].coins++;
                // if (playerID == 2) controller.p3[0].coins++;
                // if (playerID == 3) controller.p4[0].coins++;
                // if (playerID == 4) controller.p5[0].coins++;
                // if (playerID == 5) controller.p6[0].coins++;
                // if (playerID == 6) controller.p7[0].coins++;
                // if (playerID == 7) controller.p8[0].coins++;
            }
            intro.SHOW_RANKINGS();
        }
    }


    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Special")
        {
            this.selectedCard = other.GetComponent<MagicCard>();
            if (selectedCard.collected) selectedCard = null;
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.tag == "Special")
        {
            this.selectedCard = null;
        }
    }


    void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.tag == "Player")
        {
            if (!gameWinner)
            {
                CharacterHolder opponent = other.gameObject.GetComponent<CharacterHolder>();
                StartCoroutine( opponent.KnockBackCo(0.1f, 0.2f * anim.speed, this.transform) );
            }
            else 
            {
                CharacterHolder opponent = other.gameObject.GetComponent<CharacterHolder>();
                StartCoroutine( opponent.KnockBackCo(0.15f, 5f, this.transform) );
            }
        }
    }

    public IEnumerator KnockBackCo(float knockbackDuration, float knockbackPower, Transform opponent)
    {
        if (gameWinner) yield break;
        // Debug.Log("duration = " + knockbackDuration + ", power = " + knockbackPower);
        float timer = 0;
        anim.speed = 5;
        // if (chargingParticle != null) chargingParticle.SetActive(false); chargePower = 0;
        yield return new WaitForEndOfFrame();

        while (knockbackDuration > timer)
        {
            timer += Time.deltaTime;
            Vector2 direction = ( opponent.transform.position - this.transform.position ).normalized;
            rb.AddForce(-direction * knockbackPower, ForceMode2D.Impulse);
        }

        yield return new WaitForSeconds(knockbackDuration);
        rb.velocity = Vector2.zero;
    }

}
