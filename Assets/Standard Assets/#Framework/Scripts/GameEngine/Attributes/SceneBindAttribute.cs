using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GameEngine.Extensions;
using Sirenix.Utilities;
#if UNITY_EDITOR
using Consts;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Tetris
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class SceneBindAttribute : Attribute
    {
        public string SceneName { get; private set; }
        public SceneBindAttribute() { }
        public Type Type;
        public List<string> Tags = new List<string>();

        public SceneBindAttribute(object name)
        {
            this.SceneName = $"{name}";
        }

        public static Assembly GetAssemblyByName(string name = "Assembly-CSharp")
        {
            return AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == name);
        }

        public SceneBindAttribute(object name, Type component = null, params object[] tags)
        {
            SceneName = $"{name}";
            Type = component;
            tags.ForEach(t => Tags.AddOnce($"{t}"._TagKey()));
        }

#if UNITY_EDITOR
        [DidReloadScripts, InitializeOnLoadMethod]
        [MenuItem("Debug/Bind ViewManager")]
        static void AutoBindViewManager()
        {
            if (PrefabStageUtility.GetCurrentPrefabStage() != null) return;
            var scene = SceneManager.GetActiveScene();
            if (!Enum.GetNames(typeof(SceneName)).Contains(scene.name)) return;
            Debug.Log("checking AutoBindViewManager");
            GetAssemblyByName().GetExportedTypes().Where(type =>
                type.IsDefined<SceneBindAttribute>() && !(type.IsAbstract || type.IsGenericType) /*&&
                type.IsAssignableFrom(typeof(Component))*/).ForEach(type => {
                //
                var attribute = type.GetCustomAttribute<SceneBindAttribute>();
                if (attribute.SceneName != scene.name) return;
                if (Object.FindObjectOfType(type) != null) return;
                Debug.Log(scene.name);
                var viewGo = GameObject.Find($"/{scene.name}/{type.Name}") ?? new GameObject(type.Name).Of(go =>
                    go.transform.SetParent((GameObject.Find("/" + scene.name) ?? new GameObject(scene.name))
                        .transform));
                viewGo.RequireComponent(type);
                EditorSceneManager.MarkSceneDirty(scene);
                //
                Debug.Log($"{type.GetNiceFullName()} bind to {scene.name}");
            });
        }
#endif
    }
}