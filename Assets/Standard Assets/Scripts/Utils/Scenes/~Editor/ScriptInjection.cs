#if UNITY_EDITOR

using FluentAssertions.Types;
using GameEngine.Extensions;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils.Scenes {

public class ScriptInjection {

    // [DidReloadScripts]
    // static void InjectToPrefabStage()
    // {
    //     // PrefabStage prefabStage = PrefabStageUtility.GetPrefabStage();
    //     // GameObject root = prefabStage.prefabContentsRoot;
    // }

    static bool inject(GameObject go, Type type)
    {
        if (!go.GetComponentInChildren(type, true)) {
            Debug.Log(type.GetNiceFullName());
            go.RequireComponent(type);
            var prefabStage = PrefabStageUtility.GetPrefabStage(go);

            if (prefabStage != null) {
                EditorSceneManager.MarkSceneDirty(prefabStage.scene);
            }

            // if (PrefabUtility.IsPartOfAnyPrefab(go)) {
            //     PrefabUtility.SaveAsPrefabAsset(PrefabUtility.GetNearestPrefabInstanceRoot(go),
            //         PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(go));
            // }

            return true;
        }

        return false;
    }

    //[DidReloadScripts]
    static void InjectManagers()
    {
        var root = PrefabStageUtility.GetCurrentPrefabStage()?.prefabContentsRoot;

        var list = root != null ? new List<Scene>() { root.scene } : SceneUtil.GetAllLoadedScenes();
        list.ForEach(scene => {
            Core.MainAssembly.GetExportedTypes()
                .Where(type =>
                    typeof(MonoBehaviour).IsAssignableFrom(type)
                    && type.Namespace != null
                    && type.Namespace.Contains(scene.name)
                    && type.Namespace.StartsWith("Main."))
                .ForEach(type => {
                    var go = scene.RootObject();

                    if (go == null) return;

                    //Debug.Log(type.FullName);

                    if (type.Namespace?.EndsWith(scene.name) == true) {
                        inject(go, type);
                    } else {
                        SceneUtil.GetRootPrefabs(scene, false)
                            .Where(_go => _go.name.Contains(type.Namespace.Split('.').Last()))
                            .ForEach(_go => {
                                inject(_go, type);
                            });
                    }

                    //scene.RootObject().RequireComponent(type);
                });
        });
    }

}

}

#endif