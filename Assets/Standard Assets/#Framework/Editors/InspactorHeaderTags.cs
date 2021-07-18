// using FlowCanvas;

#if UNITY_EDITOR
using GameEngine.Extensions;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Editor;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using Org.BouncyCastle.Crypto.Engines;
using ParadoxNotion.Design;

// using NodeCanvas.BehaviourTrees;
// using NodeCanvas.Framework;
// using NodeCanvas.StateMachines;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FlowCanvas;
using UniRx.Async;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.Rendering;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// using WebSocketSharp;
using Object = UnityEngine.Object;

namespace MoreTags
{
    /// <summary>
    /// https://docs.unity.cn/ScriptReference/Editor-finishedDefaultHeaderGUI.html
    /// </summary>
    [InitializeOnLoad]
    public static class EditorHeaderGUID
    {
        static Dictionary<GameObject, bool> allows = new Dictionary<GameObject, bool>();

        static EditorHeaderGUID()
        {
            Editor.finishedDefaultHeaderGUI += DisplayGUIDIfPersistent;
        }

        static bool Option_ExtraHeader = true;
        static bool Option_ShowHeaderGearButton;

        static void MakeController(GameObject go)
        {
            var ani = go.RequireComponent<Animator>();
            if (ani.runtimeAnimatorController == null) {
                var path = PrefabStageUtility.GetPrefabStage(go)?.assetPath;
                if (string.IsNullOrEmpty(path)) {
                    path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(go);
                }

                if (!string.IsNullOrEmpty(path)) {
                    Debug.Log(path);
                    var file = path.Replace(".prefab", "") + $"/{go.name}.controller";
                    if (!Directory.Exists(Path.GetDirectoryName(file))) {
                        Directory.CreateDirectory(Path.GetDirectoryName(file));
                    }

                    // Creates the controller
                    // https://docs.unity3d.com/ScriptReference/Animations.AnimatorController.html
                    var controller = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath(file);
                    ani.runtimeAnimatorController = controller;
                }
            }

            AssetDatabase.OpenAsset(ani.runtimeAnimatorController);
        }

        public static T CreateGraphOwner<T>(this GameObject go, bool asBound = true, bool open = true)
            where T : GraphOwner
        {
            var owner = go.RequireComponent<T>();

            //create new local graph and assign it to owner
//        public Graph NewAsBound()
//        {
            if (owner.graph == null) {
                if (asBound) {
                    var newGraph = (Graph) ScriptableObject.CreateInstance(owner.graphType);
                    if (newGraph != null) {
                        UndoUtility.RecordObject(owner, "New Bound Graph");
                        owner.SetBoundGraphReference(newGraph);
                        UndoUtility.SetDirty(owner);
                        owner.graph = newGraph;
                    }

                    //owner.Validate();
                    //GraphEditor.OpenWindow(owner);
                }
                else {
                    owner.graph = GraphEditor.NewAsAsset(owner);
                }
            }

//            return newGraph;
//        }
            if (owner.graph != null) {
                owner.Validate();
                if (open) {
                    GraphEditor.OpenWindow(owner);
                }
            }

            return owner;

            // if (owner.graph == null) {
            //     var newGraph = (Graph)ScriptableObject.CreateInstance(owner.graphType);
            //     UndoUtility.RecordObject(owner, "New Bound Graph");
            //     owner.SetBoundGraphReference(newGraph);
            //     UndoUtility.SetDirty(owner);
            // }
            // GraphEditor.OpenWindow(owner.graph);
        }

