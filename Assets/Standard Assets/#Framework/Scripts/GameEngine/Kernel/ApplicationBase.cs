using GameEngine.Contacts;
using GameEngine.Kernel._Appliation;
using GameEngine.Utils;
using System;
using System.Threading.Tasks;
using UniRx.Async;
using UnityEngine;
using Core_1 = Core;

#pragma warning disable 1998

namespace GameEngine.Kernel
{
    [Serializable]
    public abstract class ApplicationBase<T> : MonoBase where T : ApplicationBase<T>
    {
        protected static T m_Instance;
        static GameManager m_GameManager;

        public static GameManager GameViewRoot {
            get {
                if (m_GameManager == null) {
                    m_GameManager = FindObjectOfType<GameManager>();

                    if (m_GameManager == null) {
                        var go = GameObject.Find("/[App]") ?? new GameObject("[App]");
                        m_GameManager = go.AddComponent<GameManager>();
                    }
                }

                return m_GameManager;
            }
            private set => m_GameManager = value;
        }

        public static T Instance {
            get {
                if (m_Instance == null) {
                    m_Instance = FindObjectOfType<T>();

                    if (m_Instance == null) {
                        var go = GameViewRoot.transform.Find(typeof(T).Name)
                            ?? new GameObject(typeof(T).Name, typeof(T)).transform;

                        go.SetParent(GameViewRoot.transform);
                        m_Instance = go.gameObject.GetComponent<T>() ?? go.gameObject.AddComponent<T>();
                    }
                }

                return m_Instance;
            }
        }

        public static T GetInstance() => Instance;

        // get {
        //     lock(_lock) {
        //         if(m_Instance == null && !IsDestroying) {
        //             if(AppRoot == null) {
        //                 AppRoot = new GameObject("App");
        //             }
        //
        //             var go = new GameObject(typeof(T).Name);
        //             go.transform.parent = AppRoot.transform;
        //             m_Instance = go.AddComponent<T>();
        //         }
        //
        //         return m_Instance;
        //     }
        // }
        protected virtual async Task Awake()
        {
            await AppLoaderHander;

            if (m_Instance == null) {
                m_Instance = this as T;
            }

            await OnAwake();
        }

        protected virtual async UniTask OnAwake() { }

        protected virtual async UniTask Update()
        {
            await AppLoaderHander;

            // await OnUpdate();
        }

        // protected virtual async UniTask OnUpdate() { }

        /// <summary>
        ///     提供给 addressable 使用
        /// </summary>
        protected virtual async UniTask Start()
        {
            await AppLoaderHander;
            await OnStart();
        }

        protected virtual async UniTask OnStart() { }

        public override string ToString() => GetType().ToString();

        //注册控制器
        protected void RegisterController(string eventName, Type controllerType)
        {
            RegisterController(eventName, controllerType);
        }

        protected void RegisterController<TA>(object eventName)
        {
            RegisterController($"{eventName}", typeof(TA));
        }

        protected void RegisterController<TA>(Type controllerType)
        {
            RegisterController(typeof(TA).Name, controllerType);
        }

        protected void RegisterController<TA, TV>()
        {
            RegisterController(typeof(TA).Name, typeof(TV));
        }

        protected void RegisterView(IView view)
        {
            RegisterView(view);
        }

        //发送事件
        protected void SendEvent(string eventName, object data = null)
        {
            Core_1.SendEvent(eventName, this, data);
        }

        protected void SendEvent(object eventName, object data = null)
        {
            Core_1.SendEvent(eventName.GetType().FullName + "." + eventName, this, data);
        }

        protected void SendEvent<TE>(object data = null)
        {
            Core_1.SendEvent(typeof(TE).Name, this, data);
        }
    }

    //public abstract class ApplicationBase<T> : SingletonView<T> where T : ApplicationBase<T> { }
}