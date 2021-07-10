// using System;
// using Sirenix.OdinInspector;
// using Sirenix.Utilities;
//
// // using UnityEditor;
// // using UnityEditor.Callbacks;
// using UnityEngine;
//
// namespace Puerts {
//
// //[ExecuteAlways]
// public class SingletonEnv : MonoBehaviour {
//
//     public static JsEnv env => JS.env;
//     static SingletonEnv m_instance;
//
//     [SerializeField]
//     bool EnableUpdate;
//
//     [ShowInInspector]
//     bool Connected => env != null && PuertsDLL.InspectorTick(env.isolate);
//
//     // static JsEnv m_Env;
//     // static JsEnv m_Env_Playing;
//
//     void OnEnable()
//     {
//         if(EnableUpdate) {
// #if UNITY_EDITOR
//             if(runInEditMode && !UnityEditor.EditorApplication.isPlaying) {
//                 if(JS.instance != null) {
//                     if(EnableUpdate) {
//                         UnityEditor.EditorApplication.update -= JS.instance.checkUpdate;
//                         UnityEditor.EditorApplication.update += JS.instance.checkUpdate;
//                     } else {
//                         UnityEditor.EditorApplication.update -= JS.instance.checkUpdate;
//                     }
//                 }
//             }
// #endif
//         }
//
//         Debug.Log($"Conneced: {JS.IsConected}");
//     }
//
//     void OnDisable()
//     {
// #if UNITY_EDITOR
//         if(EnableUpdate && JS.instance != null) {
//             UnityEditor.EditorApplication.update -= JS.instance.checkUpdate;
//         }
// #endif
//     }
//
//     [Button]
//     void ReloadEnv()
//     {
//         JS.CreateEnv();
//     }
//
// #if UNITY_EDITOR
//        // [UnityEditor.Callbacks.DidReloadScripts]
//         static void OnReload()
//         {
//             if(m_instance != null && m_instance.EnableUpdate && JS.instance != null) {
//                 if(m_instance.EnableUpdate) {
//                     UnityEditor.EditorApplication.update -= JS.instance.checkUpdate;
//                     UnityEditor.EditorApplication.update += JS.instance.checkUpdate;
//                 } else {
//                     UnityEditor.EditorApplication.update -= JS.instance.checkUpdate;
//                 }
//             }
//             Debug.Log($"Conneced: {JS.IsConected}");
//         }
//
//         //[UnityEditor.InitializeOnLoadMethod, UnityEditor.InitializeOnEnterPlayMode]
//         static void CheckConnect()
//         {
//             Debug.Log($"Conneced: {JS.IsConected}");
//         }
// #endif
//
//
//     public void Update()
//     {
//         if(EnableUpdate) {
//             JS.instance?.checkUpdate();
//         }
//     }
//
// }
//
// }

