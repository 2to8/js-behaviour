// using com.unity.cloudbase;

using GameEngine.Models.Contracts;
using MoreTags.TagHelpers;
using Puerts.Attributes;
using Sirenix.OdinInspector;
using SQLite.Attributes;
using System.Collections.Generic;
using System.Linq;

// using UnityEditor.AddressableAssets;
// using UnityEditor.AddressableAssets.Settings;
#if UNITY_EDITOR

// using UnityEditor;
// using UnityEditor.AddressableAssets.Settings;
#endif
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MoreTags.Models
{
    [CreateAssetMenu(fileName = nameof(AssetLabels), menuName = "Scriptable/" + nameof(AssetLabels))]
    public class AssetLabels : DbTable<AssetLabels>
    {
        public List<AssetLabelReference> labels = new List<AssetLabelReference>();
#if UNITY_EDITOR
        UnityEditor.AddressableAssets.Settings.AddressableAssetSettings settings =>
            UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject.Settings;

        public string NewLabel;
        [Button]
        void AddLabel()
        {
            if (!string.IsNullOrEmpty(NewLabel) && !settings.GetLabels().Contains(NewLabel)) {
                settings.AddLabel(NewLabel);
            }

            if (labels.All(t => t.labelString != NewLabel)) {
                var label = new AssetLabelReference {
                    labelString = NewLabel
                };
                labels.Add(label);
            }
        }
#endif
#if UNITY_EDITOR
//        [UnityEditor.InitializeOnLoadMethod, PuertsIgnore]
//        static void AutoLoad()
//        {
//            Core.FindOrCreatePreloadAsset<AssetLabels>();
//        }

        [UnityEditor.MenuItem("Tests/Get Asset Labels")]
        static void GetAssetLabels()
        {
            Debug.Log(string.Join(", ", AssetTool.GetLables()));
        }
#endif
    }
}