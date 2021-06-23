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
