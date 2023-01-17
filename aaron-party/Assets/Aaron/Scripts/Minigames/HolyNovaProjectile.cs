using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyNovaProjectile : MonoBehaviour
{
    private void FixedUpdate() {
        transform.localScale *= 1.05f;
    }
}
