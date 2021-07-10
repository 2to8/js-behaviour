using GameEngine.Attributes;
using GameEngine.Models.Contracts;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.ResourceManagement.Util;

namespace Database
{
    [CreateAssetMenu(fileName = nameof(DBManager), menuName = "Scriptable/" + nameof(DBManager)), PreloadSetting]
    public class DBManager : SerializedScriptableObject,ISingle/*, IObjectInitializationDataProvider*/
    {
        //
        public TextAsset file;
        public TextAsset devFile;

        static DBManager _instance;

        public static DBManager instance {
            get => _instance ??= Core.FindOrCreatePreloadAsset<DBManager>();
            set => _instance = value;
        }

        void OnEnable()
        {
            _instance = _instance ??= this;
        }

        /*public ObjectInitializationData CreateObjectInitializationData()
        {
            _instance = _instance ??= Core.FindOrCreatePreloadAsset<DBManager>();
        #if UNITY_EDITOR
            if (Application.isEditor) {
                return ObjectInitializationData.CreateSerializedInitializationData<DBManager>(Name, _instance);
            }
        #endif
            return default;
        }

        public string Name => nameof(DBManager);*/
        public void SetInstance(ScriptableObject target)
        {
            _instance = target as DBManager;
        }
    }
}