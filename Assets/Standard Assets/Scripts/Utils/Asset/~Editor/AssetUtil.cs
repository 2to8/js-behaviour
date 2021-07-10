#if UNITY_EDITOR

using GameEngine.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Utils.Asset {

public static class AssetUtil {
    //
    public static IEnumerable<string> FindAssets(params string[] types)
    {
        return AssetDatabase.FindAssets("t:" + string.Join(" t:", types))
            .Select(AssetDatabase.GUIDToAssetPath);
    }

    //
   // [NUnit.Framework.Test]
    public static void TestLabels()
    {
        Debug.Log($"scriptableObject: {FindAssets(nameof(ScriptableObject)).Count()}");
        Debug.Log($"Scene: {FindAssets(nameof(Scene)).Count()}");
        Debug.Log($"GameObject: {FindAssets(nameof(GameObject)).Count()}");
        Debug.Log($"Prefab: {FindAssets("prefab").Count()}");

        static async Task<T> LoadAsset<T>(Object key) where T : Object
        {
            // if (typeof(GameObject).IsAssignableFrom(typeof(T))
            //     || typeof(Scene).IsAssignableFrom(typeof(T))
            //     || typeof(Component).IsAssignableFrom(typeof(T))) {
            // }
            await Addressables.DownloadDependenciesAsync(key);

            return await Addressables.LoadAssetAsync<T>(key);
        }
    }
}

}

#endif