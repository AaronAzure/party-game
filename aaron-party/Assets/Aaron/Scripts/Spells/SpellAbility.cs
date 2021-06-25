using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellAbility : MonoBehaviour
{
    [SerializeField] private string effectName; // INSPECTOR
    public PathFollower playerToBenefit;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Hurtbox")
        {
            Debug.Log(effectName + " TRIGGERED");
            PathFollower player =  other.transform.parent.gameObject.GetComponent<PathFollower>();
            switch (effectName)
            {
                case "Meteor_Explosion" :     { StartCoroutine( player.LOSE_COINS(-15) );    break; }
                //// case "Laser_Ray" :          { StartCoroutine( player.LOSE_COINS(-player.coins) );    break; }
                case "Darkness_Explosion" :   { player.LOSE_MP(-3);    break; }
                case "Lightning_Explosion" :  { StartCoroutine( player.LOSE_SPELL(-1) );    break; }
                case "Icicle_Explosion" :     { player.PLAYER_SLOWED();     break; }
                case "Steal_Explosion" :      { 
                    StartCoroutine( player.STEAL_COINS(-10, playerToBenefit) );     break; 
                }
                default : Debug.LogError("HAVE NOT ADDED EFFECT SPELL > " + other.name); break;
            }
            //* PLASMA PALACE, TURRET CANNON
            if (name.Contains("TurretLaserPurple")) {
                StartCoroutine( player.LOSE_ALL_COINS() );
            }
            // if (player == null) { Debug.LogError("-- couldn't find player"); }
            // else { StartCoroutine( player.LOSE_COINS(-10) ); }
        }
    }
}
