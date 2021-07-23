using GameEngine.Controllers.Contracts;
using GameEngine.Kernel;
using GameEngine.Models.Contracts;
using Puerts.Attributes;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR

// using UnityEditor;
// using UnityEditor.Animations;
#endif

namespace GameEngine.Views.Contracts {

/// <summary>
///     视图层
/// </summary>
public abstract class BaseView : ViewBase {

#if UNITY_EDITOR
    [FoldoutGroup("Controller"), PuertsIgnore]
    public UnityEditor.Animations.AnimatorController animatorController;
#endif

    // 视图层关心的事件列表
    [HideInInspector]
    public List<string> attentionEvents = new List<string>();

    [OdinSerialize, FoldoutGroup("Controller")]
    public virtual Controller controller { get; set; }

    [OdinSerialize, ShowInInspector, FoldoutGroup("Controller")]
    string ControllerFilename { get; set; }

    // 视图标识
    [HideInInspector]
    public virtual object ViewName { get; }

    protected override void Start()
    {
        base.Start();
        controller?.SetState();
    }

    //Changes the current game state
    protected override void Update()
    {
        base.Update();
        controller?.State?.OnUpdate();
    }
#if UNITY_EDITOR
    [FoldoutGroup("Controller"), Button(Name = "Object Type"), ButtonGroup("Controller/Btn"), PuertsIgnore]
    void CreateAnimatorController()
    {
        if (ControllerFilename.IsNullOrWhitespace()) {
            ControllerFilename = Core.NewGuid().Substring(0, 8);
        }

        GetOrCreateAnimControl(GetType(), "Controllers", ControllerFilename);
    }

    [PuertsIgnore]
    UnityEditor.Animations.AnimatorController GetOrCreateAnimControl(Type type, string Start = null,
        string filename = null)
    {
        var pre = type.Namespace?.Split('.').First();

        var parts = new List<string> {
            pre,
            type.Name,
            filename,
        };

        var prefix = (Start.IsNullOrWhitespace() ? string.Empty : Start + "/") +
            string.Join("_", parts.Where(s => !s.IsNullOrWhitespace()));

        var path = Directory.Exists($"{Application.dataPath}/{pre}") ? $"{pre}/Resources" : "Resources";
        animatorController = Resources.Load<UnityEditor.Animations.AnimatorController>(prefix);

        if (animatorController == null) {
            var assetPath = $"Assets/{path}/{prefix}.controller";

            var dir = Path.GetDirectoryName($"{Path.GetDirectoryName(Application.dataPath)}/{assetPath}") ?? "";

            if (!Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
            }

            animatorController = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath(assetPath);

            // AssetDatabase.CreateAsset(asset, assetPath);
            // AssetDatabase.SaveAssets();
            //
            UnityEditor.AssetDatabase.Refresh();

            //animatorController = AssetDatabase.LoadAssetAtPath<AnimatorController>(assetPath);
        }

        //EditorUtility.SetDirty(animatorController);

        //Selection.SetActiveObjectWithContext(ac, null);
        // AssetDatabase.OpenAsset(animatorController);
        // Selection.SetActiveObjectWithContext(animatorController,null);
        UnityEditor.Selection.activeObject = animatorController;

        return animatorController;
    }

    [FoldoutGroup("Controller"), Button(Name = "Remove"), ButtonGroup("Controller/Btn"), PuertsIgnore]
    void RemoveAnimatorController()
    {
        CreateAnimatorController();
        var path = UnityEditor.AssetDatabase.GetAssetPath(animatorController);
        UnityEditor.AssetDatabase.DeleteAsset(path);
        UnityEditor.AssetDatabase.Refresh();
        ControllerFilename = string.Empty;
        LinkAnimatorController();

        //FocusUnityEditorWindow(SelectWindowType.Scene);
    }

    [FoldoutGroup("Controller"), Button(Name = "Class Type"), ButtonGroup("Controller/Btn"), PuertsIgnore]
    void LinkAnimatorController()
    {
        GetOrCreateAnimControl(GetType(), "Controllers");
    }
#endif
    public string getName() => ViewName.GetType().FullName + "." + ViewName;

    public void SetState(Type newStateType = null)
    {
        controller?.SetState(newStateType);
    }

    // 注册视图关心的事件
    public virtual void RegisterViewEvents() { }

    //获取模型
    protected T GetModel<T>() where T : DbTable<T>, new() => Core.GetModel<T>();

    //发送消息
    protected void SendEvent(string eventName, object data = null)
    {
        Core.SendEvent(eventName, data);
    }

    protected void SendEvent(object eventName, object data = null)
    {
        Core.SendEvent(eventName.GetType().FullName + "." + eventName, data);
    }

    // 视图层事件处理
    public virtual void HandleEvent(object eventName, object data) { }

    //注册控制器
    protected void RegisterController(string eventName, Type controllerType)
    {
        Core.RegisterController(eventName, controllerType);
    }

    protected void RegisterController(object eventName, Type controllerType)
    {
        Core.RegisterController(eventName.GetType().FullName + "." + eventName, controllerType);
    }

    protected void RegisterController<T>(object eventName)
    {
        Core.RegisterController(eventName, typeof(T));
    }

    //发送事件
    // protected void SendEvent(string eventName, object data = null) {
    //     Core.SendEvent(eventName, data);
    // }
    //
    // protected void SendEvent(object eventName, object data = null) {
    //     Core.SendEvent(eventName.GetType().FullName + "." + eventName.ToString(), data);
    // }

}

}