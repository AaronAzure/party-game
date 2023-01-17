using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadeSpawner : MonoBehaviour
{

    [SerializeField] private Transform[] spawnPos;
    [SerializeField] private GameObject blueShade;
    [SerializeField] private GameObject redShade;
    [SerializeField] private GameObject goldShade;


    private GameController ctr;
    private MinigameManager manager;
    private PreviewManager pw;


    void Start()
    {
        if (GameObject.Find("Preview_Manager") != null) pw = GameObject.Find("Preview_Manager").GetComponent<PreviewManager>();
        if (GameObject.Find("Level_Manager") != null) manager = GameObject.Find("Level_Manager").GetComponent<MinigameManager>();
        
        // LEVEL_DIFFICULTY();
        
        if (manager == null)    StartCoroutine( Spawn(1 + 1) );
        else                    StartCoroutine( Spawn(4 + 1) );
    }

    IEnumerator Spawn(float time, int shadeType=3, List<int> prevSpawns=null)
    {
        yield return new WaitForSeconds(time);

        List<int> temp = new List<int>();
        List<int> thisSpawns = new List<int>();
        for (int i=0 ; i<spawnPos.Length ; i++) temp.Add(i);
        int r = temp[Random.Range(0, temp.Count)];
        
        // DON'T SPAWN IN PREV SPAWNS
        if (prevSpawns != null) {
            foreach (int alreadyDone in prevSpawns) temp.Remove(alreadyDone);
        }

        switch (shadeType)
        {
            case 0:
                Instantiate(goldShade, spawnPos[r].position, Quaternion.identity, this.transform);
                thisSpawns.Add(r);
                break;
            case 1:
                for (int i=0 ; i<2 ; i++)
                {
                    Instantiate(redShade, spawnPos[r].position, Quaternion.identity, this.transform);
                    thisSpawns.Add(r);
                    temp.Remove(r);
                    r = temp[Random.Range(0, temp.Count)];
                    thisSpawns.Add(r);
                }
                break;
            default: 
                for (int i=0 ; i<3 ; i++)
                {
                    Instantiate(blueShade, spawnPos[r].position, Quaternion.identity, this.transform);
                    thisSpawns.Add(r);
                    temp.Remove(r);
                    r = temp[Random.Range(0, temp.Count)];
                    thisSpawns.Add(r);
                }
                break;
        }

        if (shadeType == 0) shadeType = 4;
        StartCoroutine( Spawn(3, shadeType-1, thisSpawns) );
    }
}
