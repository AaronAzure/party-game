using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ShopMenu : MonoBehaviour
{
    private int        spellToBuy;    
    private int        manaCost; 
    private string     spellKind;
    private GameObject currentSelected;
    private bool       readyToUpdateAgain = true;
    private bool       hasPaid;
    public  bool       ignoreButton = true;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] public  GameObject confirmObj;
    [SerializeField] private PathFollower player;
    [SerializeField] private Spell[] buttonSpells;
    [SerializeField] private Image highlightKey;
    
    [SerializeField] private TextMeshProUGUI spellDesc;
    [SerializeField] private TextMeshProUGUI spellManaCost;
    [SerializeField] private Image spellIcon;

    // GRIMOIRE FULL
    [SerializeField] public GameObject discardObj;
    [SerializeField] public GameObject discardDescUi;
    [SerializeField] public TextMeshProUGUI discardDesc;
    [SerializeField] private Spell[] discardSpells; // SCRIPT ACCESS
    [SerializeField] private Image[] discardButtonSpells;
    [SerializeField] private Button  firstDiscard;

    // *************** to be bought *************** //
    [SerializeField] private string spellName;

    void Start() 
    {
        buttonSpells = gameObject.GetComponentsInChildren<Spell>();
        currentSelected = EventSystem.current.currentSelectedGameObject;

        // GET INSPECTOR SPELLS TO SCRIPT
        foreach (Spell s in buttonSpells)
        {
            if (s._price > player.coins) {
                if (s._price == 999) { s._interactable.interactable = false; }
                s._color.color = new Color(1, 0.1f, 0.1f, 1);
            }
        }
    }

    private void OnEnable() {
        EventSystem.current.SetSelectedGameObject(currentSelected);
    }


    // Update is called once per frame
    void Update()
    {
        if (!confirmObj.activeSelf && !discardObj.activeSelf) 
        {
            // HIGHLIGHT WHICH BUTTON
            if (readyToUpdateAgain)
            {
                currentSelected = EventSystem.current.currentSelectedGameObject;
                highlightKey.transform.position = EventSystem.current.currentSelectedGameObject.transform.position;
            }
            // else if (currentSelected)
            
            // THE HIGHLIGHTED BUTTON SPELL, DISPLAY INFO (NAME, DESC, COST)
            foreach (Spell s in buttonSpells)
            {
                if (!s._interactable.interactable) { continue; }
                if (currentSelected.name == s.name) 
                {
                    costText.text   = s._price.ToString();
                    spellToBuy      = s._price;
                    manaCost        = s._mpCost;
                    spellKind       = s._spellKind;

                    spellDesc.text  = s._desc; 
                    spellIcon.sprite = s._color.sprite;
                    spellManaCost.text = s._mpCost.ToString();
                    break;
                }
            }
        }
        else if (discardObj.activeSelf)
        {
            currentSelected = EventSystem.current.currentSelectedGameObject;

            // THE HIGHLIGHTED BUTTON SPELL, DISPLAY INFO (NAME, DESC, COST)
            foreach (Spell s in discardSpells)
            {
                // if (!s._interactable.interactable) { continue; }
                if (currentSelected.name == s.name) 
                {
                    discardDesc.text   = s._desc;
                    // spellIconDisc.sprite = s._color.sprite;
                    // spellManaCostDisc.text = s._mpCost.ToString();
                    break;
                }
            }
        }
    }

    public void AreYouSure()
    {
        if (!confirmObj.activeSelf && readyToUpdateAgain && player.nPurchaseLeft > 0 &&
            currentSelected.gameObject.GetComponent<Image>().color != new Color(1, 0.1f, 0.1f, 1))
        {
            readyToUpdateAgain = false;

            confirmObj.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);

            StartCoroutine(InputDelay());
        }
    }
    private IEnumerator InputDelay()
    {
        yield return new WaitForSeconds(0.1f);
        ignoreButton = false;
    }

    private IEnumerator BuyMore()
    {
        yield return new WaitForSeconds(0.1f);
        discardObj.SetActive(false);
        EventSystem.current.SetSelectedGameObject(currentSelected);
        // currentSelected = origSelected;
        readyToUpdateAgain = true;
        hasPaid = false;
    }

    // BUTTON CALL FROM UI ("A")
    public void OnYes()
    {
        if (confirmObj.activeSelf && !discardObj.activeSelf && !hasPaid && !ignoreButton)
        {
            // BUYING SPELLS
            if (spellKind != "Item")
            {
                // GRIMOIRE INVENTORY IS FULL
                if (player.spells.Count >= 3) {
                    spellName = currentSelected.name;
                    for (int i=0 ; i<player.spells.Count ; i++)
                    {
                        discardButtonSpells[i].sprite = player.spellSlots[i].sprite;
                        discardButtonSpells[i].name = player.spellSlots[i].name;
                    }
                    discardButtonSpells[3].sprite = currentSelected.GetComponent<Image>().sprite;
                    discardButtonSpells[3].name   = currentSelected.name;
                    confirmObj.SetActive(false);
                    discardObj.SetActive(true);
                    discardDescUi.SetActive(true);
                    firstDiscard.Select();
                }
                // STILL HAVE SPACE
                else {
                    hasPaid = true;
                    ignoreButton = true;
                    StartCoroutine( player.BUYING_SPELL( spellToBuy, currentSelected.name, manaCost, spellKind) );
                    player.SHOP_ORB_UPDATE(spellToBuy);
                    // player.UPDATE_SPELLS_UI();

                    // CHECK WHICH SPELL CAN STILL BE BOUGHT
                    foreach (Spell s in buttonSpells)
                    {
                        if (!s._interactable.interactable) { continue; }
                        if (s._price > player.coins) {
                            s._color.color = new Color(1, 0.1f, 0.1f, 1);
                        }
                    }

                    confirmObj.SetActive(false);
                    StartCoroutine( BuyMore() );
                }
            }
            // BUYING ITEMS
            else
            {
                hasPaid = true;
                ignoreButton = true;

                // BOUGHT A MAGIC ORB
                if (currentSelected.name == "magic-orb" || currentSelected.name == "magic-orb (sale)")
                {
                    StartCoroutine( player.BUYING_ITEM( spellToBuy , true ) );
                    player.SHOP_ORB_UPDATE(spellToBuy);
                }
                // BUYING SPELLBOOK
                else if (currentSelected.name == "SpellBook")
                {
                    player.boughtASpellBook = true;  
                    player.spellsLeftToGain = 3 - player.spells.Count;  // GAIN UNTIL FULL
                    confirmObj.SetActive(false);
                    StartCoroutine( player.BOUGHT_A_SPELLBOOK() );
                    StartCoroutine( BuyMore() );
                    return;
                }
                // BUYING UPGRADE
                else {
                    StartCoroutine( player.BUYING_ITEM( spellToBuy , false ) );
                    player.SHOP_ORB_UPDATE(spellToBuy);
                }

                switch (currentSelected.name)
                {
                    case "move-potion"  :   player.playerMove15     = true;  break;
                    case "range-potion" :   player.playerRange2     = true; player.POWER_UP();  break;
                    case "vip-badge"    :   player.playerExtraBuy   = true; player.POWER_UP();  break;
                    case "magic-orb"    :     break;
                    case "magic-orb (sale)" : break;
                }

                // CHECK WHICH SPELL CAN STILL BE BOUGHT
                foreach (Spell s in buttonSpells)
                {
                    if (!s._interactable.interactable) { continue; }
                    if (s._price > (player.coins-spellToBuy)) {
                        s._color.color = new Color(1, 0.1f, 0.1f, 1);
                    }
                }
                confirmObj.SetActive(false);
                StartCoroutine( BuyMore() );
            }
        }
    }

    // BUTTON CALL FROM UI ("A")
    public void DiscardThis(int index)
    {
        // DISCARD OLD SPELL
        if (index < 3)
        {
            player.spells.RemoveAt(index);
            StartCoroutine( player.BUYING_SPELL( spellToBuy, spellName, manaCost, spellKind) );
            player.SHOP_ORB_UPDATE(spellToBuy);
            player.UPDATE_SPELLS_UI();
        }
        // DISCARD NEW SPELL
        discardObj.SetActive(false);
        // discardDescUi.SetActive(true);
        StartCoroutine( BuyMore() );
    }

    public void OnNo()
    {
        if (confirmObj.activeSelf && !discardObj.activeSelf)
        {
            hasPaid = true;
            ignoreButton = true;
            confirmObj.SetActive(false);
            StartCoroutine( BuyMore() );
        }
        else if (discardObj.activeSelf) 
        {
            // NOTHING
        }
        else
        {
            player.FINISHED_SHOPPING();
        }
    }

}
