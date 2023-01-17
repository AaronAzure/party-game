using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
	public SpriteRenderer[] renderers;
	public int[] playerIds;
	private int nId=0;


    // Start is called before the first frame update
    void Start()
    {
        playerIds = new int[8];
		for (int i=0 ; i<playerIds.Length ; i++)
			playerIds[i] = -1;
    }

	public void Select(int pid, Sprite spr)
	{
		renderers[ nId ].sprite = spr;
		playerIds[ nId ] = pid;
		nId++;
	}

	public void Clear()
	{
		nId = 0;
		
		for (int i=0 ; i<playerIds.Length ; i++)
			playerIds[i] = -1;

		for (int i=0 ; i<renderers.Length ; i++)
			renderers[i].sprite = null;
	}
}
