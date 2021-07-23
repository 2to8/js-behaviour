using System.Collections.Generic;
using System.Threading.Tasks;
using GameEngine.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GlobalScene
{
    [DisallowMultipleComponent]
    public class Profile : Manager<Profile>
    {
        public List<ScriptableObject> Data;
        public TextAsset SqliteFile;
        public string DbVersion;
        public bool OpenNextScene = true;
        public AssetReference nextSceneAddress;
        public List<AssetReference> testPrefabs;

        public override async void Start()
        {
            base.Start();
            DontDestroyOnLoad(gameObject);
            for (var i = 0; i < testPrefabs.Count; i++) await testPrefabs[i].InstantiateAsync();
        }
    }
}