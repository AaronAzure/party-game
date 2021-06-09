using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dumplings : MonoBehaviour
{
    [SerializeField] private SpriteRenderer current;
    public Sprite[] dumplings;
    private int eaten = 0;

    private void Start() {
        current.sprite = dumplings[0];
    }

    public void MoreEaten()
    {
        if (eaten < dumplings.Length) eaten++;
        current.sprite = dumplings[eaten];
    }

}