        static void DisplayGUIDIfPersistent(Editor editor)
        {
            //用以判断，只有GameObject类型的对象才定义他的页眉UI
            var go = editor.target as GameObject;
            if (go == null) {
                return;
            }

            var m_GameObjectTag = OnEnable(go);

            //if (!EditorUtility.IsPersistent(editor.target)) return;
            // var guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(editor.target));
            // var totalRect = EditorGUILayout.GetControlRect();
            // var controlRect = EditorGUI.PrefixLabel(totalRect, EditorGUIUtility.TrTempContent("GUID"));
            // if (editor.targets.Length > 1)
            //     EditorGUI.LabelField(controlRect, EditorGUIUtility.TrTempContent("[Multiple objects selected]"));
            // else
            //     EditorGUI.SelectableLabel(controlRect, guid);

            // var go = Editor.target as GameObject;

            // ==================================================================
            // if (!go.scene.isLoaded) {
            //     EditorGUILayout.HelpBox(
            //         "Tags should be attached to on scene GameObject. Please use Prefab Tags for off scene GameObject.",
            //         MessageType.Warning);
            //     return;
            // }

            //TagSystem.CheckTagManager(go.scene);
            var list = new List<GameObject>();
            editor.targets.ForEach(t => {
                if (t is GameObject tgo) {
                    list.Add(tgo);
                }
            });

            //.Cast<Transform>().Select(item => item.gameObject);
            allows[go] = go.GetComponent<Tags>()?.enabled == true;
            var tags = go.GetComponent<Tags>();
            var idStr = "";
            var value = PrefabUtility.IsOutermostPrefabInstanceRoot(go);
            var oldValue = value;
            var ids = go.GetComponent<Tags>()?.ids ?? new List<int>();
            if (tags != null && Option_ExtraHeader) {
                idStr = ids.Any() ? $"Tags ID: {string.Join(", ", ids)} " : string.Empty;

                //value = tags.Asset != null;
            }

            GUILayout.BeginHorizontal();
            {
                var originalValue = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 0;

                // if (GUILayout.Button("FC", GUILayout.Width(30.0f))) {
                //     var graph = go.RequireComponent<FlowScriptController>();
                //     GraphEditor.OpenWindow(graph);
                // }

                // if (GUILayout.Button("BT", GUILayout.Width(30.0f))) {
                //     var graph = go.RequireComponent<BehaviourTreeOwner>();
                //     GraphEditor.OpenWindow(graph);
                // }

                //MyBoolValue = EditorGUILayout.Toggle("My Long Description Text Here", MyBoolValue);
                value = EditorGUILayout.ToggleLeft("", oldValue, GUIStyle.none, GUILayout.Width(16.0f));
                var catpion = $"{idStr}[#id] {go.GetInstanceID()}";
                Rect createBtnRect = GUILayoutUtility.GetRect(new GUIContent(catpion), EditorStyles.toolbarDropDown,
                    GUILayout.ExpandWidth(true));
                if (GUI.Button(createBtnRect, catpion, EditorStyles.toolbarDropDown)) {
                    // GenericMenu menu = new GenericMenu();
                    void handleItemClicked(object parameter)
                    {
                        Debug.Log(parameter);
                    }

                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Item 1"), false, handleItemClicked, "Item 1");
                    menu.AddItem(new GUIContent("Item 2"), false, handleItemClicked, "Item 2");
                    menu.AddItem(new GUIContent("Item 3"), false, handleItemClicked, "Item 3");
                    menu.DropDown(createBtnRect);
                }

                if (value != oldValue) {
                    if (value) {
                        Debug.Log(go._GetPrefabSavePath());
                        var prefab = PrefabUtility.SaveAsPrefabAssetAndConnect(go, go._GetPrefabSavePath(),
                            InteractionMode.AutomatedAction);
                        prefab.SyncAssets();
                    }
                    else {
                        PrefabUtility.UnpackPrefabInstance(go, PrefabUnpackMode.OutermostRoot,
                            InteractionMode.AutomatedAction);
                    }

                    EditorSceneManager.MarkSceneDirty(go.scene);
                }

                // if (EditorGUILayout.DropdownButton(new GUIContent($"{idStr}[id] {go.GetInstanceID()}"),
                //     FocusType.Passive)) {
                //     void handleItemClicked(object parameter)
                //     {
                //         Debug.Log(parameter);
                //     }
                //
                //     GenericMenu menu = new GenericMenu();
                //     menu.AddItem(new GUIContent("Item 1"), false, handleItemClicked, "Item 1");
                //     menu.AddItem(new GUIContent("Item 2"), false, handleItemClicked, "Item 2");
                //     menu.AddItem(new GUIContent("Item 3"), false, handleItemClicked, "Item 3");
                //    //  int controlID = GUIUtility.GetControlID (FocusType.Passive);
                //    // var t =  Event.current.GetTypeForControl(controlID);
                //    var oy = EditorStyles.toolbar.fixedHeight - 2;
                //    menu.DropDown(new Rect(1, oy, 1, 1));
                // }

                //EditorGUILayout.LabelField($"");
                if (Option_ShowHeaderGearButton) {
                    var popupStyle = GUI.skin.FindStyle("IconButton");
                    var popupIcon = EditorGUIUtility.IconContent("_Popup");
                    var buttonRect = EditorGUILayout.GetControlRect(false, 20f, GUILayout.MaxWidth(20f));
                    if (EditorGUI.DropdownButton(buttonRect, popupIcon, FocusType.Passive, popupStyle)) {
                        //Stuff that happens when you click the button
                        void handleItemClicked(object parameter)
                        {
                            Debug.Log(parameter);
                        }

                        //
                        // File.WriteAllBytes(Application.persistentDataPath+"/abc.bytes",JsonUtility.ToJson(Obj));
                        // var obj = ScriptableObject.CreateInstance<myobj>();
                        // var obj = JsonUtility.FromJson<myobj>(File.ReadAllText(Application.persistentDataPath + "/abc.bytes"));
                        GenericMenu menu = new GenericMenu();
                        menu.AddItem(new GUIContent("Item 1"), false, handleItemClicked, "Item 1");
                        menu.AddItem(new GUIContent("Item 2"), false, handleItemClicked, "Item 2");
                        menu.AddItem(new GUIContent("Item 3"), false, handleItemClicked, "Item 3");
                        menu.DropDown(buttonRect);
                    }
                }

                if (GUILayout.Button("#BT", GUILayout.Width(45))) {
                    // MakeController(go);
                    CreateGraphOwner<BehaviourTreeOwner>(go, true);
                }

                if (GUILayout.Button("#FSM", GUILayout.Width(45))) {
                    CreateGraphOwner<FSMOwner>(go, true);
                }

                if (GUILayout.Button("#FL", GUILayout.Width(45))) {
                    CreateGraphOwner<FlowScriptController>(go, true);
                }

                if (GUILayout.Button("+", GUILayout.Width(20))) {
                    go.GetComponents(typeof(Component)).ForEach((c, n) => {
                        if (c is Transform || c is Image) {
                            InternalEditorUtility.SetIsInspectorExpanded(c, true);
                        }
                        else {
                            InternalEditorUtility.SetIsInspectorExpanded(c, false);
                        }
                    });
                    ActiveEditorTracker.sharedTracker.ForceRebuild();
                }

                if (GUILayout.Button("-", GUILayout.Width(20))) {
                    go.GetComponents(typeof(Component)).ForEach(c => {
                        if (!(c is Transform)) {
                            InternalEditorUtility.SetIsInspectorExpanded(c, false);
                        }
                    });
                    ActiveEditorTracker.sharedTracker.ForceRebuild();
                }

                //int selected = TagManager.Instance.Elements.ContainsKey(tags.elementId);
                if (false) {
                    // var options = TagManager.Instance.Elements.Keys.ToList();
                    // var index = options.IndexOf(TagManager.Instance.Elements.Values
                    //         .FirstOrDefault(t => t.Id == tags.id)
                    //         ?.name
                    //     ?? "None");
                    // EditorGUIUtility.labelWidth = 60;
                    // index = EditorGUILayout.Popup("Element:", index,
                    //     options.Select(t => t.Replace(".", "/")).ToArray());
                }

                //tags.sid = TagManager.Instance.Elements[options[index]].Id;

                // id = EditorGUILayout.TextField("Element:", id);
                // if (id != go.name) {
                //     t1.id = id;
                // }
                EditorGUIUtility.labelWidth = originalValue;
                GUILayout.EndHorizontal();
            }

            // if (ids.Any()) {
            //     tags.Prefab = value;
            // }
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                // allows[go] = EditorGUILayout.BeginToggleGroup(
                //     $"{idStr}[id] {go.GetInstanceID()}", allows[go]);

                //list
                // if (!list.Skip(1).Any()) {
                m_GameObjectTag.OnGUI(Extensions.GetTags(go));

                //  // } else {
                //      TagManagerEditor.ShowGameObjectsTag(list);
                // // }

                // EditorGUILayout.EndToggleGroup();
                if (allows[go]) {
                    (go.GetComponent<Tags>() ?? go.AddComponent<Tags>()).enabled = true;
                }
                else if (tags != null) {
                    // if (!go.GetTags().Any()) {
                    //     Object.DestroyImmediate(tags);
                    // } else {
                    tags.enabled = false;

                    // }
                }

                EditorGUILayout.EndVertical();
            }

