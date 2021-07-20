using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Node : MonoBehaviour
{
    private AudioSource _soundNode;                 // AUDIO FOR GOING OVER A NODE/SPACE
    [SerializeField] private GameObject nodeContainer;     // space containing sprite
    

    [Header("Spell Keeper / Potion Master")]
    [Tooltip("The shop keeper number")] public int whoIsTheSeller;  // ** INSPECTOR


    [Header("Spell Related")]
    [SerializeField] private SpriteRenderer lockedOn;
    [SerializeField] public GameObject magicOrb;
    [SerializeField] public bool firstMagicOrb;
    private GameController controller;      // ** SCRIPT (RUN-TIME)


    [SerializeField] private GameObject aaronPrefab;
    [SerializeField] private GameObject aaron;
    [SerializeField] private Animator aaronAnim;
    [SerializeField] private GameObject teleportEffect;


    // TODO : which node each node connects to
    public enum Directions { none, left, right, up, down, blocked }
    [System.Serializable] public class Nexts
    {
        public GameObject node;
        public Directions direction;
        public GameObject alternative;
    }
    [Header("Paths - next node(s)")]
    public Nexts[] nexts;


    // MAGIC GATE = PATH IS LOCKED, UNLOCKED WITH 4 MANA CONSUMPTION
    [Header("Magic Gate")]
    public GameObject magicGate;
    private Animator gateAnim;
    


    [Header("Type of Space")]
    [SerializeField] private Sprite emptySpace;     //
    [SerializeField] private Sprite blueSpace;      
    [SerializeField] private Sprite redSpace;       
    [SerializeField] private Sprite goldSpace;       
    [SerializeField] private Sprite eventSpace;     //
    [SerializeField] private Sprite orbSpace;       //
    [SerializeField] private Sprite freeSpace;      //  FREE SPELL (RANDOM = CASUAL) (FIXED = COMPETITIVE)
    [SerializeField] private Sprite spellSpace;     //
    [SerializeField] private Sprite shopSpace;      //
    [SerializeField] private Sprite potionSpace;    //
    [SerializeField] private Sprite specialSpace;   //
    
    //* map-exclusive spaces
    [Header("Event Spaces")]
    [SerializeField] private Sprite boulderSpace;   // cause cave in
    [SerializeField] private Sprite boatSpace;    //  BOAT
    [SerializeField] private Sprite rotateSpace;   // laser rotate 90˚ anti-clockwise
    [SerializeField] private Sprite speedUpSpace;   // laser countdown faster


    [Header("Crystal Cavern boulder")]
    [SerializeField] private Sprite boulders;   // boulders
    private Sprite originalNode;
    public bool canBeCavedIn;
    public bool haveNotCalled;
    public int cavedSection;

    // SET THAT DOES NOT DECREASE MOVEMENT (COLLECTION)
    [SerializeField] private HashSet<Sprite> noCostSpace = new HashSet<Sprite>();   
    
    // SET THAT COUNTS TOWARDS HAPPENING BONUS (COLLECTION)
    [SerializeField] private HashSet<Sprite> greenSpaces = new HashSet<Sprite>();  
    
    // SET THAT CAN BE CONVERTED INTO TRAPS (COLLECTION)
    [SerializeField] private HashSet<Sprite> trapSpaces = new HashSet<Sprite>();  



    [Header("Player Spaces")]
    [SerializeField] private Sprite felixCoin10;
    [SerializeField] private Sprite felixCoin20;
    [SerializeField] private Sprite felixOrb;
    [SerializeField] private Sprite jacobCoin10;
    [SerializeField] private Sprite jacobCoin20;
    [SerializeField] private Sprite jacobOrb;
    [SerializeField] private Sprite laurelCoin10;
    [SerializeField] private Sprite laurelCoin20;
    [SerializeField] private Sprite laurelOrb;
    [SerializeField] private Sprite mauriceCoin10;
    [SerializeField] private Sprite mauriceCoin20;
    [SerializeField] private Sprite mauriceOrb;
    [SerializeField] private Sprite mimiCoin10;
    [SerializeField] private Sprite mimiCoin20;
    [SerializeField] private Sprite mimiOrb;
    [SerializeField] private Sprite pinkinsCoin10;
    [SerializeField] private Sprite pinkinsCoin20;
    [SerializeField] private Sprite pinkinsOrb;
    [SerializeField] private Sprite sweeterellaCoin10;
    [SerializeField] private Sprite sweeterellaCoin20;
    [SerializeField] private Sprite sweeterellaOrb;
    [SerializeField] private Sprite thanatosCoin10;
    [SerializeField] private Sprite thanatosCoin20;
    [SerializeField] private Sprite thanatosOrb;
    [SerializeField] private Sprite charlotteCoin10;
    [SerializeField] private Sprite charlotteCoin20;
    [SerializeField] private Sprite charlotteOrb;


    private SpriteRenderer _spaceType;
    private Animator _anim;
    [SerializeField] private TextMeshPro nSpace;


    [Header("Special effect animations")]
    [SerializeField] private GameObject trapCreateEffect;
    [SerializeField] private GameObject instanBlueEffect;
    [SerializeField] private GameObject instanRedEffect;
    [SerializeField] private GameObject instanGoldEffect;
    [SerializeField] private GameObject instanGreenEffect;
    [SerializeField] private GameObject instanTrap10Effect;
    [SerializeField] private GameObject instanTrap20Effect;
    [SerializeField] private GameObject instanTrapOrbEffect;
    [SerializeField] private GameObject effect10Prefab;
    [SerializeField] private GameObject effectMp3Prefab;
    [SerializeField] private GameObject effectSpell1Prefab;
    [SerializeField] private GameObject effectSlow1Prefab;
    [SerializeField] private GameObject moveStealPrefab;



    // ---------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        // * IF MAGIC GATE
        if (magicGate != null) {
            gateAnim = magicGate.GetComponent<Animator>();
        }

        if (_anim == null)      { _anim = this.GetComponentInChildren<Animator>(); }
        if (_spaceType == null) { _spaceType = nodeContainer.GetComponent<SpriteRenderer>(); }
        originalNode = _spaceType.sprite;

        // SET THAT DOES NOT DECREASE MOVEMENT (COLLECTION)
        noCostSpace.Add(emptySpace);
        noCostSpace.Add(orbSpace);
        noCostSpace.Add(freeSpace);
        noCostSpace.Add(shopSpace);
        noCostSpace.Add(potionSpace);
        noCostSpace.Add(specialSpace);
        noCostSpace.Add(boulders);

        // SET THAT COUNTS TOWARDS HAPPENING BONUS (COLLECTION)
        greenSpaces.Add(boatSpace);
        greenSpaces.Add(boulderSpace);
        greenSpaces.Add(rotateSpace);
        greenSpaces.Add(speedUpSpace);

        // SET THAT CAN BE CONVERTED INTO TRAPS (COLLECTION)
        if (true) {
            trapSpaces.Add(blueSpace);
            trapSpaces.Add(redSpace);
            trapSpaces.Add(felixCoin10);
            trapSpaces.Add(felixCoin20);
            trapSpaces.Add(felixOrb);
            trapSpaces.Add(jacobCoin10);
            trapSpaces.Add(jacobCoin20);
            trapSpaces.Add(jacobOrb);
            trapSpaces.Add(laurelCoin10);
            trapSpaces.Add(laurelCoin20);
            trapSpaces.Add(laurelOrb);
            trapSpaces.Add(mauriceCoin10);
            trapSpaces.Add(mauriceCoin20);
            trapSpaces.Add(mauriceOrb);
            trapSpaces.Add(mimiCoin10);
            trapSpaces.Add(mimiCoin20);
            trapSpaces.Add(mimiOrb);
            trapSpaces.Add(pinkinsCoin10);
            trapSpaces.Add(pinkinsCoin20);
            trapSpaces.Add(pinkinsOrb);
            trapSpaces.Add(sweeterellaCoin10);
            trapSpaces.Add(sweeterellaCoin20);
            trapSpaces.Add(sweeterellaOrb);
            trapSpaces.Add(thanatosCoin10);
            trapSpaces.Add(thanatosCoin20);
            trapSpaces.Add(thanatosOrb);
            trapSpaces.Add(charlotteCoin10);
            trapSpaces.Add(charlotteCoin20);
            trapSpaces.Add(charlotteOrb);
        }
        
        _soundNode = this.GetComponentInChildren<AudioSource>();

        // ONLY ORB SPACES SHOULD HAVE AN AARON GAMEOBJ
        if (_spaceType.sprite != orbSpace) { Destroy(aaron); }
        if (_spaceType.sprite == freeSpace) { FREE_SPELL_SPACE_SETUP(); }
    
        // SET ANIMATIONS
        CHANGE_ANIMATION();
    }

    // WALK OVER SOUND EFFECT
    public void PlaySound() {
        _soundNode.Play();
    }





    // TRUE  = DECREASE MOVEMENTS
    // FALSE = EMPTY
    public bool DOES_SPACE_DECREASE_MOVEMENT() 
    {
        if (magicGate != null) return false;    //! MAGIC GATE

        return !noCostSpace.Contains(_spaceType.sprite);
    }
    // todo WHEN CROSSING OVER A SPACE THAT DOES NOT DECREMENT MOVEMENT //? (SPECIAL SPACES)
    public bool TYPE_OF_SPECIAL_SPACE(string space)
    {
        switch (space.ToLower()) 
        {
            case "free" :   return (_spaceType.sprite == freeSpace);
            case "shop" :   return (_spaceType.sprite == shopSpace);
            case "potion" : return (_spaceType.sprite == potionSpace);
            case "orb" :    return (_spaceType.sprite == orbSpace);
            case "spec" :   return (_spaceType.sprite == specialSpace);
            case "gate" :   return (magicGate != null);
        }
        Debug.LogError("ERROR: not a registered space");
        return false;
    }


    public void OPEN_MAGIC_GATE()
    {
        if (gateAnim != null) {
            gateAnim.SetTrigger("Open");
        }
    }
    // NORMAL SPACE
    public bool SPACE_LANDED()
    {
        if (magicGate != null) return false;    //! MAGIC GATE

        return (_spaceType.sprite == blueSpace || _spaceType.sprite == redSpace ||
            _spaceType.sprite == goldSpace || _spaceType.sprite == eventSpace);
    }

    // TODO - landed on (no longer moving)
    public bool IS_GREEN() { 
        if (magicGate != null) return false;    //! MAGIC GATE

        if (greenSpaces.Contains(_spaceType.sprite)) {
            var eff = Instantiate(instanGreenEffect, transform.position, instanGreenEffect.transform.rotation);
            Destroy(eff, 4);
        }
        return (greenSpaces.Contains(_spaceType.sprite)); 
    }    // BONUS
    public bool IS_SPELL() { return (_spaceType.sprite == spellSpace); }
    // public bool IS_EVENT() { return (_spaceType.sprite == eventSpace || _spaceType.sprite == happenSpace); }
    public bool IS_BLUE() { return (_spaceType.sprite == blueSpace); }
    public bool IS_GOLD() { return (_spaceType.sprite == goldSpace); }
    public bool IS_RED() { return (_spaceType.sprite == redSpace); }
    public bool IS_ORB_TRAP()
    {
        return (_spaceType.sprite == felixOrb || _spaceType.sprite == jacobOrb || _spaceType.sprite == laurelOrb
            || _spaceType.sprite == mauriceOrb || _spaceType.sprite == mimiOrb || _spaceType.sprite == pinkinsOrb
            || _spaceType.sprite == sweeterellaOrb || _spaceType.sprite == thanatosOrb || _spaceType.sprite == charlotteOrb);
    }
    public bool IS_BOULDER_EVENT() { return (_spaceType.sprite == boulderSpace); }

    //* EVENT SPACES
    public bool IS_BLOCKED() { return (_spaceType.sprite == boulders); }
    public bool IS_BOAT() { return (_spaceType.sprite == boatSpace); }
    public bool IS_ROTATE() { return (_spaceType.sprite == rotateSpace); }
    public bool IS_SPEED_UP() { return (_spaceType.sprite == speedUpSpace); }



    // todo CRYSTAL CAVERNS 
    public void BLOCK() { 
        _spaceType.sprite = boulders; 
        _anim.Play("Empty", -1 ,0); CHANGE_ANIMATION(); 
        controller.NewBoulder(transform.parent.name, this.name); 
    }
    public void UNBLOCK() { 
        Debug.Log("         unblocked");
        _spaceType.sprite = emptySpace; 
        _anim.Play("Empty", -1 ,0); CHANGE_ANIMATION(); 
        controller.RemoveBoulder(transform.parent.name, this.name); 
    }



    // BLUE || RED || EVENT
    public int COINS_RECEIVED_FROM_SPACE()
    {
        if (_spaceType.sprite == blueSpace)
        {
            // Debug.Log("  Landed on BLUE");
            var eff = Instantiate(instanBlueEffect, transform.position, instanBlueEffect.transform.rotation);
            Destroy(eff, 4);
            return 3;
        }
        else if (_spaceType.sprite == redSpace)
        {
            // Debug.Log("  Landed on RED");
            var eff = Instantiate(instanRedEffect, transform.position, instanRedEffect.transform.rotation);
            Destroy(eff, 4);
            return -3;
        }
        else if (_spaceType.sprite == goldSpace)
        {
            // Debug.Log("  Landed on FORTUNE");
            var eff = Instantiate(instanGoldEffect, transform.position, instanGoldEffect.transform.rotation);
            Destroy(eff, 4);
            int[] fortune = new int[]{10,10,10,10,10,15,15,15,20,30};
            return fortune[ Random.Range(0, fortune.Length) ];
        }
        // else if (_spaceType.sprite == eventSpace)
        // {
        //     Debug.Log("  Landed on EVENT");
        //     Instantiate(instanGreenEffect, transform.position, instanGreenEffect.transform.rotation);
        //     return Random.Range(1,11);
        // }
        return 0;
    }

    private void CHANGE_ANIMATION()
    {
        if (magicGate != null) return;    //! MAGIC GATE

        if (_anim == null) { _anim = this.GetComponentInChildren<Animator>(); }

        magicOrb.SetActive(false);

        if (_spaceType.sprite == blueSpace)         { this._anim.SetTrigger("isBlue"); }
        else if (_spaceType.sprite == redSpace)     { this._anim.SetTrigger("isRed"); }
        else if (_spaceType.sprite == goldSpace)    { this._anim.SetTrigger("isSpell"); }
        else if (_spaceType.sprite == eventSpace)   { this._anim.SetTrigger("isEvent"); }
        else if (greenSpaces.Contains(_spaceType.sprite))  { this._anim.SetTrigger("isHappen"); }
        //// else if (_spaceType.sprite == happenSpace)  { this._anim.SetTrigger("isHappen"); }
        else if (_spaceType.sprite == spellSpace)   { this._anim.SetTrigger("isSpell"); }
        else if (_spaceType.sprite == freeSpace)    { this._anim.SetTrigger("isFree"); }
        else if (_spaceType.sprite == shopSpace)    {}
        else if (_spaceType.sprite == potionSpace)  {}
        else if (_spaceType.sprite == orbSpace)     {
            controller = GameObject.Find("Game_Controller").GetComponent<GameController>();
            this._anim.SetBool("isOrb", true);
            this._anim.SetBool("isOrb", false);

            // TURN 1 ONLY (REGISTER ORB SPACES OF MAP ONCE)
            if (controller.hasStarted == false) {
                controller.NewOrbSpace(transform.parent.name, this.name, _spaceType.sprite);
            }
        }
        // TRAP SPACES
        else { this._anim.SetTrigger("isTrap"); }

        if (canBeCavedIn) {
            originalNode = _spaceType.sprite;
            controller = GameObject.Find("Game_Controller").GetComponent<GameController>();

            // TURN 1 ONLY (REGISTER CAN BE CAVED-IN SPACES OF MAP)
            if (!controller.hasStarted && !haveNotCalled) {
                haveNotCalled = true;
                controller.NewCavedInSpace(transform.parent.name, this.name, cavedSection);
            }
        }

        // if (_spaceType.sprite != shopSpace) { _spaceType.color = new Color(1, 1, 1, 0.6f); }
    }

    // SPACES WHERE YOU CAN CAST EFFECTS
    public bool VALID_NODE_TO_CAST_EFFECT()
    {
        if (magicGate != null) return false;    //! MAGIC GATE

        return (_spaceType.sprite != emptySpace && _spaceType.sprite != orbSpace && _spaceType.sprite != shopSpace
            && _spaceType.sprite != potionSpace);
    }


    // ------------------------------------------------------------------------------------------ //
    // ------------------------------------ TRAP RELATED ---------------------------------------- //
    // SPACES THAT CAN BE TURNED INTO TRAPS (ie NOT EVENTS, EMPTY, ORB, SHOP)
    public bool VALID_NODE_TO_TRANSFORM()
    {
        if (magicGate != null) return false;    //! MAGIC GATE

        return trapSpaces.Contains(_spaceType.sprite);
        // return (_spaceType.sprite == blueSpace || _spaceType.sprite == redSpace);
        // return (_spaceType.sprite != emptySpace && _spaceType.sprite != orbSpace && _spaceType.sprite != eventSpace
        //     && _spaceType.sprite != happenSpace && _spaceType.sprite != potionSpace && _spaceType.sprite != spellSpace
        //     && _spaceType.sprite != specialSpace && _spaceType.sprite != shopSpace && _spaceType.sprite != freeSpace);
    }
    
    public void SPELL_HIGHLIGHT() { lockedOn.gameObject.SetActive(true); }
    public void SPELL_UNSELECT() { lockedOn.gameObject.SetActive(false); }
    
    public void TURN_INTO_A_TRAP(string characterName, string spellName)
    {
        // _spaceType.sprite = pinkSpace;
        switch (spellName)
        {
            case "Spell_Trap_10" : 
                if      (characterName == "Felix")       {_spaceType.sprite = felixCoin10;}   
                else if (characterName == "Jacob")       {_spaceType.sprite = jacobCoin10;}   
                else if (characterName == "Laurel")      {_spaceType.sprite = laurelCoin10;}   
                else if (characterName == "Maurice")     {_spaceType.sprite = mauriceCoin10;}   
                else if (characterName == "Mimi")        {_spaceType.sprite = mimiCoin10;}   
                else if (characterName == "Pinkins")     {_spaceType.sprite = pinkinsCoin10;}   
                else if (characterName == "Sweeterella") {_spaceType.sprite = sweeterellaCoin10;}   
                else if (characterName == "Thanatos")    {_spaceType.sprite = thanatosCoin10;}   
                else if (characterName == "Charlotte")   {_spaceType.sprite = charlotteCoin10;}   
                break;   
            case "Spell_Trap_20" : 
                if      (characterName == "Felix")       {_spaceType.sprite = felixCoin20;}   
                else if (characterName == "Jacob")       {_spaceType.sprite = jacobCoin20;}   
                else if (characterName == "Laurel")      {_spaceType.sprite = laurelCoin20;}   
                else if (characterName == "Maurice")     {_spaceType.sprite = mauriceCoin20;}   
                else if (characterName == "Mimi")        {_spaceType.sprite = mimiCoin20;}   
                else if (characterName == "Pinkins")     {_spaceType.sprite = pinkinsCoin20;}   
                else if (characterName == "Sweeterella") {_spaceType.sprite = sweeterellaCoin20;}   
                else if (characterName == "Thanatos")    {_spaceType.sprite = thanatosCoin20;}   
                else if (characterName == "Charlotte")   {_spaceType.sprite = charlotteCoin20;}   
                break;   
            case "Spell_Trap_Orb" : 
                if      (characterName == "Felix")       {_spaceType.sprite = felixOrb;}   
                else if (characterName == "Jacob")       {_spaceType.sprite = jacobOrb;}   
                else if (characterName == "Laurel")      {_spaceType.sprite = laurelOrb;}   
                else if (characterName == "Maurice")     {_spaceType.sprite = mauriceOrb;}   
                else if (characterName == "Mimi")        {_spaceType.sprite = mimiOrb;}   
                else if (characterName == "Pinkins")     {_spaceType.sprite = pinkinsOrb;}   
                else if (characterName == "Sweeterella") {_spaceType.sprite = sweeterellaOrb;}   
                else if (characterName == "Thanatos")    {_spaceType.sprite = thanatosOrb;}  
                else if (characterName == "Charlotte")   {_spaceType.sprite = charlotteOrb;}  
                break;   
        }

        this._anim.SetTrigger("isTrap");
        Instantiate(trapCreateEffect, transform.position, Quaternion.identity); // VFX

        if (controller == null) 
        {
            controller = GameObject.Find("Game_Controller").GetComponent<GameController>();
        }
        controller.NewTrap(transform.parent.name, this.name, _spaceType.sprite);
    }

    // PLAYER'S TRAP TO CARRY OVER (FROM GAME_CONTROLLER)
    public void CHANGE_SPACE_TYPE(Sprite spaceType)
    {
        if (magicGate != null) return;    //! MAGIC GATE

        if (_spaceType == null) { _spaceType = nodeContainer.GetComponent<SpriteRenderer>(); }
        _spaceType.sprite = spaceType;

        if (_anim == null) { _anim = this.GetComponentInChildren<Animator>(); }
        _anim.SetTrigger("isTrap");
    }

    public void BACK_TO_BEING_A_BOULDER()
    {
        if (_spaceType == null) { _spaceType = nodeContainer.GetComponent<SpriteRenderer>(); }
        _spaceType.sprite = boulders;

        if (_anim == null) { _anim = this.GetComponentInChildren<Animator>(); }
        _anim.SetTrigger("isTrap");
    }

    // ORB TRAP
    public int ORB_TRAP_SPACE_COST(string characterName)
    {
        if (_spaceType.sprite == felixOrb && characterName == "Felix") { return 5; }
        if (_spaceType.sprite == jacobOrb && characterName == "Jacob") { return 5; }
        if (_spaceType.sprite == laurelOrb && characterName == "Laurel") { return 5; }
        if (_spaceType.sprite == mauriceOrb && characterName == "Maurice") { return 5; }
        if (_spaceType.sprite == mimiOrb && characterName == "Mimi")   { return 5; }
        if (_spaceType.sprite == pinkinsOrb && characterName == "Pinkins") { return 5; }
        if (_spaceType.sprite == sweeterellaOrb && characterName == "Sweeterella") { return 5; }
        if (_spaceType.sprite == thanatosOrb && characterName == "Thanatos") { return 5; }
        if (_spaceType.sprite == charlotteOrb && characterName == "Charlotte") { return 5; }
        
        var eff = Instantiate(instanTrapOrbEffect, transform.position, instanTrapOrbEffect.transform.rotation);
        Destroy(eff, 4);
        return 0;
    }

    public int TRAP_SPACE_COST(string characterName)
    {
        if      (_spaceType.sprite == felixCoin10) {
            if (characterName == "Felix") { return 5; }
            var eff = Instantiate(instanTrap10Effect, transform.position, instanTrap10Effect.transform.rotation);
            Destroy(eff, 4);
            return -10;
        }
        else if (_spaceType.sprite == felixCoin20) {
            if (characterName == "Felix") { return 5; }
            var eff = Instantiate(instanTrap20Effect, transform.position, instanTrap20Effect.transform.rotation);
            Destroy(eff, 4);
            return -20;
        }


        else if (_spaceType.sprite == jacobCoin10) {
            if (characterName == "Jacob") { return 5; }
            var eff = Instantiate(instanTrap10Effect, transform.position, instanTrap10Effect.transform.rotation);
            Destroy(eff, 4);
            return -10;
        }
        else if (_spaceType.sprite == jacobCoin20) {
            if (characterName == "Jacob") { return 5; }
            var eff = Instantiate(instanTrap20Effect, transform.position, instanTrap20Effect.transform.rotation);
            Destroy(eff, 4);
            return -20;
        }

        else if (_spaceType.sprite == laurelCoin10) {
            if (characterName == "Laurel") { return 5; }
            var eff = Instantiate(instanTrap10Effect, transform.position, instanTrap10Effect.transform.rotation);
            Destroy(eff, 4);
            return -10;
        }
        else if (_spaceType.sprite == laurelCoin20) {
            if (characterName == "Laurel") { return 5; }
            var eff = Instantiate(instanTrap20Effect, transform.position, instanTrap20Effect.transform.rotation);
            Destroy(eff, 4);
            return -20;
        }

        else if (_spaceType.sprite == mauriceCoin10) {
            if (characterName == "Maurice") { return 5; }
            var eff = Instantiate(instanTrap10Effect, transform.position, instanTrap10Effect.transform.rotation);
            Destroy(eff, 4);
            return -10;
        }
        else if (_spaceType.sprite == mauriceCoin20) {
            if (characterName == "Maurice") { return 5; }
            var eff = Instantiate(instanTrap20Effect, transform.position, instanTrap20Effect.transform.rotation);
            Destroy(eff, 4);
            return -20;
        }

        else if (_spaceType.sprite == mimiCoin10) {
            if (characterName == "Mimi") { return 5; }
            var eff = Instantiate(instanTrap10Effect, transform.position, instanTrap10Effect.transform.rotation);
            Destroy(eff, 4);
            return -10;
        }
        else if (_spaceType.sprite == mimiCoin20) {
            if (characterName == "Mimi") { return 5; }
            var eff = Instantiate(instanTrap20Effect, transform.position, instanTrap20Effect.transform.rotation);
            Destroy(eff, 4);
            return -20;
        }

        else if (_spaceType.sprite == pinkinsCoin10) {
            if (characterName == "Pinkins") { return 5; }
            var eff = Instantiate(instanTrap10Effect, transform.position, instanTrap10Effect.transform.rotation);
            Destroy(eff, 4);
            return -10;
        }
        else if (_spaceType.sprite == pinkinsCoin20) {
            if (characterName == "Pinkins") { return 5; }
            var eff = Instantiate(instanTrap20Effect, transform.position, instanTrap20Effect.transform.rotation);
            Destroy(eff, 4);
            return -20;
        }

        else if (_spaceType.sprite == sweeterellaCoin10) {
            if (characterName == "Sweeterella") { return 5; }
            var eff = Instantiate(instanTrap10Effect, transform.position, instanTrap10Effect.transform.rotation);
            Destroy(eff, 4);
            return -10;
        }
        else if (_spaceType.sprite == sweeterellaCoin20) {
            if (characterName == "Sweeterella") { return 5; }
            var eff = Instantiate(instanTrap20Effect, transform.position, instanTrap20Effect.transform.rotation);
            Destroy(eff, 4);
            return -20;
        }

        else if (_spaceType.sprite == thanatosCoin10) {
            if (characterName == "Thanatos") { return 5; }
            var eff = Instantiate(instanTrap10Effect, transform.position, instanTrap10Effect.transform.rotation);
            Destroy(eff, 4);
            return -10;
        }
        else if (_spaceType.sprite == thanatosCoin20) {
            if (characterName == "Thanatos") { return 5; }
            var eff = Instantiate(instanTrap20Effect, transform.position, instanTrap20Effect.transform.rotation);
            Destroy(eff, 4);
            return -20;
        }

        else if (_spaceType.sprite == charlotteCoin10) {
            if (characterName == "Charlotte") { return 5; }
            var eff = Instantiate(instanTrap10Effect, transform.position, instanTrap10Effect.transform.rotation);
            Destroy(eff, 4);
            return -10;
        }
        else if (_spaceType.sprite == charlotteCoin20) {
            if (characterName == "Charlotte") { return 5; }
            var eff = Instantiate(instanTrap20Effect, transform.position, instanTrap20Effect.transform.rotation);
            Destroy(eff, 4);
            return -20;
        }

        return 5;
    }
    
    public string WHOS_TRAP()
    {
        if      (_spaceType.sprite == felixCoin10 || _spaceType.sprite == felixCoin20 || _spaceType.sprite == felixOrb) { 
            return "Felix"; 
        }
        else if (_spaceType.sprite == jacobCoin10 || _spaceType.sprite == jacobCoin20 || _spaceType.sprite == jacobOrb) {
            return "Jacob"; 
        }
        else if (_spaceType.sprite == laurelCoin10 || _spaceType.sprite == laurelCoin20 || _spaceType.sprite == laurelOrb) { 
            return "Laurel"; 
        }
        else if (_spaceType.sprite == mauriceCoin10 || _spaceType.sprite == mauriceCoin20 || _spaceType.sprite == mauriceOrb) { 
            return "Maurice";
        }
        else if (_spaceType.sprite == mimiCoin10 || _spaceType.sprite == mimiCoin20 || _spaceType.sprite == mimiOrb) { 
            return "Mimi"; 
        }
        else if (_spaceType.sprite == pinkinsCoin10 || _spaceType.sprite == pinkinsCoin20 || _spaceType.sprite == pinkinsOrb)  { 
            return "Pinkins"; 
        }
        else if (_spaceType.sprite == sweeterellaCoin10 || _spaceType.sprite == sweeterellaCoin20 || _spaceType.sprite == sweeterellaOrb) { 
            return "Sweeterella"; 
        }
        else if (_spaceType.sprite == thanatosCoin10 || _spaceType.sprite == thanatosCoin20 || _spaceType.sprite == thanatosOrb) { 
            return "Thanatos"; 
        }
        else if (_spaceType.sprite == charlotteCoin10 || _spaceType.sprite == charlotteCoin20 || _spaceType.sprite == charlotteOrb) { 
            return "Charlotte"; 
        }

        return null;
    }
    
    public int TRAP_MP_COST()
    {
        if      (_spaceType.sprite == felixCoin10 || _spaceType.sprite == jacobCoin10 || _spaceType.sprite == laurelCoin10
                || _spaceType.sprite == mauriceCoin10 || _spaceType.sprite == mimiCoin10 || _spaceType.sprite == pinkinsCoin10
                || _spaceType.sprite == sweeterellaCoin10 || _spaceType.sprite == thanatosCoin10 || _spaceType.sprite == charlotteCoin10) {
            return 1;
        }
        else if (_spaceType.sprite == felixCoin20 || _spaceType.sprite == jacobCoin20 || _spaceType.sprite == laurelCoin20
                || _spaceType.sprite == mauriceCoin20 || _spaceType.sprite == mimiCoin20 || _spaceType.sprite == pinkinsCoin20
                || _spaceType.sprite == sweeterellaCoin20 || _spaceType.sprite == thanatosCoin20 || _spaceType.sprite == charlotteCoin20) {
            return 2;
        }
        else if (_spaceType.sprite == felixOrb || _spaceType.sprite == jacobOrb || _spaceType.sprite == laurelOrb
                || _spaceType.sprite == mauriceOrb || _spaceType.sprite == mimiOrb || _spaceType.sprite == pinkinsOrb
                || _spaceType.sprite == sweeterellaOrb || _spaceType.sprite == thanatosOrb || _spaceType.sprite == charlotteOrb) {
            return 3;
        }

        return 0;
    }

    // ** PLAYER CASTED EFFECT SPELL (SPAWN MAGIC)
    public void EFFECT_SPELL(string spellName)
    {
        switch (spellName)
        {
            case "Spell_Effect_10" : 
                Instantiate(effect10Prefab, transform.position, Quaternion.identity); 
                if (_spaceType.sprite == boulders) UNBLOCK();
                break;
                
            case "Spell_Effect_Mana_3" : Instantiate(effectMp3Prefab, transform.position, Quaternion.identity); break;
            case "Spell_Effect_Spell_1" : Instantiate(effectSpell1Prefab, transform.position, Quaternion.identity); break;
            case "Spell_Effect_Slow_1" : Instantiate(effectSlow1Prefab, transform.position, Quaternion.identity); break;
            default : Debug.LogError("HAVE NOT ADDED EFFECT SPELL"); break;
        }
    }

    public void STEALING_COINS(PathFollower p)
    {
        if (p.stealMode)
        {
            if (moveStealPrefab != null)
            {
                var obj = Instantiate(moveStealPrefab, transform.position, moveStealPrefab.transform.rotation);
                obj.GetComponent<SpellAbility>().playerToBenefit = p;
            }
        }
    }


    // TODO - SHOW SPACES AWAY
    public void DISPLAY_MOVEMENT(int n, int moveLeft=1000)
    {
        // SPACE DECREMENTS MOVEMENT //? DISPLAY MOVEMENT
        if (!noCostSpace.Contains(_spaceType.sprite)) {
            if (nSpace.text == "x") {
                nSpace.gameObject.SetActive(true);
                nSpace.text = n.ToString();
                if (n == moveLeft) { nSpace.color = new Color(0,1,0); } //* WHERE YOU'LL LAND
                foreach (Nexts next in nexts) {
                    // Debug.Log(name);    //! DELETE
                    if (next.node != null) {
                        Node checkNode = next.node.GetComponent<Node>();
                        if (checkNode._spaceType.sprite != boulders) checkNode.DISPLAY_MOVEMENT(n + 1, moveLeft);
                    }
                }
            }
            // IF NUMBER ALREADY EXISTS, THEN DISPLAY THE SMALLER ONE //? DISPLAY MOVEMENT
            else {
                int number;
                bool success = int.TryParse(nSpace.text, out number);
                if (n == moveLeft) { nSpace.color = new Color(0,1,0); } //* WHERE YOU'LL LAND
                if (success && n < number) {
                    nSpace.gameObject.SetActive(true);
                    nSpace.text = n.ToString();
                    foreach (Nexts next in nexts) {
                        // Debug.Log(name + ", "  + n + ", " + number);    //! DELETE
                        if (next.node != null) {
                            Node checkNode = next.node.GetComponent<Node>();
                            if (checkNode._spaceType.sprite != boulders) checkNode.DISPLAY_MOVEMENT(n + 1, moveLeft);
                        }
                    }
                }
            }
        }
        // SPACE DOES NOT DECREMENT MOVEMENT //? DON'T DISPLAY
        else {
            foreach (Nexts next in nexts) {
                if (next.node != null) {
                    Node checkNode = next.node.GetComponent<Node>();
                    if (checkNode._spaceType.sprite != boulders) checkNode.DISPLAY_MOVEMENT(n, moveLeft);
                }
            }
        }
    }
    // HIDE SPACES AWAY
    public void HIDE_MOVEMENT() { 
        if (nSpace.gameObject.activeSelf || noCostSpace.Contains(_spaceType.sprite)) {
            nSpace.gameObject.SetActive(false); 
            nSpace.text = "x";
            nSpace.color = new Color(1,1,1);

            foreach (Nexts next in nexts) {
                next.node.GetComponent<Node>().HIDE_MOVEMENT();
            }
        }
    }


    // ----------------------------------------------------------------------------------------- //
    // ------------------------------------ ORB RELATED ---------------------------------------- //

    // CURRENT MAGIC ORB SPACE TO CARRY OVER FROM LAST SCENE
    public void TURN_INTO_ORB_SPACE()
    {
        magicOrb.SetActive(true);
        magicOrb.transform.parent = null;
        _spaceType.color = new Color(1,1,1,1);
        StartCoroutine( AARON_APPEARS() );
        this._anim.SetBool("isOrb", true);
    }

    IEnumerator AARON_APPEARS()
    {
        yield return new WaitForSeconds(2);
        // INSTANTIATE AARON
        if (aaronPrefab != null) { 
            Vector3 spawnPos = new Vector3(nodeContainer.transform.position.x, nodeContainer.transform.position.y );
            spawnPos += new Vector3(-2, 2);
            var orbHandler = Instantiate(aaronPrefab, spawnPos, Quaternion.identity, this.transform);
            aaron = orbHandler.gameObject;
            aaronAnim = aaron.GetComponent<Animator>();
            aaron.SetActive(false); 
        }
        aaron.SetActive(true);
        aaronAnim.Play("Aaron_Wave_Anim", -1, 0);
        var eff = Instantiate(teleportEffect, aaron.transform.position, teleportEffect.transform.rotation);
        eff.transform.parent = aaron.transform;
        Destroy(eff, 1);
    }
    IEnumerator AARON_DISSAPPEARS()
    {
        yield return new WaitForSeconds(1);

        aaronAnim.Play("Aaron_Cast_Anim", -1, 0);
        yield return new WaitForSeconds(1);
        var eff = Instantiate(teleportEffect, aaron.transform.position, teleportEffect.transform.rotation);
        Destroy(eff, 1);
        aaron.SetActive(false);
    }

    public void MAGIC_ORB_BOUGHT()
    {
        StartCoroutine( AARON_DISSAPPEARS() );
        magicOrb.SetActive(false);
        _spaceType.color = new Color(0.5f, 0.5f, 0.5f, 0.6f);
        this._anim.SetBool("isOrb", false);
    }


    // ----------------------------------------------------------------------------------------- //
    // ----------------------------------- FREE SPELL SPACE ------------------------------------ //
    
    void FREE_SPELL_SPACE_SETUP()
    {
        GameController controller = GameObject.Find("Game_Controller").GetComponent<GameController>();
        // if (controller.is) 
    }
}
