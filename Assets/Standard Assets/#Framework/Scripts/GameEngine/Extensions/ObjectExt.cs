using System.Reflection;
using UnityEngine;

namespace GameEngine.Extensions {

public static class ObjectExt {

    public static T GetInstance<T>(this Object obj)
    {
        var t = typeof(T);

        if (!t.IsAbstract) {
            var func = t.GetMethod("GetInstance",
                BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);

            if (func == null) {
                Debug.Log($"{t.Name} instance is null");

                return default;
            }

            return (T)func?.Invoke(null, null);
        }

        return default;
    }

}

}