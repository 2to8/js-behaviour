using Sirenix.Utilities;
using System;
using System.Linq;
using System.Reflection;
using UniRx.Async;
using UnityEngine;

#pragma warning disable 1998

namespace GameEngine.Kernel.Attributes {

[AttributeUsage(AttributeTargets.Class)]
public class LoadAssets : Attribute {

    public string Path;

    public static async UniTask Dispatch()
    {
        var types = Assembly.GetExecutingAssembly()
            .GetExportedTypes()
            .Where(t => t.GetCustomAttribute<LoadAssets>() != null);

        foreach (var type in types) {
            if (type.GetCustomAttribute<LoadAssets>() is LoadAssets attr /* && type.IsAssignableFrom(typeof(IModel))*/
            ) {
                // var prop = type.GetProperty("Instance");
                // if(prop != null && prop.GetValue(null) is IState state) {
                //     currentState = state;
                // }
                Debug.Log($"check class: {type.FullName}");

                // @see https://forums.asp.net/t/1323903.aspx?this+GetType+returns+null

                // 继承和static的属性必须加 flag
                var method = type.GetMethod("LoadDefaultAsset",
                    BindingFlags.Public |
                    BindingFlags.Static /*| BindingFlags.Instance*/ |
                    BindingFlags.IgnoreCase |
                    BindingFlags.FlattenHierarchy);

                // 继承和static的属性必须加 flag

                if (method != null) {
                    Debug.Log($"loading {type.Name}");
                    await (UniTask)method.Invoke(null, null);

                    var instance = type.GetProperty("Instance",
                        BindingFlags.FlattenHierarchy |
                        BindingFlags.Static |
                        BindingFlags.Public |
                        BindingFlags.IgnoreCase);

                    var t = instance?.GetValue(null, null);
                    Debug.Log($"check property: {instance != null},t = {t != null}");
                }
            }
        }

        Debug.Log("Assets loaded");

        // Assembly.GetExecutingAssembly().GetCustomAttributes<LoadAssets>().ForEach(attr => {
        //
        // });
    }

}

}