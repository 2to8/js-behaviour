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

public abstract class ViewManager<T> : Component<T> where T : ViewManager<T>
{
    //
    static T m_Instance;
    bool BindingInited;
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

    [Button]
    public void InitBinding()
    {
        if (!BindingInited) {
            //BindingInited = true;
            Debug.Log($"binding tags: {GetType().FullName}");
            GetType()
                .GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                .Where(t => t.IsDefined(typeof(TagsAttribute))).ToList().ForEach(mi => {
                    mi.GetCustomAttribute<TagsAttribute>().Invoke(mi, this);
                });
        }
    }

    protected virtual void Awake()
    {
        m_Instance ??= (T) this;
        SceneManager.sceneLoaded += (arg0, mode) => { InitBinding(); };
        //InitBinding();
    }

  
    void OnDestroy()
    {
        if (m_Instance == this) {
            m_Instance = null;
        }
    }
}