using System.IO;
using System.Linq;
using Common;
using GameEngine.Extensions;
using GameUtils;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.X509.Qualified;
using Sirenix.Utilities;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Tetris
{
    [ExecuteAlways]
    public class AnimeHelper : Component<AnimeHelper>
    {
        [SerializeField]
        Animator m_Animator;

        void Awake()
        {
            m_Animator ??= GetComponent<Animator>();
        }
        //#if UNITY_EDITOR
//        public class MyAssetModificationProcessor : AssetModificationProcessor
//        {
//            public static string[] OnWillSaveAssets(string[] paths)
//            {
//                // Get the name of the scene to save.
//                string scenePath = string.Empty;
//                string sceneName = string.Empty;
////EditorSceneManager.loadedSceneCount
//                foreach (string path in paths) {
//                    if (path.Contains(".unity")) {
//                        scenePath = Path.GetDirectoryName(path);
//                        sceneName = Path.GetFileNameWithoutExtension(path);
//                        var scene = EditorSceneManager.GetSceneByPath(path);
//                        Debug.Log($"[save]{sceneName}".ToYellow());
//                        scene.GetRootGameObjects()
//                            .SelectMany(go => go.GetComponentsInChildren<Animator>(true).ForEach((a,i) => {
//                                Debug.Log($"{i}:{a.gameObject.name}",a.gameObject);
//                            }));
//                    }
//                }
//
//                if (sceneName.Length == 0) {
//                    return paths;
//                }
//
//                // do stuff
//                return paths;
//            }
//        }
//#endif
    }
}