using System;
using UnityEngine;

namespace GameEngine.Controllers {

/// <summary>
/// Abstract class for making reload-proof singletons out of ScriptableObjects
/// Returns the asset created on the editor, or null if there is none
/// Based on https://www.youtube.com/watch?v=VBA1QCoEAX4
/// </summary>
/// <typeparam name="T">Singleton type</typeparam>
[Serializable]
public abstract class SingletonScriptableObject<T> : ScriptableObject where T : SingletonScriptableObject<T> {

    public static T Instance;

    void OnEnable()
    {
        if (Instance != null) {
            throw new UnityException(typeof(T) + " is already instantiated");
        }

        Instance = (T)this;
    }

    void OnDisable()
    {
        Instance = null;
    }

    // void OnValidate()
    // {
    //     SaveChange();
    //     Debug.Log($"{GetType().Name} changed");
    // }

    public void SaveChange()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
        Debug.Log("set dirty");
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
    #endif
    }

    protected static void SetValue<T>(ref T target, T value)
    {
        target = value;
        Instance.SaveChange();
    }

}

}