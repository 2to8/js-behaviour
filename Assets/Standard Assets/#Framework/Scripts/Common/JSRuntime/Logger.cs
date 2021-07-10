// using System.Collections.Generic;
// using Newtonsoft.Json;
// using Sirenix.Utilities;
// using UnityEngine;
//
// namespace Puerts {
//
// public class Logger {
//
//     public static bool DisableLog = true;
//     public static JsEnv js => JS.env;
//
//     [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)
// #if UNITY_EDITOR
//      , UnityEditor.InitializeOnLoadMethod, UnityEditor.InitializeOnEnterPlayMode,
//      UnityEditor.Callbacks.DidReloadScripts
// #endif
//     ]
//     static void SetLogger()
//     {
//         if(!DisableLog) {
//             //在一个日志信息上注册一个委托来被调用
//             Application.logMessageReceived -= LogCallback;
//             Application.logMessageReceived += LogCallback;
//         }
//     }
//
//     /// <summary>
//     /// log callback check
//     /// </summary>
//     /// <param name="condition">log内容.</param>
//     /// <param name="stackTrace">log产生的栈数据追踪</param>
//     /// <param name="type">log的类型.</param>
//     public static void LogCallback(string condition, string stackTrace, LogType type)
//     {
//         if(DisableLog || js == null) return;
//
//         //Helpers.LogToUnity = false;
//
//         var message = "";
//         condition = JsonConvert.SerializeObject(condition);
//         var ret = new Dictionary<object, object>();
//
//         //ret["message"] = condition;
//         var stack = new List<string>();
//
//         //ret["stack"] = stack;
//         ret["type"] = $"{type}";
//         stackTrace.TrimEnd('\n')
//             .Split('\n')
//             .ForEach((i, s) => {
//                 ret[i + 1] = s;
//             });
//         condition = JsonConvert.SerializeObject(condition);
//         var trace = JsonConvert.SerializeObject(ret);
//
//         // stackTrace = Regex.Replace(stackTrace, "^[(.*)]$", "[\n$1\n]");
//
//         //condition =
//         //condition.Replace("'", "‘").Replace("\n","\\n");
//         // stackTrace =
//         //stackTrace.Replace("'", "‘").Replace("\n","\\n");
//
//         switch(type) {
//             case LogType.Assert :
//                 message = $"console.error({condition}, {trace})";
//                 break;
//             case LogType.Error :
//                 message = $"console.error({condition}, {trace})";
//                 break;
//             case LogType.Exception :
//                 message = $"console.error({condition}, {trace})";
//                 break;
//             case LogType.Log :
//                 message = $"console.log({condition}, {trace})";
//                 break;
//
//             // case LogType.Warning :
//             //     message = $"console.info({condition}, {trace})";
//             //     break;
//         }
//         js?.Eval(message);
//
//         //Helpers.LogToUnity = true;
//     }
//
// }
//
// }

