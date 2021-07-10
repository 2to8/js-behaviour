/// <summary>
/// The script gives you choice to whether to build addressable bundles when clicking the build button.
/// For custom build script, call PreExport method yourself.
/// For cloud build, put BuildAddressablesProcessor.PreExport as PreExport command.
/// Discussion: https://forum.unity.com/threads/how-to-trigger-build-player-content-when-build-unity-project.689602/
/// </summary>
///

#if UNITY_EDITOR

using Editors;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Utils.Build {

class BuildAddressablesProcessor {
    /// <summary>
    /// Run a clean build before export.
    /// </summary>
    static public void PreExport()
    {
        Debug.Log("BuildAddressablesProcessor.PreExport start");

        OnUpdateBuild(false);
        My.WSL("npm run deploy");

        // AddressableAssetSettings.CleanPlayerContent(
        //     AddressableAssetSettingsDefaultObject.Settings.ActivePlayerDataBuilder);
        // AddressableAssetSettings.BuildPlayerContent();
        Debug.Log("BuildAddressablesProcessor.PreExport done");
    }

    public static void OnUpdateBuild(bool prompt = true)
    {
        new AssetLabelReference();

        var path = ContentUpdateScript.GetContentStateDataPath(prompt);
        if (!string.IsNullOrEmpty(path))
            ContentUpdateScript.BuildContentUpdate(AddressableAssetSettingsDefaultObject.Settings,
                path);
    }

    [InitializeOnLoadMethod]
    private static void Initialize()
    {
        BuildPlayerWindow.RegisterBuildPlayerHandler(BuildPlayerHandler);
    }

    private static void BuildPlayerHandler(BuildPlayerOptions options)
    {
        Core.isBuilding = true;

        if (EditorUtility.DisplayDialog("Build with Addressables",
            "Do you want to build a clean addressables before export?", "Build with Addressables",
            "Skip")) {
            PreExport();
        }
        BuildPlayerWindow.DefaultBuildMethods.BuildPlayer(options);
    }

}

}

#endif