using System.Collections.Generic;
using System.Threading.Tasks;
using GameEngine.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GlobalScene
{
    [DisallowMultipleComponent]
    public class Profile : SerializedMonoBehaviour
    {
        public List<ScriptableObject> Data;
        public TextAsset SqliteFile;
        public string DbVersion;
        public static Profile instance;
        public bool OpenNextScene = true;
        public AssetReference nextSceneAddress;
        public List<AssetReference> testPrefabs;

        void Awake()
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public async Task Start()
        {
            for (var i = 0; i < testPrefabs.Count; i++) await testPrefabs[i].InstantiateAsync();
        }
    }
}