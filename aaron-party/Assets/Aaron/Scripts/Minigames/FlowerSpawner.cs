using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSpawner : MonoBehaviour
{
    private List<Transform> spawnPos;
    private List<int> spawnIndex;
    private int nSpawn;
    private MinigameManager manager;
    private GameController controller;
    [SerializeField] private GameObject flowerPrefab;
    [SerializeField] private GameObject goldPrefab;
    private int goldFlower = 20;
    private int nSpawned = 0;


    void Start()
    {
        if (GameObject.Find("Level_Manager") != null) manager = GameObject.Find("Level_Manager").GetComponent<MinigameManager>();
        if (GameObject.Find("Game_Controller") != null) 
        {
            controller = GameObject.Find("Game_Controller").GetComponent<GameController>();
        }
        
        spawnPos = new List<Transform>();
        spawnIndex  = new List<int>();
        nSpawn = 10;

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

        if (manager == null)    StartCoroutine( StartSpawn(0.5f) );
        else                    StartCoroutine( StartSpawn(4) );
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
            yield return new WaitForSeconds( Random.Range(0f,0.5f) );
            int g = Random.Range(0,goldFlower);
            int rIndex = Random.Range(0, spawnIndex.Count);
            int rng    = spawnIndex[ rIndex ];
            if (g < nSpawned)
            {
                var obj = Instantiate(goldPrefab, spawnPos[rng].position, Quaternion.identity);
                obj.transform.parent = this.transform;
            }
            else 
            {
                var obj = Instantiate(flowerPrefab, spawnPos[rng].position, Quaternion.identity);
                obj.transform.parent = this.transform;

            }
            spawnIndex.RemoveAt(rIndex);
        }

        nSpawned++;
        StartCoroutine( StartSpawn(Random.Range(0,3)) );
    }
}
