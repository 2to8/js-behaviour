using GameEngine.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using DG.Tweening.Plugins.Core.PathCore;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine.Assertions;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;
using Random = System.Random;

public static partial class Core
{
    static BindingFlags fullBinding = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
                                      BindingFlags.Static | BindingFlags.FlattenHierarchy;

    static readonly StringComparison ignoreCase = StringComparison.CurrentCultureIgnoreCase;
    public static Random random => Rnd;

    public static Type GetTypeFromAllAssemblies(string typeName)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies) {
            var types = assembly.GetTypes();
            foreach (var type in types) {
                if (type.Name.Equals(typeName, ignoreCase) || type.Name.Contains('+' + typeName))

                    //+ check for inline classes
                {
                    return type;
                }
            }
        }

        return null;
    }

    public static IEnumerable<Type> FindTypes(Func<Type, bool> predicate = null)
    {
        // if (predicate == null) {
        //     throw new ArgumentNullException(nameof(predicate));
        // }
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()
            .Where(assembly => !(assembly.ManifestModule is System.Reflection.Emit.ModuleBuilder))) {
            if (!assembly.IsDynamic) {
                Type[] exportedTypes = null;
                try {
                    exportedTypes = assembly.GetExportedTypes();
                }
                catch (ReflectionTypeLoadException e) {
                    exportedTypes = e.Types;
                }

                if (exportedTypes != null) {
                    foreach (var type in exportedTypes) {
                        if (predicate == null || predicate(type)) {
                            yield return type;
                        }
                    }
                }
            }
        }
    }

    public static long TimeStamp() => DateTime.Now.GetTimeStampJS();

    // {
    //     var startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)); // 当地时区
    //     var timeStamp = (long)(DateTime.Now - startTime).TotalSeconds;                  // 相差秒数
    //     return timeStamp;
    // }
    public static DateTime fromTimeStamp(long Milliseconds = 0) => Milliseconds.ConvertTimestampJS();

    // {
    //     long unixTimeStamp = 1478162177;
    //     var startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)); // 当地时区
    //     var dt = startTime.AddSeconds(unixTimeStamp + seconds);
    //     return dt;
    // }

    public static string GetMD5Hash(string input)
    {
        // 创建一个MD5CryptoServiceProvider对象的新实例。
        var md5Hasher = MD5.Create();

        // 将输入的字符串转换为一个字节数组并计算哈希值。
        var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

        //创建一个StringBuilder对象，用来收集字节数组中的每一个字节，然后创建一个字符串。
        var sBuilder = new StringBuilder();

        // 遍历字节数组，将每一个字节转换为十六进制字符串后，追加到StringBuilder实例的结尾
        for (var i = 0; i < data.Length; i++) {
            sBuilder.Append(data[i].ToString("x2"));
        }

        // 返回一个十六进制字符串
        return sBuilder.ToString();
    }

    public static object GetInstance(Type t)
    {
        if (!t.IsAbstract) {
            var func = t.GetMethod("GetInstance",
                BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
            if (func == null) {
                Debug.Log($"{t.Name} instance is null");
                return default;
            }

            return func?.Invoke(null, null);
        }

        return default;
    }

    public static T GetInstance<T>() => (T) GetInstance(typeof(T));

    public static T RandomEnumValue<T>(Func<T, bool> action = null)
    {
        var v = Enum.GetValues(typeof(T));
        while (true) {
            var t = (T) v.GetValue(random.Next(v.Length));
            if (action == null || action(t)) {
                return t;
            }
        }
    }

    public static T RandomElement<T>(this IEnumerable<T> enumerable) => enumerable.RandomElementUsing(random);

    public static T RandomElementUsing<T>(this IEnumerable<T> enumerable, Random rand)
    {
        var enumerable1 = enumerable as T[] ?? enumerable.ToArray();
        var index = rand.Next(0, enumerable1.Count());
        return enumerable1.ElementAt(index);
    }

    public static T Ramdom<T>(this IEnumerable<T> sequence, Func<T, bool> action = null)
    {
        // argument null checking omitted
        var selection = sequence as T[] ?? sequence.ToArray();
        var randomSelection = selection;
        if (action != null) {
            var a = selection.Where(action);
            randomSelection = a as T[] ?? a.ToArray();
        }

        return randomSelection.ElementAtOrDefault(random.Next() % randomSelection.Count());
    }

    public static GameObject InstPrefab(this GameObject prefab, Transform parent = null)
    {
#if UNITY_EDITOR

        //if (!Application.isPlaying) {
        return PrefabUtility.InstantiatePrefab(prefab.GetPrefabRoot(), parent) as GameObject;

        // }
#endif
        return Object.Instantiate(prefab, parent);
    }

    public static Camera MainCamera(this Component component) => Camera.main;

    // public static T Instantiate<T>(this T prefab, Transform parent = null) where T: Object
    // {
    // #if UNITY_EDITOR
    //
    //     //if (!Application.isPlaying) {
    //     return PrefabUtility.InstantiatePrefab(prefab, parent) as T;
    //
    //     //}
    // #endif
    //     return Object.Instantiate(prefab, parent);
    // }

    public static GameObject GetPrefabRoot(this GameObject gameObject)
    {
#if UNITY_EDITOR
        var go = PrefabUtility.GetOutermostPrefabInstanceRoot(gameObject);
        if (go != null) {
            go = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GetAssetPath(go));
            if (go != null) {
                return go;
            }
        }

        var path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(gameObject);
        if (!string.IsNullOrEmpty(path)) {
            go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (go != null) {
                return go;
            }
        }

        // path = AssetDatabase.GetAssetPath(PrefabUtility.FindPrefabRoot(gameObject))
        return PrefabUtility.GetNearestPrefabInstanceRoot(gameObject) ??
               PrefabUtility.GetCorrespondingObjectFromSource(gameObject) ?? gameObject;
#endif
        return gameObject;
    }

    public static bool IsPrefab(this GameObject go)
    {
#if UNITY_EDITOR
        return go != null && (PrefabUtility.IsPartOfPrefabAsset(go) || PrefabStageUtility.GetPrefabStage(go) != null ||
                              PrefabUtility.IsPartOfPrefabInstance(go));
#endif
        return false;
    }

    public static T Instantiate<T>(this T prefab, Transform parent = null) where T : UnityEngine.Object
    {
#if UNITY_EDITOR
        Assert.IsNotNull(prefab, "prefab != null");
        if ((prefab is GameObject _go && _go.IsPrefab()) || (prefab is Component comp && comp.gameObject.IsPrefab())) {
            // var root = PrefabUtility.GetOutermostPrefabInstanceRoot(prefab);
            //
            // if (root != null) {
            //     var go = PrefabUtility.InstantiatePrefab(root, parent) as GameObject;
            //
            //     return go.GetComponent<T>();
            // }
            var path = AssetDatabase.GetAssetPath(prefab);
            if (string.IsNullOrEmpty(path)) {
                path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(prefab);
            }

            var root = !string.IsNullOrEmpty(path)
                ? AssetDatabase.LoadAssetAtPath<GameObject>(path)
                : PrefabUtility.GetOutermostPrefabInstanceRoot(prefab);
            if (root == null) {
                root = PrefabUtility.FindPrefabRoot(prefab is Component component
                    ? component.gameObject
                    : prefab as GameObject);
            }

            if (root != null) {
                var go = PrefabUtility.InstantiatePrefab(root, parent);
                if (go as T != null) return (T) go;
                var ret = typeof(T).IsAssignableFrom(typeof(Component)) && go is GameObject gameObject
                    ? gameObject.GetComponent<T>()
                    : go as T;
                if (ret != null) return ret;
            }
            //return PrefabUtility.InstantiatePrefab(prefab, parent) as T;
        }

#endif
        return Object.Instantiate(prefab, parent);
    }

    public static GameObject Instantiate(this GameObject prefab, Vector3 position, Quaternion rotation,
        Transform parent = null)
    {
#if UNITY_EDITOR
        if (prefab.IsPrefab()) {
            //if (!Application.isPlaying) {
            var go = PrefabUtility.InstantiatePrefab(prefab.GetPrefabRoot(), parent) as GameObject;
            go.transform.rotation = rotation;
            go.transform.position = position;
            return go;
        }

        //}
#endif
        return Object.Instantiate(prefab, position, rotation, parent);
    }

    public static T Instantiate<T>(this GameObject prefab, Vector3 position, Quaternion rotation,
        Transform parent = null) where T : Object =>
        (T) Instantiate((Object) prefab.GetPrefabRoot(), position, rotation, parent);

    public static T Instantiate<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        where T : Object
    {
#if UNITY_EDITOR
        if ((prefab is GameObject _go && _go.IsPrefab()) || (prefab is Component comp && comp.gameObject.IsPrefab())) {
            //if (Application.isEditor) {
            var go = PrefabUtility.InstantiatePrefab(prefab, parent);
            if (go is Component c) {
                c.transform.rotation = rotation;
                c.transform.position = position;
            }
            else if (go is GameObject g) {
                g.transform.rotation = rotation;
                g.transform.position = position;
            }

            return (T) go;
        }

        //}
#endif
        return Object.Instantiate(prefab, position, rotation, parent);
    }

    public static void BindComponent<T>() where T : Component
    {
#if UNITY_EDITOR
        var added = false;
        Selection.activeGameObject?.RequireComponent<T>(ref added);

        // if(UnityEditor.Selection.activeGameObject is GameObject go) {
        //     if()
        // }
        if (added) {
            Debug.Log($"added {typeof(T).FullName}");
        }
#endif
    }

    public static Scene CurrentScene()
    {
#if UNITY_EDITOR
        var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
        if (prefabStage != null) {
            return prefabStage.scene;
        }
#endif
        return SceneManager.GetActiveScene();
    }

    public static void SetSceneDirty()
    {
        Debug.Log("[SetDirty]");
#if UNITY_EDITOR
        var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();

        //PrefabStageUtility.GetPrefabStage(YourGameObject);
        if (prefabStage != null) {
            EditorSceneManager.MarkSceneDirty(prefabStage.scene);
            Debug.Log("set dirty");
        }
        else {
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            Debug.Log("set dirty");
        }
#endif
    }

    public static bool DebugDialog(string msg)
    {
#if UNITY_EDITOR
        // if (!Application.isPlaying && !EditorUtility.DisplayDialog("debug", msg, "继续", "停止")
        //     /* EditorUtility.DisplayCancelableProgressBar("debug", msg, 10f)*/) {
        //     return true;
        // }
        Debug.Log(msg);
#endif
        return false;
    }

    public static bool Dialog(object msg, bool check = true)
    {
        var trace = new StackTrace(true).GetFrames().Where(frame => frame.GetFileName() != null).Skip(1).Take(1).Select(
                frame =>
                    $"{frame.GetFileName()?.Replace('\\', '/').Replace(Application.dataPath, "Assets")}:{frame.GetFileLineNumber()} {frame.GetMethod().Name}()")
            .Join("\n");
#if UNITY_EDITOR
        if (!Application.isPlaying && check) {
            return EditorUtility.DisplayDialog("debug", $"{msg}\n\n{trace}", "中断", "继续");
        }

#endif
        Debug.Log(msg);
        return false;
    }

    public static void ReloadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene()
            .name);
    }
}