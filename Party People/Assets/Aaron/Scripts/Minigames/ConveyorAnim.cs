using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorAnim : MonoBehaviour
{
    private Animator _anim;

    void Start()
    {
        _anim = this.GetComponent<Animator>();
        GameController ctr = GameObject.Find("Game_Controller").GetComponent<GameController>();
        if (ctr.easy)
        {
            _anim.speed = 0.75f;
        }

        if (GameObject.Find("Level_Manager") != null) { StartCoroutine( StartAnim(4) ); }
        else { StartCoroutine( StartAnim(0.5f) ); }   
    }

    IEnumerator StartAnim(float delay)
    {
        yield return new WaitForSeconds(delay - 0.01f);
        _anim.Play("Conveyor_Anim", -1, 0);

    }
}
