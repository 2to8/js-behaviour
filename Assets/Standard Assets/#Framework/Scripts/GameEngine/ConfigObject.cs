using Puerts.Attributes;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GameEngine {

// We use this class to store general config data that can be used in the player
// https://docs.unity3d.com/ScriptReference/PlayerSettings.GetPreloadedAssets.html
public class ConfigObject : ScriptableObject {

    public string text;
    public static ConfigObject configInstance;
#if UNITY_EDITOR
    [UnityEditor.MenuItem("Assets/Create/Config Object"), PuertsIgnore]
    public static void CreateAsset()
    {
        var path = UnityEditor.EditorUtility.SaveFilePanelInProject("Save Config", "config", "asset", string.Empty);

        if (string.IsNullOrEmpty(path)) {
            return;
        }

        var configObject = CreateInstance<ConfigObject>();
        UnityEditor.AssetDatabase.CreateAsset(configObject, path);

        // Add the config asset to the build
        var preloadedAssets = UnityEditor.PlayerSettings.GetPreloadedAssets().ToList();

        if (preloadedAssets.All(t => t.GetType() != typeof(ConfigObject))) {
            preloadedAssets.Add(configObject);
            UnityEditor.PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
            AssetDatabase.SaveAssets();
        }
    }
#endif

    void OnEnable()
    {
        configInstance = this;
    }

}

}