using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// This scripts adds extensions helpfull in development
/// </summary>
public static class Helper
{
    /// <summary>
    /// Call this when delayed operation should be performed
    /// </summary>
    public static Coroutine Execute(this MonoBehaviour monoBehaviour, Action action, float time)
    {
        return monoBehaviour.StartCoroutine(DelayedAction(action, time));
    }


    static IEnumerator DelayedAction(Action action, float time)
    {
        yield return new WaitForSecondsRealtime(time);

        action();
    }
}

