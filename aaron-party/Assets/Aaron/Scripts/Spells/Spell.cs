using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//  SPELLS TO BUY IN SHOP
public class Spell : MonoBehaviour
{
    // public string   _name;
    public int    _price;
    public int    _mpCost;
    public string _spellKind;   // TRAP | EFFECT | MOVEMENT
    public string _desc;        // WHAT THE SPELL DOES

    public Button _interactable;
    public Image  _color;



    // Start is called before the first frame update
    void OnEnable()
    {
        switch (this.name)
        {
            case "Spell_Trap_10" :      _price = 5;     _mpCost = 1;    _spellKind = "Trap";
                _desc = "Create a trap, steal 10 gold from any opponent that lands on the trap.";    break;
            case "Spell_Trap_20" :      _price = 10;    _mpCost = 2;    _spellKind = "Trap";
                _desc = "Create a trap, steal 20 gold from any opponent that lands on the trap.";    break;
            case "Spell_Trap_Orb" :     _price = 20;    _mpCost = 3;    _spellKind = "Trap";
                _desc = "Create a trap, steal an orb from any opponent that lands on the trap.";    break;

            case "Spell_Effect_10" :    _price = 8;    _mpCost = 2;    _spellKind = "Effect";
                _desc = "Destroy 15 gold to all opponents on a targeted space (within range).";  break;
            case "Spell_Effect_Mana_3": _price = 12;    _mpCost = 2;    _spellKind = "Effect";
                _desc = "Destroy 3 mana to all opponents on a targeted space (within range).";    break;
            case "Spell_Effect_Spell_1": _price = 8;    _mpCost = 2;    _spellKind = "Effect";
                _desc = "Destroy a random spell to all opponents on a targeted space (within range).";    break;
            case "Spell_Effect_Slow_1": _price = 12;    _mpCost = 2;    _spellKind = "Effect";
                _desc = "Reduce max movement to 5 for all opponents on a targeted space (within range).";    break;
            case "Spell_Effect_Swap": _price = 20;    _mpCost = 3;    _spellKind = "Effect";
                _desc = "Swap places with a targeted opponent (within range).";    break;

            case "Spell_Move_Dash_5":   _price = 12;    _mpCost = 2;    _spellKind = "Move";
                _desc = "Immediately advance 4 spaces. Then proceed with your turn.";  break;
            case "Spell_Move_Dash_8":   _price = 25;    _mpCost = 3;    _spellKind = "Move";
                _desc = "Immediately advance 8 spaces. Then proceed with your turn.";    break;
            case "Spell_Move_Slowgo":     _price = 12;    _mpCost = 2;    _spellKind = "Move";
                _desc = "Reduce max movement to 5, but with Slower magic circle rotation.";    break;
            case "Spell_Move_Slow":     _price = 25;    _mpCost = 3;    _spellKind = "Move";
                _desc = "Slower magic circle rotation.";    break;
            case "Spell_Move_Steal":     _price = 7;    _mpCost = 3;    _spellKind = "Move";
                _desc = "Steal 10 coins from all opponents you pass.";    break;
            case "Spell_Move_Barrier":     _price = 10;    _mpCost = 2;    _spellKind = "Move";
                _desc = "Nullify all effects of opponent's spells or traps until the start of next turn";    break;
            case "Spell_Move_Orb":     _price = 25;    _mpCost = 3;    _spellKind = "Move";
                _desc = "Change the location of the Magic Orb space.";
                if (SceneManager.GetActiveScene().name == "Shogun_Seaport") { _desc = "Change the current boat"; }    break;


            case "SpellBook":     _price = 30;    _mpCost = 0;    _spellKind = "Item";
                _desc = "Contains three random spells";  break;


            case "move-potion":   _price = 40;    _mpCost = 0;    _spellKind = "Item";
                _desc = "Permanently increases max movement by 5.";    break;
            case "range-potion":   _price = 40;    _mpCost = 0;    _spellKind = "Item";
                _desc = "Permanently doubles your spell casting range.";    break;
            case "vip-badge":   _price = 40;    _mpCost = 0;    _spellKind = "Item";
                _desc = "VIP shop badge allows you to buy up to 3 spells per visit.";    break;
            case "magic-orb":   _price = 40;    _mpCost = 0;    _spellKind = "Item";
                _desc = "A more expensive winning condition.";
                if (SceneManager.GetActiveScene().name == "Shogun_Seaport") { _price = 20;
                    this.name = "magic-orb (sale)"; }    break;
            case "magic-orb (sale)":   _price = 40;    _mpCost = 0;    _spellKind = "Item";
                _desc = "A more expensive winning condition.";
                if (SceneManager.GetActiveScene().name == "Shogun_Seaport") { _price = 20; }    break;
            default :                   _price = 999;   _mpCost = 1;    _spellKind = "Trap";
                _desc = "No Spells :(";    break;
        }

        _interactable = this.gameObject.GetComponent<Button>();
        _color        = this.gameObject.GetComponent<Image>();
    }

    public string SpellDescription(string spellname)
    {
        switch (spellname)
        {
            case "Spell_Trap_10" :      return "Create a trap, steal 10 gold from any opponent that lands on the trap.";  
            case "Spell_Trap_20" :      return "Create a trap, steal 20 gold from any opponent that lands on the trap.";  
            case "Spell_Trap_Orb" :     return "Create a trap, steal an orb from any opponent that lands on the trap.";  

            case "Spell_Effect_10" :    return "Destroy 15 gold to all opponents on a targeted space (within range).";
            case "Spell_Effect_Mana_3": return "Destroy 3 mana to all opponents on a targeted space (within range).";  
            case "Spell_Effect_Spell_1":return "Destroy a random spell to all opponents on a targeted space (within range).";  
            case "Spell_Effect_Slow_1": return "Reduce max movement to 5 for all opponents on a targeted space (within range).";  
            case "Spell_Effect_Swap":   return "Swap places with a targeted opponent (within range).";  

            case "Spell_Move_Dash_5":   return "Immediately advance 4 spaces.";
            case "Spell_Move_Dash_8":   return "Immediately advance 8 spaces.";  
            case "Spell_Move_Slowgo":   return "Reduce max movement to 5, but with slower movement multiplier.";  
            case "Spell_Move_Slow":     return "Slower movement multiplier.";  
            case "Spell_Move_Steal":    return "Steal 10 coins from all opponents you pass.";  
            case "Spell_Move_Barrier":  return "Nullify all effects of opponent's spells or traps until the start of next turn";  
            case "Spell_Move_Orb":      return "Change the location of the Magic Orb space.";  
            default :                   return "";
        }
    }
}
