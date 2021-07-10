// using Sirenix.Utilities;
// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;
// using UnityEngine.AddressableAssets;
// using UnityEngine.AddressableAssets.ResourceLocators;
// using UnityEngine.ResourceManagement.AsyncOperations;
// using UnityEngine.ResourceManagement.ResourceProviders;
// using UnityEngine.SceneManagement;
// using UnityEngine.Serialization;
// using UnityEngine.UI;
// using Task = System.Threading.Tasks.Task;
//
// namespace GameEngine.Domain {
//
// public class App_Bootstrap : MonoBehaviour {
//
//     public static string m_SceneName;
//     public static App_Bootstrap Instance;
//     public static AssetReference m_CurrentScene;
//
//     [SerializeField]
//     Slider ProgressBar;
//
//     [SerializeField]
//     Text Status;
//
//     [FormerlySerializedAs("SceneName"), SerializeField]
//     Text m_VersionText;
//
//     [SerializeField]
//     AssetLabelReference[] LabelReferences;
//
//     const string strUpdating = "正在检查更新...";
//     AsyncOperationHandle downloadHandle;
//     long totalDownloadSize;
//     int needCount = -1;
//     static AsyncOperationHandle<SceneInstance> loadHandle;
// #if UNITY_EDITOR
//     [UnityEditor.InitializeOnEnterPlayMode]
//     static void ClearStaticProperty()
//     {
//         m_SceneName = string.Empty;
//         m_CurrentScene = null;
//     }
// #endif
//
//     [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
//     static async void Boot()
//     {
//         await Addressables.InitializeAsync().Task;
//
//         m_SceneName = SceneManager.GetActiveScene().name;
//
//         // if(Core.FindInRoot<Blackboard>() is Blackboard bb) {
//         //     var refs = bb.GetVariable<AssetReference>("SceneContext");
//         //     if(refs.value != null) {
//         //         m_CurrentScene = refs.value;
//         //     }
//         // } else
//
//         if (Core.FindInRoot<App_Bootstrap>() == null) {
//             return;
//         }
//
//         // if(m_SceneName.IsNullOrWhitespace()) {
//         //     m_SceneName = "BrickMain"; // SceneManager.GetSceneAt(0).name;
//         //
//         //     if( thisname != "Boot" && thisname != "Empty") {
//         //         m_SceneName = thisname;
//         //     }
//
//         if (m_SceneName != "Boot") {
//             //await Addressables.DownloadDependenciesAsync("preload").Task;
//             Debug.Log($"loading scene: {m_SceneName} {Instance == null}");
//             loadHandle = Addressables.LoadSceneAsync("Boot", LoadSceneMode.Single, false);
//
//             if (LoadingProgressBar.Instance != null) {
//                 LoadingProgressBar.Instance.Handle = loadHandle;
//             }
//             loadHandle.Completed += res => {
//                 if (res.Status == AsyncOperationStatus.Succeeded) {
//                     res.Result.ActivateAsync().completed += loaded => {
//                         Core.FindInRoot<App_Bootstrap>().enabled = true;
//                     };
//                 }
//             };
//             await loadHandle.Task;
//         }
//
//         //}
//     }
//
//     // [ RuntimeInitializeOnLoadMethod ]
//     // static void RuntimeCheck()
//     // {
//     //     Debug.Log(SceneManager.GetActiveScene().name);
//     // }
//
//     void Awake()
//     {
//         Instance = this;
//         ProgressBar.value = 0;
//
//         m_VersionText.text = Application.version;
//         Debug.Log($"{GetType().GetNiceName()} {m_SceneName} awake");
//
//         // DontDestroyOnLoad(gameObject);
//     }
//
//     public async void Start()
//     {
//         Debug.Log($"{GetType().GetNiceName()} {m_SceneName} start");
//
//         var checkHandle = Addressables.CheckForCatalogUpdates(false);
//         var checkResult = await checkHandle.Task;
//         Addressables.Release(checkHandle);
//         Status.text = $"{strUpdating}";
//
//         List<object> keys = null;
//
//         if (checkResult.Count > 0) {
//             var updateHandle = Addressables.UpdateCatalogs(checkResult, false);
//             IList<IResourceLocator> locators = await updateHandle.Task;
//             keys = locators.SelectMany(list => list.Keys).ToList();
//             Addressables.Release(updateHandle);
//         } else {
//             keys = new List<object> { "default" };
//         }
//         var sizeHandle = Addressables.GetDownloadSizeAsync(keys);
//         totalDownloadSize = await sizeHandle.Task;
//         Addressables.Release(sizeHandle);
//         Debug.Log($"totalSize: {totalDownloadSize}");
//
//         if (totalDownloadSize > 0) {
//             Status.text = $"需要更新: {totalDownloadSize / 1024f / 1024f:F}M";
//             downloadHandle = Addressables.DownloadDependenciesAsync(keys, Addressables.MergeMode.Union);
//             var result = await downloadHandle.Task;
//             Debug.Log($"{downloadHandle.Status}");
//             ProgressBar.value = 100;
//             Status.text = $"更新完成";
//             Addressables.Release(downloadHandle);
//         }
//         await LoadMainScene();
//     }
//
//     async void Update()
//     {
//         if (!downloadHandle.IsValid() && !loadHandle.IsValid()) {
//             return;
//         }
//
//         //m_VersionText.text = $"{BootScene}";
//
//         if (downloadHandle.IsValid() && !downloadHandle.IsDone) {
//             var percent = downloadHandle.PercentComplete;
//             ProgressBar.value = Mathf.Floor(percent * 100f);
//             Status.text = $"下载更新({(1 - percent) * totalDownloadSize / 1024f / 1024f:F}M)...";
//
//             //$"下载中：{totalDownloadSize*percent/1024f/1024f:F}M/{totalDownloadSize/1024f/1024f:F}M";
//         } else if (loadHandle.IsValid() && !loadHandle.IsDone) {
//             var percent = loadHandle.PercentComplete;
//             ProgressBar.value = Mathf.Floor(percent * 100f);
//             Status.text = $"载入中...";
//         }
//     }
//
//     async Task LoadMainScene()
//     {
//         //m_GotoNext = false;
//         //Instance.Status.text = $"加载中...";
//         //await Addressables.DownloadDependenciesAsync(BootScene).Task;
//         if (m_CurrentScene != null) {
//             loadHandle = m_CurrentScene.LoadSceneAsync(LoadSceneMode.Single, false);
//         } else {
//             loadHandle = Addressables.LoadSceneAsync(m_SceneName, LoadSceneMode.Single, false);
//         }
//
//         loadHandle.Completed += res => {
//             if (res.Status == AsyncOperationStatus.Succeeded) {
//                 res.Result.ActivateAsync();
//             } else {
//                 Debug.LogError($"[SceneLoad] {loadHandle.DebugName} error");
//             }
//         };
//
//         await loadHandle.Task;
//     }
//
// }
//
// }