using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrystalBeast : MonoBehaviour
{
    private List<SleepDuration> patterns;
    [SerializeField] private Animator   _anim;
    [SerializeField] private GameObject _hurtBox;
    [SerializeField] private GameObject _zzz;

    private MinigameManager manager;   // GET COMPONENT
    private PreviewManager pw;   // GET COMPONENT
    private AudioSource bgMusic;    // get
    private bool started;


    // Start is called before the first frame update
    void Start()
    {
        _hurtBox.SetActive(false);
        pw = GameObject.Find("Level_Manager").GetComponent<PreviewManager>();

        if (SceneManager.GetActiveScene().name == "Sneak_And_Snore") 
        {
            manager = GameObject.Find("Level_Manager").GetComponent<MinigameManager>();
            bgMusic = GameObject.Find("BG_MUSIC").GetComponent<AudioSource>();
        }

        patterns = new List<SleepDuration>();
        patterns.Add( new SleepDuration(2,  4,      0) );
        patterns.Add( new SleepDuration(2,  3,      1) );
        patterns.Add( new SleepDuration(2,  2,      2) );
        patterns.Add( new SleepDuration(2,  1,      2) );

        patterns.Add( new SleepDuration(1,  4,      0) );
        patterns.Add( new SleepDuration(1,  3,      2) );
        patterns.Add( new SleepDuration(1,  3,      1) );
        patterns.Add( new SleepDuration(1,  2,      2) );
    }

    private void Update() {
        if (manager != null) {
            if (manager.canPlay && !started) 
            {
                started = true;
                int i = Random.Range(0,patterns.Count);
                StartCoroutine( Wakeup(patterns[i].snore, patterns[i].delay, patterns[i].duration) );
            }
        }
        else if (pw != null) {
            if (pw.canPlay && !started) 
            {
                started = true;
                int i = Random.Range(0,patterns.Count);
                StartCoroutine( Wakeup(patterns[i].snore, patterns[i].delay, patterns[i].duration) );
            }
        }
    }

    private IEnumerator Wakeup(float snoreTime, float delayTime, float awakeTime)
    {
        Debug.Log("-- " + snoreTime + ", " + delayTime + ", " + awakeTime);
        yield return new WaitForSeconds(snoreTime);
        _zzz.SetActive(false);
        if (bgMusic != null) bgMusic.volume = 0.05f;

        yield return new WaitForSeconds(delayTime);
        if (awakeTime > 0) 
        {
            _hurtBox.SetActive(true);
            _anim.SetBool("Awake", true);
        }

        yield return new WaitForSeconds(awakeTime);
        _zzz.SetActive(true);
        _hurtBox.SetActive(false);
        _anim.SetBool("Awake", false);
        if (bgMusic != null) bgMusic.volume = 0.25f;


        int i = Random.Range(0,patterns.Count);
        StartCoroutine( Wakeup(patterns[i].snore, patterns[i].delay, patterns[i].duration) );
    }
}


public class SleepDuration
{
    public float snore;
    public float delay;
    public float duration;

    public SleepDuration(float newSnore, float newDelay, float newDuration)
    {
        snore = newSnore; 
        delay = newDelay; 
        duration = newDuration;
    }
}
