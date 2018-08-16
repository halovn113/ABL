using System;
using UnityEngine;
using System.Collections;

public static class MonoBehaviourExtensions
{
    private static bool _isRepeating;
    private static Action _tempAction;

    public static void Invoke(this MonoBehaviour me, Action theDelegate, float time)
    {
        _tempAction = theDelegate;
        me.StartCoroutine(ExecuteAfterTime(theDelegate, time));
    }

    private static IEnumerator ExecuteAfterTime(Action theDelegate, float delay)
    {
        yield return new WaitForSeconds(delay);
        theDelegate();
    }

    public static void StopInvoke(this MonoBehaviour me, float time)
    {
        if (_tempAction == null)
        {
            Debug.LogWarning("Warning, there is no function is invoking");
            return;
        }
        me.StartCoroutine(StopExecuteAfterTime(me, time));
    }

    private static IEnumerator StopExecuteAfterTime(MonoBehaviour me, float delay)
    {
        yield return new WaitForSeconds(delay);
        me.StopCoroutine(ExecuteAfterTime(_tempAction, 0));
    }

    public static void InvokeRepeating(this MonoBehaviour me, Action theDelegate, float delay, float time)
    {
        _isRepeating = false;
        me.StartCoroutine(ExecuteAfterTimeRepeating(theDelegate, delay, time));
    }

    private static IEnumerator ExecuteAfterTimeRepeating(Action theDelegate, float delay, float time)
    {
        yield return new WaitForSeconds(delay);
        _isRepeating = true;
        while (_isRepeating)
        {
            yield return new WaitForSeconds(time);
            theDelegate();
        }
    }

    public static void StopInvokeRepeating(this MonoBehaviour me, float delay)
    {
        me.StartCoroutine(StopExecuteAfterTimeRepeating(delay));
    }

    private static IEnumerator StopExecuteAfterTimeRepeating(float delay)
    {
        yield return new WaitForSeconds(delay);
        _isRepeating = false;
    }
}