using GameEngine.Contacts;
using GameEngine.Kernel.Attributes;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using SQLite.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using UniRx.Async;
using UnityEngine;

#pragma warning disable 1998

namespace GameEngine.Kernel {

public class Controller<T> : Model<T>, IController where T : Controller<T>, new() {

    // 视图层关心的事件列表
    [HideInInspector, Ignore]
    public List<string> attentionEvents { get; set; } = new List<string>();

    [OdinSerialize]
    protected string blobStates { get; set; }

    //执行事件
    public virtual void Execute(object data) { }

    // protected virtual async UniTask Awake() {
    //     if(m_Instance == null) {
    //         m_Instance = (T)this;
    //     }
    //
    //     Debug.Log($"{typeof(T).Name} awake");
    // }

    // [ RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad) ]
    // public static async UniTask LoadDefaultInstance() {
    //     // m_Instance = await Addressables.LoadAssetAsync<T>($"Config/{typeof(T).Name}.asset").Task;
    //
    //     if(m_Instance == null) {
    //         Addressables.InitializeAsync().Completed += res => { };
    //         await Addressables.InitializeAsync().Task;
    //         m_Instance = await Addressables.LoadAssetAsync<T>($"Config/{typeof(T).Name}.asset")
    //             .Task;
    //         Debug.Log($"{Instance.GetType().Name} loaded");
    //     }
    // }

    // 注册视图关心的事件
    public virtual void RegisterViewEvents()
    {
        GetType()
            .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
            .ForEach(info => {
                info.GetCustomAttributes<Events>()
                    .ForEach(attr => {
                        Debug.Log(
                            $"Events Add: <color=green>{attr.EventName}</color> View: <color=blue>{GetType().FullName}</color>");

                        attentionEvents.Add(attr.EventName);
                    });
            });
    }

    [Ignore]
    public IProvider Provider { get; set; }

    [ShowInInspector, OdinSerialize, TextBlob(nameof(blobStates))]
    public List<IState> States { get; set; }

    public virtual async UniTask InitController(IProvider provider)
    {
        Provider = provider;

        foreach (var state in States) {
            state.Provider = provider;
        }
    }

    //注册模型
    protected void RegisterModel(IModel model)
    {
        Core.RegisterModel(model);
    }

    protected void RegisterModel<TM>() where TM : ScriptableObject, IModel
    {
        //Core.RegisterModel(CreateInstance<TM>());
    }

    //注册视图
    protected void RegisterView(IView view)
    {
        Core.RegisterView(view);
    }

    //public abstract void Execute(object data);

    //注册控制器
    protected void RegisterController(string eventName, Type controllerType)
    {
        Core.RegisterController(eventName, controllerType);
    }

    protected void RegisterController<TE>(Type controllerType)
    {
        Core.RegisterController(typeof(TE).Name, controllerType);
    }

    //获取模型
   // protected TA GetModel<TA>() where TA : IModel => Core.GetModel<TA>();

    //获取视图
    //protected TA GetView<TA>() where TA : IView => Core.GetView<TA>();

    public override string ToString() => GetType().ToString();

    // private IState currentState;
    //
    // public IState State {
    //     get { return currentState; }
    // }

    // public Dictionary<Type, IState> States = new Dictionary<Type, IState>();
    //
    // //Changes the current game state
    // public void SetState(System.Type newStateType) {
    //     currentState?.OnDeactivate();
    //
    //     States.TryGetValue(newStateType, out currentState);
    //
    //     currentState?.OnActivate();
    // }
    //
    // public void OnActivate() {
    //
    // }
    //
    // public void OnDeactivate() {
    // }
    //
    // public void OnUpdate() {
    //

}

#if false
    /// <summary>
    /// 控制层
    /// </summary>
    public abstract class Controller : Config<Controller>
    {
        //注册模型
        protected void RegisterModel(IModel model) {
            Core.RegisterModel(model);
        }

        //注册视图
             protected void RegisterView(IView view) {
            Core.RegisterView(view);
        }

        //注册控制器
        protected void RegisterController(string eventName, Type controllerType) {
            Core.RegisterController(eventName, controllerType);
        }

        //执行事件
        public abstract void Execute(object data);

        //获取模型
        protected T GetModel<T>() where T : Model {
            return Core.GetModel<T>();
        }

        //获取视图
        protected T GetView<T>() where T : View {
            return Core.GetView<T>();
        }
    }
#endif

}