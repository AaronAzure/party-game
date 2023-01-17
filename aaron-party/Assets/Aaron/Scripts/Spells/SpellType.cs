using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellType
{
    public string spellName;
    public int    spellCost;
    public string spellKind;

    public SpellType(string newSpellName, int newSpellCost, string newSpellKind)
    {
        spellName   = newSpellName;
        spellCost   = newSpellCost;
        spellKind   = newSpellKind;
    }
}
