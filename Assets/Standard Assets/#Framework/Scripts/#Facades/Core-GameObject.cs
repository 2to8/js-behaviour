using Common.JSRuntime;
using GameEngine.Extensions;
using MoreTags;
using Sirenix.Utilities;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
#endif
using UnityEngine;

// using WebSocketSharp;

public static partial class Core
{
    public static GameObject CheckTagsFromRoot(this GameObject root)
    {
        var names = new HashSet<string>();
        root?.GetComponentsInChildren<Tags>(true)
            ?.ForEach(t => {
                //if (Env.forceReload || true) {
                    t.isAwaked = false;
               // }

                if (!t.isAwaked) {
                    t.Awake();
                    t.OnEnable();

                    //t.gameObject.AddTag(t.ids.ToArray());
                    t.Start();
                }

                if (t.gameObject.HasTag("DontShow")) {
                    t.gameObject.SetActive(false);
                }

                if (names.Add(t.gameObject.name) && t.tags.Any()) {
                    //Debug.Log($"[CheckTag] {t.gameObject.name} [ {t.tags.Join()} ]".ToBlue());
                }
            });

        return root;
    }

#if UNITY_EDITOR
    public static string GameObjectAssetPath(this GameObject go)
    {
        // if (go == null || !Application.isEditor) return string.Empty;

    #if UNITY_EDITOR
        var path = PrefabUtility.IsPartOfAnyPrefab(go)
            ? PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(go)
            : PrefabStageUtility.GetPrefabStage(go)?.assetPath;

        if (path.IsNullOrEmpty()) {
            path = go.scene.path;
        }

        return path;
    #endif
        return Application.dataPath + "/../temp";
    }
#endif
}