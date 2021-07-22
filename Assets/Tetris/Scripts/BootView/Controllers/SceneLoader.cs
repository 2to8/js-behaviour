using Common.JSRuntime;
using Database;
using GameEngine.Extensions;
using GameEngine.Models.Contracts;
using IngameDebugConsole;
using MoreTags;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utils.Settings;

namespace MainScene.BootScene.Utils
{
    [Serializable]
    public class SceneLoader : ViewManager<SceneLoader>
    {
        public static Env js;
        static bool isDownloading;
        static bool isStarted;
       // static SceneLoader m_Instance;
        string k_loading = "正在更新";

//        public static SceneLoader instance {
//            get => m_Instance ??= FindObjectOfType<SceneLoader>();
//            set => m_Instance = value;
//        }

        [OdinSerialize]
        public HashSet<ScriptableObject> preloadData = new HashSet<ScriptableObject>();

        public static Dictionary<object, GameObject> entity = new Dictionary<object, GameObject>();
        static IEnumerable<IResourceLocator> locators;

        [SerializeField]
        public Slider progress;

        // [SerializeField]
        // public Button btn;

        [FormerlySerializedAs("jsEnv")]
        [FormerlySerializedAs("Env")]
        [SerializeField]
        Env m_Env;

        [FormerlySerializedAs("env")]
        [SerializeField]
        public AssetReference m_envReference;

        [SerializeField]
        Transform m_PrivacyPage;

        [SerializeField]
        Transform m_UpdatePage;

        [SerializeField]
        Button m_AgreeButton;

        [SerializeField]
        Button m_DeagreeButton;

        [SerializeField]
        TMP_Text m_ErrorMsg;

        [SerializeField]
        Button m_UpdateButton;

        [SerializeField]
        Button m_StartGameButton;

        [SerializeField]
        DBManager m_DBManager;

        [SerializeField]
        TagManager m_TagManager;

        [SerializeField]
        ConfigManager m_ConfigManager;

        [SerializeField]
        public AssetReference nextScene;

        [SerializeField]
        public Button StartGameButton;

        [SerializeField]
        public Button UpdateButton;

        [FormerlySerializedAs("prompt")]
        [SerializeField]
        TMP_Text statusText;

        [SerializeField]
        public TMP_Text ErrorText;

        [FormerlySerializedAs("lables")]
        [SerializeField]
        AssetLabelReference[] prefabs;

        [SerializeField]
        public TMP_Text VerText;

        long downloaded;
        public static List<string> check;
        AsyncOperationHandle downloadHandle;
        bool m_Sleep;
        string sizeStr;
        long totalSize;

        // public AsyncOperationHandle dh { get; set; }
        public long currentSize { get; set; }
        public bool FirstRun { get; set; }

        async void Start()
        {
            if (!Application.isEditor && !Debug.isDebugBuild && PlayerPrefs.GetInt($"{Consts.k_Debug}", 0) != 1)
                DebugLogManager.Instance?.gameObject.SetActive(false);

            //await LoadPrefab(entity, BootLoader.id.IngameDebugConsole, BootLoader.id.BootUI);
        }

        public async Task beforeStart()
        {
            preloadData.ForEach(t => (t as ISingle)?.SetInstance(t));
            foreach (var t in prefabs) {
                Debug.Log($"start loading: {t.labelString}".ToGreen());
                downloadHandle = Addressables.DownloadDependenciesAsync(t);
                downloadHandle.Completed += operationHandle => { Debug.Log($"{t.labelString} downloaded"); };
                await downloadHandle.Task;
                Addressables.Release(downloadHandle);
            }

            await Env.ReloadEnv(null, m_envReference);
        }

        public async Task RunStart()
        {
            // VerText.text = Application.version;

            // if (js == null) {
            //     js = m_Env;
            // }
            FirstRun = PlayerPrefs.GetInt($"{Consts.k_FirstRun}") == 1;
            Debug.Log($"BootUI Starting, First Run {FirstRun}".ToYellow());
            if (FirstRun) {
                // Debug.Log(targets.Keys.Select(t => $"{t}").Join()+$" count: {targets.Count()}");
                m_PrivacyPage.SetAsLastSibling();
                m_DeagreeButton.onClick.AddListener(() => {
                    if (Application.isEditor) {
#if UNITY_EDITOR
                        EditorApplication.isPlaying = false;
#endif
                    }

                    Application.Quit();
                });
                m_AgreeButton.onClick.AddListener(async () => {
                    PlayerPrefs.SetInt($"{Consts.k_FirstRun}", 1);
                    m_UpdatePage.SetAsLastSibling();
                    await OnInitial();
                });
            }
            else {
                m_UpdatePage.SetAsLastSibling();
            }

            //if (!FirstRun) return;
            await OnInitial();
        }

