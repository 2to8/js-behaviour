using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameEngine.Extensions {

public static class MonoBehaviourExt {

    public static void StartCoroutine(this MonoBehaviour mb, Action funcs, float time = 0)
    {
        mb.StartCoroutine(CoroutineRunnerSimple(time, new[] { funcs }));
    }

    public static void StartCoroutine(this MonoBehaviour mb, params Action[] funcs)
    {
        mb.StartCoroutine(CoroutineRunnerSimple(0, funcs));
    }

    public static void StartCoroutine(this MonoBehaviour mb, float time, params Action[] funcs)
    {
        mb.StartCoroutine(CoroutineRunnerSimple(time, funcs));
    }

    static IEnumerator CoroutineRunnerSimple(float time, Action[] funcs)
    {
        foreach (var func in funcs) {
            if (time > 0) {
                yield return new WaitForSeconds(time);
            }

            func?.Invoke();

            //
            // Thanks bunny83
            yield return null;
        }
    }

    public static T FindOrNewManager<T>(this Component self) where T : Component => self.GetComponentInChildren<T>() ??
        Object.FindObjectOfType<T>() ?? self.AddComponentOnce<T>();

    public static T MyPosition<T>(this T selfComponent, Vector3 position) where T : Component
    {
        selfComponent.transform.position = position;

        return selfComponent;
    }

    public static T MyLocalScale<T>(this T selfComponent, float xyz) where T : Component
    {
        selfComponent.transform.localScale = Vector3.one * xyz;

        return selfComponent;
    }

    public static T MyRotation<T>(this T selfComponent, Quaternion rotation) where T : Component
    {
        selfComponent.transform.rotation = rotation;

        return selfComponent;
    }

}

}