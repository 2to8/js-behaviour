// #if UNITY_EDITOR
// using System.IO;
// using UnityEditor;
// using UnityEditor.Experimental.AssetImporters;
// using UnityEngine;
//
// namespace PuertsTest {
//
//
// //[ScriptedImporter(1, "cjs")]
// class CjsImporter : ScriptedImporter {
//
//     public override void OnImportAsset(AssetImportContext ctx)
//     {
//         var obj = ScriptabimleObject.CreateInstance<JsFile>();
//         var path = Directory.GetCurrentDirectory() + "/" + ctx.assetPath;
//
//         //ctx.LogImportError(path);
//         //Debug.Log(AssetDatabase.AssetPathToGUID(ctx.assetPath));
//         obj.Content = File.ReadAllText(path);
//
//         obj.id = AssetDatabase.AssetPathToGUID(ctx.assetPath) ?? $"{ctx.GetHashCode()}";
//
//         //new JsFile(File.ReadAllText(ctx.assetPath));
//         obj.setPath(ctx.assetPath);
//         ctx.AddObjectToAsset("main", obj);
//         ctx.SetMainObject(obj);
//
//         //ctx.
//     }
//
// }
//
// }
// #endif

