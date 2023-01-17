using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureSpawner : MonoBehaviour
{
    private List<Transform> spawns;
    Transform[] children;
    List<Transform> prevSpawn;
    int nSpawns = 12;
    int nCollect = 0;
    string[] names;
    [SerializeField] private GameObject gold;
        
    // Start is called before the first frame update
    void Start()
    {
        spawns      = new List<Transform>();
        prevSpawn   = new List<Transform>();
        names       = new string[nSpawns];
        children    = this.gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform child in children) { spawns.Add(child); }

        SPAWN_TREASURE();
    }

    void SPAWN_TREASURE()
    {
        for (int i=0 ; i<nSpawns ; i++)
        {
            int rng = Random.Range(0, spawns.Count);
            var go = Instantiate(gold, spawns[rng].position, Quaternion.identity);
            prevSpawn.Add(spawns[rng]);
            spawns.RemoveAt(rng);
        }
    }

    public void A_SPAWN_FOUND()
    {
        nCollect++;
        if (nCollect >= nSpawns)
        {
            nCollect = 0;
            SPAWN_MORE();
        }
    }

    void RESET_SPAWNS()
    {
        foreach (Transform child in children) { 
            spawns.Add(child); 
            for (int i=0 ; i<prevSpawn.Count ; i++)
            {
                if (prevSpawn[i].name == child.name)
                {
                    spawns.Remove(child); break;
                }
            }
        }
        prevSpawn.Clear();
    }

    public void SPAWN_MORE()
    {
        RESET_SPAWNS();

        for (int i=0 ; i<nSpawns ; i++)
        {
            int rng = Random.Range(0, spawns.Count);
            var go = Instantiate(gold, spawns[rng].position, Quaternion.identity);
            prevSpawn.Add(spawns[rng]);
            spawns.RemoveAt(rng);
        }
    }

}