        // [RuntimeInitializeOnLoadMethod]
        public async Task OnInitial()
        {
            Debug.Log("BootUI Initial");
            var NotFirstInited = !PlayerPrefs.HasKey(BootLoader.FIRST_DOWNLAODED) && !BootLoader.isInternet;
            m_UpdateButton.onClick.AddListener(async () => {
                await UpdateCatalog();
                await instance.DoStart();
            });
            m_ErrorMsg.gameObject.SetActive(NotFirstInited);
            m_UpdateButton.gameObject.SetActive(NotFirstInited);
            m_StartGameButton.gameObject.SetActive(true);
            if (!NotFirstInited) m_UpdateButton.GetComponent<Button>().onClick.Invoke();
        }

        public static async Task LoadPrefab(Dictionary<object, GameObject> container, params object[] names)
        {
            for (var i = 0; i < names.Length; i++) {
                await Addressables.DownloadDependenciesAsync($"{names[i]}", true).Task;
                container[names[i]] = await Addressables.InstantiateAsync($"{names[i]}").Task;
            }
        }

        // IEnumerator Test()
        // {
        //     yield break;
        // }

        void Update()
        {
            // if (m_Sleep) {
            //     return;
            // }
            if (!FirstRun) return;
            progress.gameObject.SetActive(isStarted && isDownloading);
            statusText.gameObject.SetActive(isStarted && isDownloading);

            //StartCoroutine(Test());
            if (isStarted) {
                if (isDownloading)
                    progress.value = downloaded / totalSize * 100;
                else
                    progress.value = 100;

                // statusText.text = Math.Abs(progress.value - 100) < float.Epsilon
                //     ? ""
                //     : $"{(totalSize - progress.value / 100f * totalSize) / 1024f / 1024f:F} Mb";
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        static void ClearCaches()
        {
            // var path = Application.persistentDataPath + "/com.unity.addressables";
            // Directory.Delete(path, true);
            // Directory.CreateDirectory(path);
            // ClearDependencyCacheForAddressable();
        }

        public async void DoUpdate()
        {
            await DoStart();
        }

        public async Task DoStart()
        {
            isStarted = true;
            await DoDownloadAssets();
        }

        public SceneLoader Sleep(bool value = true)
        {
            return this.Of(t => t.m_Sleep = value);
        }

        public static bool AddressableResourceExists(object key)
        {
            foreach (var l in Addressables.ResourceLocators) {
                IList<IResourceLocation> locs;
                if (l.Locate(key, typeof(object), out locs)) return true;
            }

            return false;
        }

        public static bool CheckExists(string key)
        {
            return AddressableResourceExists(key);
        }

        public static void ClearDependencyCacheForAddressable(string key = null)
        {
            if (key == null)
                Addressables.ResourceLocators.SelectMany(loc => loc.Keys).ForEach(k => {
                    Addressables.ClearDependencyCacheAsync(k);
                });
            else
                Addressables.ClearDependencyCacheAsync(key);
        }

        public void StartGame()
        {
            Debug.Assert(!nextScene.IsValid(), "nextScene.IsValid()");
            Addressables.LoadSceneAsync(nextScene).Completed += handle => {
                handle.Result.Scene.GetRootGameObjects().ForEach(go => go.CheckTagsFromRoot());
            };

            //nextScene.LoadSceneAsync();
        }

        public static async Task UpdateCatalog()
        {
            // yield return initHandle;
            //var check = await Addressables.CheckForCatalogUpdates(false).Task;
            check = await Addressables.CheckForCatalogUpdates().Task;

            // yield return checkHandle;
            Debug.Log($"update catalog: {check?.Count > 0}");
            if (check?.Count > 0) locators = await Addressables.UpdateCatalogs(check).Task;
            locators = locators?.Any() == true ? locators : Addressables.ResourceLocators;
        }

        async Task DoDownloadAssets()
        {
            if (locators == null) await UpdateCatalog();
            var keys = new List<object>();
            var all = locators.SelectMany(loc => loc.Keys).ToList();
            totalSize = await Addressables.GetDownloadSizeAsync(all).Task;
            var sizes = new Dictionary<object, long>();
            all.ForEach(async key => {
                try {
                    sizes[key] = await Addressables.GetDownloadSizeAsync(key).Task;
                }
                catch (InvalidKeyException) { }
            });
            var itemNum = sizes.Values.Count(t => t > 0);
            Debug.Log($"Download Size: {itemNum} items / {totalSize / 1024f / 1024f:F} Mb");
            if (totalSize > 0) {
                isDownloading = true;
                var j = 0;
                foreach (var key in sizes.Keys.Where(k => sizes[k] > 0)) //key = all[j];
                    try {
                        currentSize = sizes[key];
                        statusText.text = $"{k_loading}: {j += 1}/{itemNum}, {totalSize / 1024f / 1024f:F} Mb";
                        if (currentSize > 0) {
                            await Addressables.DownloadDependenciesAsync(key).Task;
                            downloaded += currentSize;

                            //currentSize = 0;

                            // if (dh.Status == AsyncOperationStatus.Succeeded) {
                            //     keys.Add(key);
                            // }
                        }
                    }
                    catch (InvalidKeyException) { }

                isDownloading = false;
                m_StartGameButton.onClick.Invoke();
            }
            else {
                UpdateButton.gameObject.SetActive(false);
                StartGameButton.onClick.Invoke();
            }
        }
    }
}