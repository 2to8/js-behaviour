using GameEngine.Extensions;
using MoreTags;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Utils.Scenes {

public static partial class SceneUtil {

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    static void AddSceneRestoreHooks()
    {
        SceneManager.sceneLoaded += (scene, mode) => {
           // scene.RestorePrefabs();
        };
    }

    // [RuntimeInitializeOnLoadMethod]
    // static void RemoveDontShow()
    // {
    //     TagSystem.query.tags("DontShow")
    //         .result.ForEach(go => {
    //             go.SetActive(false);
    //         });
    // }

    public static void RestorePrefabs(this Scene scene)
    {
        return;

        var all = FindInScene<PrefabReference>(scene);
        all.ForEach(t => {
            Debug.Log($"Restore Prafab: {t.path}".ToBlue());
            var go = Core.Instantiate(t.prefab, t.transform.parent);
            go.name = t.name;
            t.data.PushToTransform(go.transform);
            var index = t.data.Index;

            if (t.TryGetComponent(typeof(Tags), out var tags)) {
                if (go.TryGetComponent(typeof(Tags), out var old)) {
                    old.DestroySelf();
                }
                Object.Instantiate(tags, go.transform);
            }
            t.gameObject.DestroySelf();
            go.transform.SetSiblingIndex(index);
        });
    }

    public static IEnumerable<Scene> GetAllLoadedScenes()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++) {
            yield return SceneManager.GetSceneAt(i);
        }
    }

    public static IEnumerable<T> FindInScene<T>(Scene scene,
        Expression<Func<T, bool>> expression = null)
    {
        return scene.GetRootGameObjects()
            .SelectMany(go => go.GetComponentsInChildren<T>(true))
            .Where(expression?.Compile() ?? (arg => true));
    }

    public static GameObject RootObject(this Scene scene)
    {
        return scene.GetRootGameObjects().FirstOrDefault(go => go.name == scene.name)
            ?? new GameObject(scene.name).Tap(go => {
                SceneManager.MoveGameObjectToScene(go, scene);
                go.RequireComponent<SceneAdmin>();
                go.transform.SetAsFirstSibling();
            });
    }
}

}