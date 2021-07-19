using GameEngine.Extensions;
using IngameDebugConsole;
using Puerts;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.UI;

namespace MainScene.BootScene.Unused
{
    public class LoadStartScene : SerializedMonoBehaviour
    {
        [SerializeField]
        public Slider m_Slider;

        [SerializeField]
        public TMP_Text m_LoadingText;

        [SerializeField]
        public TMP_Text versionText;

        //public string m_SceneName = "Main";
        public AssetReference m_CurrentScene;
        public string Loading = "Loading";
        public string strUpdating = "正在检查更新...";
        public string strContinue = "Continue";

        [NonSerialized]
        public bool finished = false;

        [SerializeField]
        [Title("是否加载JS代码", horizontalLine: false)]
        public bool useJs;

        [SerializeField]
        [Multiline]
        public string JsCode;

        static Common.JSRuntime.Env js => Common.JSRuntime.Env.instance;

        [SerializeField]
        public List<AssetLabelReference> LabelReferences;

        [LabelText("编辑模式加载主场景")]
        public bool AutoLoadMainScene = false;

        [NonSerialized]
        public GenericDelegate JsAwake;

        [NonSerialized]
        public GenericDelegate JsEnable;

        [NonSerialized]
        public GenericDelegate JsUpdate;

        [NonSerialized]
        public float timer;

        [NonSerialized]
        public int current = -1;

        [NonSerialized]
        public AsyncOperationHandle downloadHandle;

        [NonSerialized]
        public long totalDownloadSize;

        [NonSerialized]
        public int needCount = -1;

        public static AsyncOperationHandle<SceneInstance> loadHandle;

        //
        //   public AssetReference globalScene;
        //  public AssetReference mainMenuScene;
        //public AssetReference configAsset;

        // async void Start()
        // {
        //     if (Application.isEditor) return;
        //
        //     await Addressables.InitializeAsync();
        //     await Addressables.DownloadDependenciesAsync("default");
        //     await Addressables.DownloadDependenciesAsync("Assets/Settings/Config.asset");
        //     var config =
        //         await Addressables.LoadAssetAsync<Settings.Config>("Assets/Settings/Config.asset");
        //     await Addressables.DownloadDependenciesAsync(config.globalScene);
        //     await Addressables.DownloadDependenciesAsync(config.mainMenuScene);
        //     await config.globalScene.LoadSceneAsync(LoadSceneMode.Additive);
        //     await config.mainMenuScene.LoadSceneAsync(LoadSceneMode.Additive);
        // }

        void Awake()
        {
            versionText.text = Application.version;
            if (useJs && string.IsNullOrEmpty(JsCode))
                useJs = false;
            else if (useJs) js.Call(JsCode, this);
            JsAwake?.Action(this);
            if (!Application.isEditor && !Debug.isDebugBuild) {
                FindObjectOfType<DebugLogManager>()?.gameObject.SetActive(false);
                GameObject.FindGameObjectsWithTag("EditorOnly").ForEach(go => { go?.DestroySelf(); });
            }
        }

        async void OnEnable()
        {
            m_Slider.value = 0;

            // await Addressables.InitializeAsync().Task;
            if (JsEnable?.Func<LoadStartScene, bool>(this) != true) await CheckUpdate();
        }

        void Update()
        {
            // m_LoadingText.gameObject.SetActive(!finished);
            // m_Slider.gameObject.SetActive(!finished);
            // versionText.gameObject.SetActive(finished);
            if (JsUpdate?.Func<LoadStartScene, bool>(this) == true) return;
            if (finished) {
                m_LoadingText.text = strContinue;
                m_Slider.value = 1f;
            }

            if (finished || !downloadHandle.IsValid() && !loadHandle.IsValid()) return;
            timer += Time.deltaTime;
            var t = (int) timer % 4;
            if (t != current) {
                current = t;
                m_LoadingText.text = Loading + " " + new string('.', current);
            }

            //m_VersionText.text = $"{BootScene}";
            if (downloadHandle.IsValid() && !downloadHandle.IsDone) {
                var percent = downloadHandle.PercentComplete;
                m_Slider.value = percent; // Mathf.Floor(percent * 100f);
                Loading = $"下载更新({(1 - percent) * totalDownloadSize / 1024f / 1024f:F}M)";

                //$"下载中：{totalDownloadSize*percent/1024f/1024f:F}M/{totalDownloadSize/1024f/1024f:F}M";
            }
            else if (loadHandle.IsValid() && !loadHandle.IsDone) {
                var percent = loadHandle.PercentComplete;
                m_Slider.value = percent; // Mathf.Floor(percent * 100f);
                Loading = $"载入中";
            }
        }

        public async Task CheckUpdate()
        {
            Debug.Log($"{GetType().GetNiceName()}  start");
            var checkHandle = Addressables.CheckForCatalogUpdates(false);
            var checkResult = await checkHandle.Task;
            if (checkHandle.IsValid()) Addressables.Release(checkHandle);
            Loading = $"{strUpdating}";
            var keys = new List<object>();
            if (checkResult.Count > 0) {
                var updateHandle = Addressables.UpdateCatalogs(checkResult, false);
                IList<IResourceLocator> locators = await updateHandle.Task;
                keys = locators.SelectMany(list => list.Keys).ToList();
                if (updateHandle.IsValid()) Addressables.Release(updateHandle);
            }

            // else {
            //     keys =   new List<object> { "Default" };
            // }
            keys.ForEach(t => Debug.Log($"{t}"));
            if (keys.Count > 0) {
                var sizeHandle = Addressables.GetDownloadSizeAsync(keys);
                totalDownloadSize = await sizeHandle.Task;
                if (sizeHandle.IsValid()) Addressables.Release(sizeHandle);
            }

            Debug.Log($"totalSize: {totalDownloadSize}");
            if (totalDownloadSize > 0) {
                Loading = $"需要更新: {totalDownloadSize / 1024f / 1024f:F}M";
                downloadHandle = Addressables.DownloadDependenciesAsync(keys, Addressables.MergeMode.Union);
                var result = await downloadHandle.Task;
                Debug.Log($"{downloadHandle.Status}");

                //  Addressables.Release(downloadHandle);
                m_Slider.value = 1;
                Loading = $"更新完成";
                finished = true;
                if (downloadHandle.IsValid()) Addressables.Release(downloadHandle);
            }
            else {
                finished = true;
            }

            await LoadMainScene();
        }

        async Task LoadMainScene()
        {
            if (!AutoLoadMainScene && Application.isEditor) return;

            //m_GotoNext = false;
            //Instance.Status.text = $"加载中...";

            // 场景和资源都要先加载依赖
            await Addressables.DownloadDependenciesAsync(m_CurrentScene, true).Task;
            if (m_CurrentScene.RuntimeKeyIsValid())
                loadHandle = m_CurrentScene.LoadSceneAsync();
            else Debug.LogError("runtime key is not valid");

            //loadHandle = m_CurrentScene.LoadSceneAsync();
            finished = false;
            loadHandle.Completed += res => {
                finished = true;
                if (res.Status == AsyncOperationStatus.Succeeded)
                    res.Result.ActivateAsync();
                else
                    Debug.LogError($"[SceneLoad] {loadHandle.DebugName} error");
            };
            await loadHandle.Task;
        }
    }
}