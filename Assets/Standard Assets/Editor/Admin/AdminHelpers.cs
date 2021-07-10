using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Consts;
using GameEngine.Extensions;
using GameEngine.Models.Contracts;
using MS.Shell.Editor;
using Sirenix.Utilities;
using Tetris;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Admin
{
    public class AdminHelpers
    {
        [MenuItem("Debug/Set Version")]
        [InitializeOnLoadMethod]
        static async void SetCurrentVersion()
        {
            var operation = EditorShell.Execute("wsl.exe git describe 2>&1");
            operation.onExit += delegate { };
            operation.onLog += (EditorShell.LogType logType, string log) => {
                string newVersion = log;
                if (newVersion != PlayerSettings.bundleVersion) {
                    Debug.Log($"New Version: {log}");
                    PlayerSettings.bundleVersion = newVersion;
                    AssetDatabase.SaveAssets();
                }
            };
            int exitCode = await operation; //support async/await
        }

        public static Assembly GetAssemblyByName(string name = "Assembly-CSharp")
        {
            return AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == name);
        }

#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        [MenuItem("Debug/Bind DB Table")]
        static void AutoBindDBTable()
        {
            // if (PrefabStageUtility.GetCurrentPrefabStage() != null) return;
            // var scene = SceneManager.GetActiveScene();
            // if (!Enum.GetNames(typeof(SceneName)).Contains(scene.name)) return;
            Debug.Log("checking AutoBindDBTable".ToRed());
            GetAssemblyByName().GetExportedTypes().Where(type =>
                /*type.IsDefined<SceneBindAttribute>() &&*/
                !(type.IsAbstract || type.IsGenericType) && typeof(TableBase).IsAssignableFrom(type)).ForEach(type => {
                Debug.Log(type.FullName);
                Core.FindOrCreatePreloadAsset(type);
                //
                // var attribute = type.GetCustomAttribute<SceneBindAttribute>();
                // if (attribute.SceneName != scene.name) return;
                // if (Object.FindObjectOfType(type) != null) return;
                // Debug.Log(scene.name);
                // var viewGo = GameObject.Find($"/{scene.name}/{type.Name}") ?? new GameObject(type.Name).Of(go =>
                //     go.transform.SetParent((GameObject.Find("/" + scene.name) ?? new GameObject(scene.name))
                //         .transform));
                // viewGo.RequireComponent(type);
                // EditorSceneManager.MarkSceneDirty(scene);
                // //
                // Debug.Log($"{type.GetNiceFullName()} bind to {scene.name}");
            });
        }
#endif
    }
}