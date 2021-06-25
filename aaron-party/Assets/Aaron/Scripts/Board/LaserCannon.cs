using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LaserCannon : MonoBehaviour
{
    public TextMeshPro countDown;
    private GameManager manager;
    [SerializeField] private Image startUi;
    [SerializeField] private Animator startUiAnim;
    [SerializeField] private AudioSource beep;

    private void Start() {
        if (manager == null) manager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
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

        yield return new WaitForSeconds(0.75f);
        manager.StartCoroutine(manager.INCREMENT_TURN());
        //// manager.NEXT_PLAYER_TURN();        
    }


}
