#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace Utils.Prefabs {

public static class PrefabUtil {

    /// <summary>
    /// https://forum.unity.com/threads/how-do-i-edit-prefabs-from-scripts.685711/
    /// How do I edit prefabs from scripts?
    /// </summary>
    public class EditPrefabAssetScope : IDisposable {

        public readonly string assetPath;
        public readonly GameObject prefabRoot;

        public EditPrefabAssetScope(string assetPath)
        {
            this.assetPath = assetPath;
            prefabRoot = PrefabUtility.LoadPrefabContents(assetPath);
        }

        public void Dispose()
        {
            PrefabUtility.SaveAsPrefabAsset(prefabRoot, assetPath);
            PrefabUtility.UnloadPrefabContents(prefabRoot);
        }
    }


    [MenuItem("Tools/Examples/Add BoxCollider to Prefab Asset")]
    static void AddBoxColliderToPrefab()
    {
        // Get the Prefab Asset root GameObject and its asset path.
        GameObject assetRoot = Selection.activeObject as GameObject;
        string assetPath = AssetDatabase.GetAssetPath(assetRoot);

        // Modify prefab contents and save it back to the Prefab Asset
        using (var editScope = new EditPrefabAssetScope(assetPath)) {
            editScope.prefabRoot.AddComponent<BoxCollider>();
        }
    }


}

}
#endif
