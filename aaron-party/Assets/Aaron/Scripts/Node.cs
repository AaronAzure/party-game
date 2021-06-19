﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Node : MonoBehaviour
{
    private AudioSource _soundNode;                 // AUDIO FOR GOING OVER A NODE/SPACE
    [SerializeField] private GameObject nodeContainer;     // space containing sprite
    
    [Tooltip("The shop keeper number")] public int whoIsTheSeller;  // ** INSPECTOR
    [SerializeField] private SpriteRenderer lockedOn;
    [SerializeField] private GameObject magicOrb;
    private GameController controller;      // ** SCRIPT (RUN-TIME)


    [SerializeField] private GameObject aaron;
    [SerializeField] private Animator aaronAnim;
    [SerializeField] private GameObject teleportEffect;


    // TODO : which node each node connects to
    public enum Directions { none, left, right, up, down }
    [System.Serializable] public class Nexts
    {
        public GameObject node;
        public Directions direction;
    }
    [Header("Paths - next node(s)")]
    public Nexts[] nexts;

    

    [Header("Type of Space")]
    [SerializeField] private Sprite emptySpace;     //
    [SerializeField] private Sprite blueSpace;      
    [SerializeField] private Sprite redSpace;       
    [SerializeField] private Sprite happenSpace;    //  BOAT
    [SerializeField] private Sprite eventSpace;     //
    [SerializeField] private Sprite orbSpace;       //
    [SerializeField] private Sprite freeSpace;      //  FREE SPELL (RANDOM = CASUAL) (FIXED = COMPETITIVE)
    [SerializeField] private Sprite spellSpace;     //
    [SerializeField] private Sprite shopSpace;      //
    [SerializeField] private Sprite potionSpace;    //
    [SerializeField] private Sprite specialSpace;   //
    [SerializeField] private HashSet<Sprite> noCostSpace = new HashSet<Sprite>();   // SET THAT DOES NOT DECREASE MOVEMENT (COLLECTION)
    [SerializeField] private HashSet<Sprite> canTurnToTrap = new HashSet<Sprite>();   // SET THAT TURN INTO TRAP (COLLECTION)//! DELETE



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
        if (_anim == null)      { _anim = this.GetComponentInChildren<Animator>(); }
        if (_spaceType == null) { _spaceType = nodeContainer.GetComponent<SpriteRenderer>(); }

        // SET THAT DOES NOT DECREASE MOVEMENT (COLLECTION)
        noCostSpace.Add(emptySpace);
        noCostSpace.Add(orbSpace);
        noCostSpace.Add(freeSpace);
        noCostSpace.Add(shopSpace);
        noCostSpace.Add(potionSpace);
        noCostSpace.Add(specialSpace);
        // canTurnToTrap.Add(); //! DELETE

        _soundNode = this.GetComponentInChildren<AudioSource>();
        if (aaron != null) { 
            aaron.SetActive(false); aaron.transform.parent = null; 
            aaronAnim = aaron.GetComponent<Animator>();
        }

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if (other.tag == "hello")
        // {
        //     // SPACES THAT DO DECREASE MOVEMENT
        //     if (_spaceType.sprite != emptySpace && _spaceType.sprite != orbSpace &&  _spaceType.sprite != freeSpace &&
        //         _spaceType.sprite != shopSpace && _spaceType.sprite != potionSpace && _spaceType.sprite != specialSpace) 
        //     {
        //         PathFollower p = other.GetComponent<PathFollower>();
        //         p.IN_CONTACT();
        //         STEALING_AURA(p);   // DOES PLAYER HAVE STEALING EFFECT ACTIVE

        //         _soundNode.Play();
        //     }
        //     // AT MAGIC ORB SPACE
        //     else if (_spaceType.sprite == orbSpace && magicOrb.activeSelf)  // BUY A MAGIC ORB
        //     {
        //         PathFollower p = other.GetComponent<PathFollower>();
        //         p.isAtMagicOrb = true;
        //     }
        //     // AT SHOP
        //     else if (_spaceType.sprite == shopSpace)                         // BUY SPELL(S)
        //     {
        //         PathFollower p = other.GetComponent<PathFollower>();
        //         if (whoIsTheSeller == 1) p.shop1 = true;    // MANUALLY EDIT IN INSPECTOR
        //         if (whoIsTheSeller == 2) p.shop2 = true;    // MANUALLY EDIT IN INSPECTOR
        //         if (whoIsTheSeller == 3) p.shop3 = true;    // MANUALLY EDIT IN INSPECTOR
        //         if (whoIsTheSeller == 4) p.shop4 = true;    // MANUALLY EDIT IN INSPECTOR
        //         p.isAtShop = true;
        //         p.RESET_PURCHASES();
        //         p.PURCHASES_LEFT();
        //     }
        //     // AT ITEM SHOP
        //     else if (_spaceType.sprite == potionSpace)                       // BUY ITEM(S)
        //     {
        //         PathFollower p = other.GetComponent<PathFollower>();
        //         if (whoIsTheSeller == 4) p.shop4 = true;    // MANUALLY EDIT IN INSPECTOR
        //         p.isAtShop = true;
        //         p.RESET_PURCHASES();
        //         p.PURCHASES_LEFT();
        //     }
        //     // AT FREE SPELL SPACE
        //     else if (_spaceType.sprite == freeSpace)
        //     {
        //         PathFollower p = other.GetComponent<PathFollower>();
        //         p.isAtFree = true;
        //         StartCoroutine( p.GAIN_FREE_SPELL() );
        //     }
        //     // AT BOAT MAGIC SPACE
        //     else if (_spaceType.sprite == specialSpace)                      // MULTIPLE EVENTS
        //     {
        //         PathFollower p = other.GetComponent<PathFollower>();
        //         if (!p.isBoat) p.MANAGER_EVENT();
        //     }
        // }
    }

    // TRUE = DECREASE MOVEMENTS
    // FALSK = EMPTY
    public bool DOES_SPACE_DECREASE_MOVEMENT() 
    {
        return !noCostSpace.Contains(_spaceType.sprite);
    }
    // todo WHEN CROSSING OVER A SPACE THAT DOES NOT DECREMENT MOVEMENT //? (SPECIAL SPACES)
    public bool TYPE_OF_SPECIAL_SPACE(string space)
    {
        switch (space) 
        {
            case "free" :   return (_spaceType.sprite == freeSpace);
            case "shop" :   return (_spaceType.sprite == shopSpace);
            case "potion" : return (_spaceType.sprite == potionSpace);
            case "orb" :    return (_spaceType.sprite == orbSpace);
            case "spec" :    return (_spaceType.sprite == specialSpace);
        }
        Debug.LogError("ERROR: not a registered space");
        return false;
    }


    // NORMAL SPACE
    public bool SPACE_LANDED()
    {
        return (_spaceType.sprite == blueSpace || _spaceType.sprite == redSpace || _spaceType.sprite == eventSpace);
    }

    public bool IS_SPELL() { return (_spaceType.sprite == spellSpace); }
    public bool IS_EVENT() { return (_spaceType.sprite == eventSpace || _spaceType.sprite == happenSpace); }
    public bool IS_BLUE() { return (_spaceType.sprite == blueSpace); }
    public bool IS_RED() { return (_spaceType.sprite == redSpace); }
    public bool IS_HAPPEN() { return (_spaceType.sprite == happenSpace); }

    public bool IS_ORB_TRAP()
    {
        return (_spaceType.sprite == felixOrb || _spaceType.sprite == jacobOrb || _spaceType.sprite == laurelOrb
            || _spaceType.sprite == mauriceOrb || _spaceType.sprite == mimiOrb || _spaceType.sprite == pinkinsOrb
            || _spaceType.sprite == sweeterellaOrb || _spaceType.sprite == thanatosOrb || _spaceType.sprite == charlotteOrb);
    }

    // BLUE || RED || EVENT
    public int COINS_RECEIVED_FROM_SPACE()
    {
        if (_spaceType.sprite == blueSpace)
        {
            Debug.Log("  Landed on BLUE");
            Instantiate(instanBlueEffect, transform.position, Quaternion.identity);
            return 3;
        }
        else if (_spaceType.sprite == redSpace)
        {
            Debug.Log("  Landed on RED");
            Instantiate(instanRedEffect, transform.position, Quaternion.identity);
            return -3;
        }
        else if (_spaceType.sprite == eventSpace)
        {
            Debug.Log("  Landed on EVENT");
            Instantiate(instanGreenEffect, transform.position, Quaternion.identity);
            return Random.Range(1,11);
        }
        return 0;
    }

    private void CHANGE_ANIMATION()
    {
        if (_anim == null) { _anim = this.GetComponentInChildren<Animator>(); }

        magicOrb.SetActive(false);

        if (_spaceType.sprite == blueSpace)         { this._anim.SetTrigger("isBlue"); }
        else if (_spaceType.sprite == redSpace)     { this._anim.SetTrigger("isRed"); }
        else if (_spaceType.sprite == eventSpace)   { this._anim.SetTrigger("isEvent"); }
        else if (_spaceType.sprite == happenSpace)  { this._anim.SetTrigger("isHappen"); }
        else if (_spaceType.sprite == spellSpace)   { this._anim.SetTrigger("isSpell"); }
        else if (_spaceType.sprite == freeSpace)    { this._anim.SetTrigger("isFree"); }
        else if (_spaceType.sprite == shopSpace)    {}
        else if (_spaceType.sprite == potionSpace)  {}
        else if (_spaceType.sprite == orbSpace)     {
            controller = GameObject.Find("Game_Controller").GetComponent<GameController>();
            this._anim.SetBool("isOrb", true);
            this._anim.SetBool("isOrb", false);

            // TURN 1 ONLY (REGISTER ORB SPACES OF MAP)
            if (controller.hasStarted == false) {
                controller.NewOrbSpace(transform.parent.name, this.name, _spaceType.sprite.ToString());
            }
        }
        // TRAP SPACES
        else { this._anim.SetTrigger("isTrap"); }

        // if (_spaceType.sprite != shopSpace) { _spaceType.color = new Color(1, 1, 1, 0.6f); }
    }

    // SPACES WHERE YOU CAN CAST EFFECTS
    public bool VALID_NODE_TO_CAST_EFFECT()
    {
        return (_spaceType.sprite != emptySpace && _spaceType.sprite != orbSpace && _spaceType.sprite != shopSpace
            && _spaceType.sprite != potionSpace);
    }


    // ------------------------------------------------------------------------------------------ //
    // ------------------------------------ TRAP RELATED ---------------------------------------- //
    // SPACES THAT CAN BE TURNED INTO TRAPS (ie NOT EVENTS, EMPTY, ORB, SHOP)
    public bool VALID_NODE_TO_TRANSFORM()
    {
        return (_spaceType.sprite == blueSpace || _spaceType.sprite == redSpace);
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
        controller.NewTrap(transform.parent.name, this.name, _spaceType.sprite.ToString());
    }

    // PLAYER'S TRAP TO CARRY OVER (FROM GAME_CONTROLLER)
    public void CHANGE_SPACE_TYPE(string spaceType)
    {
        if (_spaceType == null) { _spaceType = this.GetComponent<SpriteRenderer>(); }
        Debug.Log(spaceType);

        switch (spaceType)
        {
            // SPELL NAMES FROM .png FILES (UNDER Player Spaces in INSPECTOR Node prefab)
            case "Felix_10 (UnityEngine.Sprite)" :          _spaceType.sprite = felixCoin10; break;
            case "Felix_20 (UnityEngine.Sprite)" :          _spaceType.sprite = felixCoin20; break;
            case "Felix_Orb (UnityEngine.Sprite)" :         _spaceType.sprite = felixOrb; break;

            case "Jacob_10 (UnityEngine.Sprite)" :          _spaceType.sprite = jacobCoin10; break;
            case "Jacob_20 (UnityEngine.Sprite)" :          _spaceType.sprite = jacobCoin20; break;
            case "Jacob_Orb (UnityEngine.Sprite)" :         _spaceType.sprite = jacobOrb; break;

            case "Laurel_10 (UnityEngine.Sprite)" :         _spaceType.sprite = laurelCoin10; break;
            case "Laurel_20 (UnityEngine.Sprite)" :         _spaceType.sprite = laurelCoin20; break;
            case "Laurel_Orb (UnityEngine.Sprite)" :        _spaceType.sprite = laurelOrb; break;

            case "Maurice_10 (UnityEngine.Sprite)" :        _spaceType.sprite = mauriceCoin10; break;
            case "Maurice_20 (UnityEngine.Sprite)" :        _spaceType.sprite = mauriceCoin20; break;
            case "Maurice_Orb (UnityEngine.Sprite)" :       _spaceType.sprite = mauriceOrb; break;

            case "Mimi_10 (UnityEngine.Sprite)" :           _spaceType.sprite = mimiCoin10; break;
            case "Mimi_20 (UnityEngine.Sprite)" :           _spaceType.sprite = mimiCoin20; break;
            case "Mimi_Orb (UnityEngine.Sprite)" :          _spaceType.sprite = mimiOrb; break;

            case "Pinkins_10 (UnityEngine.Sprite)" :        _spaceType.sprite = pinkinsCoin10; break;
            case "Pinkins_20 (UnityEngine.Sprite)" :        _spaceType.sprite = pinkinsCoin20; break;
            case "Pinkins_Orb (UnityEngine.Sprite)" :       _spaceType.sprite = pinkinsOrb; break;

            case "Sweeterella_10 (UnityEngine.Sprite)" :    _spaceType.sprite = sweeterellaCoin10; break;
            case "Sweeterella_20 (UnityEngine.Sprite)" :    _spaceType.sprite = sweeterellaCoin20; break;
            case "Sweeterella_Orb (UnityEngine.Sprite)" :   _spaceType.sprite = sweeterellaOrb; break;

            case "Thanatos_10 (UnityEngine.Sprite)" :        _spaceType.sprite = thanatosCoin10; break;
            case "Thanatos_20 (UnityEngine.Sprite)" :        _spaceType.sprite = thanatosCoin20; break;
            case "Thanatos_Orb (UnityEngine.Sprite)" :       _spaceType.sprite = thanatosOrb; break;
            
            case "Charlotte_10 (UnityEngine.Sprite)" :       _spaceType.sprite = charlotteCoin10; break;
            case "Charlotte_20 (UnityEngine.Sprite)" :       _spaceType.sprite = charlotteCoin20; break;
            case "Charlotte_Orb (UnityEngine.Sprite)" :      _spaceType.sprite = charlotteOrb; break;

            default : break;
        }

        if (_anim == null) { _anim = this.GetComponentInChildren<Animator>(); }
        _anim.SetTrigger("isTrap");
    }

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
        // Debug.LogError("NOT A VALID ORB TRAP SPACE || CHARACTER NAME NOT ADDED");
        return 0;
    }

    public int TRAP_SPACE_COST(string characterName)
    {
        if      (_spaceType.sprite == felixCoin10) {
            if (characterName == "Felix") { return 5; }
            Instantiate(instanTrap10Effect, transform.position, Quaternion.identity);
            return -10;
        }
        else if (_spaceType.sprite == felixCoin20) {
            if (characterName == "Felix") { return 5; }
            Instantiate(instanTrap10Effect, transform.position, Quaternion.identity);
            Instantiate(instanTrap20Effect, transform.position, Quaternion.identity);
            return -20;
        }

        else if (_spaceType.sprite == jacobCoin10) {
            if (characterName == "Jacob") { return 5; }
            Instantiate(instanTrap10Effect, transform.position, Quaternion.identity);
            return -10;
        }
        else if (_spaceType.sprite == jacobCoin20) {
            if (characterName == "Jacob") { return 5; }
            Instantiate(instanTrap10Effect, transform.position, Quaternion.identity);
            Instantiate(instanTrap20Effect, transform.position, Quaternion.identity);
            return -20;
        }

        else if (_spaceType.sprite == laurelCoin10) {
            if (characterName == "Laurel") { return 5; }
            Instantiate(instanTrap10Effect, transform.position, Quaternion.identity);
            return -10;
        }
        else if (_spaceType.sprite == laurelCoin20) {
            if (characterName == "Laurel") { return 5; }
            Instantiate(instanTrap10Effect, transform.position, Quaternion.identity);
            Instantiate(instanTrap20Effect, transform.position, Quaternion.identity);
            return -20;
        }

        else if (_spaceType.sprite == mauriceCoin10) {
            if (characterName == "Maurice") { return 5; }
            Instantiate(instanTrap10Effect, transform.position, Quaternion.identity);
            return -10;
        }
        else if (_spaceType.sprite == mauriceCoin20) {
            if (characterName == "Maurice") { return 5; }
            Instantiate(instanTrap10Effect, transform.position, Quaternion.identity);
            Instantiate(instanTrap20Effect, transform.position, Quaternion.identity);
            return -20;
        }

        else if (_spaceType.sprite == mimiCoin10) {
            if (characterName == "Mimi") { return 5; }
            Instantiate(instanTrap10Effect, transform.position, Quaternion.identity);
            return -10;
        }
        else if (_spaceType.sprite == mimiCoin20) {
            if (characterName == "Mimi") { return 5; }
            Instantiate(instanTrap10Effect, transform.position, Quaternion.identity);
            Instantiate(instanTrap20Effect, transform.position, Quaternion.identity);
            return -20;
        }

        else if (_spaceType.sprite == pinkinsCoin10) {
            if (characterName == "Pinkins") { return 5; }
            return -10;
        }
        else if (_spaceType.sprite == pinkinsCoin20) {
            if (characterName == "Pinkins") { return 5; }
            return -20;
        }

        else if (_spaceType.sprite == sweeterellaCoin10) {
            if (characterName == "Sweeterella") { return 5; }
            return -10;
        }
        else if (_spaceType.sprite == sweeterellaCoin20) {
            if (characterName == "Sweeterella") { return 5; }
            return -20;
        }

        else if (_spaceType.sprite == thanatosCoin10) {
            if (characterName == "Thanatos") { return 5; }
            return -10;
        }
        else if (_spaceType.sprite == thanatosCoin20) {
            if (characterName == "Thanatos") { return 5; }
            return -20;
        }

        else if (_spaceType.sprite == charlotteCoin10) {
            if (characterName == "Charlotte") { return 5; }
            return -10;
        }
        else if (_spaceType.sprite == charlotteCoin20) {
            if (characterName == "Charlotte") { return 5; }
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
            case "Spell_Effect_10" : Instantiate(effect10Prefab, transform.position, Quaternion.identity); break;
            case "Spell_Effect_Mana_3" : Instantiate(effectMp3Prefab, transform.position, Quaternion.identity); break;
            case "Spell_Effect_Spell_1" : Instantiate(effectSpell1Prefab, transform.position, Quaternion.identity); break;
            case "Spell_Effect_Slow_1" : Instantiate(effectSlow1Prefab, transform.position, Quaternion.identity); break;
            default : Debug.LogError("HAVE NOT ADDED EFFECT SPELL"); break;
        }
    }

    private void STEALING_AURA(PathFollower p)
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
                    Debug.Log(name);    //! DELETE
                    if (next.node != null) next.node.GetComponent<Node>().DISPLAY_MOVEMENT(n + 1, moveLeft);
                }
            }
            // IF NUMBER ALREADY EXISTS, THEN DISPLAY THE SMALLER ONE //? DISPLAY MOVEMENT
            else {
                int number;
                bool success = int.TryParse(nSpace.text, out number);
                if (success && n < number) {
                    nSpace.gameObject.SetActive(true);
                    nSpace.text = n.ToString();
                    if (n == moveLeft) { nSpace.color = new Color(0,1,0); } //* WHERE YOU'LL LAND
                    foreach (Nexts next in nexts) {
                        Debug.Log(name + ", "  + n + ", " + number);    //! DELETE
                        if (next.node != null) next.node.GetComponent<Node>().DISPLAY_MOVEMENT(n + 1, moveLeft);
                    }
                }
            }
        }
        // SPACE DOES NOT DECREMENT MOVEMENT //? DON'T DISPLAY
        else {
            foreach (Nexts next in nexts) {
                next.node.GetComponent<Node>().DISPLAY_MOVEMENT(n, moveLeft);
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
        var eff = Instantiate(teleportEffect, aaron.transform.position, teleportEffect.transform.rotation);
        eff.transform.parent = aaron.transform;
        Destroy(eff, 1);
        aaron.SetActive(true);
        aaronAnim.Play("Aaron_Wave_Anim", -1, 0);
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
