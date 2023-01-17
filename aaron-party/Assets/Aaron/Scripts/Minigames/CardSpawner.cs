using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CardSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private string direction;
    private int[] worth = {-100,-100,-50,10,20,30,50,50,100,100};
    private int[] speeds = {2,2,4,4,6};

    private void Start() {
        if (SceneManager.GetActiveScene().name == "Card_Collectors") 
        {
            StartCoroutine( SPAWN_CARD(4) );
        }
        else { StartCoroutine( SPAWN_CARD(0.5f) ); }
    }

    private IEnumerator SPAWN_CARD(float nextSpawn)
    {
        yield return new WaitForSeconds(nextSpawn);
        var obj = Instantiate(cardPrefab, transform.position, Quaternion.identity, this.transform);
        CardPoints card = obj.GetComponent<CardPoints>();

        int rng = worth[Random.Range(0, worth.Length)];
        card.value = rng;
        rng     = speeds[Random.Range(0, speeds.Length)];
        card.moveSpeed = rng;
        card.direction = this.direction;

        StartCoroutine( SPAWN_CARD( Random.Range(1f,6f) ) );
    }
}
