#if false
namespace GameKit.Demo
{
    public partial class Utils : Bootstrap<Utils>
    {
        private static bool inited = false;
        private static bool loaded;

        // ADDRESSABLES UPDATES
        public AssetReference player;
        public AssetReference startMenu;
        public AssetLabelReference hazardsLabel;
        private List<IResourceLocation> hazardLocations;
        private AsyncOperationHandle preloadOp;

        public Slider progressBar;
        public Text versionTxt;

        // public Text restartText;
        // public Text gameOverText;
        public Text loadingText;

        public string nextSceneAddress;

        // bool gameOver;
        // bool restart;
        // int score;

        protected override async UniTask Awake()
        {
            if (loaded)
            {
                return;
            }

            DontDestroyOnLoad(gameObject);

            loaded = true;
#if XLUA
            var t = await Addressables.LoadResourceLocationsAsync("lua", typeof(TextAsset)).Task;
            foreach(var irl in t) {
                Debug.Log(irl.PrimaryKey);
                var content = await Addressables.LoadAssetAsync<TextAsset>(irl.PrimaryKey).Task;
                if(!content.text.IsNullOrWhitespace()) {
                    Debug.Log(content.text);
                    GameApp.luaAssets[irl.PrimaryKey] = content;
                }
            }
#endif

            await base.Awake();
#if XLUA
            if (Application.isEditor)
            {
                GameApp.luaEnv.DoString("require('LuaDebuggee').StartDebug('127.0.0.1', 9826)");
            }
#endif
            //SendEvent<E_StartUp>();

            //var self = await Addressables.InstantiateAsync("Prefabs/App.prefab").Task;
            //DontDestroyOnLoad(gameObject);

            // ADDRESSABLES UPDATES
            if (loadingText != null)
            {
                loadingText.text = $"正在检查更新";
            }

            preloadOp = Addressables.DownloadDependenciesAsync("preload");

            //await preloadOp.Task;
            LoadHazards();
        }

        private void LoadHazards()
        {
            Addressables.LoadResourceLocationsAsync(hazardsLabel.labelString).Completed += async op =>
            {
                if (op.Status == AsyncOperationStatus.Failed)
                {
                    if (loadingText != null)
                    {
                        loadingText.text = "资源加载错误, 重试中";
                    }

                    Debug.Log("Failed to load hazards, retrying in 1 second...");
                    Invoke(nameof(LoadHazards), 1);

                    return;
                }

                if (progressBar != null)
                {
                    progressBar.value = 0;
                }

                if (LuaScript != null)
                {
                    await LuaScript.Run();
                }
            };
        }

        // ADDRESSABLES UPDATES
        private async void OnHazardsLoaded(AsyncOperationHandle<IList<IResourceLocation>> op)
        {
            // hazardLocations = new List<IResourceLocation>(op.Result);
            //

            //
            // await startMenu.LoadSceneAsync(LoadSceneMode.Additive);

            // startMenu.LoadSceneAsync(LoadSceneMode.Additive).Completed += async op2 => {
            //     if(op2.Status == AsyncOperationStatus.Failed) {
            //         loadingText.text = "资源加载错误, 重试中";
            //
            //         Debug.Log("Failed to load player prefab. Check console for errors.");
            //         ;
            //         Invoke(nameof(LoadHazards), 1);
            //     } else {
            //         progressBar.gameObject.SetActive(false);
            //
            //         if(Instance.LuaScript != null) {
            //             await Instance.LuaScript.Run();
            //         }
            //
            //         // gameOver = false;
            //         // restart = false;
            //         // restartText.text = "";
            //         // gameOverText.text = "";
            //         // score = 0;
            //         // UpdateScore();
            //         // StartCoroutine(SpawnWaves());
            //     }
            // };

            // if(Instance.LuaScript != null) {
            //     await Instance.LuaScript.Run();
            // }
        }

        protected override async UniTask Update()
        {
            await base.Update();

            if (preloadOp.IsValid())
            {
                if (preloadOp.PercentComplete > 0)
                {
                    if (loadingText != null)
                    {
                        loadingText.text = $"下载更新: {(int) (preloadOp.PercentComplete * 100)}%";
                    }
                }

                if (progressBar != null)
                {
                    progressBar.value = preloadOp.PercentComplete;
                }

                if (preloadOp.PercentComplete.eq(1))
                {
                    Addressables.Release(preloadOp);
                    preloadOp = new AsyncOperationHandle();
                    if (loadingText != null)
                    {
                        loadingText.text = "";
                    }
                }
            }
        }

        //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void OnLoad()
        {
            if (inited)
            {
                return;
            }

            inited = true;

            Application.targetFrameRate = 30;

            //  return;

            const string sceneName = "Bootstrap"; // SceneManager.GetSceneByBuildIndex(0).name;

            //if(!Application.isEditor) {
            Addressables.DownloadDependenciesAsync("init").Completed +=
                op => { Addressables.LoadSceneAsync(sceneName); };

            //}

            //
            // if(SceneManager.GetActiveScene().name != "Boot") {
            //     await ;
            //
            // }
        }
    }
}
#endif