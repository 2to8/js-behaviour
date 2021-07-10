// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using Sirenix.OdinInspector;
// using UnityEngine;
// using Object = UnityEngine.Object;
//
// namespace Puerts {
//
// [CreateAssetMenu(fileName = "JSEngine", menuName = "ScriptableObjects/JSEngine", order = 0)]
// public partial class JS : ScriptableObject {
//
//     static JS m_Instance;
//     static Engine m_Env;
//     readonly long timeStamp = CurrentTimestamp;
//     long timeOffset;
//     public static long CurrentTimestamp => DateTimeOffset.Now.ToUnixTimeSeconds();
//
//     [SerializeField]
//     public List<string> ScriptRoot = new List<string>();
//
//     [SerializeField]
//     public JavaScriptLoader loader;
// #if UNITY_EDITOR
//     [SerializeField, HideInInspector]
//     Object m_DebugRootPath;
//
//     [ShowInInspector]
//     Object RootPath {
//         get {
//             var path = m_DebugRootPath.GetObjectPath().RemoveAssets();
//
//             if(Directory.Exists(Application.dataPath + "/" + path)) {
//                 if(!ScriptRoot.Contains(path)) {
//                     ScriptRoot.Add(path);
//                 }
//             }
//
//             return m_DebugRootPath;
//         }
//         set => m_DebugRootPath = value;
//     }
// #endif
//
//     [SerializeField]
//     int port = 9808;
//
//     bool connected;
//     public static Engine env => m_Env ?? CreateEnv();
//     int alive;
//     public static bool IsConected => env != null && PuertsDLL.InspectorTick(env.isolate);
//
//     public static Engine CreateEnv()
//     {
//         instance.alive = 0;
//         return m_Env = Engine.Create(new DefaultLoader(Application.dataPath), instance.port);
//     }
//
//     public static JS instance {
//         get => m_Instance ?? (m_Instance = Resources.LoadAll<JS>("").FirstOrDefault() ??
//             throw new Exception("resource not found"));
//         private set => m_Instance = value;
//     }
//
//     void OnEnable()
//     {
//         if(m_Instance == null) {
//             m_Instance = this;
// //            env.Eval("console.log('env reloaded')");
//         }
//     }
//
//     void OnDisable()
//     {
// //        env.Eval("console.log('env disable')");
//     }
//
//     public void checkUpdate()
//     {
//         if(!PuertsDLL.InspectorTick(m_Env.isolate)) {
//             DisConnectAll();
//             var t = CurrentTimestamp - timeStamp;
//             if(t != timeOffset) {
//                 Debug.Log("connecting");
//                 timeOffset = t;
//             }
//         } else if(!connected) {
//             Debug.Log("connected");
//             connected = true;
//         } else {
//             if((alive += 1) < 10) {
//                 m_Env?.Eval("console.log('connect alived')");
//             }
//         }
//     }
//
//     static void DisConnectAll()
//     {
//         var i = 0;
//         JsEnv.jsEnvs.ForEach(e => {
//             if(e != null && e.isolate != IntPtr.Zero && PuertsDLL.InspectorTick(e.isolate) && e != m_Env) {
//                 Debug.Log($"dispose: {i += 1}");
//                 PuertsDLL.DestroyInspector(e.isolate);
//             }
//         });
//     }
//
// }
//
// }

