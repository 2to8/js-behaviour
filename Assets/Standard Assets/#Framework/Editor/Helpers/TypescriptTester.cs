using Common.JSRuntime;
using GameEngine.Extensions;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Helpers {

[CreateAssetMenu(fileName = nameof(TypescriptTester), menuName = "ScriptableObjects/" + nameof(TypescriptTester),
    order = 0)]
public class TypescriptTester : ScriptableObject {
    public TextAsset module;
    public string path;

    //Env.Js js;

    void OnEnable()
    {
        //if (js == null) {
        //js = JsCore.Create();

        //}
    }

    void OnDisable()
    {
        //js.Dispose();
    }

    [Button, ButtonGroup("run")]
    void TestRun()
    {
        Core.ClearConsole();

        //ClearLog();
        //js.ReInit().Require(module);
        AssetDatabase.ImportAsset(module.AssetPath(),
            ImportAssetOptions.ForceUpdate | ImportAssetOptions.ImportRecursive);

        //AssetDatabase.Refresh();

        // module = AssetDatabase.LoadAssetAtPath<TextAsset>(module.AssetPath());
        module = AssetDatabase.LoadAssetAtPath<TextAsset>(module.AssetPath());

        //Resources.Load<TextAsset>(module.AssetPath().ResourcePath());
        JsGlobal.Core.ClearCache()
            .Begin()
            .Tap(env => {
                env.Require(module);
            })
            .End();

        //using (var js = JsCore.Create().Require(module)) { }
    }

    [Button, ButtonGroup("run")]
    void Reload()
    {
        JsGlobal.Core.Reload();
    }

    // public void ClearLog() //you can copy/paste this code to the bottom of your script
    // {
    //     var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
    //     var type = assembly.GetType("UnityEditor.LogEntries");
    //     var method = type.GetMethod("Clear");
    //     method.Invoke(new object(), null);
    // }
}

}