using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    // BOX CONTAINS EITHER ONE OF THESE FOUR
    private string[] boxes;   
    private int      nBox = 0;
    private int      maxBox = 16;
    private List<string> boxContents;   // 15 BOXES CONTAINING ONE OF THE FOUR   
    private LevelManager manager;   
    private GameController ctr;
    [SerializeField] private GameObject boxPrefab;
    private float spawnRate = 1.125f;    // Box.moveFor + Box.stopFor
    private bool onLeftSide;

    // Start is called before the first frame update
    void Start()
    {
        //  WHAT COULB BE INSIDE EACH BOX
        boxContents = new List<string>();
        for (int i=0 ; i<5 ; i++) { boxContents.Add( "one" ); }     // five     1 gold boxes
        for (int i=0 ; i<3 ; i++) { boxContents.Add( "three" ); }   // three    3 gold boxes
        for (int i=0 ; i<1 ; i++) { boxContents.Add( "five" ); }    // one      5 gold boxes
        for (int i=0 ; i<8 ; i++) { boxContents.Add( "bomb" ); }    // sixe     1 bomb boxes
        maxBox = boxContents.Count;

        ctr = GameObject.Find("Game_Controller").GetComponent<GameController>();
        if (ctr.easy)
        {
            spawnRate = 1.5f;
        }

        // FILL BOXES
        boxes = new string[maxBox];
        for (int i=0 ; i<boxes.Length ; i++)
        {
            int rng = Random.Range(0,boxContents.Count);
            if (i == 0) rng = 0;    // FIRST ONE ALWAYS A COIN
            if (i == 1) rng = 0;    // SECOND ONE ALSO A COIN
            boxes[i] = boxContents[ rng ];
            boxContents.RemoveAt(rng);
        }
        if (transform.position.x < 0) { onLeftSide = true; }

        if (GameObject.Find("Level_Manager") != null) {
            manager = GameObject.Find("Level_Manager").GetComponent<LevelManager>();
            StartCoroutine( START_CO(4) );
        }
        else    { StartCoroutine( START_CO(0.5f) ); }
    }

    private IEnumerator START_CO(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine( SPAWN(spawnRate) );
    }

    private IEnumerator SPAWN(float nextSpawn)
    {
        if (manager != null) if (manager.timeUp) { yield break; }
        
        if (nBox < maxBox)
        {
            var obj = Instantiate(boxPrefab, transform.position, Quaternion.identity);
            Box box = obj.GetComponent<Box>();
            box.transform.parent = this.transform;
            if (!onLeftSide) { box.moveLeft = true; }
            switch (boxes[nBox])
            {
                case "one"   : box.gold1.SetActive(true); break;
                case "three" : box.gold3.SetActive(true); break;
                case "five"  : box.gold5.SetActive(true); break;
                case "bomb"  : box.bomb.SetActive(true);  break;
            }
            nBox++;
        }

        if (manager != null) if (manager.timeUp) { yield break; }
        yield return new WaitForSeconds(nextSpawn);
        StartCoroutine( SPAWN( spawnRate ) );
    }
}
