// using Engine.Tests;
// using Newtonsoft.Json;
// using Sirenix.Utilities;
//
// namespace Engine {
//
// public static partial class JS {
//
//     public static JsRuntime Eval(string code) => JsRuntime.instance.Eval(code);
//     public static JsRuntime Eval<T>(string code, T bind) => JsRuntime.instance.Eval(code, bind);
//
//     public static JsRuntime Log(params object[] _)
//     {
//         _.ForEach(s => JsRuntime.instance.Eval($"console.log({JsonConvert.SerializeObject(s)})"));
//         return JsRuntime.instance;
//     }
//
//     public static JsRuntime Require(JsTextAsset file) => JsRuntime.instance.File(file);
//     public static JsRuntime Require(string file) => JsRuntime.instance.Eval($"require('{file}')");
//
//     public static JsRuntime Reload() => JsRuntime.instance.Reload();
//
//     public static JsRuntime Require<T>(string file, T bind) =>
//         JsRuntime.instance.Eval($"require('{file}')", bind);
//
// }
//
// }

