// using GameEngine.Extensions;
// using MoreTags;
// using NodeCanvas.Common.Design;
// using NodeCanvas.Framework;
// using Sirenix.OdinInspector;
// using Sirenix.Serialization;
// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// #if UNITY_EDITOR
// using UnityEditor;
// #endif
// using UnityEngine;
//
//
// namespace NodeCanvas {
//
// [CreateAssetMenu]
// public class TagEngine : SerializedScriptableObject {
//
//     static TagEngine m_Engine;
//
//     [OdinSerialize]
//     public Dictionary<string, TagTableData> TagTable = new Dictionary<string, TagTableData>();
//
//     public static bool isLoaded => m_Engine != null;
//
//     public static TagEngine instance {
//         get {
//             if (m_Engine == null) {
//                 m_Engine = Resources.LoadAll<TagEngine>("").FirstOrDefault();
//             #if UNITY_EDITOR
//                 if (m_Engine == null) {
//                     m_Engine = CreateInstance<TagEngine>();
//                     if (!Directory.Exists(Application.dataPath + "/Resources/Config")) {
//                         Directory.CreateDirectory(Application.dataPath + "/Resources/Config");
//                     }
//                     UnityEditor.AssetDatabase.CreateAsset(m_Engine,
//                         $"Assets/Resources/Config/{typeof(TagEngine).Name}.asset");
//                     UnityEditor.AssetDatabase.SaveAssets();
//                 }
//
//                 // Add the config asset to the build
//                 var preloadedAssets = UnityEditor.PlayerSettings.GetPreloadedAssets().ToList();
//                 if (preloadedAssets.All(t => t.GetType() != typeof(TagEngine))) {
//                     preloadedAssets.Add(m_Engine);
//                     UnityEditor.PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
//                 }
//             #endif
//             }
//             return m_Engine;
//         }
//         private set => m_Engine = value;
//     }
//
//     public void Display(Node node, float width = Single.NaN)
//     {
//         //var go = (target as Tags).gameObject;
//     #if UNITY_EDITOR
//         if (node.tagGUI == null) {
//             node.tagGUI = new TagGUI();
//             if (!width.eq(Single.NaN)) {
//                 node.tagGUI.maxWidth = width;
//             }
//             node.tagGUI.OnItemString += TagManagerEditor.OnItemString;
//             node.tagGUI.OnItemColor += TagManagerEditor.OnItemColor;
//             node.tagGUI.OnRightClickItem += TagManagerEditor.OnRightClickItem;
//             node.tagGUI.OnAddItem += (item) => {
//                 if (!string.IsNullOrEmpty(item))
//                     AddTag(node, item);
//                 else {
//                     var menu = new GenericMenu();
//                     foreach (var tag in TagPreset.GetPresets().Union(TagSystem.GetAllTags()).Except(node.GetTags()))
//                         menu.AddItem(new GUIContent(tag), false, () => AddTag(node, tag));
//                     menu.ShowAsContext();
//                 }
//             };
//             node.tagGUI.OnClickItem += (item) => {
//                 RemoveTag(node, item);
//             };
//             node.AddGameObjectTag();
//         }
//         node.tagGUI.OnGUI(node.GetTags().OrderBy(tag => TagPreset.GetTagOrder(tag)));
//     #endif
//     }
//
//     void RemoveTag(Node node, string tag)
//     {
//         node.RemoveTag(tag);
//         UndoUtility.SetDirty(node.graph);
//         Debug.Log($"[Remove: node.tag] {node.tag}");
//
//         //EditorSceneManager.MarkSceneDirty(go.scene);
//     }
//
// #if UNITY_EDITOR
//     [UnityEditor.Callbacks.DidReloadScripts]
// #endif
//     [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
//     static void InitLoad()
//     {
//         instance.test();
//     }
//
//     void test()
//     {
//         Debug.Log("[TagEngine] Data Loaded");
//     }
//
//     void OnEnable() => instance = this;
//
//     void OnValidate()
//     {
//     #if UNITY_EDITOR
//         if (this != null && Application.isPlaying) {
//             if (AssetDatabase.GetAssetPath(this) != null) {
//                 UnityEditor.EditorUtility.SetDirty(this);
//                 UnityEditor.AssetDatabase.SaveAssets();
//             }
//         }
//     #endif
//     }
//
//     void OnDisable() => OnValidate();
//
//     void AddTag(Node node, string tag)
//     {
//         if (!TagSystem.AllTags().Contains(tag)) {
//             TagSystem.AddTag(tag);
//         #if UNITY_EDITOR
//             TagSystem.SetTagColor(tag, TagPreset.GetPresetColor(tag));
//         #endif
//         }
//         node.AddTag(tag);
//     #if UNITY_EDITOR
//         UndoUtility.SetDirty(node.graph);
//     #endif
//         Debug.Log($"[Add: node.tag]  {node.tag}");
//
//         //EditorSceneManager.MarkSceneDirty(go.scene);
//     }
//
// }
//
// }

