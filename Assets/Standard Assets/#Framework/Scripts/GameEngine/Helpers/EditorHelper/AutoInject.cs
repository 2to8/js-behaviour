#if UNITY_EDITOR
using GameEngine.Attributes;
using Sirenix.Utilities;
using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace GameEngine.Helpers.EditorHelper {

[InitializeOnLoad]
public static class AutoInject {

    static AutoInject()
    {
        var scene = SceneManager.GetActiveScene();

        //Debug.Log($"{scene.name} inject...");
        var changed = false;

        //Debug.Log(Assembly.GetExecutingAssembly().FullName);
        var assembly = Assembly.GetExecutingAssembly();

        // this is making the assumption that all assemblies we need are already loaded.

        //AppDomain.CurrentDomain.GetAssemblies().ForEach(assembly =>
        //{

        //Debug.Log(assembly.FullName);
        assembly.GetTypes()
            .ForEach((type, i) => {
                if (!type.IsSubclassOf(typeof(MonoBehaviour))) {
                    return;
                }

                if (!$"{type.FullName}".ToUpper().Contains(scene.name.Split('-', '_')[0].ToUpper())) {
                    return;
                }

                type.GetCustomAttributes<Inject>(false)
                    .ForEach(inject => {
                        if (inject.type != null) {
                            Object.FindObjectsOfType(inject.type)
                                .ForEach(o => {
                                    var mb = (Component)o;

                                    if (mb.gameObject.name.Contains(inject.name ?? inject.type.Name,
                                        StringComparison.OrdinalIgnoreCase)) {
                                        if (mb.GetComponent(type) == null) {
                                            changed = true;
                                            mb.gameObject.AddComponent(type);
                                            Debug.Log($"{mb.name} Add: {type.FullName}", mb.gameObject);
                                        }
                                    }
                                });
                        }

                        // Object.FindObjectsOfType<Transform>().ForEach(go =>
                        // {
                        //     if(go.name.ToUpper()
                        //         .Contains((inject.name ?? type.Name).ToUpper())){
                        //         if(go.GetComponent(type) == null){
                        //             changed = true;
                        //             go.AddComponentOnce(type);
                        //         }
                        //     }
                        // });
                    });

                // Debug.Log($"{type.FullName}");
            });

        // });

        if (changed) {
            Debug.Log("changed");
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            Debug.Log("set dirty");
        }

        // foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        // {
        //     foreach (Type type in assembly.GetTypes())
        //     {
        //         var attribs = type.GetCustomAttributes(typeof(Inject), false);
        //         if (attribs?.Length > 0)
        //         {
        //             // add to a cache.
        //         }
        //     }
        // }
    }

}

}
#endif