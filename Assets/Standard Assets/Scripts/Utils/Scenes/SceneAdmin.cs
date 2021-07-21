using Common.JSRuntime;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Threading.Tasks;
using Consts;
using Sirenix.Serialization;
using Sirenix.Utilities;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Utils.Scenes
{
    [DisallowMultipleComponent, ExecuteAlways]
    public class SceneAdmin : SerializedMonoBehaviour
    {
//        //
//        // public static IEnumerable<Scene> GetAllLoadedScenes()
//        // {
//        //     for (int i = 0; i < SceneManager.sceneCount; i++) {
//        //         yield return SceneManager.GetSceneAt(i);
//        //     }
//        // }
        public static Dictionary<Scene, SceneAdmin> instances = new Dictionary<Scene, SceneAdmin>();
        public List<AssetReference> OtherScenes = new List<AssetReference>();

        [SerializeField]
        AssetReference m_EnvReference;

        [SerializeField]
        Env m_Env;

        [SerializeField]
        public AssetReference CurrentSceneAsset;

        [SerializeField, HideInInspector]
        public string ScenePath;
//
#if UNITY_EDITOR
        [ShowInInspector]
        public string m_ScenePath => ScenePath = AssetDatabase.GUIDToAssetPath(CurrentSceneAsset?.AssetGUID);
//
//        [InitializeOnLoadMethod, DidReloadScripts]
//        static void CheckAndLoadThis()
//        {
//            //Debug.Log("OnEnable called");
//            //SceneManager.sceneLoaded -= OnSceneLoaded;
//            //SceneManager.sceneLoaded += OnSceneLoaded;
//            EditorApplication.delayCall -= OnDelayCall;
//            EditorApplication.delayCall += OnDelayCall;
//        }
//
//        static void OnDelayCall()
//        {
//            Debug.Log(SceneManager.GetSceneByName("JsGlobal").isLoaded);
//            if (!SceneManager.GetSceneByName("JsGlobal").isLoaded) {
//                //Debug.LogError("load global scene");
//                EditorSceneManager.OpenScene("Assets/Res/Scenes/JsGlobal.unity", OpenSceneMode.Additive);
//            }
//        }
//
//        // called second
//        static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//        {
//            OnDelayCall();
////            Debug.Log("OnSceneLoaded: " + scene.name);
////            Debug.Log(mode);
//        }
#endif
//
        protected void Awake()
        {
            instances[gameObject.scene] = this;
            LoadScenes();
        }

        [Button]
        void LoadScenes()
        {
            foreach (var reference in OtherScenes) {
#if UNITY_EDITOR
                Debug.Log(AssetDatabase.GUIDToAssetPath(reference.AssetGUID));
#endif
                reference?.LoadSceneAsync(LoadSceneMode.Additive, true);
            }
        }
    }
}