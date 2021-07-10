using GameEngine.Extensions;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
#endif

namespace Helpers {

public class TypescriptModuleManager : OdinMenuEditorWindow {

    public const string MenuName = "Js Manager";

    // /// <summary>
    // /// 获取继承 ScriptableObject 且不是Editor相关的所有自定义类（也就是自己编写的类）
    // /// </summary>
    // static HashSet<Type> scriptableObjectTypes = AssemblyUtilities.GetTypes(AssemblyTypeFlags.CustomTypes)
    //     .Where(t =>
    //         //$"{t.Namespace}".StartsWith((typeof(T).Namespace ?? "").Split('.').First())  &&
    //         t.IsClass && typeof(M).IsAssignableFrom(t) && typeof(ScriptableObject).IsAssignableFrom(t) &&
    //         !typeof(EditorWindow).IsAssignableFrom(t) && !typeof(Editor).IsAssignableFrom(t))
    //     .ToHashSet();

    static HashSet<TextAsset> allModules => AssetDatabase.FindAssets("t:TextAsset", new[] {
            "Assets", "Packages",
        })

        //Resources.LoadAll<TextAsset>("")
        .Where(t =>

            //t.AssetPath().EndsWith(".ts")||
            t.AssetPath().EndsWith(".js.txt") ||
            t.AssetPath().EndsWith(".js") ||
            t.AssetPath().EndsWith(".mjs") ||
            t.AssetPath().EndsWith(".cjs"))
        .Select(t => AssetDatabase.LoadAssetAtPath<TextAsset>(t.AssetPath()))
        .ToHashSet();

    public static string prefix => "";
    bool Loaded;

    protected override void OnEnable()
    {
        base.OnEnable();
        Loaded = true;
    }

    [MenuItem("Puerts/" + MenuName, priority = 201)]
    protected static void ShowDialog()
    {
        var path = prefix;
        var obj = Selection.activeObject; //当前鼠标选中的 Object

        if (obj && AssetDatabase.Contains(obj)) {
            path = AssetDatabase.GetAssetPath(obj);

            if (!Directory.Exists(path)) //主要用来判断所选的是文件还是文件夹
            {
                path = Path.GetDirectoryName(path); //如果是文件则获取对应文件夹的全名称
            }
        }

        //设置窗口对应属性
        var window = CreateInstance<TypescriptModuleManager>();
        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500); //设置窗口的宽和高
        window.titleContent = new GUIContent(MenuName);
        window.targetFolder = path.Trim('/'); //避免出现 / 造成路径不对
        window.Show();

        //window.ShowPopup();
        //window.ShowTab();
        //window.ShowUtility();
    }

    /// <summary>
    /// 选中的 ScriptableObject（等待创建）
    /// </summary>
    TypescriptTester previewObject;

    /// <summary>
    /// 创建 ScriptableObject 时文件存储的目标文件夹
    /// </summary>
    public string targetFolder;

    Vector2 scroll;

