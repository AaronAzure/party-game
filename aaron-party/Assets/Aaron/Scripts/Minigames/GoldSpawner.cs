using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldSpawner : MonoBehaviour
{
    private List<Transform> spawnPos;
    private List<int> spawnIndex;
    private int nSpawn;
    private int maxSpawn;
    private MinigameManager manager;
    private GameController controller;
    [SerializeField] private GameObject goldPrefab;
    [SerializeField] private GameObject effPrefab;
    private int nCollected;


    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("Level_Manager") != null) manager = GameObject.Find("Level_Manager").GetComponent<MinigameManager>();
        if (GameObject.Find("Game_Controller") != null) 
        {
            controller = GameObject.Find("Game_Controller").GetComponent<GameController>();
        }
        
        spawnPos = new List<Transform>();
        spawnIndex  = new List<int>();
        if (controller.nPlayers < 5) nSpawn = 5 * controller.nPlayers;
        else nSpawn = 4 * controller.nPlayers;

        int i=0;
        foreach (Transform child in this.transform)
        {
            foreach (Transform grandChild in child)
            {
                foreach (Transform greatGrandChild in grandChild)
                {
                    spawnIndex.Add(i);
                    i++;
                    spawnPos.Add(greatGrandChild.transform);
                }
            }
        }
        maxSpawn = (i+1);
    

        if (manager == null)    StartCoroutine( StartSpawn(0.5f) );
        else                    StartCoroutine( StartSpawn(4) );
    }

    public void SpawnAgain()
    {
        nCollected++;
        if (nCollected >= nSpawn - 2) { StartCoroutine( StartSpawn(0) ); nCollected = 0; }
    }

    private void ResetSpawns()
    {
        spawnIndex.Clear();
        int i = 0;
        foreach (Transform child in this.transform) 
        {
            foreach (Transform grandChild in child) 
            {
                foreach (Transform greatGrandChild in grandChild) 
                {
                    spawnIndex.Add(i);
                    i++;
                    spawnPos.Add(greatGrandChild.transform);
                }
            }
        }
    }

    IEnumerator StartSpawn(float delay)
    {
        if (nSpawn >= spawnIndex.Count) { ResetSpawns(); }
        yield return new WaitForSeconds(delay);
        for (int i=0 ; i<nSpawn ; i++)
        {
            int rIndex = Random.Range(0, spawnIndex.Count);
            int rng    = spawnIndex[ rIndex ];
            var obj = Instantiate(goldPrefab, spawnPos[rng].position, Quaternion.identity);
            obj.transform.parent = this.transform;
            obj.GetComponent<Gold>().spawner = this.GetComponent<GoldSpawner>();
            var eff = Instantiate(effPrefab, spawnPos[rng].position, effPrefab.transform.rotation);
            Destroy(eff, 0.6f);
            spawnIndex.RemoveAt(rIndex);
        }
    }
    
}
