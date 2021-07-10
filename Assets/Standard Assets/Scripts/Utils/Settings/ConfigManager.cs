using GameEngine.Attributes;
using GameEngine.Models.Contracts;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Utils.Settings
{
    [CreateAssetMenu(fileName = nameof(ConfigManager), menuName = "Scriptable/" + nameof(ConfigManager))]
    [PreloadSetting]
    public class ConfigManager : DbTable<ConfigManager>
    {
        public static ConfigManager instance;

        //
        public AssetReference globalScene;
        public AssetReference mainMenuScene;
        public string test;
    }
}