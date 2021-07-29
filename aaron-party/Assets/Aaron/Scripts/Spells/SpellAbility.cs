using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellAbility : MonoBehaviour
{
    [SerializeField] private string effectName; // INSPECTOR
    public PathFollower playerToBenefit;

    private void Start() {
        if (effectName == "" || effectName == null) {
            effectName = this.name;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Hurtbox")
        {
            Debug.Log(effectName + " TRIGGERED");
            PathFollower player =  other.transform.parent.gameObject.GetComponent<PathFollower>();
            switch (effectName)
            {
                case string a when a.Contains("Meteor_Explosion") :     { StartCoroutine( player.LOSE_COINS(-15) );    break; }
                case string a when a.Contains("Poison_Mist") :          { player.LOSE_MP(-2);    break; }
                case string a when a.Contains("Darkness_Explosion") :   { player.LOSE_MP(-3);    break; }
                case string a when a.Contains("Lightning_Explosion") :  { StartCoroutine( player.LOSE_SPELL(-1) );    break; }
                case string a when a.Contains("Icicle_Explosion") :     { player.PLAYER_SLOWED();     break; }
                case string a when a.Contains("Steal_Explosion") :      { 
                    StartCoroutine( player.STEAL_COINS(-10, playerToBenefit) );     break; 
                }
                //* PLASMA PALACE, TURRET CANNON
                case string a when a.Contains("Steal_Explosion") :      { StartCoroutine( player.LOSE_ALL_COINS() );  break; }
                
                default : Debug.LogError("HAVE NOT ADDED EFFECT SPELL > " + effectName); break;
            }
            //* PLASMA PALACE, TURRET CANNON
            // if (name.Contains("TurretLaserPurple")) {
            //     StartCoroutine( player.LOSE_ALL_COINS() );
            // }
            // if (player == null) { Debug.LogError("-- couldn't find player"); }
            // else { StartCoroutine( player.LOSE_COINS(-10) ); }
        }
    }
}
