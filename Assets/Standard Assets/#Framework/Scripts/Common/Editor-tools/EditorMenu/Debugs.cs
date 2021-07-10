using Puerts.Attributes;
using Sirenix.Utilities;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;

namespace Common.EditorMenu {

public static class AddressableAssetsUtil {

    // [UnityEditor.MenuItem("Addressable/Build")]
    // public static void Build()
    // {
    //     // BuildTargetはよしなに設定すること
    //     BuildScript.PrepareRuntimeData(true, false, true, true, false,
    //         UnityEditor.BuildTargetGroup.Standalone, UnityEditor.BuildTarget.StandaloneOSX);
    // }

}
#if UNITY_EDITOR
[PuertsIgnore]
public class Debugs {

    [UnityEditor.MenuItem("Puerts/Debug/find events.js")]
    static void FindEventsJs()
    {
        Debug.Log(Resources.LoadAll("events.js").Length);
    }

    [UnityEditor.MenuItem("Puerts/Debug/All Addressable Names")]
    static void ShowAssetNames()
    {
        var names = Addressables.ResourceLocators.OfType<ResourceLocationMap>()
            .SelectMany(locationMap => locationMap.Locations.Keys.Select(key => key.ToString()));
        names.ForEach((s, i) => {
            Debug.Log($"[{i}] {s}");
        });
        Debug.Log($"count: {names.Count()}");
    }

}
#endif

}