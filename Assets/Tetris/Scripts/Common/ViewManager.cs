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

    // [SerializeField]
    // protected bool m_AllowMulti;

    public static T instance {
        get {
            if (m_Instance == null) {
                m_Instance = Core.FindOrCreateManager<T>(typeof(T).GetCustomAttribute<SceneBindAttribute>()?.SceneName
                    /*?? typeof(T).Namespace?.Split('.').Last()*/);
                m_Instance?.InitBinding();
            }

            return m_Instance;
        }
    }

    public void InitBinding()
    {
        if (!BindingInited) {
            BindingInited = true;
            GetType()
                .GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                .Where(t => t.IsDefined(typeof(TagsAttribute))).ToList().ForEach(mi => {
                    mi.GetCustomAttribute<TagsAttribute>().Invoke(mi, this);
                });
        }
    }

    [Button]
    protected virtual void Awake()
    {
        m_Instance ??= this as T;
        InitBinding();
    }

    void OnDestroy()
    {
        if (m_Instance == this) {
            m_Instance = null;
        }
    }
}