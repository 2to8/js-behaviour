using FlowCanvas.Nodes;
using GameEngine.Extensions;
using GameEngine.Kernel;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

// using Unity.Entities;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// using WebSocketSharp;
using Object = UnityEngine.Object;
using WaitForEndOfFrame = UnityEngine.WaitForEndOfFrame;

public static partial class Core
{
    public static Vector2 ScreenToRectPos(this RectTransform rectTransform, Vector2 screen_pos)
    {
        var canvas = rectTransform.GetComponentInParent<Canvas>();
        if (canvas.renderMode != RenderMode.ScreenSpaceOverlay && canvas.worldCamera != null) {
            //Canvas is in Camera mode
            Vector2 anchorPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screen_pos, canvas.worldCamera,
                out anchorPos);
            return anchorPos;
        }
        else {
            //Canvas is in Overlay mode
            Vector2 anchorPos = screen_pos - new Vector2(rectTransform.position.x, rectTransform.position.y);
            anchorPos = new Vector2(anchorPos.x / rectTransform.lossyScale.x, anchorPos.y / rectTransform.lossyScale.y);
            return anchorPos;
        }
    }

    public static IEnumerable<Scene> GetAllLoadedScenes()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++) {
            yield return SceneManager.GetSceneAt(i);
        }
    }

    static readonly List<Action> m_ReloadActions = new List<Action>();

    public static DirectoryInfo CreateNewFolder(this string folderPath)
    {
#if UNITY_EDITOR
        if (folderPath == null) {
            folderPath = AssetDatabase.GetAssetPath(Selection.activeInstanceID);
        }

        // string folderPath = AssetDatabase.GetAssetPath(Selection.activeInstanceID);
        if (folderPath.Contains(".")) {
            folderPath = folderPath.Remove(folderPath.LastIndexOf('/'));
        }

        var dir = Directory.CreateDirectory(folderPath + "/New Folder");
        AssetDatabase.Refresh();
        return dir;
#endif
        return null;
    }

    // public static T Instantiate<T>(T original, Vector3 position, Quaternion rotation) where T : Object => (T) Object.Instantiate((Object) original, position, rotation);
    //
    // public static T Instantiate<T>(
    //     T original,
    //     Vector3 position,
    //     Quaternion rotation,
    //     Transform parent)
    //     where T : Object
    // {
    //     return (T) Object.Instantiate((Object) original, position, rotation, parent);
    // }
    //
    // public static T Instantiate<T>(T original, Transform parent) where T : Object => Object.Instantiate<T>(original, parent, false);
    //
    // public static T Instantiate<T>(T original, Transform parent, bool worldPositionStays) where T : Object => (T) Object.Instantiate((Object) original, parent, worldPositionStays);
    public static void Destroy(this GameObject gameObject) => gameObject?.DestroySelf();

    // public static Component SetEcsComponent<T>(this Component component, Func<T, T> action = null)
    //     where T : struct, IComponentData
    // {
    //     var type = typeof(T).Assembly.GetType(typeof(T).FullName + "Authoring");
    //
    //     if (type == null) {
    //         return null;
    //     }
    //
    //     var ecsComponent = component.RequireComponent(type);
    //     component.RequireComponent<ConvertToEntity>().ConversionMode =
    //         ConvertToEntity.Mode.ConvertAndInjectGameObject;
    //     var data = new T();
    //
    //     if (action != null) {
    //         ecsComponent.GetType()
    //             .GetFields(BindingFlags.Public | BindingFlags.Instance)
    //             .ForEach(t => {
    //                 var field = data.GetType()
    //                     .GetField(t.Name, BindingFlags.Public | BindingFlags.Instance);
    //
    //                 if (field != null) {
    //                     field.SetValue(data, t.GetValue(ecsComponent));
    //                 }
    //             });
    //         data = action.Invoke(data);
    //     }
    //
    //     ecsComponent.GetType()
    //         .GetFields(BindingFlags.Public | BindingFlags.Instance)
    //         .ForEach(t => {
    //             var field = data.GetType()
    //                 .GetField(t.Name, BindingFlags.Public | BindingFlags.Instance);
    //
    //             if (field != null) {
    //                 t.SetValue(ecsComponent, field.GetValue(data));
    //             }
    //         });
    //
    //     return ecsComponent;
    // }

    public static GameObject PrefabRoot(this GameObject gameObject)
    {
        GameObject root = null;
#if UNITY_EDITOR
        var prefabStage = PrefabStageUtility.GetPrefabStage(gameObject);
        if (prefabStage != null) {
            root = prefabStage.prefabContentsRoot;
        }
#endif
        return root;
    }

    public static T[] FindAllOrInPrefab<T>(this GameObject gameObject) where T : Component =>
        PrefabRoot(gameObject) != null ? gameObject?.GetComponentsInChildren<T>(true) : Object.FindObjectsOfType<T>();
