using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxSpawner : MonoBehaviour
{
    [SerializeField] private Transform topLeft;
    [SerializeField] private Transform bottomRight;
    [SerializeField] private GameObject toSpawn;
    [SerializeField] private int nSpawn = 15;


    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("Game_Controller") != null && GameObject.Find("Game_Controller").TryGetComponent(out GameController ctr))
        {
            if (ctr.easy) nSpawn = Random.Range(12,19);
            if (ctr.norm) nSpawn = Random.Range(15,26);
            if (ctr.hard) nSpawn = Random.Range(15,26);
        }

        for (int i=0 ; i<nSpawn ; i++) {
            float x = Random.Range(topLeft.position.x, bottomRight.position.x);
            float y = Random.Range(bottomRight.position.y, topLeft.position.y);
            var spawned = Instantiate(toSpawn, new Vector3(x, y), Quaternion.identity, this.transform);
            spawned.name += " (" + (i + 1) + ")";
            RandomPatrol fox = spawned.GetComponent<RandomPatrol>();
            fox.upperBound  = topLeft.position.y - 1;
            fox.lowerBound  = bottomRight.position.y + 1;
            fox.leftBound   = topLeft.position.x + 1;
            fox.rightBound  = bottomRight.position.x - 1;
        }
    }
}
