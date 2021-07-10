using Puerts;
using Puerts.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
#endif

namespace Common.JSRuntime.Javacripts {

//只是演示纯用js实现MonoBehaviour逻辑的可能，
//但从性能角度这并不是最佳实践，会导致过多的跨语言调用
public class JsBehaviour : MonoBehaviour {

    public bool InitializeOnLoad = true;
    public string moduleName; //可配置加载的js模块
    public Action _jsAwake;
    public Action _jsStart;
    public Action _jsUpdate;
    public Action _jsFixedUpdate;
    public Action _jsOnDisable;
    public Action _jsOnEnable;
    public Action _jsOnDestroy;
    public static JsEnv _jsEnv => JsEnvExt.Alive;
    public JsArg[] args;
    public JsArgInt[] argsInt;
    public JsArgFloat[] argsFloat;
    public JsArgBool[] argsBool;
    public JsArgString[] argsString;

    public string GetPath(string moduleName) => null;

    void Awake()
    {
        if (!string.IsNullOrEmpty(moduleName) && InitializeOnLoad) {
            _jsEnv.Eval<Action<JsBehaviour>>($"require('{GetPath(moduleName)}').init;")(this);
            _jsAwake?.Invoke();
        }
    }

    public void InitCS()
    {
        _jsEnv.Eval<Action<JsBehaviour>>($"require('{GetPath(moduleName)}').init;")(this);
        Init();
    }

    public void Init()
    {
        _jsAwake?.Invoke();
        _jsStart?.Invoke();
    }

    void Start()
    {
        if (!string.IsNullOrEmpty(moduleName) && InitializeOnLoad) {
            _jsStart?.Invoke();
        }
    }

    public int GetInt(string propertyName)
    {
        for (var i = 0; i < argsInt.Length; i++) {
            if (argsInt[i].name == propertyName) {
                return argsInt[i].value;
            }
        }

        return 0;
    }

    public List<int> GetIntList(string listName)
    {
        var intList = new List<int>();

        for (var i = 0; i < argsInt.Length; i++) {
            if (argsInt[i].name == listName) {
                intList.Add(argsInt[i].value);
            }
        }

        return intList;
    }

    public bool GetBool(string propertyName)
    {
        for (var i = 0; i < argsBool.Length; i++) {
            if (argsBool[i].name == propertyName) {
                return argsBool[i].value;
            }
        }

        return false;
    }

    public List<bool> GetBoolList(string listName)
    {
        var boolList = new List<bool>();

        for (var i = 0; i < argsBool.Length; i++) {
            if (argsBool[i].name == listName) {
                boolList.Add(argsBool[i].value);
            }
        }

        return boolList;
    }

    public string GetString(string propertyName)
    {
        for (var i = 0; i < argsString.Length; i++) {
            if (argsString[i].name == propertyName) {
                return argsString[i].value;
            }
        }

        return null;
    }

    public List<string> GetStringList(string listName)
    {
        var stringList = new List<string>();

        for (var i = 0; i < argsString.Length; i++) {
            if (argsString[i].name == listName) {
                stringList.Add(argsString[i].value);
            }
        }

        return stringList;
    }

    public float GetFloat(string propertyName)
    {
        for (var i = 0; i < argsFloat.Length; i++) {
            if (argsFloat[i].name == propertyName) {
                return argsFloat[i].value;
            }
        }

        return 0f;
    }

    public List<float> GetFloatList(string listName)
    {
        var floatList = new List<float>();

        for (var i = 0; i < argsFloat.Length; i++) {
            if (argsFloat[i].name == listName) {
                floatList.Add(argsFloat[i].value);
            }
        }

        return floatList;
    }

    public Object GetObject(string propertyName)
    {
        for (var i = 0; i < args.Length; i++) {
            if (args[i].name == propertyName) {
                return args[i].value;
            }
        }

        return null;
    }

    public List<Object> GetObjectList(string listName)
    {
        var objectList = new List<Object>();

        for (var i = 0; i < args.Length; i++) {
            if (args[i].name == listName) {
                objectList.Add(args[i].value);
            }
        }

        return objectList;
    }

    void Update()
    {
        if (_jsEnv.IsDisposed() == false) {
            _jsUpdate?.Invoke();
            _jsEnv.Tick();
        }
    }

