using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class EffectSpell : MonoBehaviour
{
    private SpriteRenderer itself;
    [SerializeField] private PathFollower spellCasterPlayer;
    [SerializeField] public TextMeshPro  mpCost;
    public bool specialEffect;  // ADJUST IN INSPECTOR
    private bool inRange = true;
    public bool nodeLocked;
    private Node spaceToCastEffect;
    public PathFollower targetedPlayer;
    public string spellName;


    private void Start() {
        itself = gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnEnable() {
        mpCost.text = "-" + spellCasterPlayer.spellMpCost.ToString();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // NODE EFFECT
        if (!specialEffect)
        {
            if (other.tag == "Node" && inRange)
            {
                Node space = other.transform.parent.GetComponent<Node>();
                if (space.VALID_NODE_TO_CAST_EFFECT())
                {
                    nodeLocked = true;
                    spaceToCastEffect = space;
                    spaceToCastEffect.SPELL_HIGHLIGHT();
                    mpCost.text = "-" + spellCasterPlayer.spellMpCost.ToString();
                }
            }
        }
        // SPECIFIC PLAYER EFFECT
        else 
        {
            if (other.tag == "Hurtbox" && inRange)
            {
                PathFollower p = other.GetComponent<HurtBoxPlayer>().player;
                if (p == null) Debug.LogError("FAILURE TO DETECT PLAYER");
                // Debug.Log("found player");
                nodeLocked = true;
                targetedPlayer = p;
                targetedPlayer.LOCKED_ON();
                mpCost.text = "-" + spellCasterPlayer.spellMpCost.ToString();
            }
        }
        if (other.tag == "AOE") { inRange = true; itself.color = new Color(0,0,0,1); }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!specialEffect)
        {
            if (other.tag == "Node")
            {
                if (spaceToCastEffect != null)
                {
                    nodeLocked = false;
                    spaceToCastEffect.SPELL_UNSELECT();
                    mpCost.text = "-" + spellCasterPlayer.spellMpCost.ToString();
                }
            }
        }
        else
        {
            if (other.tag == "Hurtbox")
            {
                if (targetedPlayer != null)
                {
                    nodeLocked = false;
                    targetedPlayer.LOCKED_OFF();
                    mpCost.text = "-" + spellCasterPlayer.spellMpCost.ToString();
                }
            }
        }
        if (other.tag == "AOE") { inRange = false; itself.color = new Color(0,0,0,0.4f); }
    }

    public void PLAYER_CAST_EFFECT()
    {
        // if (nodeLocked)
        if (nodeLocked && spellCasterPlayer.mpBar.value >= spellCasterPlayer.spellMpCost)
        {
            spaceToCastEffect.EFFECT_SPELL(spellName);
            spellCasterPlayer.USE_MP(spellCasterPlayer.spellMpCost);
        }
    }
    
    public void PLAYER_CAST_SPECIAL()
    {
        // SWAP SPACES
        if (spellName == "Spell_Effect_Swap")
        {
            if (nodeLocked && spellCasterPlayer.mpBar.value >= spellCasterPlayer.spellMpCost)
            {
                Vector3 tempPos     = spellCasterPlayer.transform.position;
                string  tempPath    = spellCasterPlayer.data.path;
                
                // SET CASTER'S NEW LOCATION
                string newPath                          = targetedPlayer.data.path;

                spellCasterPlayer.SET_NEW_PATH(newPath);
                spellCasterPlayer.currentNode           = targetedPlayer.currentNode;
                spellCasterPlayer.transform.position    = targetedPlayer.currentNode.transform.position;
                spellCasterPlayer.currentNode.transform.position = spellCasterPlayer.transform.position;
                spellCasterPlayer.RESET_TARGET_SPELL_CAM();

                Vector3 asideDif = targetedPlayer.transform.position - targetedPlayer.currentNode.transform.position; 

                // SET TARGET'S NEW LOCATION
                string oldPath                      = tempPath; 
                targetedPlayer.SET_NEW_PATH(oldPath);
                targetedPlayer.data.pos             = tempPos;
                targetedPlayer.transform.position   = tempPos + asideDif;
                targetedPlayer.currentNode.transform.position = targetedPlayer.data.pos;


                spellCasterPlayer.USE_MP(spellCasterPlayer.spellMpCost);
            }
        }
        // STEAL ORB
        else if (spellName == "Spell_Effect_Orb") 
        {
            if (nodeLocked && spellCasterPlayer.mpBar.value >= spellCasterPlayer.spellMpCost)
            {
                if (targetedPlayer.orbs > 0) {
                    spellCasterPlayer.STEALING_ORB_EFFECT(targetedPlayer);
                    spellCasterPlayer.USE_MP(spellCasterPlayer.spellMpCost);
                }
            }
        }
    }

    // IEnumerator STEALING_ORB()
    // {
    //     spellCasterPlayer.USE_MP(spellCasterPlayer.spellMpCost);
    //     StartCoroutine( targetedPlayer.LOSE_ORBS(1) );
    //     yield return new WaitForSeconds(1);
    //     StartCoroutine( spellCasterPlayer.ORB_GAINED(1) );
    // }
}
