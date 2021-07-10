using GameEngine.Contacts;
using GameEngine.Kernel.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static partial class Core
{
    //名字---模型
    // static Dictionary<string, IModel> models = new Dictionary<string, IModel>();

    //名字---视图
    // static Dictionary<IView, GameObject> views = new Dictionary<IView, GameObject>();

    //事件名字--控制器类型
    // static Dictionary<string, Type> commandMap = new Dictionary<string, Type>();
    static readonly Dictionary<Type, IController> Controllers = new Dictionary<Type, IController>();

    //注册模型
    public static void RegisterModel(IModel model)
    {
        //models[model.GetType().Name] = model;
    }

    //注册视图
    public static void RegisterView(IView view)
    {
        // if (view == null || views.ContainsKey(view)) {
        //     return;
        // }

        //if (views.ContainsKey(view.Name)) return;
        //views[view] = (view as Component)?.gameObject;
        view.RegisterViewEvents(); //注册关心的事件
    }

    //注册控制器
    // public static void RegisterController(string eventName, Type controllerType)
    // {
    //     commandMap[eventName] = controllerType;
    // }

    public static void SendEvent(object eventName, object data = null)
    {
        SendEvent(eventName.GetType().FullName + ":" + eventName, null, data);
        Debug.Log($"Event: {eventName.GetType().FullName + ":" + eventName}");
    }

    //发送事件
    // public static void SendEvent(string eventName, object data = null)
    // {
    //     SendEvent(eventName, null, data);
    // }

    public static void RegisterController<T, TV>()
    {
        commandMap[typeof(T).Name] = typeof(TV);
    }

    public static void SendEvent<T>(object sender, object data = null)
    {
        SendEvent(typeof(T).Name, sender, data);
    }

    public static IController GetController(Type t)
    {
        //return World.Active.GetOrCreateSystem(t) as SystemBase;
        Controllers.TryGetValue(t, out var controller);

        return controller;
    }

    public static T GetController<T>() where T : IController, new() => GetController<T>(typeof(T));

    public static T GetController<T>(Type type) where T : IController, new()
    {
        Controllers.TryGetValue(type, out var controller);

        if (controller == null) {
            controller = (T) Activator.CreateInstance(type);
            Controllers[type] = controller;
        }

        return (T) controller;
    }

    //发送事件
    public static void SendEvent(string eventName, object sender, object data = null)
    {
        //控制层
        Debug.Log($"SendEvent: <color=green>{eventName}</color> from {sender?.GetType().FullName}");
        commandMap.TryGetValue(eventName, out var t);

        if (t != null) {
            var commandInstance = GetController(t);

            //GetController(t);
            if (commandInstance != null) {
                Debug.Log($"Excute Controller: <color=green>{t.FullName}</color>");
                commandInstance.Execute(data);

                // 控制器事件处理
                Events.Dispatch(eventName, commandInstance, sender, data);
            }
        }

        //视图层
        // foreach (var v in views.Keys.ToList()) {
        //     if (v == null || v as MonoBehaviour == null || (v as MonoBehaviour).enabled == false) {
        //         continue;
        //     }
        //
        //     if (v.attentionEvents.Contains(eventName)) {
        //         // if(v is IState) {
        //         //     Debug.Log(
        //         //         $"{(v as MonoBehaviour).gameObject.name} state: {v.Name} ,{(v as IState).m_Provider.State}");
        //         // }
        //
        //         if (v is IState state && state.Provider?.State != v) {
        //             continue;
        //         }
        //
        //         Debug.Log($"Excute {sender?.GetType().Name}: <color=green>{eventName}</color>");
        //         v.Handle(eventName, sender, data);
        //     }
        // }
    }

    // //获取模型
    // public static T GetModel<T>() where T : IModel
    // {
    //     foreach (var m in models.Values) {
    //         if (m is T model) {
    //             return model;
    //         }
    //     }
    //
    //     return default;
    // }
    //
    // //获取视图
    // public static T GetView<T>() where T : IView
    // {
    //     foreach (var v in views.Keys) {
    //         if (v is T view) {
    //             return view;
    //         }
    //     }
    //
    //     return default;
    // }

    public static void Quit()
    {
        // save any game data here
#if UNITY_EDITOR
        if (Application.isEditor) {
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;

            return;
        }

#endif
        Application.Quit();
    }
}