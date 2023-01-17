using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoisonMist : MonoBehaviour
{

    [SerializeField] private GameObject mistPrefab;
    [SerializeField] private int        nMist;

    // Start is called before the first frame update
    void Start()
    {
        MistExplosion();
    }

    public void MistExplosion()
    {
        float angle = 360f / nMist;

        for (int i=0 ; i<nMist ; i++) 
        {
            if (mistPrefab != null) 
            {
                var obj = Instantiate(mistPrefab, this.transform.position, mistPrefab.transform.rotation, this.transform);
                int val = i % 2 == 0 ? 1 : -1;
                obj.transform.rotation = Quaternion.Euler( 
                    new Vector3(angle * i, -90 * val, 90 * val) 
                );
            }
        }
    }
}