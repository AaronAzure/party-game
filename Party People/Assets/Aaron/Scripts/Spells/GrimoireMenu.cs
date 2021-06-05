using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class GrimoireMenu : MonoBehaviour
{

    [SerializeField] public Button[] _slots;
    [SerializeField] private Image highlightKey;
    [SerializeField] private PathFollower player;
    [SerializeField] public TextMeshProUGUI desc;
    [SerializeField] public Image manaCost;
    [SerializeField] public Sprite[] mpSprite;
    private Spell currentSpell;

    private void Start() {
        highlightKey.gameObject.SetActive(false);
        manaCost.sprite = mpSprite[0];
    }


    private void Update() 
    {
        if (player.spells.Count > 0)
        {
            highlightKey.gameObject.SetActive(true);
            highlightKey.transform.position = EventSystem.current.currentSelectedGameObject.transform.position;
            Spell sp = EventSystem.current.currentSelectedGameObject.GetComponent<Spell>();
            if (currentSpell == null || sp != currentSpell)
            {
                currentSpell = sp;
                desc.text = sp._desc;
                manaCost.sprite = mpSprite[sp._mpCost];
            }
            currentSpell = sp;
            desc.text = sp._desc;
            manaCost.sprite = mpSprite[sp._mpCost];
        }
        else 
        {
            highlightKey.gameObject.SetActive(false);
            manaCost.sprite = mpSprite[0];
        }
    }

    public void CHECK_EMPTY_GRIMOIRE()
    {
        if (player.spells.Count == 0) { desc.text = "No Spells :("; }
    } 

    public void CASTING_SPELL()
    {
        Sprite currentSelectedSpell = EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite;
    }
}
