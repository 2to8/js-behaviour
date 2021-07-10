// #if UNITY_EDITOR
// using Puerts.Attributes;
// using UnityEngine.ResourceManagement.ResourceProviders;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Reflection;
// using System.Text;
// using System.Text.RegularExpressions;
// using UnityEditor;
// using UnityEditor.AddressableAssets.Settings;
// using UnityEditor.AddressableAssets.Settings.GroupSchemas;
// using UnityEditor.Build.Pipeline.Utilities;
// using UnityEngine;
// using UnityEngine.ResourceManagement.Util;
//
// namespace MoreTags.TagHelpers {
//
// [PuertsIgnore]
// public static class AddressableUtils {
//
//     [MenuItem("Tools/Hoge")]
//     private static void Hoge()
//     {
//         var group = AddressableUtils.GetOrCreateGroup("TestGroupName");
//         var bundledAssetGroupSchema = group.Schemas
//             .OfType<BundledAssetGroupSchema>()
//             .FirstOrDefault();
//         var serializedType = new SerializedType {
//             Value = typeof(ResourceProviderBase),
//             ValueChanged = true,
//         };
//         var type = typeof(BundledAssetGroupSchema);
//         var name = "m_AssetBundleProviderType";
//         var attr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
//         var value = type.GetField(name, attr);
//         value.SetValue(bundledAssetGroupSchema, serializedType);
//     }
//
//     private static AddressableAssetSettings m_settings;
//
//     // AddressableAssetSettings を取得します
//     public static AddressableAssetSettings GetSettings()
//     {
//         if (m_settings != null) return m_settings;
//
//         var guidList = AssetDatabase.FindAssets("t:AddressableAssetSettings");
//         var guid = guidList.FirstOrDefault();
//         var path = AssetDatabase.GUIDToAssetPath(guid);
//         var settings = AssetDatabase.LoadAssetAtPath<AddressableAssetSettings>(path);
//         m_settings = settings;
//         return settings;
//     }
//
//     // 指定された名前のグループを取得もしくは作成します
//     public static AddressableAssetGroup GetOrCreateGroup(string groupName)
//     {
//         var settings = GetSettings();
//         var groups = settings.groups;
//         var group = groups.Find(c => c.name == groupName);
//
//         // 既に指定された名前のグループが存在する場合は
//         // そのグループを返します
//         if (group != null) return group;
//
//         // Content Packing & Loading
//         var bunlAssetGroupSchema = ScriptableObject.CreateInstance<BundledAssetGroupSchema>();
//
//         // Content Update Restriction
//         var contentUpdateGroupSchema = ScriptableObject.CreateInstance<ContentUpdateGroupSchema>();
//
//         // AddressableAssetGroup の Inspector に表示されている Schema
//         var schemas = new List<AddressableAssetGroupSchema> {
//             bunlAssetGroupSchema,
//             contentUpdateGroupSchema,
//         };
//
//         // 指定された名前のグループを作成して返します
//         return settings.CreateGroup(groupName: groupName, setAsDefaultGroup: false, readOnly: false,
//             postEvent: true, schemasToCopy: schemas);
//     }
//
//     // 指定されたアセットにアドレスを割り当ててグループに追加します
//     public static void SetAddressToAssetOrFolder(string path, string address, string groupName)
//     {
//         var targetParent = GetOrCreateGroup(groupName);
//         SetAddressToAssetOrFolder(path: path, address: address, targetParent: targetParent);
//     }
//
//     // 指定されたアセットにアドレスを割り当ててグループに追加します
//     public static void SetAddressToAssetOrFolder(string path, string address,
//         AddressableAssetGroup targetParent)
//     {
//         var settings = GetSettings();
//         var guid = AssetDatabase.AssetPathToGUID(path);
//         var entry = settings.FindAssetEntry(guid);
//
//         // すでに同じアドレスとグループが設定されている場合処理をスキップします
//         if (entry != null && entry.address == address && entry.parentGroup == targetParent) {
//             return;
//         }
//
//         // アセットをグループに追加します
//         entry = settings.CreateOrMoveEntry(guid: guid, targetParent: targetParent, readOnly: false,
//             postEvent: true);
//         if (entry == null) {
//             var sb = new StringBuilder();
//             sb.AppendLine("[AddressableUtils] Failed AddressableAssetSettings.CreateOrMoveEntry.");
//             sb.AppendLine($"path: {path}");
//             sb.AppendLine($"address: {address}");
//             sb.AppendLine($"targetParent: {targetParent.Name}");
//             Debug.LogError(sb.ToString());
//             return;
//         }
//
//         // アセットにアドレスを割り当てます
//         entry.SetAddress(addr: address, postEvent: true);
//     }
//
//     // 指定されたアセットにラベルを割り当てます
//     public static void SetLabelToAssetOrFolder(string path, string label, bool enable)
//     {
//         var settings = GetSettings();
//         var guid = AssetDatabase.AssetPathToGUID(path);
//         var entry = settings.FindAssetEntry(guid);
//
//         // アセットにラベルを割り当てます
//         // enable が false なら解除します
//         // force に true を渡すと、Settings に存在しないラベルが指定された場合、
//         // Settings に自動でラベルが追加されます
//         entry?.SetLabel(label: label, enable: enable, force: true, postEvent: true);
//     }
//
//     // デフォルト以外のすべてのグループを削除します
//     public static void RemoveAllGroupWithoutDefault()
//     {
//         var settings = GetSettings();
//         var groups = settings.groups;
//         for (int i = groups.Count - 1; 0 <= i; i--) {
//             var group = groups[i];
//             if (group.IsDefaultGroup()) continue;
//
//             settings.RemoveGroup(group);
//         }
//     }
//
//     // デフォルト以外のすべての空のグループを削除します
//     public static void RemoveAllEmptyGroupWithoutDefault()
//     {
//         var settings = GetSettings();
//         var groups = settings.groups;
//         for (int i = groups.Count - 1; 0 <= i; i--) {
//             var group = groups[i];
//             if (group.IsDefaultGroup()) continue;
//             if (0 < group.entries.Count) continue;
//
//             settings.RemoveGroup(group);
//         }
//     }
//
//     // Play Mode Script のインデックスを設定します
//     public static void SetActivePlayModeDataBuilderIndex(int index)
//     {
//         var settings = GetSettings();
//         settings.ActivePlayModeDataBuilderIndex = index;
//     }
//
//     // アドレスが設定されているすべてのアセットを返します
//     public static List<AddressableAssetEntry> GetAllAssets()
//     {
//         var settings = GetSettings();
//         var assets = new List<AddressableAssetEntry>();
//         settings.GetAllAssets(assets, true);
//         return assets;
//     }
//
//     // 重複しているすべてのアドレスを返します
//     public static IEnumerable<string> GetDuplicateAddress()
//     {
//         return GetAllAssets().GroupBy(c => c.address).Where(c => 1 < c.Count()).Select(c => c.Key);
//     }
//
//     // グループを名前でソートします
//     public static void SortGroups()
//     {
//         var settings = GetSettings();
//         var groups = settings.groups;
//         groups.Sort((a, b) => a.Name.CompareTo(b.Name));
//     }
//
//     // AddressableAssetsWindow の描画を更新します
//     public static void RepaintAddressableAssetsWindow()
//     {
//         const BindingFlags attr = BindingFlags.Instance
//             | BindingFlags.Public
//             | BindingFlags.NonPublic;
//         const string assemblyName = "Unity.Addressables.Editor";
//         const string windowTypeName = "UnityEditor.AddressableAssets.GUI.AddressableAssetsWindow";
//         const string groupEditorTypeName =
//             "UnityEditor.AddressableAssets.GUI.AddressableAssetsSettingsGroupEditor";
//         var assembly = Assembly.Load(assemblyName);
//         var windowType = assembly.GetType(windowTypeName);
//         var windows = Resources.FindObjectsOfTypeAll(windowType);
//         var isOpen = 1 <= windows.Length;
//         if (!isOpen) return;
//
//         var window = windows[0] as EditorWindow;
//         var groupEditorType = assembly.GetType(groupEditorTypeName);
//         var groupEditorField = windowType.GetField("m_GroupEditor", attr);
//         var method = groupEditorType.GetMethod("Reload", attr);
//         var groupEditor = groupEditorField.GetValue(window);
//         method.Invoke(groupEditor, new object[0]);
//         window.Repaint();
//     }
//
//     // すべてのキャッシュをクリアします
//     // ビルドしたアセットバンドルのキャッシュを削除する関数は ClearBuildCache です
//     public static bool ClearDownloadCache()
//     {
//         return Caching.ClearCache();
//     }
//
//     // すべてのキャッシュをクリアします
//     // ビルドしたアセットバンドルのキャッシュを削除する関数は ClearBuildCache です
//     public static bool ClearDownloadCache(int expiration)
//     {
//         return Caching.ClearCache(expiration);
//     }
//
//     // 従来のアセットバンドルの仕組みで使用するアセットバンドル名をすべて削除します
//     public static void RemoveAllAssetBundleName()
//     {
//         foreach (var n in AssetDatabase.GetAllAssetBundleNames()) {
//             AssetDatabase.RemoveAssetBundleName(n, true);
//         }
//     }
//
//     // 指定された Schema をすべてのグループから取得します
//     public static IEnumerable<T> GetAllSchemas<T>() where T : AddressableAssetGroupSchema
//     {
//         var settings = GetSettings();
//         var groups = settings.groups;
//         var schemas = groups.SelectMany(c => c.Schemas).OfType<T>();
//         return schemas;
//     }
//
//     // 指定された BundledAssetGroupSchema の AssetBundle Provider を設定します
//     public static void SetAssetBundleProviderType(this BundledAssetGroupSchema schema,
//         Type assetBundleProviderType)
//     {
//         const string name = "m_AssetBundleProviderType";
//         const BindingFlags attr = BindingFlags.Instance
//             | BindingFlags.Public
//             | BindingFlags.NonPublic;
//         var serializedType = new SerializedType {
//             Value = assetBundleProviderType,
//             ValueChanged = true,
//         };
//         var type = typeof(BundledAssetGroupSchema);
//         var value = type.GetField(name, attr);
//         value.SetValue(schema, serializedType);
//     }
//
//     // アセットバンドルをビルドします
//     public static void Build()
//     {
//         AddressableAssetSettings.BuildPlayerContent();
//     }
//
//     // アセットバンドルをクリーンビルドします
//     public static void CleanBuild()
//     {
//         ClearBuildCache();
//         Build();
//     }
//
//     // ビルドしたアセットバンドルのキャッシュを削除します
//     // ダウンロードしたアセットバンドルのキャッシュを削除する関数は ClearDownloadCache です
//     public static void ClearBuildCache()
//     {
//         AddressableAssetSettings.CleanPlayerContent();
//         BuildCache.PurgeCache(false);
//     }
//
//     // すべてのグループ名を返します
//     public static IEnumerable<string> GetAllGroupName()
//     {
//         var settings = GetSettings();
//         var groupNames = settings.groups.Select(c => c.Name);
//         return groupNames;
//     }
//
//     // すべてのアドレスを返します
//     public static IEnumerable<string> GetAllAddress()
//     {
//         var regex = new Regex(@"(.*)\[.*\]");
//         var addresses = GetAllAssets()
//             .Select(c => c.address)
//
//             // アドレスの一覧にスプライトも含まれてしまうので
//             // スプライト（アドレスの末尾に[]が付くもの）は除外する
//             .Select(c => regex.Replace(c, "$1"))
//
//             // 重複しているアドレスも1つにまとめる
//             // 重複しているアドレスを知りたい場合は
//             // GetDuplicateAddress を使用する
//             .GroupBy(c => c)
//             .Select(c => c.Key);
//         return addresses;
//     }
//
//     // ダウンロードしたアセットバンドルのキャッシュが保存されるフォルダのパスを返します
//     public static string GetDownloadCacheFolderPath()
//     {
//         const Environment.SpecialFolder folderType = Environment.SpecialFolder.LocalApplicationData;
//         var folderPath = Environment.GetFolderPath(folderType);
//         var companyName = Application.companyName;
//         var productName = Application.productName;
//         var path = $"{folderPath}Low/Unity/{companyName}_{productName}";
//         path = path.Replace("/", "\\");
//         return path;
//     }
//
//     // すべてのラベルを返します
//     public static IEnumerable<string> GetAllLabel()
//     {
//         var settings = GetSettings();
//         var so = new SerializedObject(settings);
//         var labelTable = so.FindProperty("m_LabelTable");
//         var labelNames = labelTable.FindPropertyRelative("m_LabelNames");
//         for (int i = 0; i < labelNames.arraySize; i++) {
//             var labelName = labelNames.GetArrayElementAtIndex(i);
//             yield return labelName.stringValue;
//         }
//     }
//
// }
//
// }
// #endif