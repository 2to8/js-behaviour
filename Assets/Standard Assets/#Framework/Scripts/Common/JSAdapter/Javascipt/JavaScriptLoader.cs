// using System.Collections.Generic;
// using System.IO;
// using Engine.Extensions;
// using UnityEngine;
// using static System.IO.Path;
// using static UnityEngine.Application;
//
// namespace Puerts {
//
// public class JavaScriptLoader : Puerts.ILoader {
//
//     public List<string> debugRoot = new List<string>();
//     public bool showPath;
//
//     public JavaScriptLoader(string debugRoot)
//     {
//         this.debugRoot.Add(debugRoot);
//     }
//
//     public JavaScriptLoader(List<string> debugRoot)
//     {
//         this.debugRoot = debugRoot;
//     }
//
//     public bool Expired(string moduleName) => false;
//
//     public bool FileExists(string filepath)
//     {
//         if(filepath.StartsWith("puerts/") || !isEditor) return true;
//
//         if(showPath) {
//             Debug.Log($"{filepath} => {GetScriptDebugPath(filepath).RemoveAssets()}");
//         }
//         return File.Exists(GetScriptDebugPath(filepath));
//     }
//
//     public string GetScriptDebugPath(string filepath)
//     {
//         if(filepath.StartsWith("puerts/")) {
//             return Combine(dataPath, "Libraries/Puerts/Src/Resources", filepath, ".txt").FullPath();
//         }
//         filepath = filepath.Contains(".") ? filepath : filepath + ".js";
//         var ret = filepath.FullPath();
//         if(File.Exists(ret)) return ret;
//
//         foreach(var s in debugRoot) {
//             string test = $"{s}/{filepath}".FullPath();
//             if(File.Exists(test)) {
//                 //if(showPath) Debug.Log(test);
//                 ret = test;
//                 break;
//             }
//             test = $"{s}/{filepath}".Replace(".js", ".mjs").FullPath();
//             if(File.Exists(test)) {
//                 //if(showPath) Debug.Log(test);
//                 ret = test;
//                 break;
//             }
//         }
//
//         return ret;
//     }
//
//     public string ReadFile(string filepath, out string debugpath)
//     {
//         debugpath = GetScriptDebugPath(filepath);
//         if(filepath.StartsWith("puerts/")) {
//             var asset = Resources.Load<UnityEngine.TextAsset>(filepath);
//             return asset.text;
//         }
//         return File.Exists(debugpath) ? File.ReadAllText(debugpath) : string.Empty;
//     }
//
//     public void Close() { }
//
// }
//
// }

