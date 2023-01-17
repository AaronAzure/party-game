using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    [SerializeField] private Animator anim;

    private void Start() 
    {
        anim.Play("Oval_Transition", -1, 0.5f);
    }
}
