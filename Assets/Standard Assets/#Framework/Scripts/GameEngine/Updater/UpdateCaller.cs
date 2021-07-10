// using System;
// using UniRx.Async;
//
// #if UNITY_EDITOR
// using UnityEditor;
// using UnityEditor.Callbacks;
// #endif
//
// using UnityEngine;
// using UnityEngine.Networking.PlayerConnection;
// using UnityEngine.SceneManagement;
//
// namespace GameEngine.Updater {
//
// [ExecuteAlways]
// public class UpdateCaller : MonoBehaviour {
//
//     private static UpdateCaller instance;
//
// #if UNITY_EDITOR
//     [InitializeOnLoadMethod]
//     static void OnProjectLoadedInEditor()
//     {
//         Debug.Log("[InitializeOnLoadMethod] Project loaded in Unity Editor");
//         LoadAttach();
//     }
//
//     [DidReloadScripts]
//     static void OnDidReloadScripts()
//     {
//         Debug.Log("[DidReloadScripts] Project loaded in Unity Editor");
//         LoadAttach();
//     }
//
//     static void LoadAttach(bool unload = false)
//     {
//         EditorApplication.playModeStateChanged -= PlaymodeChanged;
//         EditorApplication.update -= OnEditorUpdate;
//         EditorApplication.delayCall -= OnDelayCall;
//         UnityEditor.SceneManagement.EditorSceneManager.sceneLoaded -= OnSceneLoaded;
//
//         if (!unload) {
//             EditorApplication.playModeStateChanged += PlaymodeChanged;
//             EditorApplication.update += OnEditorUpdate;
//             EditorApplication.delayCall += OnDelayCall;
//             UnityEditor.SceneManagement.EditorSceneManager.sceneLoaded += OnSceneLoaded;
//         }
//     }
//
//     static void PlaymodeChanged(PlayModeStateChange mode)
//     {
//         Debug.Log($"{mode}");
//
//         if (!EditorApplication.isCompiling) {
//             LoadAttach(mode != PlayModeStateChange.EnteredEditMode);
//         }
//
//         if (!EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isCompiling) {
//             //if (mode == PlayModeStateChange.ExitingEditMode) {
//             //}
//         }
//     }
//
//     static void OnEditorUpdate() { }
//
//     static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//     {
//         Debug.Log("scene loaded");
//     }
//
//     static void OnDelayCall() { }
// #endif
//
//     public static void AddUpdateCallback(Action updateMethod)
//     {
//         if (instance == null) {
//             instance = new GameObject("[Update Caller]").AddComponent<UpdateCaller>();
//             instance.transform.SetParent(Camera.main?.transform);
//         }
//         instance.updateCallback += updateMethod;
//     }
//
//     private Action updateCallback;
//
//     private void Update()
//     {
//         updateCallback();
//     }
//
// }
//
// }

