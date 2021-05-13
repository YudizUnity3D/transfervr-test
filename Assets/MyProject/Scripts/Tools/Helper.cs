using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
// using SimpleJSON;
// using BestHTTP.SocketIO;
using System.Text;
using System.Text.RegularExpressions;


public static class Helper
{
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

