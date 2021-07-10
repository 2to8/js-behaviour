using System;
using System.Collections;
using UnityEngine;

namespace GameEngine.Updater {

/// <summary>
/// Dispatches an event every frame when a MonoBehaviour's coroutines are resumed
/// </summary>
/// <author>Jackson Dunstan, http://JacksonDunstan.com/articles/3382
/// <license>MIT</license>
public class CoroutineUpdater : IUpdater {

    MonoBehaviour monoBehaviour;
    Coroutine coroutine;
    float m_DeltaTime;

    public float deltaTime { get => m_DeltaTime; set => m_DeltaTime = value; }

    /// <summary>
    /// Dispatched every frame
    /// </summary>
    public event Action OnUpdate;

    /// <summary>
    /// The MonoBehaviour to run the coroutine with.
    /// Setting this stops any previous coroutine.
    /// </summary>
    public MonoBehaviour MonoBehaviour {
        get => monoBehaviour;
        set {
            Stop();
            monoBehaviour = value;
            Start();
        }
    }

    /// <summary>
    /// Start dispatching OnUpdate every frame
    /// </summary>
    public void Start()
    {
        if (coroutine == null && monoBehaviour) {
            coroutine = monoBehaviour.StartCoroutine(DispatchOnUpdate());
        }
    }

    /// <summary>
    /// Stop dispatching OnUpdate every frame
    /// </summary>
    public void Stop()
    {
        if (coroutine != null && monoBehaviour) {
            monoBehaviour.StopCoroutine(coroutine);
        }
        coroutine = null;
    }

    IEnumerator DispatchOnUpdate()
    {
        while (true) {
            yield return new WaitForEndOfFrame();
            deltaTime = Time.deltaTime;
            OnUpdate?.Invoke();

            yield return null;
        }
    }

}

}