using Common.JSRuntime;
using GameEngine;
using GameEngine.Extensions;
using GameEngine.Models.Contracts;
using MoreTags;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniRx.Async;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.Rendering;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MainScene.BootScene.Utils
{
/*
// Usage example:
UnityWebRequest www = new UnityWebRequest();
// ...
await www.SendWebRequest();
Debug.Log(req.downloadHandler.text);
*/

    public class BootLoader : ViewManager<BootLoader>
    {
        //
        static Scene scene => SceneManager.GetActiveScene();
        public const string FIRST_DOWNLAODED = "FIRST_DOWNLAODED";

        [FormerlySerializedAs("m_AssetReferenceGameObject")]
        [FormerlySerializedAs("bootUi")]
        [SerializeField]
        AssetReferenceGameObject bootUiAsset;

        [SerializeField]
        GameObject prefab;

        [SerializeField]
        AssetReference[] scriptables;

        [SerializeField]
        AssetReference[] settings;

        // [FormerlySerializedAs("Canvas"), SerializeField]
        // Canvas m_Canvas;
        //public static BootLoader instance => m_instance ??= FindObjectOfType<BootLoader>();
        public static string resUrl;

        public enum id
        {
            BootScene,
            BootUI,
            IngameDebugConsole
        }

        static SceneLoader loader;
        public static string url = "https://static.joycraft.mobi/data/1.0";

        // static readonly string[] preloads = {
        //     /* $"{id.IngameDebugConsole}",*/ $"{id.BootUI}"
        // };
        public static bool isInternet;

        [SerializeField]
        AssetLabelReference[] SettingLabel;

     

        IEnumerator GetRequest(string url, Action<UnityWebRequest> callback)
        {
            using (var request = UnityWebRequest.Get(url)) {
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("Accept", "application/json");
                request.certificateHandler = new BypassCertificate();

                // Send the request and wait for a response
                yield return request.SendWebRequest();
                callback?.Invoke(request);
            }
        }

        public static async Task<bool> hasNetwork()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable) return false;
            var getRequest = new UnityWebRequest("https://static.joycraft.mobi/data/url.json");

            //UnityWebRequest.Get("https://static.joycraft.mobi/data/url.json");
            await getRequest.SendWebRequest();
            var result = getRequest.downloadHandler.text;
            if (!result.IsNullOrEmpty()) {
                var tmp = new resUrl();
                JsonUtility.FromJsonOverwrite(result, tmp);
                if (!tmp.url.IsNullOrEmpty()) {
                    url = tmp.url;
                    return true;
                }
            }

            return false;
        }

        // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        // static void OnInit()
        // {
        //     if (SceneManager.GetActiveScene().name != "BootScene") return;
        //    // FindObjectOfType<SceneLoader>()?.gameObject.SetActive(false);
        // }

        // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        //[RuntimeInitializeOnLoadMethod]
        async void Start()
        {
            {
                // if (Application.isEditor && SceneManager.GetActiveScene().name != "BootScene") return;
                if (Application.internetReachability == NetworkReachability.NotReachable) {
                    offline();
                }
                else {
                    isInternet = true;
                    download();

                    // StartCoroutine(GetRequest("https://static.joycraft.mobi/data/url.json", req => {
                    //     if (req.result == UnityWebRequest.Result.ConnectionError) {
                    //         Debug.Log($"{req.error}");
                    //     } else {
                    //         Debug.Log(req.downloadHandler.text);
                    //         var tmp = new resUrl();
                    //         JsonUtility.FromJsonOverwrite(req.downloadHandler.text, tmp);
                    //
                    //         if (!tmp.url.IsNullOrEmpty()) {
                    //             url = tmp.url;
                    //
                    //             isInternet = true;
                    //             download();
                    //
                    //             return;
                    //         }
                    //     }
                    //     offline();
                    // }));
                }

                //isInternet = await hasNetwork();

                // if (!isInternet) {
                //     target.SetActive(true);
                //
                //     return;
                // }
            }
        }

        void offline()
        {
            isInternet = false;
            Debug.Assert(prefab != null, "bootUI != null");
            Instantiate(prefab, null, true).CheckTagsFromRoot();

            //bootUI.gameObject.SetActive(true);
        }

        void download()
        {
            if (Application.isEditor || Debug.isDebugBuild) url = "http://127.0.0.1:3310";
            Addressables.InitializeAsync().Completed += async handle => {
                await SceneLoader.UpdateCatalog();

                // Addressables.DownloadDependenciesAsync(instance.bootUi).Completed += operationHandle => {
                //     //instance.target.DestroySelf();
                //     instance.bootUi.InstantiateAsync().Completed += async asyncOperationHandle => {
                //         await SceneLoader.OnInitial();
                //     };
                // };

                //await Addressables.DownloadDependenciesAsync(SceneLoader.check).Task;

                // foreach (var key in preloadData.Keys) {
                //     // preloadData[key].GetType().GetProperty()
                //     if (key is ISingle single) {
                //         single.SetInstance(await preloadData[key].LoadAssetAsync<ScriptableObject>().Task);
                //     }
                // }
                //
                // foreach (var reference in settings) {
                //     if (reference != null) {
                //         var res = await reference.LoadAssetAsync<ScriptableObject>().Task;
                //         (res as ISingle)?.SetInstance(res);
                //     }
                // }

                // Transforms
                await Addressables.DownloadDependenciesAsync(bootUiAsset, true).Task;
                Addressables.LoadAssetAsync<GameObject>(bootUiAsset).Completed += async h => {
                    if (h.Result == null) {
                        Debug.Log($"{h.Status}");
                        return;
                    }

                    if (!PlayerPrefs.HasKey(FIRST_DOWNLAODED))
                        PlayerPrefs.SetString(FIRST_DOWNLAODED, $"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}");

                    //await h.Result.GetComponentInChildren<SceneLoader>().OnInitial();
                    var go = Instantiate(h.Result);
                    var p = SceneLoader.instance ?? go.GetComponentInChildren<SceneLoader>();
                    await p.beforeStart();
                    await p.RunStart();
                    go.CheckTagsFromRoot();
                };
            };
        }
    }
}