using System;
using GameEngine.Attributes;
using GameEngine.Extensions;
using Sirenix.OdinInspector;
using System.Linq;
using System.Reflection;
using Common;
using MoreTags.Attributes;
using Tetris;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Scenes;

public abstract class Manager<T> : View<T> where T : Manager<T>
{
    //
    static T m_Instance;
    public static bool instanceExists => m_Instance != null;

    // [SerializeField]
    // protected bool m_AllowMulti;
    //   protected virtual void OnEnable()
//    {
//         InitBinding();
//    }

    public static T instance {
        get {
            if (m_Instance == null) {
                m_Instance = Core.FindOrCreateManager<T>(typeof(T).GetCustomAttribute<SceneBindAttribute>()?.SceneName
                    /*?? typeof(T).Namespace?.Split('.').Last()*/);
                m_Instance?.InitBinding();
            }

            return m_Instance;
        }
        protected set => m_Instance = value;
    }

 

    protected virtual void Awake()
    {
        m_Instance ??= (T) this;

        //SceneManager.sceneLoaded += (arg0, mode) => { InitBinding(); };
        //InitBinding();
    }

    protected virtual void OnDestroy()
    {
        if (m_Instance == this) {
            m_Instance = null;
        }
    }
}