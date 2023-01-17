using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightSafe : MonoBehaviour
{
    [SerializeField] private Transform[] targets;
    public bool movingLight;
    public bool doneMoving;
    private bool shrinker;
    public int nLight;
    private MinigameManager manager;
    private PreviewManager pw;
    public int currentPos;
    private int index;
    private float moveSpeed = 2;
    [SerializeField] private GameObject newSpotlightPrefab;

    void Start()
    {
        if (nLight == 0) {
            if (GameObject.Find("Level_Manager") != null) StartCoroutine( Wait(4) );
            else StartCoroutine( Wait(0.5f) );
        }
        else StartCoroutine( Wait(0) );
    }

    IEnumerator Wait(float delay)
    {
        yield return new WaitForSeconds(delay);

        doneMoving = false;
        if (nLight % 3 == 0 && nLight != 0) { movingLight = true;  }
        else   { movingLight = false; }
        if (nLight % 2 == 0 && nLight % 3 != 0 && nLight != 0) { shrinker = true;  }
        else   { shrinker = false; }
        if (!movingLight) StartCoroutine( RESIZE() );
    }

    IEnumerator RESIZE()
    {
		// GETS BIGGER
        if (!shrinker)
        {
            for (int i=0 ; i<6 ; i++)
            {
                yield return new WaitForSeconds(0.3f);
                if (i % 2 == 0) 
                {
                    for ( int s=0 ; s<25 ; s++)
                    {
                        yield return new WaitForEndOfFrame();
                        transform.localScale *= 1.01f;
                    }
                }
                else 
                {
                    for ( int s=0 ; s<25 ; s++)
                    {
                        yield return new WaitForEndOfFrame();
                        transform.localScale /= 1.01f;
                    }
                }
            }
            StartCoroutine( NEXT_LIGHT() );
            for (int i=0 ; i<1000 ; i++)
            {
                if (transform.localScale.x < 0) yield break;
                yield return new WaitForEndOfFrame();
                transform.localScale /= 1.05f;
            }
        }
		// GETS SMALLER
        else
        {
            for (int i=0 ; i<6 ; i++)
            {
                yield return new WaitForSeconds(0.3f);
                if (i % 2 != 0) 
                {
                    for ( int s=0 ; s<25 ; s++)
                    {
                        yield return new WaitForEndOfFrame();
                        transform.localScale *= 1.025f;
                    }
                }
                else 
                {
                    for ( int s=0 ; s<25 ; s++)
                    {
                        yield return new WaitForEndOfFrame();
                        transform.localScale /= 1.025f;
                    }
                }
            }
            StartCoroutine( NEXT_LIGHT() );
            for (int i=0 ; i<1000 ; i++)
            {
                if (transform.localScale.x < 0) yield break;
                yield return new WaitForEndOfFrame();
                transform.localScale /= 1.05f;
            }
        }
    }

    IEnumerator NEXT_LIGHT()
    {
        if (newSpotlightPrefab != null)
        {
            int rng;
            do  { rng = Random.Range(0, targets.Length); } while (rng == this.currentPos);
            var obj = Instantiate(newSpotlightPrefab, targets[rng].position, newSpotlightPrefab.transform.rotation);
            obj.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            SpotlightSafe nextLight = obj.GetComponent<SpotlightSafe>();
            nextLight.nLight = this.nLight + 1;
            nextLight.currentPos = rng;

            if (nLight % 3 == 0 && nLight != 0) {
                int posToMove;
                do  { posToMove = Random.Range(0, targets.Length); } while (posToMove == rng);
                nextLight.index = posToMove;
            }
        }
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }

    void FixedUpdate() 
    {
        if (movingLight)
        {
            transform.position = Vector3.MoveTowards(transform.position, targets[index].position, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targets[index].position) < 0.001f && !doneMoving)
            {
                doneMoving = true;
                StartCoroutine( NEXT_LIGHT() );
            }
        }
    }

}
