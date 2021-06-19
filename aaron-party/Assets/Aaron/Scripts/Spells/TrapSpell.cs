using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TrapSpell : MonoBehaviour
{
    private SpriteRenderer itself;
    [SerializeField] private PathFollower spellCasterPlayer;
    [SerializeField] public TextMeshPro  mpCost;
    private int extraMpToOverwrite;
    private bool inRange = true;
    private bool nodeLocked;
    private Node spaceToTransform;
    public string spellName;

    private void Start() {
        itself = gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnEnable() {
        mpCost.text = "-" + (spellCasterPlayer.spellMpCost + extraMpToOverwrite).ToString();
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.tag == "Node" && inRange)
        {
            Node space = other.transform.parent.GetComponent<Node>();
            if (space.VALID_NODE_TO_TRANSFORM())
            {
                nodeLocked = true;
                spaceToTransform = space;
                spaceToTransform.SPELL_HIGHLIGHT();
                extraMpToOverwrite = spaceToTransform.TRAP_MP_COST();
                if (extraMpToOverwrite != 0) { mpCost.color = new Color(1,0,0); }
                mpCost.text = "-" + (spellCasterPlayer.spellMpCost + extraMpToOverwrite).ToString();
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Node" && inRange)
        {
            Node space = other.transform.parent.GetComponent<Node>();
            if (space.VALID_NODE_TO_TRANSFORM())
            {
                nodeLocked = true;
                spaceToTransform = space;
                spaceToTransform.SPELL_HIGHLIGHT();
                extraMpToOverwrite = spaceToTransform.TRAP_MP_COST();
                if (extraMpToOverwrite != 0) { mpCost.color = new Color(1,0,0); }
                mpCost.text = "-" + (spellCasterPlayer.spellMpCost + extraMpToOverwrite).ToString();
            }
        }
        if (other.tag == "AOE") { inRange = true; itself.color = new Color(0,0,0,1); }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Node")
        {
            if (spaceToTransform != null)
            {
                nodeLocked = false;
                spaceToTransform.SPELL_UNSELECT();
                mpCost.color = new Color(1,1,1);
                mpCost.text = "-" + spellCasterPlayer.spellMpCost.ToString();
                extraMpToOverwrite = 0;
            }
        }
        if (other.tag == "AOE") { inRange = false; itself.color = new Color(0,0,0,0.4f); }
    }

    public void PLAYER_CAST_TRAP()
    {
        if (nodeLocked && spellCasterPlayer.mpBar.value >= (spellCasterPlayer.spellMpCost + extraMpToOverwrite))
        {
            spaceToTransform.TURN_INTO_A_TRAP(spellCasterPlayer.characterName, spellName);
            spellCasterPlayer.USE_MP(spellCasterPlayer.spellMpCost + extraMpToOverwrite);
        }
    }
}
