using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellNodeSpawner : MonoBehaviour
{
    private List<Transform> spawnPos;
    private List<int> spawnIndex;
    private int nSpawn;
    private LevelManager manager;
    private PreviewManager pw;
    private GameController controller;
    [SerializeField] private GameObject nodePrefab;
    private int nCollected;
    private bool spawnOnce;
    private GameObject[] spawned;


    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("Level_Manager") != null) manager = GameObject.Find("Level_Manager").GetComponent<LevelManager>();
        if (GameObject.Find("Preview_Manager") != null) pw    = GameObject.Find("Preview_Manager").GetComponent<PreviewManager>();
        if (GameObject.Find("Game_Controller") != null) controller = GameObject.Find("Game_Controller").GetComponent<GameController>();
        
        spawnPos = new List<Transform>();
        spawnIndex  = new List<int>();
        if (controller.nPlayers <= 2) nSpawn = 8;
        else nSpawn = 12;
        spawned = new GameObject[nSpawn];

        int i=0;
        foreach (Transform child in this.transform)
        {
            spawnIndex.Add(i);
            i++;
            spawnPos.Add(child.transform);
        }

        StartCoroutine( StartSpawn(0) );
    }

    public void SpawnAgain()
    {
        nCollected++;
        if (nCollected >= nSpawn) { StartCoroutine( StartSpawn(0) ); nCollected = 0; }
    }

    private void ResetSpawns()
    {
        spawnIndex.Clear();
        int i = 0;
        foreach (Transform child in this.transform)
        {
            spawnIndex.Add(i);
            i++;
            spawnPos.Add(child.transform);
        }
    }
    public void TEDIOUS()
    {
        StartCoroutine( TITAN_NODE_FOUND() );
    }

    public IEnumerator TITAN_NODE_FOUND()
    {
        Debug.Log("LOL");
        yield return new WaitForSeconds(0.1f);
        for (int i=0; i<spawned.Length ; i++)
        {
            Debug.Log("xxx");
            if (spawned[i] != null) { Destroy(spawned[i]); Debug.Log("spawner called"); }
        }
        
        yield return new WaitForSeconds(3.75f);
        StartCoroutine(StartSpawn(0.1f));

    }

    IEnumerator StartSpawn(float delay)
    {
        if (manager != null)    { if ( (controller.nPlayers - manager.nPlayersOut) <= 2) { nSpawn = 8; } }
        else if (pw != null)    { if ( (controller.nPlayers - pw.nPlayersOut) <= 2) { nSpawn = 8; } }
        if (nSpawn >= spawnIndex.Count) { ResetSpawns(); }

        yield return new WaitForSeconds(delay);
        int titanSpell = Random.Range(0,nSpawn);
        for (int i=0 ; i<nSpawn ; i++)
        {
            int rIndex = Random.Range(0, spawnIndex.Count);
            int rng    = spawnIndex[ rIndex ];
            if (!spawnOnce) rng = i+1;
            var obj = Instantiate(nodePrefab, spawnPos[rng].position, Quaternion.identity);
            obj.transform.parent = this.transform;
            spawned[i] = obj;
            FreeSpellNode node = obj.GetComponent<FreeSpellNode>();
            node.spawner = this.GetComponent<SpellNodeSpawner>();
            if (titanSpell == i) node.isTitanSpell = true;
            spawnIndex.RemoveAt(rIndex);
        }
        spawnOnce = true;
    }
}