            // editor.Repaint();
        }

        // static TagGUI m_GameObjectTag;

        static TagGUI OnEnable(GameObject go)
        {
            //var go = (target as Tags).gameObject;
            var m_GameObjectTag = new TagGUI();
            m_GameObjectTag.OnItemString += TagManagerEditor.OnItemString;
            m_GameObjectTag.OnItemColor += TagManagerEditor.OnItemColor;
            m_GameObjectTag.OnRightClickItem += TagManagerEditor.OnRightClickItem;
            m_GameObjectTag.OnAddItem += (item) => {
                if (!string.IsNullOrEmpty(item)) {
                    AddTag(go, item);
                }
                else {
                    var menu = new GenericMenu();
                    foreach (var tag in TagPreset.GetPresets().Union(TagSystem.Tags()).Except(go.GetTags())) {
                        menu.AddItem(new GUIContent(tag.Replace(".", "/")), false, () => AddTag(go, tag));
                    }

                    menu.ShowAsContext();
                }
            };
            m_GameObjectTag.OnClickItem += (item) => { RemoveTag(go, item); };
            return m_GameObjectTag;
        }

        static void RemoveTag(GameObject go, string tag)
        {
            go.RemoveTag(tag);
            EditorSceneManager.MarkSceneDirty(go.scene);
            Debug.Log("set dirty");
        }

        static void AddTag(GameObject go, string tag)
        {
            if (!TagSystem.AllTags().Contains(tag)) {
                TagSystem.AddTag(tag);
                TagSystem.SetTagColor(tag, TagPreset.GetPresetColor(tag));
            }

            go.AddTag(tag);
            go.RequireComponent<Tags>().enabled = true;
            EditorSceneManager.MarkSceneDirty(go.scene);
            Debug.Log("set dirty");
        }
    }
}
#endif