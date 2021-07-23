using GameEngine.Extensions;
using GameEngine.Kernel;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

#if UNITY_EDITOR
#endif

namespace GameEngine.Providers {

public abstract class ProviderManager<T, S> : ProviderManager, IList<ProviderManager<T, S>> where T : ProviderManager<T, S> where S : Controller<T, S> {

    protected static T m_instance;
    public static S m_System;
    readonly IList<ProviderManager<T, S>> m_List = new List<ProviderManager<T, S>>();
    bool m_Awaked = false;
    public S system => m_System;

    public static T Instance {
        get {
            // if( m_instance == null ) {
            //     m_instance = Core.FindObjectOfTypeAll<T>();
            // }
            if (m_instance == null && (m_instance = Core.FindObjectOfTypeAll<T>()) == null) {
                // (m_instance = Main.Instance.GetComponentInChildren<T>()) == null ) {
                var go = GameObject.Find("/" + DefaultBootstrap.Instance.gameObject.name + "/" + typeof(T).Name) ??
                    new GameObject(typeof(T).Name);

                if (go.transform.parent == null) {
                    go.transform.SetParent(DefaultBootstrap.Instance.transform);
                }

                m_instance = go.RequireComponent<T>();
            }

            return m_instance;
        }
    }

    public IEnumerator<ProviderManager<T, S>> GetEnumerator() => m_List.GetEnumerator();

    //yield break;
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Add(ProviderManager<T, S> item)
    {
        m_List.Add(item);
    }

    public void Clear()
    {
        m_List.Clear();
    }

    public bool Contains(ProviderManager<T, S> item) => m_List.Contains(item);

    public void CopyTo(ProviderManager<T, S>[] array, int arrayIndex)
    {
        m_List.CopyTo(array, arrayIndex);
    }

    public bool Remove(ProviderManager<T, S> item) => m_List.Remove(item);

    public int Count => m_List.Count;
    public bool IsReadOnly => m_List.IsReadOnly;

    public int IndexOf(ProviderManager<T, S> item) => m_List.IndexOf(item);

    public void Insert(int index, ProviderManager<T, S> item)
    {
        m_List.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        m_List.RemoveAt(index);
    }

    public ProviderManager<T, S> this[int index] { get => m_List[index]; set => m_List[index] = value; }

    protected virtual void Awake()
    {
        if (m_instance == null) {
            m_instance = (T)this;
        }

        // Task(sys => sys.RunMethod<OnAwake>());
        GetType()
            .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            .Where(t => typeof(IAutoload).IsAssignableFrom(t.FieldType))
            .ForEach(t => {
                if (t.GetValue(this) == null) {
                    t.SetValue(this, Activator.CreateInstance(t.FieldType));
                }

                var func = t.GetValue(this)
                    ?.GetType()
                    .GetMethod("Autoload",
                        BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (func != null) {
                    func.Invoke(t.GetValue(this), null);
                }
            });
    }

    // public void bind<T1>(ref TextUI<T1> field, TextUI<T1> target)
    //     where T1 : IComparable, IConvertible, IEquatable<T1>
    // {
    //     if( field != null && target != null ) {
    //         target.ui = field.ui;
    //     }
    //
    //     if( target?.ui is Text text ) {
    //         text.text = $"{target.value}";
    //     } else if( target?.ui is Toggle toggle ) {
    //         toggle.isOn = (bool)(object)target.value;
    //     }
    //
    //     field = target;
    // }

    protected virtual void OnEnable() { }

    protected virtual void Start()
    {
        system?.RunMethod<OnAwake>();
        system?.RunMethod<OnStart>();
    }

    // protected virtual void OnEnable()
    // {
    //    // Task(sys => sys.RunMethod<OnEnable>());
    // }
    //
    //
    // protected virtual void OnDestroy()
    // {
    //    // Task(sys => sys.RunMethod<OnDestroy>());
    // }
    //
    //
    // protected virtual void OnDisable()
    // {
    //    // Task(sys => sys.RunMethod<OnDisable>());
    // }

    public void Task(Action<S> action)
    {
        if (system == null) {
            return;
        }

        var t = system.mb;
        system.mb = (T)this;
        action?.Invoke(system);
        system.mb = t;
    }

    public IEnumerable<ProviderManager<T, S>> Enabled()
    {
        return this.Where(t => t != null && t.transform != null);
    }

}

public abstract class ProviderManager : SerializedMonoBehaviour { }

}