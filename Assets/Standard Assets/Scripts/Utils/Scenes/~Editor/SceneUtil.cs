#if UNITY_EDITOR
using GameEngine.Attributes;
using UnityEditor.SceneManagement;
using GameEngine.Extensions;
using GameEngine.Kernel;
using MoreTags;
using Puerts.Attributes;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Tetris;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Asset;
using Object = UnityEngine.Object;

//using Utils.Asset;
using Scene = UnityEngine.SceneManagement.Scene;

namespace Utils.Scenes
{
    public static partial class SceneUtil
    {
        public static IEnumerable<GameObject> GetRootPrefabs(this Scene scene, bool rootOnly = true)
        {
            return FindInScene<Transform>(scene, tr => tr != null && PrefabUtility.IsPartOfAnyPrefab(tr))
                .Select(tr =>
                    rootOnly
                        ? PrefabUtility.GetOutermostPrefabInstanceRoot(tr.gameObject)
                        : PrefabUtility.GetNearestPrefabInstanceRoot(tr.gameObject))
                .Distinct();
        }

    #if UNITY_EDITOR
        [MenuItem("Tools/绑定脚本/更新自动绑定到Tag的脚本", false, 100)]
        [InitializeOnLoadMethod, PuertsIgnore]
        static void EditorMenu()
        {
            // var glboalScene = AssetEdit.FindAssets(nameof(Scene) + " GlobalScene").FirstOrDefault();
            //
            // if (!string.IsNullOrEmpty(glboalScene)
            //     && !SceneManager.GetSceneByPath(glboalScene).isLoaded) {
            //     EditorSceneManager.OpenScene(glboalScene, OpenSceneMode.Additive);
            // }

            // PlayerSettings.GetPreloadedAssets().ForEach(t =>  Object.Instantiate(t));

            if (TagManager.instance == null) {
                Core.FindOrCreatePreloadAsset<TagManager>();
            }

            var allTypes = Core.FindTypes(t => t.IsDefined<SceneBindAttribute>()).ToList();

            return;

            Core.GetAllLoadedScenes()
                .ForEach(scene => {
                    var go = scene.GetRootGameObjects()
                        .FirstOrDefault(t => t.name == scene.name || t.GetComponent<SceneAdmin>() != null);

                    if (go == null) {
                        go = new GameObject(scene.name, typeof(SceneAdmin));
                        SceneManager.MoveGameObjectToScene(go, scene);
                    }

                    if (go.GetComponent<SceneAdmin>() == null) {
                        go.AddComponent<SceneAdmin>();
                    }

                    go.transform.SetAsFirstSibling();
                    go.name = scene.name;
                    Debug.Log(scene.name.ToBlue());

                    var CurrentTypes = allTypes
                        .Where(t => t.GetCustomAttribute<SceneBindAttribute>()?.SceneName == scene.name)
                        .ToList();

                    if (CurrentTypes.Any()) {
                        scene.GetRootGameObjects()
                            .SelectMany(g =>
                                g.GetComponentsInChildren<Tags>(true)
                                    .Where(tag => /*!PrefabUtility.IsPartOfAnyPrefab(tag.gameObject) &&*/ tag.ids.Any()))
                            .ForEach(tags => {
                                CurrentTypes.ForEach(target => {
                                    var attr = target.GetCustomAttribute<SceneBindAttribute>();

                                    if (attr.Tags.Any()
                                        && attr.Tags.All(tag => tags.gameObject.HasTag(tag))
                                        && (attr.Type == null
                                            || tags.gameObject.TryGetComponent(attr.Type, out var tmp))
                                        && !tags.gameObject.TryGetComponent(target, out var t2)) {
                                        tags.gameObject.RequireComponent(target);
                                        Debug.Log($"Add Type: {target.GetNiceName()} to: {tags.gameObject.name}".ToGreen(),
                                            tags.gameObject);
                                    }
                                });
                            });
                    }
                });
        }
    #endif
        [InitializeOnLoadMethod]
        static void SetSceneEvents()
        {
            EditorSceneManager.sceneOpening += OnSceneOpening;
            EditorSceneManager.sceneClosing += OnSceneClosing;
            EditorSceneManager.sceneOpened += OnSceneOpened;
        }

