using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyQuest : MonoBehaviour
{
    public LobbyControls playerCalled;
    public bool sideQuestMode;
    [SerializeField] private AudioSource bossMusic;
    [SerializeField] private AudioSource origMusic;

    [SerializeField] private Transform[] telePos;
    [SerializeField] private GameObject teleportCirclePrefab;
    [SerializeField] private GameObject warpedPrefab;
    [SerializeField] private GameObject teleportHitBox;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private Image      healthUI;
    [SerializeField] private Slider hpBar;
    private bool takingDamage;
    private int maxHealth;

    private float attackRate = 2;
    private bool attackBack;
    private bool teleportingAround;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject projectilePrefab2;


    private IEnumerator OnCollisionEnter2D(Collision2D other) {
        if (sideQuestMode)
        {
            if (other.gameObject.tag == "Player" && healthBar.activeSelf && !takingDamage && hpBar.value > 0)
            {
                if (hpBar.value <= 0) yield break;
                // StopCoroutine( ATTACK_BACK() );
                // attackBack = false;
                healthUI.color = new Color(1,1,1);
                yield return new WaitForEndOfFrame();
                StartCoroutine( UpdateHealth() );

                yield return new WaitForSeconds(0.05f);
                healthUI.color = new Color(0.9333334f,0.06666667f,0.1333333f);

            // TELEPORT TO A DIFFERENT SPACE
                int rng = Random.Range(0,telePos.Length);
                while (telePos[rng].position == this.transform.position)
                {
                    rng = Random.Range(0,telePos.Length);
                }
                if (anim != null) anim.Play("Aaron_Cast_Anim", -1, 0);
                var obj = Instantiate(teleportCirclePrefab, transform.position, Quaternion.identity);
                Destroy(obj, 1);
                
                yield return new WaitForSeconds(1);
                var effect2 = Instantiate(warpedPrefab, transform.position, warpedPrefab.transform.rotation);
                Destroy(effect2, 1);
                yield return new WaitForEndOfFrame();
                this.transform.position = telePos[rng].position;
                var effect = Instantiate(warpedPrefab, transform.position, warpedPrefab.transform.rotation);
                Destroy(effect, 1);
                teleportHitBox.SetActive(true);
                
                yield return new WaitForEndOfFrame();
                teleportHitBox.SetActive(false);
            }
        }
    }

    public IEnumerator START_SIDE_QUEST()
    {
        if (sideQuestMode) yield break;

        sideQuestMode = true;
        hpBar.value = hpBar.maxValue;
        takingDamage = true;
        origMusic.Stop();
        bossMusic.Play();

        int rng = Random.Range(0,telePos.Length);
        while (telePos[rng].position == this.transform.position)
        {
            rng = Random.Range(0,telePos.Length);
        }
        if (anim != null) anim.Play("Aaron_Cast_Anim", -1, 0);
        var obj = Instantiate(teleportCirclePrefab, transform.position, Quaternion.identity);
        Destroy(obj, 1);
        
        yield return new WaitForSeconds(1);
        this.transform.position = telePos[rng].position;
        takingDamage = false;

        yield return new WaitForEndOfFrame();
        healthBar.SetActive(true);
    }

    private IEnumerator UpdateHealth()
    {
        takingDamage = true;
        hpBar.value--;

        yield return new WaitForSeconds(1.1f);
        takingDamage = false;
        if (!attackBack) {attackBack = true; StartCoroutine( ATTACK_BACK() ); }
        if (hpBar.value <= 5 && !teleportingAround) { teleportingAround = true; StartCoroutine( TELEPORT_AROUND() ); }
    }

    private IEnumerator ATTACK_BACK()
    {
        yield return new WaitForSeconds(attackRate);
        if (!takingDamage)
        {
            if (hpBar.value >= 9)
            {
                anim.Play("Aaron_Cast_Anim", -1, 0);
                for (int i=0; i<4 ; i++)
                {
                    var atk = Instantiate(projectilePrefab, transform.position, Quaternion.Euler( new Vector3(0,0,45 + (90*i) ) ));
                    Destroy(atk.gameObject, 1.5f);
                }
            }
            else if (hpBar.value >= 7)
            {
                attackRate = 1.5f;
                anim.Play("Aaron_Cast_Anim", -1, 0);
                for (int i=0; i<4 ; i++)
                {
                    var atk = Instantiate(projectilePrefab, transform.position, Quaternion.Euler( new Vector3(0,0,45 + (90*i) ) ));
                    Destroy(atk.gameObject, 1.5f);
                }
            }
            else if (hpBar.value >= 5)
            {
                anim.Play("Aaron_Cast_Anim", -1, 0);
                for (int i=0; i<4 ; i++)
                {
                    var atk = Instantiate(projectilePrefab, transform.position, Quaternion.Euler( new Vector3(0,0,45 + (90*i) ) ));
                    Destroy(atk.gameObject, 1.5f);
                }
                yield return new WaitForSeconds(attackRate/2);
                for (int i=0; i<4 ; i++)
                {
                    var atk = Instantiate(projectilePrefab, transform.position, Quaternion.Euler( new Vector3(0,0,0 + (90*i) ) ));
                    Destroy(atk.gameObject, 1.5f);
                }
            }
            else if (hpBar.value >= 3)
            {
                anim.Play("Aaron_Cast_Anim", -1, 0);
                for (int i=0; i<4 ; i++)
                {
                    var atk = Instantiate(projectilePrefab2, transform.position, Quaternion.Euler( new Vector3(0,0,45 + (90*i) ) ));
                    Destroy(atk.gameObject, 1.5f);
                }
                yield return new WaitForSeconds(0.2f);
                for (int i=0; i<4 ; i++)
                {
                    var atk = Instantiate(projectilePrefab2, transform.position, Quaternion.Euler( new Vector3(0,0,0 + (90*i) ) ));
                    Destroy(atk.gameObject, 1.5f);
                }
                yield return new WaitForSeconds(0.2f);
                for (int i=0; i<4 ; i++)
                {
                    var atk = Instantiate(projectilePrefab2, transform.position, Quaternion.Euler( new Vector3(0,0,45 + (90*i) ) ));
                    Destroy(atk.gameObject, 1.5f);
                }
                yield return new WaitForSeconds(0.2f);
                for (int i=0; i<4 ; i++)
                {
                    var atk = Instantiate(projectilePrefab2, transform.position, Quaternion.Euler( new Vector3(0,0,0 + (90*i) ) ));
                    Destroy(atk.gameObject, 1.5f);
                }
            }
            else if (hpBar.value >= 2)
            {
                anim.Play("Aaron_Cast_Anim", -1, 0);
                for (int i=0; i<4 ; i++)
                {
                    var atk = Instantiate(projectilePrefab2, transform.position, Quaternion.Euler( new Vector3(0,0,45 + (90*i) ) ));
                    Destroy(atk.gameObject, 1.5f);
                }
                for (int i=0; i<4 ; i++)
                {
                    var atk = Instantiate(projectilePrefab2, transform.position, Quaternion.Euler( new Vector3(0,0,0 + (90*i) ) ));
                    Destroy(atk.gameObject, 1.5f);
                }
                yield return new WaitForSeconds(0.2f);
                for (int i=0; i<4 ; i++)
                {
                    var atk = Instantiate(projectilePrefab2, transform.position, Quaternion.Euler( new Vector3(0,0,22.5f + (90*i) ) ));
                    Destroy(atk.gameObject, 1.5f);
                }
                for (int i=0; i<4 ; i++)
                {
                    var atk = Instantiate(projectilePrefab2, transform.position, Quaternion.Euler( new Vector3(0,0,67.5f + (90*i) ) ));
                    Destroy(atk.gameObject, 1.5f);
                }
                yield return new WaitForSeconds(0.2f);
                for (int i=0; i<4 ; i++)
                {
                    var atk = Instantiate(projectilePrefab2, transform.position, Quaternion.Euler( new Vector3(0,0,45 + (90*i) ) ));
                    Destroy(atk.gameObject, 1.5f);
                }
                for (int i=0; i<4 ; i++)
                {
                    var atk = Instantiate(projectilePrefab2, transform.position, Quaternion.Euler( new Vector3(0,0,0 + (90*i) ) ));
                    Destroy(atk.gameObject, 1.5f);
                }
            }
            else if (hpBar.value == 1) {}
            else    { DEFEATED(); yield break; }
        }
        StartCoroutine( ATTACK_BACK() );
    }
    
    private IEnumerator TELEPORT_AROUND()
    {
        if (hpBar.value > 3)        yield return new WaitForSeconds(5);
        else if (hpBar.value == 1)  yield return new WaitForSeconds(0.3f);
        else                        yield return new WaitForSeconds(3);

        int rng = Random.Range(0,telePos.Length);
        while (telePos[rng].position == this.transform.position)
        {
            rng = Random.Range(0,telePos.Length);
        }
        if (anim != null) anim.Play("Aaron_Cast_Anim", -1, 0);
        var obj = Instantiate(teleportCirclePrefab, transform.position, Quaternion.identity);
        Destroy(obj, 1);
        
        yield return new WaitForSeconds(1);
        var effect2 = Instantiate(warpedPrefab, transform.position, warpedPrefab.transform.rotation);
        Destroy(effect2, 1);

        yield return new WaitForEndOfFrame();
        this.transform.position = telePos[rng].position;
        var effect = Instantiate(warpedPrefab, transform.position, warpedPrefab.transform.rotation);
        Destroy(effect, 1);
        teleportHitBox.SetActive(true);

        yield return new WaitForEndOfFrame();
        teleportHitBox.SetActive(false);

        if (hpBar.value == 1)
        {
            anim.Play("Aaron_Cast_Anim", -1, 0);
            for (int i=0; i<4 ; i++)
            {
                var atk = Instantiate(projectilePrefab2, transform.position, Quaternion.Euler( new Vector3(0,0,45 + (90*i) ) ));
                Destroy(atk.gameObject, 1.5f);
            }
            for (int i=0; i<4 ; i++)
            {
                var atk = Instantiate(projectilePrefab2, transform.position, Quaternion.Euler( new Vector3(0,0,0 + (90*i) ) ));
                Destroy(atk.gameObject, 1.5f);
            }
            yield return new WaitForSeconds(0.2f);
            for (int i=0; i<4 ; i++)
            {
                var atk = Instantiate(projectilePrefab2, transform.position, Quaternion.Euler( new Vector3(0,0,22.5f + (90*i) ) ));
                Destroy(atk.gameObject, 1.5f);
            }
            for (int i=0; i<4 ; i++)
            {
                var atk = Instantiate(projectilePrefab2, transform.position, Quaternion.Euler( new Vector3(0,0,67.5f + (90*i) ) ));
                Destroy(atk.gameObject, 1.5f);
            }
            yield return new WaitForSeconds(0.2f);
            for (int i=0; i<4 ; i++)
            {
                var atk = Instantiate(projectilePrefab2, transform.position, Quaternion.Euler( new Vector3(0,0,45 + (90*i) ) ));
                Destroy(atk.gameObject, 1.5f);
            }
            for (int i=0; i<4 ; i++)
            {
                var atk = Instantiate(projectilePrefab2, transform.position, Quaternion.Euler( new Vector3(0,0,0 + (90*i) ) ));
                Destroy(atk.gameObject, 1.5f);
            }
        }
                

        if (hpBar.value <= 0) { DEFEATED(); yield break; }
        else  { StartCoroutine( TELEPORT_AROUND() ); }
    }

    

    void DEFEATED()
    {
        if (!origMusic.isPlaying)
        {
            healthBar.SetActive(false);
            bossMusic.Stop();
            origMusic.Play();
            StartCoroutine( playerCalled.CAM_IN() );
        }
    }

}
