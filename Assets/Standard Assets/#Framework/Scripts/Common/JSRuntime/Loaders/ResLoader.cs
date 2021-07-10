// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using System.Runtime.Remoting.Proxies;
// using UnityEditor;
// using UnityEditor.VersionControl;
// using UnityEngine;
//
// namespace Puerts.Scripts {
//
// [CreateAssetMenu(fileName = nameof(ResLoader), menuName = "ScriptableObjects/" + nameof(ResLoader), order = 0)]
// public class ResLoader : ScriptableObject, ILoader {
//
//     TextAsset result;
//     string realpath = "";
//     string module = "";
//     string root = "";
//     public static int port = 9229;
//
//     // public string path(string path)
//     // {
//     //     return Path.GetFullPath(Path.Combine(root, Path.GetDirectoryName(path) + "",
//     //             Path.GetFileNameWithoutExtension(path)))
//     //         .Replace('\\', '/')
//     //         .Trim('/', '.');
//     // }
//     static ResLoader m_defaultLoader;
//     static JsEnv jsEnv;
//
//     public static ResLoader defaultLoader {
//         get => m_defaultLoader ?? (m_defaultLoader = CreateInstance<ResLoader>());
//         set => m_defaultLoader = value;
//     }
//
//     IEnumerable<string> assets;
//
//     public ResLoader Root(string path)
//     {
//         root = path;
//         return this;
//     }
//
//     public static JsEnv alive {
//         get {
//             if ( jsEnv?.isNotAlive) {
//                 if (!JsEnv.hasAlive(env => env.loader.GetType().IsInstanceOfType(defaultLoader))) {
//                     JsEnv.defaultLoader = defaultLoader;
//                     jsEnv = new JsEnv(defaultLoader, JsEnv.defaultPort);
//                 } else {
//                     jsEnv = JsEnv.jsEnvs.First(env => env.isAlive && env.loader.GetType().IsInstanceOfType(defaultLoader));
//                 }
//             }
//             return jsEnv;
//         }
//     }
//
//     public bool FileExists(string filepath)
//     {
//         filepath = $"{filepath}";
//         if (assets == null || !assets.Any()) {
//             assets = AssetDatabase.FindAssets("t:TextAsset", new[] { "Assets", "Pacakages" })
//                 .Select(AssetDatabase.GUIDToAssetPath)
//                 .Where(s => s.EndsWith(".js") || s.EndsWith(".js.txt") || s.EndsWith(".cjs"));
//
//             //.Select(AssetDatabase.LoadAssetAtPath<TextAsset>);
//         }
//         realpath = assets.FirstOrDefault(s =>
//                 s.Contains(filepath) || s.Contains(filepath.Substring(0, filepath.IndexOf('.')))) ??
//             string.Empty;
//         if (!string.IsNullOrEmpty(realpath)) {
//             result = AssetDatabase.LoadAssetAtPath<TextAsset>(realpath);
//
//             //Debug.Log($"{filepath} => {realpath}");
//             return true;
//         }
//         Debug.Log(filepath);
//         Debug.Log(string.Join(", ", assets));
//         return false;
//     }
//
//     public string ReadFile(string filepath, out string debugpath)
//     {
//         if (FileExists(filepath)) {
//             debugpath = realpath;
//             return result.text;
//         }
//         debugpath = null;
//         return null;
//     }
//
// }
//
// }