    void FixedUpdate()
    {
        _jsFixedUpdate?.Invoke();
    }

    void OnDisable()
    {
        _jsOnDisable?.Invoke();
    }

    void OnEnable()
    {
        _jsOnEnable?.Invoke();
    }

    void OnDestroy()
    {
        _jsOnDestroy?.Invoke();
        _jsAwake = null;
        _jsStart = null;
        _jsUpdate = null;
        _jsFixedUpdate = null;
        _jsOnDisable = null;
        _jsOnEnable = null;
        _jsOnDestroy = null;
    }

}

[Serializable]
public struct JsArg {

    public string name;
    public Object value;

}

[Serializable]
public struct JsArgInt {

    public string name;
    public int value;

}

[Serializable]
public struct JsArgFloat {

    public string name;
    public float value;

}

[Serializable]
public struct JsArgBool {

    public string name;
    public bool value;

}

[Serializable]
public struct JsArgString {

    public string name;
    public string value;

}

#if UNITY_EDITOR
[CustomEditor(typeof(JsBehaviour)), CanEditMultipleObjects, PuertsIgnore]
public class JsBindingEditor : Editor {

    JsBehaviour ins;
    SerializedProperty argsProp;
    SerializedProperty argsIntProp;
    SerializedProperty argsFloatProp;
    SerializedProperty argsBoolProp;
    SerializedProperty argsStringProp;
    SerializedProperty moduleProp;
    SerializedProperty initializeOnLoadProp;

    //当前选中行
    int select;
    int selectInt;
    int selectFloat;
    int selectBool;
    int selectString;

    //组件缓存
    Dictionary<SerializedProperty, State> components;
    readonly float ObjectPropertyWidth = 200f;
    readonly float ComponentPropertyWidth = 200f;

    void OnEnable()
    {
        ins = target as JsBehaviour;
        argsProp = serializedObject.FindProperty("args");
        argsIntProp = serializedObject.FindProperty("argsInt");
        argsFloatProp = serializedObject.FindProperty("argsFloat");
        argsBoolProp = serializedObject.FindProperty("argsBool");
        argsStringProp = serializedObject.FindProperty("argsString");
        moduleProp = serializedObject.FindProperty("moduleName");
        initializeOnLoadProp = serializedObject.FindProperty("InitializeOnLoad");

        //Debug.Log("OnEnable");
        select = -1;

        if (components != null) {
            components.Clear();
        }
        components = new Dictionary<SerializedProperty, State>();
    }

    void OnDisable()
    {
        //Debug.Log("OnDisable");
        if (components != null) {
            components.Clear();
        }
        components = null;
    }

    List<Object> GetCompoents(Object obj)
    {
        if (obj != null) {
            var lst = new List<Object>() { };
            var type = obj.GetType();

            //使用反射, 获取GameObject和Transform组件
            var gameObjectProperty = type.GetProperty("gameObject");
            var transformProperty = type.GetProperty("transform");
            var gobj = (gameObjectProperty == null ? null : gameObjectProperty.GetValue(obj, null)) as GameObject;
            var trf = (transformProperty == null ? null : transformProperty.GetValue(obj, null)) as Transform;

            //使用反射调用GetComponents方法, 获取所有组件, 如果有gameObject则从Gameobject对象中获取所有组件(排除obj自身对排序的干扰)
            if (gobj != null) {
                type = gobj.GetType();
            }
            var get_components = (from method in type.GetMethods()
                where method.Name == "GetComponents" &&
                    method.ReturnType == typeof(Component[]) &&
                    method.GetParameters().Length == 1 &&
                    method.GetParameters()[0].ParameterType == typeof(Type)
                select method).FirstOrDefault();

            if (get_components != null) {
                var components = get_components.Invoke(gobj ?? obj, new object[] {
                    typeof(Component),
                }) as Component[];

                foreach (var o in components) {
                    if (!lst.Contains(o)) {
                        lst.Add(o);
                    }
                }
            }

            //obj自身
            if (!lst.Contains(obj)) {
                lst.Add(obj);
            }

            //通过Type名进行排序
            lst = (from o in lst orderby o.GetType().Name select o).ToList();

            //GameObject / Transform
            if (trf != null) {
                lst.Remove(trf);
                lst.Insert(0, trf);
            }

            if (gobj != null) {
                lst.Remove(gobj);
                lst.Insert(0, gobj);
            }

            return lst;
        }

        return new List<Object>() { };
    }

