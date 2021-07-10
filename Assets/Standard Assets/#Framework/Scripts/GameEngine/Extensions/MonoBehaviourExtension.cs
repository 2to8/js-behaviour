using System;
using System.Collections;
using UnityEngine;

namespace GameEngine.Extensions {

public static class MonoBehaviourExtension {

    public static void StartCoroutine(this MonoBehaviour mb, Action funcs)
    {
        mb.StartCoroutine(CoroutineRunnerSimple(new[] { funcs }));
    }

    public static void StartCoroutine(this MonoBehaviour mb, params Action[] funcs)
    {
        mb.StartCoroutine(CoroutineRunnerSimple(funcs));
    }

    static IEnumerator CoroutineRunnerSimple(Action[] funcs)
    {
        foreach (var func in funcs) {
            func?.Invoke();

            // yield return new WaitForSeconds(.01f);
            // Thanks bunny83
            yield return null;
        }
    }

}

}