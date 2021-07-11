using GameEngine.Extensions;
using Puerts;
using Puerts.Attributes;
using System;
using System.Reflection;
using UnityEngine;

namespace Common.JSRuntime.Settings {


public static class AutoRegisterActions {

    [RegisterEnv]
    public static Action<JsEnv> RegisterActions() => jsEnv => {
        var type = typeof(AutoRegisterActions).Assembly.GetType("PuertsStaticWrap.PuertsHelper");
        var method = type?.GetMethod("UsingActions",
            BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
        var ret = method?.Invoke(null, new object[] { jsEnv });

        if (method != null) {
            Debug.Log("[JsEnv-Actions-Registed]".ToGreen(ret));
        }
    };

}

}