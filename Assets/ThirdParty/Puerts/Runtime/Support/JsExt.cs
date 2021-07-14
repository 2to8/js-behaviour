using App.Runtime;
using Puerts;
using UnityEngine;

namespace App.Support
{
    public static partial class JsExt
    {
        public static JsEnv Env(this object target) => Main.js;
        public static void JsCall<T>(this T target, string fn) => Env(target).Call(target, fn);
        public static void JsCall<T, T1>(this T target, string fn, T1 arg1) => Env(target).Call(target, fn, arg1);

        public static void JsCall<T, T1, T2>(this T target, string fn, T1 arg1, T2 arg2) =>
            Env(target).Call(target, fn, arg1, arg2);

        public static void JsCall<T, T1, T2, T3>(this T target, string fn, T1 arg1, T2 arg2, T3 arg3) =>
            Env(target).Call(target, fn, arg1, arg2, arg3);

        public static void JsCall<T, T1, T2, T3, T4>(this T target, string fn, T1 arg1, T2 arg2, T3 arg3, T4 arg4) =>
            Env(target).Call(target, fn, arg1, arg2, arg3, arg4);

        public static void JsCall<T, T1, T2, T3, T4, T5>(this T target, string fn, T1 arg1, T2 arg2, T3 arg3, T4 arg4,
            T5 arg5) =>
            Env(target).Call(target, fn, arg1, arg2, arg3, arg4, arg5);
    }
}