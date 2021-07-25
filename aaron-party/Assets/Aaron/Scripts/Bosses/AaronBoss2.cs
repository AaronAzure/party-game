using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AaronBoss2 : MonoBehaviour
{
   [SerializeField] private Transform teleParent;
    int prevRng;
    [SerializeField] private List<Transform> telePos;
    [SerializeField] private GameObject teleportCirclePrefab;
    [SerializeField] private GameObject warpedPrefab;
    [SerializeField] private GameObject teleportHitBox;
    [SerializeField] private Animator anim;

    GameController ctr;
    MinigameManager manager;
    PreviewManager pw;
    GameObject instances;
    private GameObject character;

    private float attackRate = 1.5f;
    private float[] angles;
    private float[] smlAngles;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject projectileFastPrefab;
    [SerializeField] private GameObject curvePrefab;
    [SerializeField] private GameObject straightPrefab;
    [SerializeField] private GameObject straightFastPrefab;
    [SerializeField] private GameObject holyNovaPrefab;

    [Header("UI")]
    [SerializeField] private GameObject defeated;
    [SerializeField] private TextMeshProUGUI perent;
    [SerializeField] private Slider slider;
    IEnumerator co;

    private int easyHp = 100;
    private int normHp = 150;
    private int hardHp = 200;

    



    // Start is called before the first frame update
    void Start()
    {
        character = this.gameObject;
        telePos = new List<Transform>();
        foreach (Transform child in teleParent) { telePos.Add(child); }
        
        angles = new float[32];
        for (int i=0 ; i<angles.Length ; i++) { angles[i] = (i*11.25f); }
        smlAngles = new float[4];
        for (int i=0 ; i<smlAngles.Length ; i++) { smlAngles[i] = (i*11.25f); }

        if (GameObject.Find("Level_Manager") != null) {
            manager = GameObject.Find("Level_Manager").GetComponent<MinigameManager>();
            instances = manager.gameObject;
        }
        if (GameObject.Find("Preview_Manager") != null)
        {
            pw      = GameObject.Find("Preview_Manager").GetComponent<PreviewManager>();
            instances = pw.gameObject;
        }
        ctr = GameObject.Find("Game_Controller").GetComponent<GameController>();
        
        if      (ctr.hard) slider.maxValue = hardHp * Mathf.Pow(1.5f, (ctr.nPlayers-1) );
        else if (ctr.norm) slider.maxValue = normHp * Mathf.Pow(1.5f, (ctr.nPlayers-1) );
        else               slider.maxValue = easyHp * Mathf.Pow(1.5f, (ctr.nPlayers-1) );
        slider.value = slider.maxValue;
        Debug.Log("multiplier = " + Mathf.Pow(1.5f, (ctr.nPlayers-1) ));

        if (GameObject.Find("Level_Manager") != null) StartCoroutine( TELEPORT_AROUND(5, false) );
        else StartCoroutine( TELEPORT_AROUND(1.5f, false) );
    }

    IEnumerator TELEPORT_AROUND(float delay, bool newPos)
    {
        yield return new WaitForSeconds(delay);
        int rng = Random.Range(0,telePos.Count);
        if (newPos)
        {
            while (prevRng == rng) rng = Random.Range(0,telePos.Count);
        }

        if (anim != null) anim.Play("Aaron_Cast_Anim", -1, 0);
        var obj = Instantiate(teleportCirclePrefab, transform.position, Quaternion.identity);
        obj.transform.localScale *= 0.3f;
        Destroy(obj, 1);
        
        yield return new WaitForSeconds(1);
        var effect2 = Instantiate(warpedPrefab, transform.position, warpedPrefab.transform.rotation);
        effect2.transform.localScale *= 0.6f;
        Destroy(effect2, 1);

        yield return new WaitForEndOfFrame();
        this.transform.position = telePos[rng].position;
        var effect = Instantiate(warpedPrefab, transform.position, warpedPrefab.transform.rotation);
        effect.transform.localScale *= 0.6f;
        Destroy(effect, 1);

        prevRng = rng;
        if (!newPos)    {co = Attacks( Random.Range(0,7) ); StartCoroutine( co );}
        else            {co = Attacks( Random.Range(1,7) ); StartCoroutine( co );}
    }


    IEnumerator Attacks(int atkPattern)
    {
        if (manager != null) {if (manager.timeUp) yield break; }
        // DON'T ATTACK, TELEPORT
        if ( atkPattern == 0 ) { StartCoroutine( TELEPORT_AROUND(0.25f, true) ); yield break; }
        if (ctr.easy) yield return new WaitForSeconds(1);
        if (ctr.norm) yield return new WaitForSeconds(0.75f);
        if (ctr.hard) yield return new WaitForSeconds(0.6f);

        switch (atkPattern)
        {
            // SPLIT ATTACK
            case 1 : 
                if (ctr.easy)
                {
                    anim.Play("Aaron_Cast_Anim", -1, 0);
                    for (int i=0; i<4 ; i++)
                    {
                        var atk = Instantiate(straightFastPrefab, transform.position, Quaternion.Euler( new Vector3(0,0,0 + (90*i) ) ));
                        atk.transform.parent = instances.transform;
                        atk.transform.localScale *= 0.5f;
                        Destroy(atk.gameObject, 3);
                    } 
                    for (int i=0; i<4 ; i++)
                    {
                        var atk = Instantiate(straightFastPrefab, transform.position, Quaternion.Euler( new Vector3(0,0,22.5f + (90*i) ) ));
                        atk.transform.parent = instances.transform;
                        atk.transform.localScale *= 0.5f;
                        Destroy(atk.gameObject, 3);
                    }
                    for (int i=0; i<4 ; i++)
                    {
                        var atk = Instantiate(straightFastPrefab, transform.position, Quaternion.Euler( new Vector3(0,0,67.5f + (90*i) ) ));
                        atk.transform.parent = instances.transform;
                        atk.transform.localScale *= 0.5f;
                        Destroy(atk.gameObject, 3);
                    }
                    yield return new WaitForSeconds(attackRate/2);
                    anim.Play("Aaron_Cast_Anim", -1, 0);
                    for (int i=0; i<4 ; i++)
                    {
                        var atk = Instantiate(straightFastPrefab, transform.position, Quaternion.Euler( new Vector3(0,0,22.5f + (90*i) ) ));
                        atk.transform.parent = instances.transform;
                        atk.transform.localScale *= 0.5f;
                        Destroy(atk.gameObject, 3);
                    }
                    for (int i=0; i<4 ; i++)
                    {
                        var atk = Instantiate(straightFastPrefab, transform.position, Quaternion.Euler( new Vector3(0,0,45 + (90*i) ) ));
                        atk.transform.parent = instances.transform;
                        atk.transform.localScale *= 0.5f;
                        Destroy(atk.gameObject, 3);
                    }
                    for (int i=0; i<4 ; i++)
                    {
                        var atk = Instantiate(straightFastPrefab, transform.position, Quaternion.Euler( new Vector3(0,0,67.5f + (90*i) ) ));
                        atk.transform.parent = instances.transform;
                        atk.transform.localScale *= 0.5f;
                        Destroy(atk.gameObject, 3);
                    }

                }
                if (ctr.norm)
                {
                    anim.Play("Aaron_Cast_Anim", -1, 0);
                    for (int i=0; i<4 ; i++)
                    {
                        var atk = Instantiate(straightFastPrefab, transform.position, Quaternion.Euler( new Vector3(0,0,0 + (90*i) ) ));
                        atk.transform.parent = instances.transform;
                        atk.transform.localScale *= 0.5f;
                        Destroy(atk.gameObject, 3);
                    } 
                    for (int i=0; i<4 ; i++)
                    {
                        var atk = Instantiate(straightFastPrefab, transform.position, Quaternion.Euler( new Vector3(0,0,11.25f + (90*i) ) ));
                        atk.transform.parent = instances.transform;
                        atk.transform.localScale *= 0.5f;
                        Destroy(atk.gameObject, 3);
                    }
                    for (int i=0; i<4 ; i++)
                    {
                        var atk = Instantiate(straightFastPrefab, transform.position, Quaternion.Euler( new Vector3(0,0,22.5f + (90*i) ) ));
                        atk.transform.parent = instances.transform;
                        atk.transform.localScale *= 0.5f;
                        Destroy(atk.gameObject, 3);
                    }
                    for (int i=0; i<4 ; i++)
                    {
                        var atk = Instantiate(straightFastPrefab, transform.position, Quaternion.Euler( new Vector3(0,0,67.5f + (90*i) ) ));
                        atk.transform.parent = instances.transform;
                        atk.transform.localScale *= 0.5f;
                        Destroy(atk.gameObject, 3);
                    }
                    for (int i=0; i<4 ; i++)
                    {
                        var atk = Instantiate(straightFastPrefab, transform.position, Quaternion.Euler( new Vector3(0,0,78.75f + (90*i) ) ));
                        atk.transform.parent = instances.transform;
                        atk.transform.localScale *= 0.5f;
                        Destroy(atk.gameObject, 3);
                    } 
                    yield return new WaitForSeconds(attackRate/2);
                    anim.Play("Aaron_Cast_Anim", -1, 0);
                    for (int i=0; i<4 ; i++)
                    {
                        var atk = Instantiate(straightFastPrefab, transform.position, Quaternion.Euler( new Vector3(0,0,22.5f + (90*i) ) ));
                        atk.transform.parent = instances.transform;
                        atk.transform.localScale *= 0.5f;
                        Destroy(atk.gameObject, 3);
                    }
                    for (int i=0; i<4 ; i++)
                    {
                        var atk = Instantiate(straightFastPrefab, transform.position, Quaternion.Euler( new Vector3(0,0,33.75f + (90*i) ) ));
                        atk.transform.parent = instances.transform;
                        atk.transform.localScale *= 0.5f;
                        Destroy(atk.gameObject, 3);
                    }
                    for (int i=0; i<4 ; i++)
                    {
                        var atk = Instantiate(straightFastPrefab, transform.position, Quaternion.Euler( new Vector3(0,0,45 + (90*i) ) ));
                        atk.transform.parent = instances.transform;
                        atk.transform.localScale *= 0.5f;
                        Destroy(atk.gameObject, 3);
                    }
                    for (int i=0; i<4 ; i++)
                    {
                        var atk = Instantiate(straightFastPrefab, transform.position, Quaternion.Euler( new Vector3(0,0,56.25f + (90*i) ) ));
                        atk.transform.parent = instances.transform;
                        atk.transform.localScale *= 0.5f;
                        Destroy(atk.gameObject, 3);
                    } 
                    for (int i=0; i<4 ; i++)
                    {
                        var atk = Instantiate(straightFastPrefab, transform.position, Quaternion.Euler( new Vector3(0,0,67.5f + (90*i) ) ));
                        atk.transform.parent = instances.transform;
                        atk.transform.localScale *= 0.5f;
                        Destroy(atk.gameObject, 3);
                    }

                }
                if (ctr.hard)
                {
                    anim.Play("Aaron_Cast_Anim", -1, 0);
                    for (int i=0; i<4 ; i++)
                    {
                        var atk = Instantiate(straightFastPrefab, transform.position, Quaternion.Euler( new Vector3(0,0,11.25f + (90*i) ) ));
                        atk.transform.parent = instances.transform;
                        atk.transform.localScale *= 0.5f;
                        Destroy(atk.gameObject, 3);
                    }
                    for (int i=0; i<4 ; i++)
                    {
                        var atk = Instantiate(straightFastPrefab, transform.position, Quaternion.Euler( new Vector3(0,0,22.5f + (90*i) ) ));
                        atk.transform.parent = instances.transform;
                        atk.transform.localScale *= 0.5f;
                        Destroy(atk.gameObject, 3);
                    }
                    for (int i=0; i<4 ; i++)
                    {
                        var atk = Instantiate(straightFastPrefab, transform.position, Quaternion.Euler( new Vector3(0,0,67.5f + (90*i) ) ));
                        atk.transform.parent = instances.transform;
                        atk.transform.localScale *= 0.5f;
                        Destroy(atk.gameObject, 3);
                    }
                    for (int i=0; i<4 ; i++)
                    {
                        var atk = Instantiate(straightFastPrefab, transform.position, Quaternion.Euler( new Vector3(0,0,78.75f + (90*i) ) ));
                        atk.transform.parent = instances.transform;
                        atk.transform.localScale *= 0.5f;
                        Destroy(atk.gameObject, 3);
                    } 
                    for (int i=0; i<4 ; i++)
                    {
                        var atk = Instantiate(straightFastPrefab, transform.position, Quaternion.Euler( new Vector3(0,0,0 + (90*i) ) ));
                        atk.transform.parent = instances.transform;
                        atk.transform.localScale *= 0.5f;
                        Destroy(atk.gameObject, 3);
                    } 
                    yield return new WaitForSeconds(attackRate/2);
                    anim.Play("Aaron_Cast_Anim", -1, 0);
                    for (int i=0; i<4 ; i++)
                    {
                        var atk = Instantiate(straightFastPrefab, transform.position, Quaternion.Euler( new Vector3(0,0,33.75f + (90*i) ) ));
                        atk.transform.parent = instances.transform;
                        atk.transform.localScale *= 0.5f;
                        Destroy(atk.gameObject, 3);
                    }
                    for (int i=0; i<4 ; i++)
                    {
                        var atk = Instantiate(straightFastPrefab, transform.position, Quaternion.Euler( new Vector3(0,0,56.25f + (90*i) ) ));
                        atk.transform.parent = instances.transform;
                        atk.transform.localScale *= 0.5f;
                        Destroy(atk.gameObject, 3);
                    } 
                    for (int i=0; i<4 ; i++)
                    {
                        var atk = Instantiate(straightFastPrefab, transform.position, Quaternion.Euler( new Vector3(0,0,22.5f + (90*i) ) ));
                        atk.transform.parent = instances.transform;
                        atk.transform.localScale *= 0.5f;
                        Destroy(atk.gameObject, 3);
                    }
                    for (int i=0; i<4 ; i++)
                    {
                        var atk = Instantiate(straightFastPrefab, transform.position, Quaternion.Euler( new Vector3(0,0,67.5f + (90*i) ) ));
                        atk.transform.parent = instances.transform;
                        atk.transform.localScale *= 0.5f;
                        Destroy(atk.gameObject, 3);
                    }
                    for (int i=0; i<4 ; i++)
                    {
                        var atk = Instantiate(straightFastPrefab, transform.position, Quaternion.Euler( new Vector3(0,0,45 + (90*i) ) ));
                        atk.transform.parent = instances.transform;
                        atk.transform.localScale *= 0.5f;
                        Destroy(atk.gameObject, 3);
                    }
                    yield return new WaitForSeconds(attackRate/2);
                    anim.Play("Aaron_Cast_Anim", -1, 0);
                    for (int i=0; i<4 ; i++)
                    {
                        var atk = Instantiate(straightFastPrefab, transform.position, Quaternion.Euler( new Vector3(0,0,11.25f + (90*i) ) ));
                        atk.transform.parent = instances.transform;
                        atk.transform.localScale *= 0.5f;
                        Destroy(atk.gameObject, 3);
                    }
                    for (int i=0; i<4 ; i++)
                    {
                        var atk = Instantiate(straightFastPrefab, transform.position, Quaternion.Euler( new Vector3(0,0,22.5f + (90*i) ) ));
                        atk.transform.parent = instances.transform;
                        atk.transform.localScale *= 0.5f;
                        Destroy(atk.gameObject, 3);
                    }
                    for (int i=0; i<4 ; i++)
                    {
                        var atk = Instantiate(straightFastPrefab, transform.position, Quaternion.Euler( new Vector3(0,0,67.5f + (90*i) ) ));
                        atk.transform.parent = instances.transform;
                        atk.transform.localScale *= 0.5f;
                        Destroy(atk.gameObject, 3);
                    }
                    for (int i=0; i<4 ; i++)
                    {
                        var atk = Instantiate(straightFastPrefab, transform.position, Quaternion.Euler( new Vector3(0,0,78.75f + (90*i) ) ));
                        atk.transform.parent = instances.transform;
                        atk.transform.localScale *= 0.5f;
                        Destroy(atk.gameObject, 3);
                    } 
                    for (int i=0; i<4 ; i++)
                    {
                        var atk = Instantiate(straightFastPrefab, transform.position, Quaternion.Euler( new Vector3(0,0,0 + (90*i) ) ));
                        atk.transform.parent = instances.transform;
                        atk.transform.localScale *= 0.5f;
                        Destroy(atk.gameObject, 3);
                    } 
                    yield return new WaitForSeconds(attackRate/2);
                    anim.Play("Aaron_Cast_Anim", -1, 0);
                    for (int i=0; i<4 ; i++)
                    {
                        var atk = Instantiate(straightFastPrefab, transform.position, Quaternion.Euler( new Vector3(0,0,33.75f + (90*i) ) ));
                        atk.transform.parent = instances.transform;
                        atk.transform.localScale *= 0.5f;
                        Destroy(atk.gameObject, 3);
                    }
                    for (int i=0; i<4 ; i++)
                    {
                        var atk = Instantiate(straightFastPrefab, transform.position, Quaternion.Euler( new Vector3(0,0,56.25f + (90*i) ) ));
                        atk.transform.parent = instances.transform;
                        atk.transform.localScale *= 0.5f;
                        Destroy(atk.gameObject, 3);
                    } 
                    for (int i=0; i<4 ; i++)
                    {
                        var atk = Instantiate(straightFastPrefab, transform.position, Quaternion.Euler( new Vector3(0,0,22.5f + (90*i) ) ));
                        atk.transform.parent = instances.transform;
                        atk.transform.localScale *= 0.5f;
                        Destroy(atk.gameObject, 3);
                    }
                    for (int i=0; i<4 ; i++)
                    {
                        var atk = Instantiate(straightFastPrefab, transform.position, Quaternion.Euler( new Vector3(0,0,67.5f + (90*i) ) ));
                        atk.transform.parent = instances.transform;
                        atk.transform.localScale *= 0.5f;
                        Destroy(atk.gameObject, 3);
                    }
                    for (int i=0; i<4 ; i++)
                    {
                        var atk = Instantiate(straightFastPrefab, transform.position, Quaternion.Euler( new Vector3(0,0,45 + (90*i) ) ));
                        atk.transform.parent = instances.transform;
                        atk.transform.localScale *= 0.5f;
                        Destroy(atk.gameObject, 3);
                    }
                }
                break;
            // STRAIGHT
            case 2 : 
                float atkSpeed = 0.4f;
                if (!ctr.hard) atkSpeed = 0.6f; // EASY & NORM ATK SPEED
                if (ctr.easy)
                {
                    // 0
                    anim.Play("Aaron_Cast_Anim", -1, 0);
                    for (int i=0; i<6 ; i++) { ATTACK_60(0,i,0); }
                    yield return new WaitForSeconds(atkSpeed);
                    // 11.25
                    anim.Play("Aaron_Cast_Anim", -1, 0);
                    for (int i=0; i<6 ; i++) { ATTACK_60(11.25f,i,0); }
                    yield return new WaitForSeconds(atkSpeed);
                    // 22.5
                    anim.Play("Aaron_Cast_Anim", -1, 0);
                    for (int i=0; i<6 ; i++) { ATTACK_60(22.5f,i,0); }
                    yield return new WaitForSeconds(atkSpeed);
                    // 37.75
                    anim.Play("Aaron_Cast_Anim", -1, 0);
                    for (int i=0; i<6 ; i++) { ATTACK_60(37.75f,i,0); }
                    yield return new WaitForSeconds(atkSpeed);
                    // 0
                    anim.Play("Aaron_Cast_Anim", -1, 0);
                    for (int i=0; i<6 ; i++) { ATTACK_60(0,i,0); }
                    yield return new WaitForSeconds(atkSpeed);
                    // 11.25
                    anim.Play("Aaron_Cast_Anim", -1, 0);
                    for (int i=0; i<6 ; i++) { ATTACK_60(11.25f,i,0); }
                    yield return new WaitForSeconds(atkSpeed);
                    // 22.5
                    anim.Play("Aaron_Cast_Anim", -1, 0);
                    for (int i=0; i<6 ; i++) { ATTACK_60(22.25f,i,0); }
                    yield return new WaitForSeconds(atkSpeed);
                    // 37.75
                    anim.Play("Aaron_Cast_Anim", -1, 0);
                    for (int i=0; i<6 ; i++) { ATTACK_60(37.75f,i,0); }
                }
                else
                {
                    // 0
                    anim.Play("Aaron_Cast_Anim", -1, 0);
                    for (int i=0; i<8 ; i++) { ATTACK_45(0,i,0); }
                    yield return new WaitForSeconds(atkSpeed);
                    // 11.25
                    anim.Play("Aaron_Cast_Anim", -1, 0);
                    for (int i=0; i<8 ; i++) { ATTACK_45(11.25f,i,0); }
                    yield return new WaitForSeconds(atkSpeed);
                    // 22.5
                    anim.Play("Aaron_Cast_Anim", -1, 0);
                    for (int i=0; i<8 ; i++) { ATTACK_45(22.5f,i,0); }
                    yield return new WaitForSeconds(atkSpeed);
                    // 37.75
                    anim.Play("Aaron_Cast_Anim", -1, 0);
                    for (int i=0; i<8 ; i++) { ATTACK_45(37.75f,i,0); }
                    yield return new WaitForSeconds(atkSpeed);
                    // 0
                    anim.Play("Aaron_Cast_Anim", -1, 0);
                    for (int i=0; i<8 ; i++) { ATTACK_45(0,i,0); }
                    yield return new WaitForSeconds(atkSpeed);
                    // 11.25
                    anim.Play("Aaron_Cast_Anim", -1, 0);
                    for (int i=0; i<8 ; i++) { ATTACK_45(11.25f,i,0); }
                    yield return new WaitForSeconds(atkSpeed);
                    // 22.5
                    anim.Play("Aaron_Cast_Anim", -1, 0);
                    for (int i=0; i<8 ; i++) { ATTACK_45(22.25f,i,0); }
                    yield return new WaitForSeconds(atkSpeed);
                    // 37.75
                    anim.Play("Aaron_Cast_Anim", -1, 0);
                    for (int i=0; i<8 ; i++) { ATTACK_45(37.75f,i,0); }
                }
                break;
            // STRAIGHT (RANDOM ANGLES)
            case 3 : 
                anim.Play("Aaron_Cast_Anim", -1, 0);
                int nAtk = 9;
                if (ctr.easy) nAtk = 4;
                if (ctr.norm) nAtk = 6;
                for (int x=0; x<nAtk ; x++)
                {
                    anim.Play("Aaron_Cast_Anim", -1, 0);
                    int r = Random.Range(0, smlAngles.Length);
                    if (ctr.easy) {
                        for (int i=0; i<8 ; i++) { ATTACK_60(0,i,0); }
                    }
                    else {
                        for (int i=0; i<8 ; i++)
                        {
                            var atk = Instantiate(straightFastPrefab, transform.position, 
                                Quaternion.Euler( new Vector3(0,0,smlAngles[r] + (45*i) ) ));
                            atk.transform.localScale *= 0.5f;
                            Destroy(atk.gameObject, 3);
                        }
                    }
                    yield return new WaitForSeconds(attackRate/3);
                }                
                
                break;
            // CIRCULAR
            case 5 : 
                anim.Play("Aaron_Cast_Anim", -1, 0);
                yield return new WaitForSeconds(0.5f);
                int r1 = Random.Range(0,angles.Length);
                int nTime = 32;
                float delay = 0.15f;
                if (ctr.easy) { delay = 0.5f; }
                if (ctr.norm) delay = 0.25f;
                for (int i=0 ; i<nTime ; i++)
                {
                    anim.Play("Aaron_Cast_Anim", -1, 0.5f);
                    ATTACK_11_25(r1, i, 0);
                    ATTACK_11_25(r1, i, 90);
                    ATTACK_11_25(r1, i, 180);
                    ATTACK_11_25(r1, i, 270);
                    if (ctr.easy) i++;
                    yield return new WaitForSeconds(delay);
                }
                break;
            // CURVE SHOT (ORBITAL)
            case 4 : 
                anim.Play("Aaron_Cast_Anim", -1, 0);
                if (ctr.hard)
                {
                    for (int i=0; i<4 ; i++)
                    {
                        var atk = Instantiate(curvePrefab, transform.position, Quaternion.Euler( new Vector3(0,0,45 + (90*i) ) ));
                        atk.transform.parent = instances.transform;
                        atk.transform.localScale *= 0.5f;
                        Destroy(atk.gameObject, 3);
                    }
                    for (int i=0; i<4 ; i++)
                    {
                        var atk = Instantiate(curvePrefab, transform.position, Quaternion.Euler( new Vector3(0,0,0 + (90*i) ) ));
                        atk.transform.parent = instances.transform;
                        atk.transform.localScale *= 0.5f;
                        Destroy(atk.gameObject, 3);
                    } 
                    yield return new WaitForSeconds(0.7f);
                }
                anim.Play("Aaron_Cast_Anim", -1, 0);
                if (ctr.easy)
                {
                    for (int i=0; i<4 ; i++)
                    {
                        ATTACK_45_CURVE(0, (2*i), 0);
                    } 
                }
                else 
                {
                    for (int i=0; i<8 ; i++)
                    {
                        ATTACK_45_CURVE(0, i, 0);
                    } 
                }
                yield return new WaitForSeconds(1f);
                break;
            // HOLY NOVA
            case 6 :
                if (ctr.easy)
                {
                    HOLY_NOVA();
                }
                else if (ctr.norm)
                {
                    HOLY_NOVA();
                    yield return new WaitForSeconds(1);
                    HOLY_NOVA();
                }
                else 
                {
                    HOLY_NOVA();
                    yield return new WaitForSeconds(1);
                    HOLY_NOVA();
                    yield return new WaitForSeconds(1);
                    HOLY_NOVA();
                }
                break;
        }

        yield return new WaitForEndOfFrame();
        if (manager != null) {if (manager.timeUp) yield break; }
        if (ctr.easy) StartCoroutine( TELEPORT_AROUND(2, false) );
        if (ctr.norm) StartCoroutine( TELEPORT_AROUND(1.5f, false) );
        if (ctr.hard) StartCoroutine( TELEPORT_AROUND(1, false) );
    }

    void ATTACK_11_25(int rng, int n, float extraDeg)
    {
        var atk = Instantiate(straightFastPrefab, transform.position, 
            Quaternion.Euler( new Vector3(0,0,angles[rng] + (11.25f*n) + extraDeg ) ));
        atk.transform.parent = instances.transform;
        atk.transform.localScale *= 0.5f;
        Destroy(atk.gameObject, 3);
    }

    void ATTACK_45(float startDeg, int n, float extraDeg)
    {
        var atk = Instantiate(straightFastPrefab, transform.position, 
            Quaternion.Euler( new Vector3(0,0,startDeg + (45*n) ) ));
        atk.transform.parent = instances.transform;
        atk.transform.localScale *= 0.5f;
        Destroy(atk.gameObject, 3);
    }
    
    void ATTACK_60(float startDeg, int n, float extraDeg)
    {
        var atk = Instantiate(straightFastPrefab, transform.position, 
            Quaternion.Euler( new Vector3(0,0,startDeg + (60*n) ) ));
        atk.transform.parent = instances.transform;
        atk.transform.localScale *= 0.5f;
        Destroy(atk.gameObject, 3);
    }

    void ATTACK_45_CURVE(float startDeg, int n, float extraDeg)
    {
        var atk = Instantiate(projectileFastPrefab, transform.position, 
            Quaternion.Euler( new Vector3(0,0,startDeg + (45*n) ) ));
        atk.transform.parent = instances.transform;
        atk.transform.localScale *= 0.5f;
        Destroy(atk.gameObject, 3);
    }

    void HOLY_NOVA()
    {
        anim.Play("Aaron_Cast_Anim", -1, 0);
            var nova = Instantiate(holyNovaPrefab, transform.position, holyNovaPrefab.transform.rotation);
        nova.transform.parent = instances.transform;
        Destroy(nova, 3);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Safe")
        {
            StartCoroutine(Damaged());
        }
    }

    private IEnumerator Damaged()
    {
        foreach (Transform child in character.transform)  
        {  
            if (child.name != "Shadow") {
                if (child.TryGetComponent(out SpriteRenderer cs)) { cs.color = new Color(1,0,0); }
            }
            foreach (Transform grandChild in child)
            {
                if (grandChild.name != "Shadow") {
                    if (grandChild.TryGetComponent(out SpriteRenderer gs)) { gs.color = new Color(1,0,0); }
                }
            }
        }
        if (slider.value > 0) {
            slider.value--;
            perent.text = ((slider.value/slider.maxValue)*100).ToString("F1") + "%";
            if (slider.value == 0) perent.text = "Mercy!";
        }
        // DEFEATED
        if (!manager.timeUp && slider.value == 0) {
            defeated.SetActive(true);
            StopCoroutine(co);
            StartCoroutine( manager.GameOver() );
            yield break;
        }

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        foreach (Transform child in character.transform)  
        {  
            if (child.name != "Shadow") {
                if (child.TryGetComponent(out SpriteRenderer cs)) { cs.color = new Color(1,1,1,1); }
            }
            foreach (Transform grandChild in child)
            {
                if (grandChild.name != "Shadow") {
                    if (grandChild.TryGetComponent(out SpriteRenderer gs)) { gs.color = new Color(1,1,1,1); }
                }
            }
        }
    }

}
