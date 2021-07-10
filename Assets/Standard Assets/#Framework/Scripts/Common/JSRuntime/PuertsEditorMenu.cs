#if UNITY_EDITOR
using Puerts;
using Puerts.Attributes;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Common.JSRuntime {

[PuertsIgnore]
public class PuertsEditorMenu {

    [MenuItem("Tests/Find JsEnvInstace")]
    static void FindJsInstance()
    {
        Debug.Log(Resources.LoadAll<JsMain>("").Length);
    }

    [MenuItem("Puerts/Support/使用手册")]
    static void OpenManual() =>
        Application.OpenURL("https://github.com/Tencent/puerts/blob/master/doc/unity/manual.md");

    [MenuItem("Puerts/Debug/chrome")]
    static void OpenDebug() => Application.OpenURL(Application.dataPath + "/debug.html");

    static EditorApplication.CallbackFunction EditorUpdate = () => { };

    [MenuItem("Puerts/Debug/TestClass")]
    static void OpenDebugTestClass()
    {
        var jsEnv = new JsEnv(new DefaultLoader(Application.dataPath), 9228);
        EditorUpdate = () => {
            jsEnv.Tick();
        };
        EditorApplication.update += EditorUpdate;
        jsEnv.WaitDebugger();

        //var content = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/dist/TestClass.cjs");
        var content = File.ReadAllText(Application.dataPath + "/Assets/dist/TestClass.cjs");
        Debug.Log($"{content.Length}");
        jsEnv.Eval(content);

        //Application.OpenURL(Application.dataPath + "/debug.html");
    }

}

}
#endif