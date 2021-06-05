using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class Zombie : MonoBehaviour
{
    [SerializeField] private AIDestinationSetter dest;
    [SerializeField] private AIPath aIPath;
    [SerializeField] private GameObject character;
    public CentralZombieSpawner spawner;
    private LevelManager manager;
    private float scaleX;
    // public GameObject targets;
    private float minDistance;


    private void Start() {
        scaleX = character.transform.localScale.x;
        Debug.Log(dest.name + " moving towards " + spawner.targets[0].name);
        if (GameObject.Find("Level_Manager") != null) {
            manager = GameObject.Find("Level_Manager").GetComponent<LevelManager>();
        }
    }


    private void FixedUpdate()
    {
        // this.transform.eulerAngles = new Vector3(0,0,0);
        if (manager != null) 
        {
            if (!manager.timeUp)
            {
                if (spawner.targets.Count > 0)
                {
                    dest.target = spawner.targets[0].transform;
                    for (int i=0 ; i<spawner.targets.Count ; i++)
                    {
                        if (i==0)
                        {
                            minDistance = Vector3.Distance(spawner.targets[i].transform.position, this.transform.position);
                            dest.target = spawner.targets[i].transform;
                        }   
                        else 
                        {
                            float newDistance = Vector3.Distance(spawner.targets[i].transform.position, this.transform.position);
                            if (newDistance < minDistance) 
                            {
                                minDistance = Vector3.Distance(spawner.targets[i].transform.position, this.transform.position);
                                dest.target = spawner.targets[i].transform;
                            }
                        }
                    }
                    if (aIPath.desiredVelocity.x > 0) {
                        character.transform.localScale = new Vector3(-scaleX, scaleX, scaleX);
                    }
                    else {
                        character.transform.localScale = new Vector3(scaleX, scaleX, scaleX);
                    }
                }
                
            }
            else {
                aIPath.canMove = false;
            }
        }
        else
        {
            if (spawner.targets.Count > 0)
            {
                dest.target = spawner.targets[0].transform;
                for (int i=0 ; i<spawner.targets.Count ; i++)
                {
                    if (i==0)
                    {
                        minDistance = Vector3.Distance(spawner.targets[i].transform.position, this.transform.position);
                        dest.target = spawner.targets[i].transform;
                    }   
                    else 
                    {
                        float newDistance = Vector3.Distance(spawner.targets[i].transform.position, this.transform.position);
                        if (newDistance < minDistance) 
                        {
                            minDistance = Vector3.Distance(spawner.targets[i].transform.position, this.transform.position);
                            dest.target = spawner.targets[i].transform;
                        }
                    }
                }
                
                if (aIPath.desiredVelocity.x > 0) {
                    character.transform.localScale = new Vector3(-scaleX, scaleX, scaleX);
                }
                else {
                    character.transform.localScale = new Vector3(scaleX, scaleX, scaleX);
                }
            }
        }
    }

    public void CAPTURED_A_HUMAN(GameObject human)
    {
        spawner.PLAYER_CAPTURED(human);
    }
}
