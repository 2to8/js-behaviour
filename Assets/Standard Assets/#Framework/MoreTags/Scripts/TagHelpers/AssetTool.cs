#if UNITY_EDITOR
using Puerts.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MoreTags.TagHelpers {

[PuertsIgnore]
public class AssetTool {

    static readonly Dictionary<Type, object> Cache = new Dictionary<Type, object>();

    public static AddressableAssetSettings DefaultSettings =>
        AddressableAssetSettingsDefaultObject.Settings;

    public static void AddLabel(string name) =>
        AddressableAssetSettingsDefaultObject.Settings.AddLabel(name);

    public static void RemoveLable(string name) =>
        AddressableAssetSettingsDefaultObject.Settings.RemoveLabel(name);

    public static List<string> LabelNames()
    {
        AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
        BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
        object labelTable = settings.GetType()
            .GetProperty("labelTable", bindingFlags)
            ?.GetValue(settings);
        List<string> labelNames = (List<string>)labelTable?.GetType()
            .GetProperty("labelNames", bindingFlags)
            ?.GetValue(labelTable);
        return labelNames;
    }

    public static AddressableAssetSettings settings =>
        GetAssetInstance<AddressableAssetSettings>("AddressableAssetSettings");

    public static void SetLabel(string label)
    {
        // if you haven't changed default name "addressableAssetSettings" , otherwise go find asset for AddressableAssetSettings under AddressableAssetData
        //var get = settings.GetLabels();
        if (!GetLables().Contains(label)) settings.AddLabel(label);
    }

    public static List<string> GetLables() => settings.GetLabels();

    public static T GetAssetInstance<T>(string assetName = null) where T : ScriptableObject
    {
        if (Cache.ContainsKey(typeof(T))) {
            return (T)Cache[typeof(T)];
        }



        // this method should find any scriptableObject of given type and given name , can reuse this method for other SO
        var assetList = GetAllInstances<T>().ToList();
        var asset = string.IsNullOrEmpty(assetName)
            ? assetList.FirstOrDefault()
            : assetList.FirstOrDefault(o =>
                o.name == assetName); // most likely we should get it here
        return (T)(Cache[typeof(T)] = asset != null ? asset : assetList.FirstOrDefault());
    }

    public static T[] GetAllInstances<T>() where T : ScriptableObject
    {
        var assetList = Resources.FindObjectsOfTypeAll<T>();
        return assetList;
    }

}

}
#endif