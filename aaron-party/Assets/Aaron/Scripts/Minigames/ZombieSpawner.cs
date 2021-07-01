using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{

    [SerializeField] private Zombie zombiePrefab;
    [SerializeField] private Animator anim;
    private MinigameManager manager;
    public CentralZombieSpawner hub;


    public IEnumerator StartSpawn(float delay)
    {
        yield return new WaitForSeconds(delay);

        anim.Play("Zombie_Grave_Anim", -1 , 0);
        anim.SetTrigger("zombieSpawned");
        yield return new WaitForSeconds(2);
        var obj = Instantiate(zombiePrefab, transform.position - new Vector3(0,1), Quaternion.identity);
        obj.transform.parent = this.transform;
        obj.spawner = hub;

        // StartCoroutine( StartSpawn(Random.Range(5,11)) );
    }

}
