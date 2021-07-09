using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class TitleScreen : MonoBehaviour
{
    private int playerID = 0;
    private Player player;
    private bool skipped;
    [SerializeField] private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(playerID);
        StartCoroutine(TransitionOver());

        List<int> ints = new List<int>();
        ints.Add(0);

        //! DELTETE
        // List<Pid> pid = new List<Pid>();
        // List<int> tied = new List<int>();
        // pid.Add( new Pid(0, 100) );
        // pid.Add( new Pid(1, 100) );
        // pid.Add( new Pid(2, 150) );
        // pid.Add( new Pid(3, 100) );

        // for (int i=0 ; i<pid.Count - 1 ; i++) {
        //     if (pid[i].score == pid[i+1].score) {
        //         if (!tied.Contains(pid[i].id))      tied.Add( pid[i].id );
        //         if (!tied.Contains(pid[i+1].id))    tied.Add( pid[i+1].id );
        //         string debug = "";
        //         foreach (int num in tied)   debug += num.ToString();
        //         Debug.LogError(debug);
        //     }
        // }

    }

    void Update() {
        if ( (player.GetButtonDown("A") || player.GetButtonDown("B")) && !skipped) {
            skipped = true;
            anim.Play("Logo_Anim", -1, 0.9306f);
        }
    }

    IEnumerator TransitionOver()
    {
        yield return new WaitForSeconds(12);
        skipped = true;
    }
}


public class Pid
{
    public int id;
    public int score;

    public Pid(int newId, int newScore)
    {
        id    = newId;
        score = newScore;
    }
}