using App.Runtime;
using Puerts;
using UnityEngine;

namespace App.Support
{
    public static partial class Exts
    {
        public static JsEnv Js(this object target) => Main.js;
        public static void JsCall<T>(this T target, string fn) => Js(target).Call(target, fn);
    }
}