using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LaserCannon : MonoBehaviour
{
    public TextMeshPro countDown;
    private GameManager manager;
    private GameObject turret;
    [SerializeField] private Image startUi;
    [SerializeField] private Animator startUiAnim;
    [SerializeField] private AudioSource beep;
    [SerializeField] private AudioSource laserSound;
    [SerializeField] private GameObject hurtbox;
    [SerializeField] private Transform shotAngle;
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject chargingEffect;

    private bool firing;
    private float timer;

    private void Start() {
        if (GameObject.Find("Game_Manager") != null) manager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
    }

    private void Update() {
        if (firing) {
            timer += Time.deltaTime / 3f;

            manager.mainCam.transform.position = 
                Vector3.Lerp(manager.mainCam.transform.position, shotAngle.position, timer);
        }
    }

    public IEnumerator CountDown(int x) {
        yield return new WaitForSeconds(0.5f);
        startUi.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);
        startUiAnim.Play("Character_Circle_End_Anim", -1, 0);

        yield return new WaitForSeconds(0.75f);
        startUi.gameObject.SetActive(false);
        beep.Play();
        countDown.text = x.ToString();

        // LAUNCH CANNON
        if (x == 0)
        {
            yield return new WaitForSeconds(0.25f);
            firing = true;
            laserSound.Play();
            chargingEffect.SetActive(true);

            yield return new WaitForSeconds(4.5f);
            // if (hurtbox != null) hurtbox.SetActive(true);
            chargingEffect.SetActive(false);
            for (int i=0 ; i<90 ; i++) {
                var atk = Instantiate(projectile, transform.position, Quaternion.Euler( new Vector3(0,0,180 + i) ));
                Destroy(atk, 5);
            }

            yield return new WaitForSeconds(3f);
            firing = false;
            manager.StartCoroutine(manager.INCREMENT_TURN());
        }
        // CHARGING UP
        else {
            yield return new WaitForSeconds(0.75f);
            manager.StartCoroutine(manager.INCREMENT_TURN());
        }
        //// manager.NEXT_PLAYER_TURN();        
    }


}
