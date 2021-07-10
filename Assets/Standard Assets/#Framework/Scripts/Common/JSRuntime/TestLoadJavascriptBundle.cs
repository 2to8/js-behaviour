//using Puerts;
//using Sirenix.OdinInspector;
//using Sirenix.Utilities;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using UnityEditor;
//using UnityEngine;
//
//namespace Common.JSRuntime {
//
//[Serializable]
//public class ToggleableClass {
//
//    [HideLabel, TableColumnWidth(20, false)]
//    public bool x;
//
//    [HideInInspector]
//    public string Text;
//
//    public TextAsset content;
//
//}
//
////[ExecuteAlways]
//public class TestLoadJavascriptBundle : MonoBehaviour, ILoader {
//
//    [TableList]
//    public List<ToggleableClass> Toggleable = new List<ToggleableClass>();
//
//    [SerializeField]
//    List<TextAsset> bundleFile = new List<TextAsset>();
//
//    [SerializeField]
//    List<string> paths = new List<string>();
//
//    [MultiLineProperty(15), HideLabel, SerializeField, FoldoutGroup("code")]
//    string code;
//
//    [SerializeField]
//    bool runCode;
//
//    JsEnv m_JsEnv;
//
//    [SerializeField]
//    int debugPort = 9229;
//
//    public static JsEnv jsEnv => JsEnv.Alive;
//
//    string filename(string filename) => !filename.Contains(".") ? filename += ".js" : filename;
//
//    void OnEnable()
//    {
//    #if UNITY_EDITOR
//        paths.Clear();
//        bundleFile.ForEach(t => paths.Add(AssetDatabase.GetAssetPath(t)));
//    #endif
//    }
//
//    public bool Expired(string moduleName) => false;
//
//    public bool FileExists(string filepath)
//    {
//        var path = filename(filepath);
//    #if UNITY_EDITOR
//        if (bundleFile.Any(t => Path.GetFileName(AssetDatabase.GetAssetPath(t)) == path)) {
//            return true;
//        }
//    #endif
//
//        if (paths.Any(t => Path.GetFileName(t) == path)) {
//            return true;
//        }
//
//        if (Resources.Load(path) != null) {
//            return true;
//        }
//
//        return false;
//    }
//
//    public string ReadFile(string filepath, out string debugpath)
//    {
//        debugpath = null;
//        var path = filename(filepath);
//        TextAsset textAsset = null;
//
//    #if UNITY_EDITOR
//        textAsset = bundleFile.FirstOrDefault(t => Path.GetFileName(AssetDatabase.GetAssetPath(t)) == path);
//
//        if (textAsset != null) {
//            return textAsset.text;
//        }
//    #endif
//
//        paths.ForEach((t, i) => {
//            if (Path.GetFileName(t) == path && i < bundleFile.Count) {
//                textAsset = bundleFile[i];
//
//                return;
//            }
//        });
//
//        if (textAsset != null) {
//            return textAsset.text;
//        }
//
//        return Resources.Load<TextAsset>(path)?.text;
//    }
//
//    [Button(ButtonSizes.Large), ButtonGroup("run")]
//    void Run()
//    {
//        OnEnable();
//        bundleFile.ForEach((t, i) => {
//            var path = Path.GetFileName(paths[i]);
//            Debug.Log(path);
//            jsEnv.Eval($"require('{path}')");
//        });
//
//        if (runCode) {
//            jsEnv.Eval(code);
//        }
//        Debug.Log("finish");
//    }
//
//    [Button(ButtonSizes.Large), ButtonGroup("run")]
//    void ResetJsEnv()
//    {
//        jsEnv.Dispose();
//        Debug.Log("finish");
//    }
//
//}
//
//}