    State GetState(SerializedProperty prop)
    {
        State v;

        if (!components.TryGetValue(prop, out v) || v.refObject != prop.objectReferenceValue) {
            var _components = GetCompoents(prop.objectReferenceValue);
            var _names = (from c in _components where c != null select c.GetType().Name).ToArray();
            var _index = _components.IndexOf(prop.objectReferenceValue);

            //重命名同类型组件
            var _name_dict = new Dictionary<string, int>();

            foreach (var _name in _names) {
                if (_name_dict.ContainsKey(_name)) {
                    var _count = _name_dict[_name];
                    _name_dict[_name] = _count == 0 ? 2 : _count++;
                } else {
                    _name_dict.Add(_name, 0);
                }
            }

            for (var i = _names.Length - 1; i >= 0; i--) {
                var _count = _name_dict[_names[i]];

                if (_count > 0) {
                    _name_dict[_names[i]] = _count - 1;
                    _names[i] += "(" + _count + ")";
                }
            }
            v = new State(_index, _names, _components);
            v.refObject = prop.objectReferenceValue;
            components.Remove(prop);
            components.Add(prop, v);
        }

        return v;
    }

    void SetState(SerializedProperty prop, State state)
    {
        if (state.index >= 0 && state.index < state.components.Count) {
            prop.objectReferenceValue = state.components[state.index];
        }
        components.Remove(prop);
        components.Add(prop, state);
    }

    public override void OnInspectorGUI()
    {
        if (ins == null || argsProp == null) {
            base.OnInspectorGUI();

            return;
        }
        serializedObject.Update();
        GUI.enabled = false;
        EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((JsBehaviour)target), typeof(JsBehaviour),
            false);
        GUI.enabled = true;
        EditorGUILayout.PropertyField(initializeOnLoadProp);
        EditorGUILayout.PropertyField(moduleProp);

    #region Args Menu

        //Args Menu
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("====Object Property====");
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Size", GUILayout.Width(50f));
        argsProp.arraySize = EditorGUILayout.IntField(argsProp.arraySize, GUILayout.Width(50f));
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("○")) {
            //Refresh
            if (components != null) {
                components.Clear();
            }
        }
        GUILayout.Space(5f);

        if (GUILayout.Button("+")) {
            //Add Row
            argsProp.arraySize++;
            select = argsProp.arraySize - 1;

            //Name
            argsProp.GetArrayElementAtIndex(select).FindPropertyRelative("name").stringValue = "arg" + select;
        }

        if (GUILayout.Button("-")) {
            //Remove Row
            if (select >= 0) {
                argsProp.DeleteArrayElementAtIndex(select);

                if (select >= argsProp.arraySize) {
                    @select = argsProp.arraySize - 1;
                }
            } else if (argsProp.arraySize > 0) {
                argsProp.arraySize--;
            }
        }
        GUILayout.Space(5f);

        if (GUILayout.Button("↑") && select > 0) {
            //Move Up Row
            argsProp.MoveArrayElement(select, --select);
        }

        if (GUILayout.Button("↓") && select >= 0 && select < argsProp.arraySize - 1) {
            //Move Down Row
            argsProp.MoveArrayElement(select, ++select);
        }
        EditorGUILayout.EndHorizontal();

        //Args Title
        EditorGUILayout.BeginHorizontal();
        argsProp.isExpanded = EditorGUILayout.Foldout(argsProp.isExpanded, "Name");
        GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField("Object", GUILayout.Width(ObjectPropertyWidth));
        EditorGUILayout.LabelField("Component", GUILayout.Width(ComponentPropertyWidth));
        EditorGUILayout.EndHorizontal();

