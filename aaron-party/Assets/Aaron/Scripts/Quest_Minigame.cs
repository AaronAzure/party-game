using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Quest_Minigame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RETURN_BACK_TO_MAP());
    }

    IEnumerator RETURN_BACK_TO_MAP()
    {
        yield return new WaitForSeconds(0.2f);
        string mySavedScene = PlayerPrefs.GetString("sceneName");
        SceneManager.LoadScene(mySavedScene);
    }
}
