using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralZombieSpawner : MonoBehaviour
{
    private ZombieSpawner[] spawners;
    private GameController ctr;
    private MinigameManager manager;
    private PreviewManager pw;
    public List<GameObject> targets;
    private int nSpawn;

    private void Start() {
        if (GameObject.Find("Level_Manager") != null) manager = GameObject.Find("Level_Manager").GetComponent<MinigameManager>();
        ctr = GameObject.Find("Game_Controller").GetComponent<GameController>();
        if (ctr.hard) nSpawn = 5;
        
        spawners = this.GetComponentsInChildren<ZombieSpawner>();
        foreach(ZombieSpawner zs in spawners) { zs.hub = this.GetComponent<CentralZombieSpawner>(); }

        targets = new List<GameObject>();

        StartCoroutine(DelayGetPlayers());
    }

    IEnumerator DelayGetPlayers()
    {
        yield return new WaitForSeconds(0.1f);

        if (manager != null) {
            Debug.Log("Adding players");
            for (int i=0 ; i<manager.players.Length ; i++) { 
                Debug.Log(manager.players[i].name);
                targets.Add( manager.players[i].gameObject ); 
            }
            StartCoroutine( SelectSpawner(7) );
        }
        else {
            pw = GameObject.Find("Preview_Manager").GetComponent<PreviewManager>();
            for (int i=0 ; i<pw.players.Length ; i++) { targets.Add( pw.players[i].gameObject ); }
            StartCoroutine( SelectSpawner(3) );
        }

        Debug.Log("--  nTargets = " + targets.Count);
        foreach (GameObject player in targets) Debug.Log(player.name);
    }

    public void PLAYER_CAPTURED(GameObject captured)
    {
        foreach (GameObject player in targets)
        {
            if (player == captured) 
            {
                targets.Remove(player); break;
            }
        }
    }

    IEnumerator SelectSpawner(float delay)
    {
        nSpawn++;

        yield return new WaitForSeconds(delay);
        if (manager != null) if (manager.timeUp) yield break;
        int rng = Random.Range(0, spawners.Length);
        StartCoroutine( spawners[rng].StartSpawn(0) );
        if (nSpawn > 4)
        {
            int rng2 = Random.Range(0, spawners.Length);
            while (rng2==rng) { rng2 = Random.Range(0, spawners.Length); }
            StartCoroutine( spawners[rng2].StartSpawn(0) );
			if (ctr.hard)
			{
				if (nSpawn > 12)
				{
					int rng3 = Random.Range(0, spawners.Length);
					while (rng3==rng||rng3==rng2) { rng3 = Random.Range(0, spawners.Length); }
					StartCoroutine( spawners[rng3].StartSpawn(0) );
					if (nSpawn > 18)
					{
						int rng4 = Random.Range(0, spawners.Length);
						while (rng4==rng||rng4==rng2||rng4==rng3) { rng4 = Random.Range(0, spawners.Length); }
						StartCoroutine( spawners[rng4].StartSpawn(0) );
					}
				}
			}
        }

        if (ctr.hard)
        {
            if (nSpawn <= 3) StartCoroutine( SelectSpawner( 4 ) );
            else StartCoroutine( SelectSpawner( 2.5f ) );
        }
        else 
        {
            if (nSpawn <= 3) StartCoroutine( SelectSpawner( 5 ) );
            else StartCoroutine( SelectSpawner( 3 ) );
        }
    }
}
