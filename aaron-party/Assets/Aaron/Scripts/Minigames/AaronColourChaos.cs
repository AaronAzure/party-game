using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AaronColourChaos : MonoBehaviour
{

    private Animator _anim;
    [SerializeField] private Transform[] spacesToTeleport;
    [SerializeField] private SpriteRenderer[] blanks;
    [SerializeField] private Sprite[] spacesWarningSprites;
    [SerializeField] private GameObject teleportSpellPrefab;
    private MinigameManager manager;
    private GameController ctr;
    private int nTimesCast;
    private int nCircles = 1;
    IEnumerator co;



    void Start()
    {
        _anim = GetComponent<Animator>();
        ctr = GameObject.Find("Game_Controller").GetComponent<GameController>();
        if (ctr.hard) { nCircles = 2; }

        // StartCoroutine(TeleportElimination());
        if (SceneManager.GetActiveScene().name == "Colour_Chaos") {
            if (GameObject.Find("Level_Manager") != null) {
                manager = GameObject.Find("Level_Manager").GetComponent<MinigameManager>();
            }
            StartCoroutine(StartDelay());
        }
        else {
            StartCoroutine(TeleportElimination());
        }
    }

    private IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(4);
        co = TeleportElimination();
        StartCoroutine( co );
    }

    private IEnumerator TeleportElimination()
    {
        if (manager != null) if (manager.timeUp) { yield break; }

        List<Integers> indexes = new List<Integers>();
        int[] arr = new int[nCircles];
        for ( int i=0 ; i<spacesToTeleport.Length ; i++ )   { indexes.Add( new Integers(i)); }
        // for ( int i=0 ; i<indexes.Count ; i++ ) Debug.Log("indexes - " + indexes[i].n);


        // SELECT SPACE TO TELEPORT
        for ( int i=0 ; i<nCircles ; i++ )
        {
            blanks[i].gameObject.SetActive(true);
            int rng = indexes[ Random.Range(0, indexes.Count) ].n;
            arr[i] = rng;
            blanks[i].sprite = spacesWarningSprites[rng];
            for (int r=0 ; r<indexes.Count ; r++) 
            {
                if (indexes[r].n == rng) { indexes.Remove(indexes[r]); break; }
            }
        }
        // for ( int i=0 ; i<arr.Length ; i++ )    Debug.Log("arr - " + arr[i]);

        yield return new WaitForSeconds(2.5f);
        _anim.Play("Aaron_Cast_Anim", -1, 0);
        
        yield return new WaitForSeconds(0.4f);

        for ( int i=0 ; i<arr.Length ; i++ )
        {
            blanks[i].gameObject.SetActive(false);
            var spell = Instantiate(teleportSpellPrefab, spacesToTeleport[arr[i]].position, Quaternion.identity);
            spell.GetComponent<AudioSource>().volume = 0;
            Destroy(spell, 0.5f);
        }
        
        nTimesCast++;

        // ATTACK PATTERN
        if      (ctr.easy)
        {
            if      (nTimesCast == 2) { nCircles++; }       // 2
            else if (nTimesCast == 3) { nCircles++; }       // 3
            else if (nTimesCast == 4) { nCircles++; }       // 4
            else if (nTimesCast == 6) { nCircles++; }       // 5
            else if (nTimesCast == 8) { nCircles++; }       // 6
            else if (nTimesCast == 11) { nCircles++; }      // 7
            else if (nTimesCast == 14) { nCircles++; }      // 8
        }
        else if (ctr.norm)
        {
            if      (nTimesCast == 1) { nCircles++; }       // 2
            else if (nTimesCast == 2) { nCircles += 2; }    // 4
            else if (nTimesCast == 4) { nCircles += 2; }    // 6
            else if (nTimesCast == 8) { nCircles++; }       // 7
            else if (nTimesCast == 12) { nCircles++; }      // 8
        }
        else if (ctr.hard)
        {
            if      (nTimesCast == 1) { nCircles += 2; }    // 4
            else if (nTimesCast == 2) { nCircles += 2; }    // 6
            else if (nTimesCast == 5) { nCircles++; }       // 7
            else if (nTimesCast == 8) { nCircles++; }       // 8
        }

        yield return new WaitForSeconds(0.1f);
        StartCoroutine(TeleportElimination());
    }
}


public class Integers
{
    public int n;

    public Integers(int x)
    {
        n = x;
    }
}