        //Element Array
        if (argsProp.isExpanded) {
            for (var i = 0; i < argsProp.arraySize; i++) {
                var element = argsProp.GetArrayElementAtIndex(i);
                var el_name = element.FindPropertyRelative("name");
                var el_value = element.FindPropertyRelative("value");
                var state = GetState(el_value);
                var index = state.index;
                var tog = select == i;

                //GUI
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(10f);
                var g_name = GUILayout.TextField(el_name.stringValue) ?? "";
                var g_obj = EditorGUILayout.ObjectField(el_value.objectReferenceValue, typeof(Object), true,
                    GUILayout.Width(ObjectPropertyWidth));
                var g_index = EditorGUILayout.Popup(index, state.names, GUILayout.Width(ComponentPropertyWidth));
                var g_tog = EditorGUILayout.Toggle(tog, GUILayout.Width(15f));
                EditorGUILayout.EndHorizontal();

                //GUI Update
                el_name.stringValue = g_name;

                if (el_value.objectReferenceValue != g_obj) {
                    var before_type = state.Now();

                    //New Object
                    el_value.objectReferenceValue = g_obj;
                    state = GetState(el_value);
                    index = int.MinValue;
                    g_index = state.IndexOf(before_type);
                }

                if (g_index != index) {
                    state.index = g_index;
                    SetState(el_value, state);
                }

                if (g_tog) {
                    @select = i;
                } else if (tog) {
                    @select = -1;
                }
            }
        }

    #endregion

    #region ArgsInt Menu