#if UNITY_EDITOR
    [DidReloadScripts]
    static void ClearReloadActions()
    {
        //ReloadActions.ForEach(t => t?.Invoke());
        m_ReloadActions.Clear();
    }
#endif
    public static void RunOnceOnScriptReload(ref Action refs, Action action)
    {
        if (refs == null) {
            if (action != null && !m_ReloadActions.Contains(action)) {
                action.Invoke();
                m_ReloadActions.Add(action);
            }

            refs = action;
        }
    }

    // public static ScriptableObject FindOrCreatePreloadAsset(Type type) =>
    //     FindOrCreatePreloadAsset(ScriptableObject.CreateInstance(type));

    public static ScriptableObject FindOrCreatePreloadAsset(Type type)
    {
#if UNITY_EDITOR

        // var ret = AssetDatabase.FindAssets($"t:{type.FullName}")
        //     .Select(guid => AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), type))
        //     .FirstOrDefault(t => t != null);
        var ret = PlayerSettings.GetPreloadedAssets().FirstOrDefault(t => t?.GetType() == type) as ScriptableObject ??
            CheckLoadAsset(type) as ScriptableObject;
#else
        var ret = DB.Table(ScriptableObject.CreateInstance(type)).FirstOrDefault() ;
#endif
#if UNITY_EDITOR
        if (ret == null || AssetDatabase.GetAssetPath(ret).IsNullOrEmpty()) {
            ret = ScriptableObject.CreateInstance(type);
            var configAssetDir = "Assets/Settings";
            ;
            var path = $"{configAssetDir}/{type.Name}_{Core.NewGuid()}.asset".CreateDirFromFilePath();
            AssetDatabase.CreateAsset(ret, path);
            AssetDatabase.SaveAssets();
            ret = AssetDatabase.LoadAssetAtPath(path.AssetPath(), type) as ScriptableObject;
            Assert.IsNotNull(ret, $"{type.Name} != null");
            Debug.Log($"{type.FullName} created");
        }

        if (ret != null) {
            // Add the config asset to the build
            var preloadedAssets = PlayerSettings.GetPreloadedAssets().ToList();
            if (preloadedAssets.All(t => t?.GetType() != type)) {
                Debug.Log($"add preset: {type.FullName}",ret);
                preloadedAssets.Add(ret);
                PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
                AssetDatabase.SaveAssets();
            }
            else {
                Debug.Log(
                    $"preset exists: {type.FullName} {AssetDatabase.GetAssetPath(PlayerSettings.GetPreloadedAssets().FirstOrDefault(t => t?.GetType() == type))}",
                    ret);
            }
        }

#endif
        return ret;
    }

#if UNITY_EDITOR
    public static Object CheckLoadAsset(Type type) =>
        AssetDatabase.FindAssets($"t:{type.FullName}")
            .Select(guid => AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), type))
            .FirstOrDefault(t => t != null);

    public static T CheckLoadAsset<T>() where T : Object =>
        AssetDatabase.FindAssets($"t:{typeof(T).FullName}")
            .Select(guid => AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid)))
            .FirstOrDefault(t => t != null);
#endif

    public static T FindOrCreatePreloadAsset<T>(T scriptableObject = default) where T : ScriptableObject
    {
#if UNITY_EDITOR
        var ret = PlayerSettings.GetPreloadedAssets().FirstOrDefault(t => t?.GetType() == typeof(T)) as T ??
            CheckLoadAsset<T>();
#else
            var ret = DB.Table<T>().FirstOrDefault() ;
#endif
#if UNITY_EDITOR
        if (ret == null) {
            var configAssetDir = "Assets/Settings";
            ret = DB.Table<T>().FirstOrDefault();
            var path = $"{configAssetDir}/{typeof(T).Name}.asset".CreateDirFromFilePath();
            AssetDatabase.CreateAsset(ret, path);
            AssetDatabase.SaveAssets();
            ret = AssetDatabase.LoadAssetAtPath<T>(path.AssetPath());
            Assert.IsNotNull(ret, "ret != null");
            Debug.Log($"{typeof(T).FullName} created");

            //AssetDatabase.SaveAssets();
        }

        if (ret != null) {
            // Add the config asset to the build
            var preloadedAssets = PlayerSettings.GetPreloadedAssets().ToList();
            if (preloadedAssets.All(t => t?.GetType() != null && t.GetType() != typeof(T))) {
                preloadedAssets.Add(ret);
                PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
                AssetDatabase.SaveAssets();
            }
        }

#endif
        return ret;
    }

    public static T FindInPrefabOrScene<T>() where T : Component
    {
        //var manager = Core.FindObjectOfTypeAll<T>();
        var m_Instance = FindObjectOfTypeAll<T>();
        if (m_Instance == null) {
#if UNITY_EDITOR
            var found = AssetDatabase.FindAssets("t:GameObject", new[] {"Assets", "Packages"})
                .Select(guid => AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guid)))
                .FirstOrDefault(go => go.GetComponentInChildren<T>() != null);
            if (found != null) {
                m_Instance = (PrefabUtility.InstantiatePrefab(found) as GameObject)?.GetComponentInChildren<T>();
            }
