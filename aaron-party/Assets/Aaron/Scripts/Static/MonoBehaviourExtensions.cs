using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class MonoBehaviourExtensions
{
    public static void CallWithDelay(this MonoBehaviour mono, Action method, float delay=0)
    {
        mono.StartCoroutine( CallWithDelayRoutine (method, delay) );
    }

    static IEnumerator CallWithDelayRoutine(Action method, float delayTime=0)
    {
        yield return new WaitForSeconds(delayTime);
        method();
    }
}
