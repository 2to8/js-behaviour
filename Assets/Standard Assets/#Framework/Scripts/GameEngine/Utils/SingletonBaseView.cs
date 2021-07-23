using GameEngine.Kernel;
using GameEngine.Kernel._Appliation;
using GameEngine.Views.Contracts;
using UnityEngine;

namespace GameEngine.Utils {

public abstract class SingletonBaseView<T> : Views.Contracts.BaseView<T> where T : SingletonBaseView<T> {

    protected static T m_Instance;
    static GameObject m_Root;
    public static GameObject Root => m_Root ?? (m_Root = GameBootstrapV1.Instance.gameObject);

    public static T Instance {
        get {
            //Debug.Log(Artecs.Config.Prefabs[typeof(T)]);
            //return null;

            if (m_Instance == null && (m_Instance = Core.FindObjectOfTypeAll<T>()) == null) {
                var asset = Resources.Load<T>($"Prefabs/{typeof(T).Name}");

                if (asset != null) {
                    m_Instance = Instantiate(asset, Root.transform);
                } else {
                    var go = new GameObject(typeof(T).Name, typeof(T));
                    go.transform.SetParent(Root.transform);
                    m_Instance = go.GetComponent<T>();
                }

                // var root = GameObject.Find("/[APPLICATION]") ?? new GameObject("[APPLICATION]");
                //
                // if(root == null) {
                //     Debug.LogError("error");
                // }
                //
                // Debug.Assert(root != null, "Root == null");
                // Debug.Assert(Config.model != null, "Config.data == null");
                // Debug.Assert(Config.model.Prefabs != null, "Config.data.Prefabs == null");
                // var go = Instantiate(Config.model.Prefabs[typeof(T)], root.transform);
                // go.name = go.name.Replace("(Clone)", "");
                // m_Instance = go.GetComponentInChildren<T>() ?? go.RequireComponent<T>();

                // if ( typeof(T) == typeof(Artecs) ) {
                // }
                // var root = GameObject.Find("/[App]");
                // if ( root == null ) {
                //     var res = Resources.Load<GameObject>(typeof(T).FullName?.Split('.').First() + "/Prefabs/app");
                //     if ( res != null ) {
                //         root      = Instantiate(res);
                //         root.name = root.name.Replace("(Clone)", "");
                //     }
                // }
                // root = root ?? new GameObject("[App]");
                //
                // var go = typeof(T) == typeof(Artecs) ? root
                //     : root.transform.Find(typeof(T).Name)?.gameObject ?? new GameObject(typeof(T).Name);
                // if ( go.transform.parent == null && go != root ) {
                //     go.transform.SetParent(root.transform);
                // }

                // m_Instance = go.RequireComponent<T>();
            }

            return m_Instance;
        }
    }

    public static T GetInstance() => Instance;

    protected override void Awake()
    {
        base.Awake();

        if (m_Instance == null) {
            m_Instance = this as T;
        }
    }

}

}