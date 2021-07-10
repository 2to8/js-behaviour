using Common;
using GameEngine.Extensions;
using MoreTags;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Helpers {

//Class to hold custom gui styles
public static class MyGUIStyles {

    static GUIStyle m_line = null;

    //constructor
    static MyGUIStyles()
    {
        m_line = new GUIStyle("box");
        m_line.border.top = m_line.border.bottom = 1;
        m_line.margin.top = m_line.margin.bottom = 1;
        m_line.padding.top = m_line.padding.bottom = 1;
    }

    public static GUIStyle EditorLine => m_line;

}

public class EventWindow : OdinEditorWindow {

    [MenuItem("Window/事件管理器")]
    static void Open()
    {
        var window = GetWindow(typeof(EventWindow));
        var texture = Resources.Load<Texture>("settings");
        window.titleContent = new GUIContent("事件", texture, "Just the tip");
    }

    static bool isTypeGeted;

    [DidReloadScripts]
    static void ScriptCompiled()
    {
        isTypeGeted = false;
    }

    Vector2 scrollPosition;

    Dictionary<Component, SerializedObject> serializedObjectDict = new Dictionary<Component, SerializedObject>();

    Dictionary<Component, SerializedObject> serializedObjectDictNext = new Dictionary<Component, SerializedObject>();

    readonly List<GameObject> targetGameObjects = new List<GameObject>();
    bool activeOnly = false;
    bool showFullPath = true;
    bool current = false;

    enum PickupEventType {

        All,
        EventExist,
        Missing,

    }

    PickupEventType pickupEvent = PickupEventType.All;

    public static void DrawUILine(Color color, int thickness = 1, int padding = 10)
    {
        var r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
        r.height = thickness;
        r.y += padding / 2;
        r.x -= 2;
        r.width += 6;
        EditorGUI.DrawRect(r, color);
    }

    protected override void OnGUI()
    {
        EditorGUIUtility.LookLikeInspector();

        if (isTypeGeted == false) {
            isTypeGeted = true;
            GetTargetClassMembers();
            ListupTargetGameObjects();
        }
        var position = new Rect();
        float toolbarHeight = 20;
        var btnWidth = 34;

        //GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(24));
        position.y = 1;

        // Tool bar
        // GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(10)});
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar,
            GUILayout.Height( /*EditorStyles.toolbar.fixedHeight*/toolbarHeight), GUILayout.ExpandWidth(true));
        {
            // GUI.skin.button.fixedHeight = 16;
            // GUI.skin.button.fontSize = 12;

            // GUILayout.BeginHorizontal(EditorStyles.toolbar);
            EditorGUI.BeginChangeCheck();
            {
                {
                    position.width = btnWidth;
                    position.height = toolbarHeight;

                    // EditorGUI.LabelField(position, " Event");
                    position.x += 5;
                    position.width = btnWidth;
                    pickupEvent = (PickupEventType)EditorGUI.EnumPopup(position, pickupEvent);
                }
                {
                    position.x += btnWidth + 8;
                    position.width = btnWidth;
                    current = EditorGUI.Toggle(position, current);
                    position.x += 18;
                    position.width = btnWidth;
                    EditorGUI.LabelField(position, "物体");
                }
                {
                    position.x += btnWidth;
                    position.width = btnWidth;
                    activeOnly = EditorGUI.Toggle(position, activeOnly);
                    position.x += 16;
                    position.width = btnWidth;
                    EditorGUI.LabelField(position, "场景");
                }
                {
                    position.x += btnWidth;
                    position.width = btnWidth;
                    showFullPath = EditorGUI.Toggle(position, showFullPath);
                    position.x += 16;
                    position.width = btnWidth;
                    EditorGUI.LabelField(position, "路径");
                }

                // {
                //     var width = 64;
                //     var height = 20;
                //
                //     //position.x = Screen.width - width;
                //     position.x += btnWidth;
                //     position.x += 16;
                //     position.width = width;
                //
                //     //position.height = height;
                //     GUI.Button(position, "Update");
                // }
            }

            if (EditorGUI.EndChangeCheck()) {
                ListupTargetGameObjects();
            }
        }
        GUILayout.Box(GUIContent.none, MyGUIStyles.EditorLine, GUILayout.ExpandWidth(true), GUILayout.Height(1f));
        EditorGUILayout.EndHorizontal();

        // GUILayout.FlexibleSpace();
        //
        // GUILayout.EndHorizontal();

        // var rect = EditorGUILayout.BeginHorizontal();
        // Handles.color = Color.black;
        // Handles.DrawLine(new Vector2(rect.x - 15, rect.y), new Vector2(rect.width + 15, rect.y));
        // EditorGUILayout.EndHorizontal();
        // EditorGUILayout.Space();

        //DrawUILine(Color.black);

        // Contents

        // GUI.skin.button.fixedHeight = 20;
        // GUI.skin.button.fontSize = 18;
        position.x = 0;
        position.y += toolbarHeight + 2;
        position.width = Screen.width / EditorGUIUtility.pixelsPerPoint;
        position.height = Screen.height / EditorGUIUtility.pixelsPerPoint - position.y - 22;

        // GUILayout.BeginHorizontal();
        // GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(1)});
        // GUILayout.EndHorizontal();
        GUILayout.BeginArea(position);
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        foreach (var gameObject in targetGameObjects) {
            if (null == gameObject) {
                continue;
            }
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Select")) //"慖戰"
                {
                    Selection.activeGameObject = gameObject;
                }

                if (!gameObject.GetComponent<XEventTrigger>()) {
                    if (GUILayout.Button("Create Trigger")) //"慖戰"
                    {
                        gameObject.RequireComponent<XEventTrigger>();
                    }
                }
                GUILayout.Label(GetObjectName(gameObject));
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginVertical("box");
            {
                var components = gameObject.GetComponents(typeof(Component));

                foreach (var component in components) {
                    EventMember eventMember;

                    if (targetTypeDict.TryGetValue(component.GetType(), out eventMember)) {
                        SerializedObject so;

                        if (!serializedObjectDict.TryGetValue(component, out so)) {
                            so = new SerializedObject(component);
                        }
                        so.Update();
                        serializedObjectDictNext[component] = so;
                        EditorGUILayout.LabelField(component.GetType().Name);

                        foreach (var member in eventMember.members) {
                            var prop = so.FindProperty(member);

                            if (prop != null) {
                                // fix: 添加绑定
                                var info = component.GetType()
                                    .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                    .FirstOrDefault(p => p.Name == member);
                                Assert.IsNotNull(info, member);

                                if (gameObject.GetComponent<XEventTrigger>() is XEventTrigger trigger) {
                                    if (trigger.triggers == null) {
                                        trigger.triggers =
                                            new Dictionary<Component, Dictionary<string, EventTriggerData>>();
                                    }

                                    if (!trigger.triggers.ContainsKey(component)) {
                                        trigger.triggers[component] = new Dictionary<string, EventTriggerData>();
                                    }

                                    if (!trigger.triggers[component].ContainsKey(member)) {
                                        trigger.triggers[component][member] = CreateInstance<EventTriggerData>();
                                        trigger.triggers[component][member].fieldInfo = info;
                                    }
                                    var target = trigger.triggers[component][member];
                                    GUILayout.BeginVertical("box");

                                    if (!target.editor) {
                                        Editor.CreateCachedEditor(target, null, ref target.editor);
                                    }
                                    target.target = gameObject;
                                    EditorGUI.BeginChangeCheck();
                                    target.editor.OnInspectorGUI();

                                    if (EditorGUI.EndChangeCheck()) {
                                        Debug.Log("changed");
                                        EditorUtility.SetDirty(target);
                                        Debug.Log("set dirty");
                                        trigger.triggers[component][member] = target;
                                        var tags = gameObject.RequireComponent<Tags>();
                                        target.IntTreview.ForEach(v => {
                                            if (v.Item1 > 0 && !tags.ids.Contains(v.Item1)) {
                                                Debug.Log(v.Item1);
                                                gameObject.AddTag(v.Item1);
                                            }
                                        });

                                        //
                                    }
                                    GUILayout.EndVertical();
                                }
                                EditorGUILayout.PropertyField(prop, true);
                            } else {
                                EditorGUILayout.LabelField("not find class:" +
                                    component.GetType() +
                                    " property:" +
                                    member);
                            }
                        }
                        so.ApplyModifiedProperties();
                    }
                }
            }
            GUILayout.EndVertical();
        }
        EditorGUILayout.EndScrollView();
        GUILayout.EndArea();
        SwapSerializedObjectDict();
    }

    /// <summary>
    /// 僞乕僎僢僩偲側傞GameObject儕僗僩傾僢僾
    /// </summary>
    void ListupTargetGameObjects()
    {
        targetGameObjects.Clear();
        GameObject[] gameObjectAll;

        if (activeOnly) {
            gameObjectAll = (GameObject[])FindObjectsOfType(typeof(GameObject));
        } else {
            gameObjectAll = (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject));
        }

        if (current && Selection.activeGameObject != null) {
            gameObjectAll = new[] { Selection.activeGameObject };
        } else {
            current = false;
        }

        foreach (var gameObject in gameObjectAll) {
            var components = gameObject.GetComponents(typeof(Component));

            if (null == components ||
                !components.Any(component =>
                    null != component && null != targetTypeDict && targetTypeDict.ContainsKey(component.GetType()))) {
                continue;
            }

            if (pickupEvent != PickupEventType.All) {
                if (!components.Any(component => IsEventExist(component))) {
                    continue;
                }
            }
            targetGameObjects.Add(gameObject);
        }
    }

    /// <summary>
    /// GameObject偺昞帵柤徧庢摼
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    string GetObjectName(GameObject obj)
    {
        if (showFullPath) {
            return GetFullPath(obj);
        }

        return obj.name;
    }

    string GetFullPath(GameObject obj)
    {
        var stack = new Stack<string>();
        var current = obj.transform;

        while (null != current) {
            stack.Push(current.name);
            current = current.parent;
        }
        var path = string.Join("/", stack.ToArray());

        return path;
    }

    bool IsEventExist(Component component)
    {
        EventMember eventMember;

        if (targetTypeDict.TryGetValue(component.GetType(), out eventMember)) {
            if (IsEventExist(component, eventMember)) {
                return true;
            }
        }

        return false;
    }

    bool IsEventExist(Component component, EventMember eventMember)
    {
        var type = component.GetType();

        foreach (var member in eventMember.members) {
            var members = type.GetMember(member, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            if (members == null || members.Length == 0) {
                //EditorGUILayout.LabelField("not found member:" + member);
            } else {
                var m = members[0];
                object obj = null;

                if (m.MemberType == MemberTypes.Field) {
                    var info = (FieldInfo)m;
                    obj = info.GetValue(component);
                } else if (m.MemberType == MemberTypes.Property) {
                    var info = (PropertyInfo)m;
                    obj = info.GetValue(component, null);
                }

                if (obj != null) {
                    if (obj.GetType().IsSubclassOf(typeof(UnityEventBase))) {
                        var eventBase = (UnityEventBase)obj;

                        if (0 < eventBase.GetPersistentEventCount()) {
                            return IsEventExist(eventBase);
                        }
                    } else if (obj.GetType() == typeof(List<EventTrigger.Entry>)) {
                        var list = (List<EventTrigger.Entry>)obj;

                        foreach (var entry in list) {
                            if (0 < entry.callback.GetPersistentEventCount()) {
                                return IsEventExist(entry.callback);
                            }
                        }
                    }
                }
            }
        }

        return false;
    }

    bool IsEventExist(UnityEventBase unityEvent)
    {
        for (var i = 0; i < unityEvent.GetPersistentEventCount(); i++) {
            var target = unityEvent.GetPersistentTarget(i);

            if (pickupEvent == PickupEventType.Missing) {
                if (IsMissing(target)) {
                    return true;
                }
            } else if (pickupEvent == PickupEventType.EventExist) {
                if (null != target) {
                    return true;
                }
            }
        }

        return false;
    }

    bool IsMissing(UnityEngine.Object obj)
    {
        try {
            var name = obj.name;
        } catch (MissingReferenceException) {
            return true;
        } catch {
            return false;
        }

        return false;
    }

    void SwapSerializedObjectDict()
    {
        var temp = serializedObjectDict;
        serializedObjectDict = serializedObjectDictNext;
        serializedObjectDictNext = temp;
        serializedObjectDictNext.Clear();
    }

    class EventMember {

        public Type type;
        public List<string> members = new List<string>();
        public Editor editor;

    }

    Dictionary<Type, EventMember> targetTypeDict = new Dictionary<Type, EventMember>();

    void GetTargetClassMembers()
    {
        targetTypeDict.Clear();

        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies()) {
            foreach (var type in asm.GetTypes()) {
                if (!type.IsSubclassOf(typeof(Component))) {
                    continue;
                }

                foreach (var info in type.GetFields(BindingFlags.Public | BindingFlags.Instance)) {
                    if (IsObsolete(info)) {
                        continue;
                    }

                    if (IsTargetType(info.FieldType)) {
                        EntryClassMember(type, info);
                    }
                }

                foreach (var info in type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)) {
                    if (IsObsolete(info)) {
                        continue;
                    }

                    if (!IsSerializeField(info)) {
                        continue;
                    }

                    if (IsTargetType(info.FieldType)) {
                        EntryClassMember(type, info);
                    }
                }

                foreach (var info in type.GetProperties(BindingFlags.Public |
                    BindingFlags.NonPublic |
                    BindingFlags.Instance)) {
                    if (IsObsolete(info)) {
                        continue;
                    }

                    if (!IsSerializeField(info)) {
                        continue;
                    }

                    if (IsTargetType(info.PropertyType)) {
                        EntryClassMember(type, info);
                    }
                }
            }
        }
    }

    void EntryClassMember(Type type, MemberInfo m)
    {
        // var attrString = string.Join(" ", Attribute.GetCustomAttributes(m).Select(_ => _.GetType().ToString()).ToArray());
        // var mes = "type:" + type + " name:" + m.Name + " attribute:" + attrString;
        // Debug.Log(mes);
        EventMember eventMember;

        if (!targetTypeDict.TryGetValue(type, out eventMember)) {
            eventMember = new EventMember { type = type };
            targetTypeDict[type] = eventMember;
        }
        eventMember.members.Add(m.Name);
    }

    static bool IsTargetType(Type type)
    {
        if (type.IsSubclassOf(typeof(UnityEventBase)) || type == typeof(List<EventTrigger.Entry>)) {
            return true;
        }

        return false;
    }

    static bool IsObsolete(MemberInfo memberInfo) => memberInfo.IsDefined(typeof(ObsoleteAttribute), true);

    static bool IsSerializeField(MemberInfo memberInfo) => memberInfo.IsDefined(typeof(SerializeField), true);

}

}