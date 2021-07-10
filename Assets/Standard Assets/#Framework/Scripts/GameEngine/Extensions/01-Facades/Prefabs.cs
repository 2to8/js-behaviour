using MoreTags;
using Sirenix.Utilities;
using System.IO;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor.AddressableAssets.Settings;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.Experimental.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.AddressableAssets;
// using WebSocketSharp;

namespace GameEngine.Extensions {

public static class Prefabs {

    public static void SyncAssets(this GameObject gameObject, string path = null)
    {
        return;

    #if UNITY_EDITOR
        if (path.IsNullOrEmpty()) {
            path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(gameObject);
        }

        if (path.IsNullOrEmpty()) {
            path = AssetDatabase.GetAssetPath(gameObject);
        }
        var guid = AssetDatabase.AssetPathToGUID(path);

        if (guid.IsNotNullOrEmpty()) {
            var config = AddressableAssetSettingsDefaultObject.Settings;
            var entry = config.FindAssetEntry(guid);

            if (entry == null) {
                entry = config.CreateOrMoveEntry(guid, config.FindGroup("Prefabs") ?? config.DefaultGroup);
            }
            var tags = gameObject.RequireComponent<Tags>();
            tags.ids.RemoveAll(t => !TagSystem.ids.Contains(t));
            tags.tags.Where(t => !entry.labels.Contains(t))
                .ForEach(tag => {
                    entry.SetLabel(tag, true);
                });
            entry.labels.Where(t => !gameObject.GetTags().Contains(t)).ForEach(label => gameObject.AddTag(label));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            //var tags = gameObject.RequireComponent<Tags>();

            if (!tags.asset.IsValid()) {
                Debug.Log(tags.gameObject.name);
                tags.asset = new AssetReference(guid);

                // EditorUtility.DisplayDialog("test", AssetDatabase.GetAssetPath(tags.Asset.editorAsset), "ok");
            }

            Debug.Log("saving");

            if (gameObject != PrefabStageUtility.GetCurrentPrefabStage()?.prefabContentsRoot) {
                if (PrefabStageUtility.GetCurrentPrefabStage() != null  ) {
                    PrefabUtility.SaveAsPrefabAssetAndConnect(gameObject, path, InteractionMode.AutomatedAction);
                }
            }
        }
    #endif
    }

    public static string _GetPrefabSavePath(this GameObject gameObject, bool getDirName = false)
    {
    #if UNITY_EDITOR
        var stage = PrefabStageUtility.GetCurrentPrefabStage();
        var path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(gameObject);
        var root = PrefabUtility.GetNearestPrefabInstanceRoot(gameObject) ?? stage?.prefabContentsRoot;

        if (path.IsNullOrEmpty()) {
            path = stage?.assetPath;
        }

        if (path.IsNullOrEmpty()) {
            path = gameObject.scene.path;
        }

        var dir = path.Replace("\\", "/").Replace(".prefab", "").Replace(".unity", "");

        return getDirName
            ? $"{dir}/{gameObject.name}".CreateDirFromFilePath(false)
            : $"{dir}/{root?.name ?? gameObject.scene.name}-{gameObject.name}.prefab".CreateDirFromFilePath();

    #endif
        return Application.dataPath + "/../Temp";
    }
}

}