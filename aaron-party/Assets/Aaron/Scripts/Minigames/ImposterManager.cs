using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImposterManager : MonoBehaviour
{
	[System.Serializable]
	public class Group
	{
		public int[] num;
	}
	private GameController ctr;
    private MinigameManager manager;
    private PreviewManager pw;


	public Animator aaron;
	public GameObject explosion;
	public GameObject spotlight;
	public GameObject transformEffect;
	public Group[] groups;
	[Space] public int m=1;
	public int n=9;
	[Space] public int correctSpawn; // [ 0 , 1 , 2 , 3 ]
	[Space] public int correctAnim;
	public int wrongAnim;
	[Space] public GameObject genuine;
	public Transform[] spawns;
	public Selector[] selected;
	public List<Animator> spawned;
	private float[] times;
	public float guessDuration=10f;
	public float revealDuration=2f;
	public bool debug=true;

	


    // Start is called before the first frame update
    void Start()
    {
		if (GameObject.Find("Level_Manager") != null)
		{
			manager = GameObject.Find("Level_Manager").GetComponent<MinigameManager>();
			manager.imposterManager = this;
		}
		if (GameObject.Find("Game_Controller") != null)
        	ctr = GameObject.Find("Game_Controller").GetComponent<GameController>();

        // if (ctr.hard) 
		// 	nSpawn = 5;
		
        spawned = new List<Animator>();

		StartCoroutine( SpawnNewAarons( manager == null ? 1f : 4.5f ) );
    }

	IEnumerator SpawnNewAarons(float startDelay=0.5f)
	{
		foreach (Animator spawn in spawned)
			if (spawn != null)
				Destroy(spawn.gameObject);
		spawned.Clear();

		yield return new WaitForSeconds(startDelay);

		List<float> temp = new List<float>();
		for (int i=0 ; i<8 ; i++)
			temp.Add(0.125f * i);

		int correct;
		int wrong;
		if (Random.Range(0,2) == 0) 
		{
			correct	= groups[0].num[ Random.Range(0, groups[0].num.Length)];
			
			// MAKE SURE INCORRECT ANSWER IS NOT THE SAME AS THE CORRECT ANSWER
			wrong	= groups[0].num[ Random.Range(0, groups[0].num.Length)];
			while (wrong == correct)
				wrong	= groups[0].num[ Random.Range(0, groups[0].num.Length)];
		}
		else
		{
			correct	= groups[1].num[ Random.Range(0, groups[1].num.Length)];
			
			// MAKE SURE INCORRECT ANSWER IS NOT THE SAME AS THE CORRECT ANSWER
			wrong	= groups[1].num[ Random.Range(0, groups[1].num.Length)];
			while (wrong == correct)
				wrong	= groups[1].num[ Random.Range(0, groups[1].num.Length)];
		}
		correctAnim = correct;
		wrongAnim = wrong;

		correctSpawn = Random.Range(0, spawns.Length);

		for (int i=0 ; i<spawns.Length ; i++)
			Instantiate(explosion, spawns[i].position, explosion.transform.rotation, this.transform);
			
		yield return new WaitForSeconds(0.2f);
		for (int i=0 ; i<spawns.Length ; i++)
		{
			int a = i == correctSpawn ? correct : wrong;
			if (aaron != null)
			{
				var obj = Instantiate(aaron, spawns[i].position , Quaternion.identity, this.transform);
				
				float rng = temp[Random.Range(0, temp.Count)];
				obj.Play("aaron-" + a + "-anim", 0 , rng);
				temp.Remove(rng);

				spawned.Add(obj);
			}
		}
	}

	public IEnumerator REVEAL_ANSWER()
	{
		// REVEAL ANSWER
		transformEffect.SetActive(false);
		transformEffect.SetActive(true);
		for (int i=0 ; i<spawned.Count ; i++)
		{
			spawned[i].Play("aaron-" + (i==correctSpawn ? correctAnim : wrongAnim) + "-anim", 0 , 0);
		}
		spotlight.SetActive(false);
		spotlight.SetActive(true);
		spotlight.transform.position = spawns[correctSpawn].position;

		// REVEAL ANSWER
		yield return new WaitForSeconds(revealDuration);
		for (int i=0 ; i<spawns.Length ; i++)
			if (i != correctSpawn)
				Instantiate(explosion, spawns[i].position, explosion.transform.rotation, this.transform);

		yield return new WaitForSeconds(0.2f);
		for (int i=spawned.Count - 1 ; i>=0 ; i--)
			if (i != correctSpawn)
				Destroy(spawned[i].gameObject);


		yield return new WaitForSeconds(1);
		Instantiate(explosion, spawns[correctSpawn].position, explosion.transform.rotation, this.transform);

		yield return new WaitForSeconds(0.2f);
		Destroy(spawned[correctSpawn].gameObject);
		spotlight.SetActive(false);

		yield return new WaitForSeconds(1);
		StartCoroutine( SpawnNewAarons( 0 ) );
	}


	public int[] WhoGuessedCorrectly()
	{
		return selected[ correctSpawn ].playerIds;
	}

	public void CLEAR_SELECRION()
	{
		for (int i=0 ; i<selected.Length ; i++)
			selected[i].Clear();
	}

	
}
