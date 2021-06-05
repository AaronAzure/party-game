using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeSpan : MonoBehaviour
{
    [SerializeField] private float destroyAfter;

    private void Start() {
        Destroy(this.gameObject, destroyAfter);
    }
}
