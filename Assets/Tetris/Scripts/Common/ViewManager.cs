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

    // [SerializeField]
    // protected bool m_AllowMulti;

    public static T instance =>
        m_Instance ??= Core.FindOrCreateManager<T>(typeof(T).GetCustomAttribute<SceneBindAttribute>().SceneName
            /*?? typeof(T).Namespace?.Split('.').Last()*/);

    [Button]
    protected virtual void Awake()
    {
        m_Instance ??= this as T;
        GetType().GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
            .Where(t => t.IsDefined(typeof(TagsAttribute))).ToList().ForEach(mi => {
                mi.GetCustomAttribute<TagsAttribute>().Invoke(mi, this);
            });
    }

    void OnDestroy()
    {
        if (m_Instance == this) {
            m_Instance = null;
        }
    }
}