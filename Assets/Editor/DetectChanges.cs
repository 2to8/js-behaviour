using System.Linq;
using App.Runtime;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

public class DetectChanges : AssetPostprocessor  
{
     
    static void OnPostprocessAllAssets (
        string[] importedAssets,
        string[] deletedAssets,
        string[] movedAssets,
        string[] movedFromAssetPaths) 
    {
        if (importedAssets.FirstOrDefault(str => str.Contains(".js")) != null) {
            Main.js.ClearModuleCache();
            Main.js.Eval("require('index')");
        }
//        foreach (string str in importedAssets)
//        {
//            if(!str.Contains(".js")) continue;
//            Debug.Log("Reimported Asset: " + str);
//            
////            string[] splitStr = str.Split('/', '.');
////             
////            string folder = splitStr[splitStr.Length-3];
////            string fileName = splitStr[splitStr.Length-2];
////            string extension = splitStr[splitStr.Length-1];
////            Debug.Log("File name: " + fileName);
////            Debug.Log("File type: " + extension);                
//        }
//         
        foreach (string str in deletedAssets)
            Debug.Log("Deleted Asset: " + str);
 
        for (int i=0;i<movedAssets.Length;i++)
            Debug.Log("Moved Asset: " + movedAssets[i] + " from: " + movedFromAssetPaths[i]);        
    }
}