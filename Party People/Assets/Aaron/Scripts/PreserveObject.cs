using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreserveObject : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(this);
        this.gameObject.SetActive(false);
    }
}