#endif
        }

        return m_Instance;
    }

    public static void ClearConsole() //you can copy/paste this code to the bottom of your script
    {
#if UNITY_EDITOR
        var assembly = Assembly.GetAssembly(typeof(Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method?.Invoke(new object(), null);
#endif
    }

    // layout 动态添加内容后需要强制刷新,否则无法自动布局
    //LayoutRebuilder.ForceRebuildLayoutImmediate(recttransform);
    //参数为挂有Layout组件的recttransform
    //    为了确保能够正确的刷新
    //建议放在一个协程中，待帧结束后检测一次，若没有刷新再执行一次
    public static IEnumerator UpdateLayout(this RectTransform rect)
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
        yield return new WaitForEndOfFrame();
        var vecScale = rect.localScale;
        var width = rect.rect.width;
        var height = rect.rect.height;
        while (rect.rect.width == 0) {
            Debug.Log(rect.rect.width);
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
            yield return new WaitForEndOfFrame();
        }
    }

    public static IEnumerable<GameObject> GetParents(this GameObject gameObject)

        // => gameObject?.GetComponentsInParent<Transform>().Select(t => t.gameObject);
    {
        var gos = new List<GameObject>();
        while (gameObject != null) {
            gos.Add(gameObject);
            gameObject = gameObject.transform.parent?.gameObject;
        }

        return gos;
    }

    public static IEnumerable<GameObject> GetParents(this Component component) => component?.gameObject.GetParents();

    public static List<PropertyAttribute> GetFieldAttributes(FieldInfo field)
    {
        if (field == null) {
            return null;
        }

        var attrs = field.GetCustomAttributes(typeof(PropertyAttribute), true);
        if (attrs.Any()) {
            return new List<PropertyAttribute>(attrs.Select(e => e as PropertyAttribute).OrderBy(e => -e.order));
        }

        return null;
    }

    public static T FindOrCreateManager<T>(string sceneName = null) where T : Component
    {
        var result = Object.FindObjectOfType<T>();
        if (result != null && sceneName == SceneManager.GetActiveScene().name) return result;
        Assert.IsFalse(SceneManager.sceneCount == 0, "No opened Scene");
        var scene = sceneName.IsNullOrEmpty() || sceneName == SceneManager.GetActiveScene().name
            ? SceneManager.GetActiveScene()
            : SceneManager.GetSceneByName(sceneName);
        if (!scene.IsValid()) {
            scene = SceneManager.GetActiveScene();
            sceneName = scene.name;
        }

        Assert.IsTrue(scene.IsValid(), $"{scene.name} scene.IsValid()");

        //Assert.IsTrue(!scene.isLoaded);
        Debug.Log($"ScenenName: {scene.name}".ToBlue());
        var root = scene.GetRootGameObjects().FirstOrDefault(t => t.name == sceneName) ??
            new GameObject(sceneName).Of(go => SceneManager.MoveGameObjectToScene(go, scene));
        return root.GetComponentInChildren<T>(true) ??
            (root.transform.Find(typeof(T).Name) ?? new GameObject(typeof(T).Name)
                .Of(go => go.transform.SetParent(root.transform)).transform).RequireComponent<T>();
    }

    public static GameObject CreateGameObjectByPath(this Component root, string path) =>
        root?.gameObject.CreateGameObjectByPath(path);

    public static GameObject CreateGameObjectByPath(this GameObject root, string path)
    {
        var current = root?.transform;
        foreach (var name in path.Trim('/').Split('/').Select(t => t.Trim()).Where(t => !t.IsNullOrWhitespace())) {
            current = current?.Find(name) ?? new GameObject(name).transform.Of(t1 => t1.SetParent(current));
        }

        return current?.gameObject;
    }
}