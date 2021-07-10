using GameEngine.Contacts;
using GameEngine.Kernel.Attributes;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UniRx.Async;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.UI;

#pragma warning disable 1998

namespace GameEngine.Kernel {

public class View<T> : ApplicationBase<T>, IView where T : View<T> {

    [FoldoutGroup("Core"), ShowInInspector]
    public List<IController> controllers { get; set; } = new List<IController>();

    protected override async UniTask Awake()
    {
        await base.Awake();
        await RegisterControllers();
        RegisterView(this);
    }

    public bool AddressableResourceExists(object key)
    {
        foreach (var l in Addressables.ResourceLocators) {
            IList<IResourceLocation> locs;

            if (key != null && l.Locate(key, typeof(ScriptableObject), out locs)) {
                return true;
            }
        }

        return false;
    }

    public void RegisterController(IController instance)
    {
        if (instance != null && !controllers.Contains(instance)) {
            controllers.Add(instance);
        }
    }

    /// <summary>
    ///     todo: 需要放在 base.awake() 前面
    /// </summary>
    /// <param name="instance"></param>
    /// <typeparam name="TC"></typeparam>
    /// <returns></returns>
    protected async Task RegisterController<TC>(IController instance = null) where TC : ScriptableObject, IController
    {
        if (instance == null && AddressableResourceExists($"Config/{typeof(TC).Name}.asset")) {
            instance = await Addressables.LoadAssetAsync<TC>($"Config/{typeof(TC).Name}.asset").Task;
            Debug.Log($"Config/{typeof(TC).Name}.asset loaded");
        }

        if (instance == null) {
            instance = ScriptableObject.CreateInstance<TC>();
        }

        if (instance != null && !controllers.Contains(instance)) {
            controllers.Add(instance);
        }

        if (instance != null) {
            instance.Provider = GetComponentInParent<IProvider>();
        }
    }

    protected virtual async UniTask RegisterControllers() { }

    // 视图标识
    [HideInInspector]
    public virtual string Name { get; set; } = typeof(T).Name;

#if XLUA
        [FoldoutGroup("Core")]  [ ShowInInspector, OdinSerialize ]
        public LuaProxy LuaScript { get; set; }
#endif

    // 视图层关心的事件列表
    [HideInInspector]
    public List<string> attentionEvents { get; set; } = new List<string>();

    // 注册视图关心的事件
    //public virtual void RegisterViewEvents() { }

    // 注册视图关心的事件
    public virtual void RegisterViewEvents()
    {
        GetType()
            .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .ForEach(info => {
                info.GetCustomAttributes<Events>(true)
                    .ForEach(t => {
                        var v = info.GetValue(this);
                        var btn = v as Button ?? GetComponent<Button>() ?? GetComponentInParent<Button>();

                        btn?.onClick.AddListener(() => {
                            Debug.Log("button clicked");

                            if (!(this is IState) || this is IState state && state == state.Provider.State) {
                                SendEvent(t.EventName);
                            }
                        });
                    });
            });

        GetType()
            .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
            .ForEach(info => {
                info.GetCustomAttributes<Events>()
                    .ForEach(attr => {
                    #if LOG_STATE
                        Debug.Log( $"Events Add: <color=green>{attr.EventName}</color> View: <color=blue>{GetType().FullName}</color>");
                    #endif
                        if (!attentionEvents.Contains(attr.EventName)) {
                            attentionEvents.Add(attr.EventName);
                        }
                    });
            });

        controllers.ForEach(ctx => {
            ctx.GetType()
                .GetMethods(BindingFlags.NonPublic |
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.FlattenHierarchy)
                .ForEach(info => {
                    info.GetCustomAttributes<Events>()
                        .ForEach(attr => {
                        #if LOG_STATE || true
                            Debug.Log(
                                $"Events Add: <color=green>{attr.EventName}</color> View: <color=blue>{GetType().FullName}</color>");
                        #endif
                            if (!attentionEvents.Contains(attr.EventName)) {
                                attentionEvents.Add(attr.EventName);
                            }
                        });
                });
        });
    }

    public override string ToString() => GetType().ToString();

    //获取模型
    //protected IModel GetModel<TA>() where TA : IModel => Core.GetModel<TA>();

    //发送消息

    // 视图层事件处理
    // public virtual void HandleEvent(string eventName, object data) {
    //     Events.Dispatch(eventName, this, null, data);
    // }

    // 视图层事件处理
    public virtual void Handle(string eventName, object sender, object data)
    {
        Events.Dispatch(eventName, this, sender, data);
    }

}

#if false
    /// <summary>
    /// 视图层
    /// </summary>
    public  abstract class View : ApplicationBase<View> {

        // 视图标识
        [HideInInspector]
        public abstract string Name { get; }

        // 视图层关心的事件列表
        [HideInInspector]
        public List<string> attentionEvents = new List<string>();

        // 注册视图关心的事件
        public virtual void RegisterViewEvents()
        {

        }

        //获取模型
        protected IModel GetModel<T>() where T:IModel
        {
            return Core.GetModel<T>();
        }

        //发送消息
        protected void SendEvent(string eventName,object data = null)
        {
            Core.SendEvent(eventName, data);
        }

        // 视图层事件处理
        public abstract void HandleEvent(string eventName,object data);
        public abstract void Handle(string eventName, object sender, object data);
    }
#endif

}