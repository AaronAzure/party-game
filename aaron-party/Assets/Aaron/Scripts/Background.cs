using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] private GameObject _bg;

    // Start is called before the first frame update
    void Start()
    {
        _bg.gameObject.SetActive(true);
    }

}