        //ArgsInt Menu
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("====Int Property====");
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Size", GUILayout.Width(50f));
        argsIntProp.arraySize = EditorGUILayout.IntField(argsIntProp.arraySize, GUILayout.Width(50f));
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("+")) {
            //Add Row
            argsIntProp.arraySize++;
            selectInt = argsIntProp.arraySize - 1;

            //Name
            argsIntProp.GetArrayElementAtIndex(selectInt).FindPropertyRelative("name").stringValue = "arg" + selectInt;
        }

        if (GUILayout.Button("-")) {
            //Remove Row
            if (selectInt >= 0) {
                argsIntProp.DeleteArrayElementAtIndex(selectInt);

                if (selectInt >= argsIntProp.arraySize) {
                    selectInt = argsIntProp.arraySize - 1;
                }
            } else if (argsIntProp.arraySize > 0) {
                argsIntProp.arraySize--;
            }
        }
        GUILayout.Space(5f);

        if (GUILayout.Button("↑") && selectInt > 0) {
            //Move Up Row
            argsIntProp.MoveArrayElement(selectInt, --selectInt);
        }

        if (GUILayout.Button("↓") && selectInt >= 0 && selectInt < argsIntProp.arraySize - 1) {
            //Move Down Row
            argsIntProp.MoveArrayElement(selectInt, ++selectInt);
        }
        EditorGUILayout.EndHorizontal();

        //Args Title
        EditorGUILayout.BeginHorizontal();
        argsIntProp.isExpanded = EditorGUILayout.Foldout(argsIntProp.isExpanded, "Name");
        GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField("Int Value", GUILayout.Width(ObjectPropertyWidth));
        EditorGUILayout.EndHorizontal();

        //Element Array
        if (argsIntProp.isExpanded) {
            for (var i = 0; i < argsIntProp.arraySize; i++) {
                var element = argsIntProp.GetArrayElementAtIndex(i);
                var el_name = element.FindPropertyRelative("name");
                var el_value = element.FindPropertyRelative("value");
                var tog = selectInt == i;

                //GUI
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(10f);
                var g_name = GUILayout.TextField(el_name.stringValue) ?? "";
                el_value.intValue =
                    EditorGUILayout.IntField(el_value.intValue, GUILayout.Width(ComponentPropertyWidth));
                var g_tog = EditorGUILayout.Toggle(tog, GUILayout.Width(15f));
                EditorGUILayout.EndHorizontal();

                //GUI Update
                el_name.stringValue = g_name;

                if (g_tog) {
                    selectInt = i;
                } else if (tog) {
                    selectInt = -1;
                }
            }
        }

    #endregion

    #region ArgsFloat Menu

        //ArgsFloat Menu
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("====Float Property====");
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Size", GUILayout.Width(50f));
        argsFloatProp.arraySize = EditorGUILayout.IntField(argsFloatProp.arraySize, GUILayout.Width(50f));
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("+")) {
            //Add Row
            argsFloatProp.arraySize++;
            selectFloat = argsFloatProp.arraySize - 1;

            //Name
            argsFloatProp.GetArrayElementAtIndex(selectFloat).FindPropertyRelative("name").stringValue =
                "arg" + selectFloat;
        }

        if (GUILayout.Button("-")) {
            //Remove Row
            if (selectFloat >= 0) {
                argsFloatProp.DeleteArrayElementAtIndex(selectFloat);

                if (selectFloat >= argsFloatProp.arraySize) {
                    selectFloat = argsFloatProp.arraySize - 1;
                }
            } else if (argsFloatProp.arraySize > 0) {
                argsFloatProp.arraySize--;
            }
        }
        GUILayout.Space(5f);

        if (GUILayout.Button("↑") && selectFloat > 0) {
            //Move Up Row
            argsFloatProp.MoveArrayElement(selectFloat, --selectFloat);
        }

        if (GUILayout.Button("↓") && selectFloat >= 0 && selectFloat < argsFloatProp.arraySize - 1) {
            //Move Down Row
            argsFloatProp.MoveArrayElement(selectFloat, ++selectFloat);
        }
        EditorGUILayout.EndHorizontal();

        //Args Title
        EditorGUILayout.BeginHorizontal();
        argsFloatProp.isExpanded = EditorGUILayout.Foldout(argsFloatProp.isExpanded, "Name");
        GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField("Float Value", GUILayout.Width(ObjectPropertyWidth));
        EditorGUILayout.EndHorizontal();

        //Element Array
        if (argsFloatProp.isExpanded) {
            for (var i = 0; i < argsFloatProp.arraySize; i++) {
                var element = argsFloatProp.GetArrayElementAtIndex(i);
                var el_name = element.FindPropertyRelative("name");
                var el_value = element.FindPropertyRelative("value");
                var tog = selectFloat == i;

                //GUI
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(10f);
                var g_name = GUILayout.TextField(el_name.stringValue) ?? "";
                el_value.floatValue =
                    EditorGUILayout.FloatField(el_value.floatValue, GUILayout.Width(ComponentPropertyWidth));
                var g_tog = EditorGUILayout.Toggle(tog, GUILayout.Width(15f));
                EditorGUILayout.EndHorizontal();

                //GUI Update
                el_name.stringValue = g_name;

                if (g_tog) {
                    selectFloat = i;
                } else if (tog) {
                    selectFloat = -1;
                }
            }
        }

    #endregion

    #region ArgsBool Menu

        //ArgsBool Menu
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("====Boolean Property====");
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Size", GUILayout.Width(50f));
        argsBoolProp.arraySize = EditorGUILayout.IntField(argsBoolProp.arraySize, GUILayout.Width(50f));
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("+")) {
            //Add Row
            argsBoolProp.arraySize++;
            selectBool = argsBoolProp.arraySize - 1;

            //Name
            argsBoolProp.GetArrayElementAtIndex(selectBool).FindPropertyRelative("name").stringValue =
                "arg" + selectBool;
        }

        if (GUILayout.Button("-")) {
            //Remove Row
            if (selectBool >= 0) {
                argsBoolProp.DeleteArrayElementAtIndex(selectBool);

                if (selectBool >= argsBoolProp.arraySize) {
                    selectBool = argsBoolProp.arraySize - 1;
                }
            } else if (argsBoolProp.arraySize > 0) {
                argsBoolProp.arraySize--;
            }
        }
        GUILayout.Space(5f);

        if (GUILayout.Button("↑") && selectBool > 0) {
            //Move Up Row
            argsBoolProp.MoveArrayElement(selectBool, --selectBool);
        }

        if (GUILayout.Button("↓") && selectBool >= 0 && selectBool < argsBoolProp.arraySize - 1) {
            //Move Down Row
            argsBoolProp.MoveArrayElement(selectBool, ++selectBool);
        }
        EditorGUILayout.EndHorizontal();

        //Args Title
        EditorGUILayout.BeginHorizontal();
        argsBoolProp.isExpanded = EditorGUILayout.Foldout(argsBoolProp.isExpanded, "Name");
        GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField("Boolean Value", GUILayout.Width(ObjectPropertyWidth));
        EditorGUILayout.EndHorizontal();

        //Element Array
        if (argsBoolProp.isExpanded) {
            for (var i = 0; i < argsBoolProp.arraySize; i++) {
                var element = argsBoolProp.GetArrayElementAtIndex(i);
                var el_name = element.FindPropertyRelative("name");
                var el_value = element.FindPropertyRelative("value");
                var tog = selectBool == i;

                //GUI
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(10f);
                var g_name = GUILayout.TextField(el_name.stringValue) ?? "";
                el_value.boolValue =
                    EditorGUILayout.Toggle(el_value.boolValue, GUILayout.Width(ComponentPropertyWidth));
                var g_tog = EditorGUILayout.Toggle(tog, GUILayout.Width(15f));
                EditorGUILayout.EndHorizontal();

                //GUI Update
                el_name.stringValue = g_name;

                if (g_tog) {
                    selectBool = i;
                } else if (tog) {
                    selectBool = -1;
                }
            }
        }

    #endregion

    #region ArgsString Menu

        //ArgsString Menu
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("====String Property====");
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Size", GUILayout.Width(50f));
        argsStringProp.arraySize = EditorGUILayout.IntField(argsStringProp.arraySize, GUILayout.Width(50f));
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("+")) {
            //Add Row
            argsStringProp.arraySize++;
            selectString = argsStringProp.arraySize - 1;

            //Name
            argsStringProp.GetArrayElementAtIndex(selectString).FindPropertyRelative("name").stringValue =
                "arg" + selectString;
        }

        if (GUILayout.Button("-")) {
            //Remove Row
            if (selectString >= 0) {
                argsStringProp.DeleteArrayElementAtIndex(selectString);

                if (selectString >= argsStringProp.arraySize) {
                    selectString = argsStringProp.arraySize - 1;
                }
            } else if (argsStringProp.arraySize > 0) {
                argsStringProp.arraySize--;
            }
        }
        GUILayout.Space(5f);

        if (GUILayout.Button("↑") && selectString > 0) {
            //Move Up Row
            argsStringProp.MoveArrayElement(selectString, --selectString);
        }

        if (GUILayout.Button("↓") && selectString >= 0 && selectString < argsStringProp.arraySize - 1) {
            //Move Down Row
            argsStringProp.MoveArrayElement(selectString, ++selectString);
        }
        EditorGUILayout.EndHorizontal();

        //Args Title
        EditorGUILayout.BeginHorizontal();
        argsStringProp.isExpanded = EditorGUILayout.Foldout(argsStringProp.isExpanded, "Name");
        GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField("String Value", GUILayout.Width(ObjectPropertyWidth));
        EditorGUILayout.EndHorizontal();

        //Element Array
        if (argsStringProp.isExpanded) {
            for (var i = 0; i < argsStringProp.arraySize; i++) {
                var element = argsStringProp.GetArrayElementAtIndex(i);
                var el_name = element.FindPropertyRelative("name");
                var el_value = element.FindPropertyRelative("value");
                var tog = selectString == i;

                //GUI
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(10f);
                var g_name = GUILayout.TextField(el_name.stringValue) ?? "";
                el_value.stringValue =
                    EditorGUILayout.TextField(el_value.stringValue, GUILayout.Width(ComponentPropertyWidth));
                var g_tog = EditorGUILayout.Toggle(tog, GUILayout.Width(15f));
                EditorGUILayout.EndHorizontal();

                //GUI Update
                el_name.stringValue = g_name;

                if (g_tog) {
                    selectString = i;
                } else if (tog) {
                    selectString = -1;
                }
            }
        }

    #endregion

        //保存更改
        serializedObject.ApplyModifiedProperties();
    }

    class State {

        public Object refObject { get; set; }
        public int index { get; set; }
        public string[] names { get; private set; }
        public List<Object> components { get; private set; }

        public State(int index, string[] names, List<Object> components)
        {
            this.index = index;
            this.names = names;
            this.components = components;
        }

        public string Now()
        {
            if (index >= 0 && index < names.Length) {
                return names[index];
            }

            return "";
        }

        public int IndexOf(string type)
        {
            //Type是重命名的类型
            var repeat_i = type.IndexOf("(");

            if (repeat_i >= 0) {
                type = type.Substring(0, repeat_i);
            }

            //在names中查找Type, Name中可能包含重命名
            for (var i = 0; i < names.Length; i++) {
                if (type == names[i] || names[i].Contains(type + "(")) {
                    return i;
                }
            }

            return index;
        }

    }

}
#endif

}