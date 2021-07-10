using System;
using UnityEngine;

namespace GameEngine.Extensions {

public static class ComponentExtension {

    public static T Tap<T>(this T obj, Action<T> action)
    {
        action?.Invoke(obj);

        return obj;
    }

    public static T Of<T>(this T obj, Action<T> action)
    {
        action?.Invoke(obj);

        return obj;
    }

    public static TResult Tap<T, TResult>(this T obj, Func<T, TResult> action) =>
        action != null ? action.Invoke(obj) : default;

    // public static void Bind<T1>(this Component component, ref TextUI<T1> field, TextUI<T1> target)
    //     where T1 : /*IComparable, IConvertible, */IEquatable<T1>
    // {
    //     if(field != null && target != null) {
    //         target.ui = field.ui;
    //     }
    //
    //     if(target?.ui is Text text) {
    //         text.text = $"{target.value}";
    //     } else if(target?.ui is Toggle toggle) {
    //         toggle.isOn = (bool)(object)target.value;
    //     }
    //
    //     field = target;
    // }

    // public static string GetPath(this Transform current)
    // {
    //     if (current?.parent == null) {
    //         return "/" + current?.name;
    //     }
    //
    //     return current.parent.GetPath() + "/" + current.name;
    // }
    //
    // public static string GetPath(this Component component) => component.transform.GetPath() + "/" + component.GetType();

   //public static string GetPath(this GameObject gameObject) => gameObject.transform.GetPath();

}

}