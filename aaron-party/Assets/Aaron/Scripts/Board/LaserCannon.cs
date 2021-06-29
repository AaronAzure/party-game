using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LaserCannon : MonoBehaviour
{
    public TextMeshPro countDown;
    private GameController ctr;
    private GameManager manager;
    [SerializeField] private GameObject turret;
    [SerializeField] private Image startUi;
    [SerializeField] private Animator startUiAnim;
    [SerializeField] private AudioSource beep;
    [SerializeField] private AudioSource laserSound;
    [SerializeField] private GameObject hurtbox;
    [SerializeField] private Transform shotAngle;
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject chargingEffect;
    [SerializeField] private Transform barrelPos;

    private float rotateZ;
    private float rotateSpeed = 5f;
    private bool firing;
    private float timer;


    private void Start() {
        if (GameObject.Find("Game_Manager") != null) manager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (GameObject.Find("Game_Controller") != null) ctr = GameObject.Find("Game_Controller").GetComponent<GameController>();
        if (ctr.turnNumber == 1) {
            PlayerPrefs.SetFloat("TurretRotation", 0);
        }
        else {
            rotateZ = PlayerPrefs.GetFloat("TurretRotation");
            turret.transform.Rotate(0, 0, rotateZ);
        }
    }

    private void Update() {
        if (firing) {
            timer += Time.deltaTime / 3f;

            manager.mainCam.transform.position = 
                Vector3.Lerp(manager.mainCam.transform.position, shotAngle.position, timer);
        }
    }

    public IEnumerator CountDown(int x, bool endOfTurn) {
        yield return new WaitForSeconds(0.5f);
        startUi.gameObject.SetActive(true);
        startUiAnim.Play("Character_Circle_Anim", -1, 0);

        yield return new WaitForSeconds(1f);
        startUiAnim.Play("Character_Circle_End_Anim", -1, 0);

        yield return new WaitForSeconds(0.75f);
        startUiAnim.Rebind();
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
                var atk = Instantiate(projectile, barrelPos.position, Quaternion.Euler( new Vector3(0,0,270 - i + rotateZ) ));
                Destroy(atk, 5);
            }

            yield return new WaitForSeconds(3f);
            firing = false;
            AudioSource bgMusic = GameObject.Find("BACKGROUND_MUSIC").GetComponent<AudioSource>();
            bgMusic.volume *= 6;

            if (endOfTurn)  StartCoroutine( manager.INCREMENT_TURN() );
            else {
                StartCoroutine( manager.EVENT_OVER_RETURN_TO_PLAYER("Laser Countdown") ); 
                countDown.text = manager.startingCharge.ToString();
            }
        }
        // CHARGING UP
        else {
            yield return new WaitForSeconds(0.75f);
            if (endOfTurn)  StartCoroutine( manager.INCREMENT_TURN() );
            else            StartCoroutine( manager.EVENT_OVER_RETURN_TO_PLAYER("Laser Countdown") );
        }
        //// manager.NEXT_PLAYER_TURN();        
    }

    public IEnumerator ROTATE_TURRET() {
        float totalRotation = 0; 
        float rotationAmt = rotateSpeed * Time.deltaTime;
        while(Mathf.Abs(totalRotation) < 90) 
        {
            yield return new WaitForSeconds(0.01f);
            turret.transform.Rotate(0, 0, -rotationAmt * 7.5f);
            totalRotation += (-rotationAmt * 7.5f);
        }
        rotateZ = PlayerPrefs.GetFloat("TurretRotation");
        Debug.Log("  " + rotateZ + "  ,  " + totalRotation);
        PlayerPrefs.SetFloat("TurretRotation", rotateZ - 90);
        rotateZ = PlayerPrefs.GetFloat("TurretRotation");

        yield return new WaitForSeconds(1.5f);
        StartCoroutine( manager.EVENT_OVER_RETURN_TO_PLAYER("Laser Countdown") );
    }

}
