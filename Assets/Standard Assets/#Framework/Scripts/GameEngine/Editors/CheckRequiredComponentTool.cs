#if UNITY_EDITOR && ECS
using Engine.Extensions;
using Engine.Kernel;
using Sirenix.Utilities;
using Unity.Entities;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Engine.Editors {

public class CheckRequiredComponentTool {

    [ MenuItem("Game/Check Required Component") ]
    public static void CheckRequired()
    {
        var all = PrefabStageUtility.GetCurrentPrefabStage() != null
            ? StageUtility.GetCurrentStageHandle().FindComponentsOfType<MonoBehaviour>()
            : Core.FindObjectsOfTypeAll<MonoBehaviour>().ToArray();

        Debug.Log(
            $"prefab path: {PrefabStageUtility.GetCurrentPrefabStage()?.prefabAssetPath} monobehaviour count: {all?.Length}");

        var added = 0;

        all?.ForEach(mb => {
            if( mb == null ) {
                return;
            }

            if( mb.GetType().Name.EndsWith("Authoring") ) {
                mb.gameObject.RequireComponent<ConvertToEntity>();
            }

            if( mb.GetComponent<ConvertToEntity>() is ConvertToEntity convertToEntity ) {
                if( convertToEntity.ConversionMode != ConvertToEntity.Mode.ConvertAndInjectGameObject ) {
                    added += 1;
                    convertToEntity.ConversionMode = ConvertToEntity.Mode.ConvertAndInjectGameObject;
                }

                // if( mb.GetComponent<GameObjectEntity>() == null ) {
                //     added += 1;
                //     mb.gameObject.RequireComponent<GameObjectEntity>();
                // }
            }

            mb.GetType()
                .GetCustomAttributes<RequireComponent>(true)
                .ForEach(attr => {
                    if( attr.m_Type0 != null && mb.GetComponent(attr.m_Type0) == null ) {
                        added += 1;
                        mb.gameObject.AddComponent(attr.m_Type0);
                        Debug.Log($"{mb.GetType().Name} added {attr.m_Type0.Name}");
                    }

                    if( attr.m_Type1 != null && mb.GetComponent(attr.m_Type1) == null ) {
                        added += 1;
                        mb.gameObject.AddComponent(attr.m_Type1);
                        Debug.Log($"{mb.GetType().Name} added {attr.m_Type1.Name}");
                    }

                    if( attr.m_Type2 != null && mb.GetComponent(attr.m_Type2) == null ) {
                        added += 1;
                        mb.gameObject.AddComponent(attr.m_Type2);
                        Debug.Log($"{mb.GetType().Name} added {attr.m_Type2.Name}");
                    }

                    //if(added != null) {
                    //LightmapEditorSettings.bakeResolution = 14;

                    // }
                });
        });

        Debug.Log($"Changed: {added}");

        //PrefabStageUtility.GetPrefabStage(YourGameObject);
        if( added > 0 ) {
            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();

            if( prefabStage != null ) {
                EditorSceneManager.MarkSceneDirty(prefabStage.scene);
                Debug.Log("set dirty");
            } else {
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
                Debug.Log("set dirty");
            }
        }
    }

}

}
#endif