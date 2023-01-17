using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeSpellNode : MonoBehaviour
{

    [SerializeField] private Animator anim;
    [SerializeField] private GameObject magicCircle;
    public SpellNodeSpawner spawner;
    public bool isTitanSpell;
    private int nInside = 0;



    public bool GET_SPELL() 
    { 
        if (isTitanSpell) {spawner.TEDIOUS();}
        return (isTitanSpell); 
    }


    public void HIGHLIGHT(int n)
    {
        nInside += n;
        if (nInside > 0)    magicCircle.SetActive(true);
        else                magicCircle.SetActive(false);
    }
}
