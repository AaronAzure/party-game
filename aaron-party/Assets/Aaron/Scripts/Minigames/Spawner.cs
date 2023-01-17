using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject spawnPrefab;
    [SerializeField] private GameObject spawnPrefab2;
    [SerializeField] private Transform[] spawnPos;
    private int timesToSpawn    = 4;
    private float spawnRate     = 1.1f;
    private int nSpawned        = 0;
    private bool newWave      = true;
    private MinigameManager manager;
    private GameController ctr;
    // private GameObject instances;

    private void Start() 
    {
        if (GameObject.Find("Game_Controller") != null) {
            ctr = GameObject.Find("Game_Controller").GetComponent<GameController>();
        }

        if (GameObject.Find("Level_Manager") != null) {
            manager = GameObject.Find("Level_Manager").GetComponent<MinigameManager>();
            StartCoroutine( START_CO(4) );
        }
        else    { StartCoroutine( START_CO(0.5f) ); }
    }

    private IEnumerator START_CO(float delay)
    {
        if (ctr.hard)  { nSpawned = 8; timesToSpawn = 7; spawnRate = 0.8f; }
        
        yield return new WaitForSeconds(delay);
        StartCoroutine( SPAWN(spawnRate) );
    }

    private IEnumerator SPAWN(float nextSpawn)
    {
        if (manager != null) if (manager.timeUp) { yield break; }

        List<int> indexes = new List<int>();
        for ( int i=0 ; i<spawnPos.Length ; i++ )   { indexes.Add(i); }

        if (!newWave)
        {
            for ( int i=0 ; i<timesToSpawn ; i++ )
            {
                int rng = indexes[ Random.Range(0, indexes.Count) ];
                var obj = Instantiate(spawnPrefab, spawnPos[ rng ].position, Quaternion.identity);
                obj.transform.localScale /= 2.2f;
                obj.transform.parent = this.transform;
                if ( i == 0 && nSpawned > 5 && nSpawned % 3 == 0) obj.transform.localScale *= 1.4f;
                if ( i == 3 && nSpawned > 10 && nSpawned % 4 == 0) obj.transform.localScale *= 1.4f;
                if ( i == 1 && nSpawned > 11 && nSpawned % 2 == 0) obj.transform.localScale *= 1.4f;
                if ( i == 2 && nSpawned > 18 && nSpawned % 2 == 0) obj.transform.localScale *= 1.4f;

                for (int r=0 ; r<indexes.Count ; r++) 
                {
                    if (indexes[r] == rng) { indexes.Remove(indexes[r]); break; }
                }
            }
        }
        else
        {
            for ( int i=0 ; i<timesToSpawn ; i++ )
            {
                int rng = indexes[ Random.Range(0, indexes.Count) ];
                var obj = Instantiate(spawnPrefab2, spawnPos[ rng ].position, Quaternion.identity);
                obj.transform.localScale /= 2.2f;
                obj.transform.parent = this.transform;
                if ( i == 0 && nSpawned > 5 && nSpawned % 3 == 0)  { 
                    obj.transform.localScale *= 1.4f;
                    obj.GetComponent<Penguin>().knockbackPower = 2.1f;
                }
                if ( i == 3 && nSpawned > 10 && nSpawned % 4 == 0) { 
                    obj.transform.localScale *= 1.4f;
                    obj.GetComponent<Penguin>().knockbackPower = 2.1f;
                }
                if ( i == 1 && nSpawned > 11 && nSpawned % 2 == 0) { 
                    obj.transform.localScale *= 1.4f;
                    obj.GetComponent<Penguin>().knockbackPower = 2.1f;
                }
                if ( i == 2 && nSpawned > 18 && nSpawned % 2 == 0) { 
                    obj.transform.localScale *= 1.4f;
                    obj.GetComponent<Penguin>().knockbackPower = 2.1f;
                }

                for (int r=0 ; r<indexes.Count ; r++) 
                {
                    if (indexes[r] == rng) { indexes.Remove(indexes[r]); break; }
                }
            }
        }

        nSpawned++;
        if      (nSpawned == 1) { spawnRate = 0.9f; timesToSpawn = 5;}
        else if (nSpawned == 4) { timesToSpawn = 6; }
        else if (nSpawned == 7) { spawnRate = 0.8f; }
        // else if (nSpawned == 11) { newWave = true; yield return new WaitForSeconds(1.1f); }
        else if (nSpawned == 11) { timesToSpawn = 7; }
        else if (nSpawned == 16) { spawnRate = 0.6f; }
        else if (nSpawned == 19) { timesToSpawn = 8; }

        yield return new WaitForSeconds(nextSpawn);
        StartCoroutine( SPAWN( spawnRate ) );
    }

    private IEnumerator SPAWN_HARD(float nextSpawn)
    {
        if (manager != null) if (manager.timeUp) { yield break; }

        List<int> indexes = new List<int>();
        for ( int i=0 ; i<spawnPos.Length ; i++ )   { indexes.Add(i); }

        if (!newWave)
        {
            for ( int i=0 ; i<timesToSpawn ; i++ )
            {
                int rng = indexes[ Random.Range(0, indexes.Count) ];
                var obj = Instantiate(spawnPrefab, spawnPos[ rng ].position, Quaternion.identity);
                obj.transform.localScale /= 2.2f;
                obj.transform.parent = this.transform;
                if ( i == 0 && nSpawned > 5 && nSpawned % 3 == 0) obj.transform.localScale *= 1.4f;
                if ( i == 3 && nSpawned > 10 && nSpawned % 4 == 0) obj.transform.localScale *= 1.4f;
                if ( i == 1 && nSpawned > 11 && nSpawned % 2 == 0) obj.transform.localScale *= 1.4f;
                if ( i == 2 && nSpawned > 18 && nSpawned % 2 == 0) obj.transform.localScale *= 1.4f;

                for (int r=0 ; r<indexes.Count ; r++) 
                {
                    if (indexes[r] == rng) { indexes.Remove(indexes[r]); break; }
                }
            }
        }
        else
        {
            for ( int i=0 ; i<timesToSpawn ; i++ )
            {
                int rng = indexes[ Random.Range(0, indexes.Count) ];
                var obj = Instantiate(spawnPrefab2, spawnPos[ rng ].position, Quaternion.identity);
                obj.transform.localScale /= 2.2f;
                obj.transform.parent = this.transform;
                if ( i == 0 && nSpawned > 5 && nSpawned % 3 == 0)  { 
                    obj.transform.localScale *= 1.4f;
                    obj.GetComponent<Penguin>().knockbackPower = 2.1f;
                }
                if ( i == 3 && nSpawned > 10 && nSpawned % 4 == 0) { 
                    obj.transform.localScale *= 1.4f;
                    obj.GetComponent<Penguin>().knockbackPower = 2.1f;
                }
                if ( i == 1 && nSpawned > 11 && nSpawned % 2 == 0) { 
                    obj.transform.localScale *= 1.4f;
                    obj.GetComponent<Penguin>().knockbackPower = 2.1f;
                }
                if ( i == 2 && nSpawned > 18 && nSpawned % 2 == 0) { 
                    obj.transform.localScale *= 1.4f;
                    obj.GetComponent<Penguin>().knockbackPower = 2.1f;
                }

                for (int r=0 ; r<indexes.Count ; r++) 
                {
                    if (indexes[r] == rng) { indexes.Remove(indexes[r]); break; }
                }
            }
        }

        nSpawned++;
        if      (nSpawned == 1) { spawnRate = 0.9f; timesToSpawn = 5;}
        else if (nSpawned == 4) { timesToSpawn = 6; }
        else if (nSpawned == 7) { spawnRate = 0.8f; }
        // else if (nSpawned == 11) { newWave = true; yield return new WaitForSeconds(1.1f); }
        else if (nSpawned == 11) { timesToSpawn = 7; }
        else if (nSpawned == 16) { spawnRate = 0.6f; }
        else if (nSpawned == 19) { timesToSpawn = 8; }

        yield return new WaitForSeconds(nextSpawn);
        StartCoroutine( SPAWN( spawnRate ) );
    }
}