    TextAsset SelectedType {
        get {
            var m = MenuTree.Selection.LastOrDefault();

            //因为可以多选，所以返回选中的是一个列表，这里返回的是列表的最后一个Object
            return m?.Value as TextAsset;
        }
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        MenuWidth = 300; //菜单的宽度
        WindowPadding = Vector4.zero;
        var tree = new OdinMenuTree(false);

        //不支持多选
        tree.Config.DrawSearchToolbar = true;

        //开启搜索状态
        tree.DefaultMenuStyle = OdinMenuStyle.TreeViewStyle;

        //菜单设置成树形模式

        //筛选所有非抽象的类 并获取对应的路径
        tree.AddRange(allModules.Select(t => t.AssetPath()), GetMenuPathForType).AddThumbnailIcons();
        tree.SortMenuItemsByName();
        tree.Selection.SelectionConfirmed += x => {
            Debug.Log($"双击确认并创建：{x}");
            CreateAsset();
        };
        tree.Selection.SelectionChanged += e => {
            //每当选择发生更改时发生进行回调2次，一次SelectionCleared 一次是ItemAdded
            // if (previewObject && !AssetDatabase.Contains(previewObject)) {
            //     DestroyImmediate(previewObject);
            // }
            if (e != SelectionChangedType.ItemAdded) {
                return;
            }
            var m = MenuTree.Selection.LastOrDefault(); // string

            //Debug.Log($"{m?.Value?.GetType().GetNiceFullName()} {m?.Value}");
            if (m?.Value != null && Loaded) {
                if (previewObject && !AssetDatabase.Contains(previewObject)) {
                    DestroyImmediate(previewObject);
                }
                previewObject = CreateInstance<TypescriptTester>();
                Camera.main.Tap(t => { });

                //previewObject.OnEnable();
                previewObject.path = m.Value.ToString().AssetPath();
                previewObject.module = AssetDatabase.LoadAssetAtPath<TextAsset>(previewObject.path);

                //Resources.Load<TextAsset>(previewObject.path);
            }

            //
            // if (t != null && !t.IsAbstract) {
            //     var func = t.GetMethod("GetInstance",
            //         BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
            //
            //     if (func == null) {
            //         Debug.Log($"{t.Name} instance is null");
            //     }
            //
            //     previewObject = func?.Invoke(null, null) as ScriptableObject;
            //
            //     if (previewObject == null) {
            //         previewObject = CreateInstance(t) as ScriptableObject;
            //     }
            // }
        };

        return tree;
    }

    string GetMenuPathForType(string t) =>

        // if (t != null && scriptableObjectTypes.Contains(t)) {
        //     var name = t.GetNiceName(); // t.Name.Split('`').First()
        //
        //     // .SplitPascalCase(); //主要是为了去除泛型相关 例如：Sirenix.Utilities.GlobalConfig`1[Sirenix.Serialization.GlobalSerializationConfig]
        //
        //     return $"{t?.Namespace}".Replace($"{GetType().Namespace?.Split('.').First()}.", "")
        //             .Replace("Gameplay.", "")
        //             .Split('.')
        //             .First()
        //         + "/"
        //         + name;
        //
        //     return GetMenuPathForType(t.BaseType) + "/" + name;
        // }
        //return "";
        t;

    protected override IEnumerable<object> GetTargets()
    {
        yield return previewObject;
    }

    protected override void DrawEditor(int index)
    {
        //GUILayout.Box("test");
        var myStyle = GUI.skin.GetStyle("HelpBox");
        myStyle.richText = true;

        //GUI.skin.box.richText = true;
        if (previewObject) {
            EditorGUILayout.HelpBox($"<size=14>{previewObject.module?.AssetPath()}</size>", MessageType.None);
        }

        //scroll 内容滑动条的XY坐标
        scroll = GUILayout.BeginScrollView(scroll);
        {
            base.DrawEditor(index);
        }
        GUILayout.EndScrollView();

        if (previewObject) {
            GUILayout.FlexibleSpace();                   //插入一个空隙
            SirenixEditorGUI.HorizontalLineSeparator(5); //插入一个水平分割线
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Open Script", GUILayoutOptions.Height(30))) {
                openScript();
            }

            if (GUILayout.Button("Create Asset", GUILayoutOptions.Height(30))) {
                CreateAsset();
            }
            GUILayout.EndHorizontal();
        }
    }

    void openScript()
    {
        if (previewObject) {
            var script = MonoScript.FromScriptableObject(previewObject);

            //var path = AssetDatabase.GetAssetPath( script );
            AssetDatabase.OpenAsset(script);
        }
    }

    void CreateAsset()
    {
        if (previewObject) {
            Selection.activeObject = previewObject;

            return;

            var dest = targetFolder + "/new " + MenuTree.Selection.First().Name.ToLower() + ".asset";
            dest = AssetDatabase.GenerateUniqueAssetPath(dest); //创建唯一路径 重名后缀 +1
            Debug.Log($"要创建的为{previewObject}");
            AssetDatabase.CreateAsset(previewObject, dest);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Selection.activeObject = previewObject;

            //EditorApplication.delayCall += Close; //如不需要创建后自动关闭可将本行注释
        }
    }

}

}