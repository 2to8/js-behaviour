using GameEngine.Models.Contracts;
using GameEngine.Views.Contracts;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace GameEngine.Controllers.Contracts {

[Serializable]
public abstract class ControllerBase : SerializedStateMachineBehaviour {

    //注册模型
    protected void RegisterModel(ScriptableObject model)
    {
        Core.RegisterModel(model);
    }

    protected void RegisterModel<T>() where T : DbTable<T>, new()
    {
        Core.RegisterModel(DbTable<T>.Create());
    }

    //注册视图
    protected void RegisterView(View view)
    {
        Core.RegisterView(view);
    }

    //注册控制器
    protected void RegisterController(string eventName, Type controllerType)
    {
        Core.RegisterController(eventName, controllerType);
    }

    protected void RegisterController<T>(object eventName)
    {
        Core.RegisterController(eventName, typeof(T));
    }

    protected void RegisterController(object eventName, Type controllerType)
    {
        Core.RegisterController(eventName.GetType().FullName + "." + eventName, controllerType);
    }

    //执行事件
    public virtual void Execute(object data) { }

    //获取模型
    protected T GetModel<T>() where T : DbTable<T>, new() => Core.GetModel<T>();

    //获取视图
    protected T GetView<T>() where T : View => Core.GetView<T>();

}

}