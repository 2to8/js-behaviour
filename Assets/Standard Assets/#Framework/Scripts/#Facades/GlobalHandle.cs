using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;

[ExecuteAlways]
public class GlobalHandle : SerializedMonoBehaviour {
    //

    public static GlobalHandle instance {
        get {
            if (m_instance == null) {
            #if UNITY_EDITOR
                var path = AssetDatabase.FindAssets("t:scene GlobalScene")
                    .Select(AssetDatabase.GUIDToAssetPath)
                    .FirstOrDefault();
                var scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);

                if (m_instance == null) {
                    m_instance = scene.GetRootGameObjects()
                        .SelectMany(go => go.GetComponentsInChildren<GlobalHandle>(true))
                        .FirstOrDefault();
                }

            #endif
            }

            return m_instance;
        }
    }

    static GlobalHandle m_instance;

    public Dictionary<Type, Action> runner = new Dictionary<Type, Action>();

    void OnEnable()
    {
        m_instance = this;
        runner.ForEach(kv => kv.Value?.Invoke());
    }

}