        static Dictionary<string, Action<Scene>> ScenAction = new Dictionary<string, Action<Scene>>();

        static void OnSceneOpening(string path, OpenSceneMode mode)
        {
            Debug.Log($"Opening {path} [mode:{mode}]".ToGreen());

            if (Application.isPlaying) return;

            ScenAction[path] = scene => {
                // var go = new GameObject("test");
                // SceneManager.MoveGameObjectToScene(go, scene);
            };

            var glboalScene = AssetUtil.FindAssets(nameof(Scene) + " GlobalScene").FirstOrDefault();

            if (glboalScene.IsNullOrWhitespace() || glboalScene == path) return;

            if (!SceneManager.GetSceneByPath(glboalScene).isLoaded) {
                EditorSceneManager.OpenScene(glboalScene, OpenSceneMode.Additive);
            }
        }

        /// <summary>
        /// 删除场景里面的预制体保存为引用, 可以有效减少场景的体积
        /// 加载场景时用脚本重新生成
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="removingscene"></param>
        static void OnSceneClosing(Scene scene, bool removingscene)
        {
            Debug.Log($"Closing {scene.name} [Loaded: {scene.isLoaded}]".ToBlue());

            return;

            if (string.IsNullOrEmpty(scene.path) || Application.isPlaying || !scene.isLoaded) return;

            //     FindInScene<Transform>(scene, tr => tr != null && PrefabUtility.IsPartOfAnyPrefab(tr))
            //         .Select(tr => PrefabUtility.GetOutermostPrefabInstanceRoot(tr))
            //         .Where(t => t != null)
            //         .Distinct()
            //         .ForEach(go => {
            //             Debug.Log(PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(go).ToYellow());
            //             PrefabReference.Save(go.transform);
            //
            //             // var root = scene.GetRootGameObjects().FirstOrDefault(t => t.name == scene.name)
            //             //     ?? new GameObject(scene.name).Tap(t =>
            //             //         SceneManager.MoveGameObjectToScene(t, scene));
            //             // var agent = root.RequireComponent<SceneAdmin>();
            //
            //             //var tr = go.transform;
            //             // Debug.Log(JsonConvert.SerializeObject((tr.localPosition, tr.localRotation,
            //             //     tr.localScale, tr.GetSiblingIndex())));
            //         });
            //     EditorSceneManager.MarkSceneDirty(scene);
            //     EditorSceneManager.SaveScene(scene);
            // }
        }

        /// <summary>
        /// Opened 再打包时也会被调用, 但是打包时不会调用OnOpening, 可以用来做判断
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="mode"></param>
        static void OnSceneOpened(Scene scene, OpenSceneMode mode)
        {
            Debug.Log($"{scene.name} Opened [Loaded: {scene.isLoaded}]".ToYellow());

            if (string.IsNullOrEmpty(scene.path) || Application.isPlaying) return;

            //EditorUtility.DisplayDialog("loading", scene.path, "ok");

            //if (Core.isBuilding) return;

            // 用 OnOpening 判断是否编辑器里打开
            if (ScenAction.TryGetValue(scene.path, out var action)) {
                scene.RestorePrefabs();
                action.Invoke(scene);
                ScenAction.Remove(scene.path);
            }
        }

        static void OnSceneSaving(Scene scene, string path)
        {
            Debug.Log($"Saving {path} [Loaded: {scene.isLoaded}]".ToYellow());
        }

        // [NUnit.Framework.Test]
        public static void _SceneUtilsSimplePasses()
        {
            //Core.FindObjectOfTypeAll<>()

            // Use the Assert class to test conditions.
            Debug.Log("Test");
        }

        // A UnityTest behaves like a coroutine in PlayMode
        // and allows you to yield null to skip a frame in EditMode
        //[UnityEngine.TestTools.UnityTest]
        public static IEnumerator SceneUtilsWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // yield to skip a frame
            yield return null;
        }
    }
}
#endif