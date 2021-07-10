using GameEngine.Extensions;
using Sirenix.Utilities;
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameEngine.Utils {

public static class ResHelper {

    public static T Load<T>(object target = null, Func<string, T> action = null) where T : Object
    {
        var isComponent = typeof(Component).IsAssignableFrom(typeof(T)) ||
            typeof(GameObject).IsAssignableFrom(typeof(T));

        var isEnum = target != null && target.GetType() != typeof(Type) && target.GetType() != typeof(string);
        string sp;

        if (target is Type type) {
            sp = type.FullName;
        } else {
            sp = target is string str
                ? str
                : (target == null ? typeof(T).FullName : target.GetType().FullName) ?? "Games";
        }

        sp = sp.Replace("+", "-");

        // var type   =   :  target.GetType();
        var prefix = sp.Split('.').First();

        // if ( target != null ) {
        //     prefix = target.GetType().Name + target;
        //     type   = target.GetType();
        // }
        var parts = sp.Split('.');
        var suffix = "Settings/" + (parts?.Length > 1 ? parts[parts.Length - 2] + "_" : "");

        if (isComponent) {
            suffix = "Prefabs/" + parts[parts.Length - 1].Replace("+", "_") + "_";
        }

        var fileName = (suffix + (isEnum ? target : parts.Last())).Replace("+", "-").Replace(".", "_");
        var instance = Resources.Load<T>($"{prefix}/{fileName}");
    #if UNITY_EDITOR

        if (instance == null) {
            var cd = Path.GetDirectoryName(Application.dataPath);
            var path = $"{Application.dataPath}/{prefix}";

            if (!Directory.Exists(path)) {
                var dir = new DirectoryInfo(Application.dataPath);

                foreach (var dChild in dir.GetDirectories("*")) {
                    var tmp = $"{Application.dataPath}/{dChild.Name}/{prefix}";

                    if (Directory.Exists(tmp) && dChild.Name != "Resources") {
                        path = tmp;

                        break;
                    }
                }
            }

            if (!Directory.Exists(path)) {
                path = $"Assets/Resources/{prefix}";
            } else {
                path = path.Replace(Application.dataPath, "Assets") + $"/Resources/{prefix}";
            }

            //Directory.GetCurrentDirectory();
            // path = Directory.Exists($"{cd}/Assets/{prefix}")
            //     ? $"Assets/{prefix}/Resources/{prefix}"
            //     : $"Assets/Resources/{prefix}";

            // if ( !Directory.Exists($"{cd}/{path}") ) {
            //     Directory.CreateDirectory($"{cd}/{path}");
            // }
            var ext = isComponent ? ".prefab" : ".asset";
            var assetName = $"{path}/{fileName}{ext}";

            if (isEnum) {
                assetName = $"{path}/{fileName}{ext}";
            }

            var cdir = Path.GetDirectoryName(Path.Combine(Path.GetDirectoryName(Application.dataPath), assetName));

            if (!Directory.Exists(cdir)) {
                Directory.CreateDirectory(cdir);
            }

            if (isComponent) {
                var gameObject = new GameObject(typeof(T).Name);

                if (typeof(Component).IsAssignableFrom(typeof(T))) {
                    gameObject.AddComponent(typeof(T));
                }

                PrefabUtility.SaveAsPrefabAssetAndConnect(gameObject, assetName, InteractionMode.AutomatedAction);

                Debug.Log(cdir);

                //  InteractionMode.UserAction);
                Debug.Log($"{prefix}/{fileName}");

                if (Resources.Load<T>($"{prefix}/{fileName}") == null) {
                    Debug.LogError($"create failed: {prefix}/{fileName}");
                }

                gameObject.DestroySelf();
            } else if (typeof(ScriptableObject).IsAssignableFrom(typeof(T))) {
                //m_Instance = Addressables.LoadAssetAsync<T>($"Data/Config/{typeof(T).Name}.asset").Result;
                //m_Instance = AssetDatabase.LoadAssetAtPath<T>(assetName);

                //  Resources.Load<T>("Config/" + typeof(T).Name);
                //if(m_Instance == null) {
                var asset = action?.Invoke(assetName);

                if (asset == null) {
                    asset = ScriptableObject.CreateInstance(typeof(T)) as T;
                }

                if (asset != null) {
                    Debug.Log(cdir);
                    Debug.Log($"resource name: {prefix}/{fileName}");

                    //var path = "Assets/Resources/Data";
                    //var assetPathAndName =
                    // UnityEditor.AssetDatabase.GenerateUniqueAssetPath(path + "/" + typeof(T).Name +
                    //     ".asset");
                    AssetDatabase.CreateAsset(asset, assetName);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                } else {
                    Debug.Log($"{typeof(T).GetNiceName()} Can't CreateInstance");
                }
            }

            instance = Resources.Load<T>($"{prefix}/{fileName}");

            if (instance == null) {
                Debug.Log($"{typeof(T).GetNiceName()} Can't load");
            }

            // }
        }

    #endif
        return instance;
    }

}

}