#if UNITY_EDITOR
#endif
using Common.JSRuntime.Settings;
using GameEngine.Attributes;
using GameEngine.Extensions;
using GameEngine.Models.Contracts;
using GameEngine.Utils;
using MoreTags;
using NodeCanvas.Framework;
using NodeCanvas.Tasks.Actions;
using Org.BouncyCastle.Crypto.Tls;
using Puerts;
using Puerts.Attributes;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.IO;
using System.Linq;
using Sirenix.Serialization;
using UniRx.Async;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UniWebServer;
using Utils.Scenes;
using Object = UnityEngine.Object;
using Task = System.Threading.Tasks.Task;

namespace Common.JSRuntime
{
    [PuertsIgnore, PreloadSetting]
    public partial class Env : DbTable<Env>, ILoader, IDisposable
    {
        //public TextAsset bundleFile;

        //static Env m_Instance;
        new static Env Instance => instance;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        static void clearStatic()
        {
            if (m_Instance != null) {
                m_Instance.Inited = false;
                m_Instance = null;
            }
        }

        bool Inited { get; set; }

        [SerializeField]
        public GameObject RedisPrefab;

        [SerializeField]
        AssetReference m_Reference;

        public static bool useInstanceOnly = false;
        public bool Running;
        public string root = string.Empty;
        public bool m_WaitDebug = true;
        public bool enabled;

        static RedisClient redis =>
            RedisClient.Instance; /*
            ?? (useRedis && instance?.RedisPrefab != null
                ? Instantiate(instance.RedisPrefab).GetComponent<RedisClient>()
                : null);*/

        static bool useRedis => false && (Debug.isDebugBuild || Application.isEditor);

        [Title("主模块", horizontalLine: false)]
        public TextAsset mainModule;

        //[ShowInInspector]
        int size => mainModule?.text.Length ?? 0;

        [SerializeField]
        string path = "/redis";

        [SerializeField]
        AssetReference m_EmptySceneReference;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        static void clear_env()
        {
            m_Instance = null;
        }

        public AssetLabelReference[] presetLabel;
        public AssetLabelReference[] bootstrapLabel;
        const string requestChannel = "test-channel";
        const string responseChannel = "Response";
        EmbeddedWebServerComponent server;
        public JsonResponse response = new JsonResponse();
        RedisClient.OnReceivedPubSubMessageDelegate SubMessageDelegate;

        [SerializeField]
        int m_DebugPort = 9229;

        bool disposed;
        string lastModuleName;

        //public JsEnvObject Require(string moduleName) => Require<object>(moduleName, null);
        public object lastResult;
#if UNITY_EDITOR
        EditorApplication.CallbackFunction m_CallbackFunction;
#endif
        JsEnv m_Env;
        public Dictionary<string, TextAsset> modules = new Dictionary<string, TextAsset>();
        TextAsset result;
        Object rootObj;
        public List<Type> UsedTypes = new List<Type>();

        public static Env instance {
            get {
                if (m_Instance == null) {
                    if (Application.isEditor) {
                        m_Instance = Core.FindOrCreatePreloadAsset<Env>();
                    }
                    else {
                        return null;
                    }
                }

                //if (!m_Instance.Inited) m_Instance.Init();
                return m_Instance;
            }
            set {
                m_Instance = value;

                //m_Instance.Init();
            }
        }

        [SerializeField]
        AssetReference m_Asset;

        public static async Task ReloadEnv(Action action = null, AssetReference assetReference = null)
        {
            var isFirst = m_Instance == null;
            if (!isFirst) {
                await Addressables.InitializeAsync().Task;
            }

            var check = await Addressables.CheckForCatalogUpdates(false).Task;

            // yield return checkHandle;
            Debug.Log($"[reload] update catalog: {check?.Count > 0}");
            if (check?.Count > 0) {
                var locators = await Addressables.UpdateCatalogs(check).Task;
                if (locators?.Any() == true) {
                    try {
                        var keys = locators.SelectMany(t => t.Keys);
                        var size = await Addressables.GetDownloadSizeAsync(keys).Task;
                        if (size > 0) {
                            await Addressables.DownloadDependenciesAsync(keys, Addressables.MergeMode.Union, true).Task;
                            Debug.Log($"[reload] catalog download: {size}");
                            foreach (var key in keys) {
                                var s = await Addressables.GetDownloadSizeAsync(key).Task;
                                if (s > 0) {
                                    Debug.Log($"[reload] {key} =>  {s}");
                                    await Addressables.DownloadDependenciesAsync(key, true).Task;
                                }
                            }
                        }
                    }
                    catch (InvalidKeyException) { }
                }

                // var list = Addressables.ResourceLocators;
                var all = Addressables.ResourceLocators.SelectMany(loc => loc.Keys).ToList();
                var totalSize = await Addressables.GetDownloadSizeAsync(all).Task;
                Debug.Log($"[reload] total size: {totalSize} num: {locators.Count()}/{all.Count()}");
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
                    var j = 0;
                    foreach (var key in sizes.Keys.Where(k => sizes[k] > 0)) {
                        //key = all[j];
                        try {
                            var currentSize = sizes[key];
                            Debug.Log($" {j += 1}/{itemNum}, {totalSize / 1024f / 1024f:F} Mb");
                            if (currentSize > 0) {
                                await Addressables.DownloadDependenciesAsync(key, true).Task;
                            }
                        }
                        catch (InvalidKeyException) { }
                    }

                    //await Addressables.DownloadDependenciesAsync(locators).Task;
                }
            }

            await Addressables.DownloadDependenciesAsync("Env", true).Task;
            instance = await Addressables.LoadAssetAsync<Env>("Env").Task;

            // await Addressables.DownloadDependenciesAsync(m_Asset).Task;
            // instance = await m_Asset.LoadAssetAsync<Env>().Task;
            await Addressables.DownloadDependenciesAsync(instance.m_TagManagerAsset, true).Task;
            TagManager.instance = await Addressables.LoadAssetAsync<TagManager>(instance.m_TagManagerAsset).Task;

            //TagManatager.Instance.SetInstance();
            if (!isFirst && SceneAdmin.instances.TryGetValue(SceneManager.GetActiveScene(), out var asset)) {
                if (asset.CurrentSceneAsset != null && asset.CurrentSceneAsset.RuntimeKeyIsValid()) {
                    var size = await Addressables.GetDownloadSizeAsync(asset.CurrentSceneAsset).Task;
                    Debug.Log($"check scene reload size: {size}".ToYellow());
                    if (size > 0 || check?.Count > 0) {
                        Debug.Log($"reload scene".ToRed());

                        //asset.CurrentSceneAsset?.UnLoadScene();
                        await Addressables.DownloadDependenciesAsync(asset.CurrentSceneAsset, true).Task;
                        await Addressables.DownloadDependenciesAsync(instance.m_EmptySceneReference, true).Task;
                        Debug.Log($"reload scene2".ToRed());

                        // var result = await Addressables.LoadResourceLocationsAsync(instance.m_EmptySceneReference).Task;

                        // SceneManager.LoadSceneAsync(result[0].InternalId)
                        //     .Of(tt => tt.completed += async operation => {
                        //var scene = SceneManager.GetSceneByPath(result[0].InternalId);

                        //Error while getting Asset Bundle: The AssetBundle 'http://localhost:3310/data/1.0/StandaloneWindows64/sceneprefabs_scenes_main_34ce6590b10d775fd5c5520bd524c2ed.bundle' can't be loaded because another AssetBundle with the same files is already loaded.

                        //scene.GetRootGameObjects().ForEach(go => go.CheckTagsFromRoot());
                        var result = await Addressables.LoadResourceLocationsAsync(instance.m_EmptySceneReference).Task;
                        SceneManager.LoadSceneAsync(result[0].InternalId).Of(t => t.completed += async handle => {
                            await Addressables.LoadSceneAsync(asset.CurrentSceneAsset).Of(sh => {
                                if (sh.Status == AsyncOperationStatus.Succeeded) {
                                    var scene = sh.Result.Scene;
                                    scene.GetRootGameObjects().ForEach(go => go.CheckTagsFromRoot());
                                }

                                Addressables.ResourceManager.Acquire(sh);
                                Addressables.Release(sh);
                            }).Task;
                        });

                        //    });
                    }
                }
            }

            action?.Invoke();
        }

        public void OnScriptReload(Action<Env> action = null, bool reset = false)
        {
//            if (action == null || reset) {
//                env.OnReloadScript = null;
//            }
//
//            if (action != null) {
//                env.OnReloadScript += () => { action.Invoke(this); };
//            }
        }

        [OdinSerialize]
        public Dictionary<string, string> paths = new Dictionary<string, string>();

#if UNITY_EDITOR
        [ButtonGroup("config")]
        void SetPaths()
        {
            modules.ForEach((tk, i) => {
                if (tk.Value != null) {
                    var path = AssetDatabase.GetAssetPath(tk.Value);
                    if (!path.IsNullOrWhitespace()) {
                        paths[tk.Key] = path;
                    }
                }
            });
        }
#endif

        void Init()
        {
            if (Inited) return;
            if (!modules.TryGetValue("bootstrap", out var main)) {
                modules["bootstrap"] = mainModule;
            }

            Debug.Log($" JS INIT: use redis: {useRedis && redis?.Connect() == true}".ToRed());
            if (useRedis && redis?.Connect() == true) {
                // if (useRedis && redis?.Connect() == true) {
                if (SubMessageDelegate == null) {
                    SubMessageDelegate = async (ch, message) => {
                        Debug.Log($"{ch} sub".ToYellow());
                        if (ch == requestChannel) {
                            if (message == "reload") {
                                Debug.Log("reload javacript".ToYellow());
                                env.ClearModuleCache();

                                //
                                // Init();
                                //env.Dispose();
                                Inited = false;
                                forceReload = true;
                                await ReloadEnv();
                                Init();

                                // SceneManager.LoadSceneAsync(0).completed += operation => {
                                SceneManager.GetActiveScene().GetRootGameObjects().ForEach(go => {
                                    go.CheckTagsFromRoot();
                                });

                                //};
                            }
                            else {
                                var res = new JsonResponse();
                                res["Source"] = message;
                                try {
                                    Debug.Log(res["Result"] = env.Eval<object>(message));
                                }
                                catch (Exception e) {
                                    Debug.LogError(res["Error"] = e.ToString());
                                }

                                redis.Publish(responseChannel, res);
                            }
                        }
                    };
                }

                redis.Subscribe(requestChannel);
                redis.OnReceivedPubSubMessage -= SubMessageDelegate;
                redis.OnReceivedPubSubMessage += SubMessageDelegate;

                // }
                var content = new TextAsset(redis.Get("ts://app.cjs"));
                Debug.Assert(!content.text.IsNullOrEmpty(), $"!content.text.IsNullOrEmpty() {content.text.Length}");
                if (!content.text.IsNullOrEmpty()) {
                    Debug.Log($"Reset Bootstrap".ToYellow());
                    modules["bootstrap"] = content;
                }
            }

            // 加载sourcemap
            Call("require('sourcemap')");
            Debug.Log($"bootstrap module size: {modules["bootstrap"].text.Length}");
            Call("require('index').default || require('index')"/*, new BindScript()*/);
            Inited = true;
        }

        public static bool forceReload { get; set; }

        // public static bool IsInited => m_Instance != null;

        public JsEnv env {
            get {
                if (m_Env.IsDisposed() != false) {
                    Debug.Log("JsEnv Reload 01".ToRed());
                    m_Env = new JsEnv(this,
                        m_WaitDebug && (Application.isEditor || Debug.isDebugBuild) ? m_DebugPort : -1);
                    AutoRegisterActions.RegisterActions().Invoke(m_Env);
                    Init();
                }

                return m_Env;
            }
            set => m_Env = value;
        }

        [ShowInInspector, PuertsIgnore]
        Object setRoot {
            get {
#if UNITY_EDITOR
                if (rootObj != null) {
                    root = AssetDatabase.GetAssetPath(rootObj);
                }
#endif
                return rootObj;
            }
            set => rootObj = value;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            enabled = true;

            // #if UNITY_EDITOR
            //     m_WaitDebug = PuertsDebug.DebugMode;
            //     if (!Application.isPlaying) {
            //         if (m_WaitDebug) {
            //             if (m_CallbackFunction == null) {
            //                 m_CallbackFunction = async () => {
            //                     await OnUpdate();
            //                 };
            //             }
            //             EditorApplication.update -= m_CallbackFunction;
            //             EditorApplication.update += m_CallbackFunction;
            //         } else if (m_CallbackFunction != null) {
            //             EditorApplication.update -= m_CallbackFunction;
            //         }
            //     }
            // #endif
            if (m_Env.IsDisposed() != false && Application.isPlaying) {
                Debug.Log("[begin]".ToPadSides().ToRed());
#if UNITY_EDITOR
                m_Instance ??= this;

                // if (!m_Instance.Inited) {
                //     m_Instance.Init();
                // }
#endif

                //     Debug.Log("JsEnv Reload 02".ToRed());
                //     JsEnv.OnInit = AutoRegisterActions.RegisterActions();
                //     m_Env = new JsEnv(this, m_WaitDebug && (Application.isEditor || Debug.isDebugBuild) ? m_DebugPort : -1);
                //Call("console.log('appliction init');");
            }

            //useRedis = true;
        }

        [RuntimeInitializeOnLoadMethod]
        static void TestEnv()
        {
            Debug.Log($"[Test] {m_Instance == null}");
            if (m_Instance == null && Application.isEditor) {
                m_Instance = Core.FindOrCreatePreloadAsset<Env>();

                // if (!m_Instance.Inited) {
                //     m_Instance.Init();
                // }
            }
        }

        public Env WaitDebug(Func<bool?, bool> func = null)
        {
            if (m_Env == null) {
                return this;
            }

            while (Application.isEditor && !m_Env.DebuggerJoined()) {
#if UNITY_EDITOR
                if (func == null || !func.Invoke(null) ||
                    !func.Invoke(EditorUtility.DisplayDialog("debug", "wait debug...", "Yes", "No"))) {
                    break;
                }
#endif
            }

            return this;
        }

        public Env WaitDebug(ref bool NoWait)
        {
            if (NoWait || m_Env == null) {
                return this;
            }

            while (Application.isEditor && !m_Env.DebuggerJoined()) {
#if UNITY_EDITOR
                if (!EditorUtility.DisplayDialog("debug", "wait debug...", "Yes", "No")) {
                    NoWait = true;
                    break;
                }
#endif
            }

            return this;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (m_Env?.IsDisposed() == false) {
                Debug.Log("[end]".ToPadSides().ToRed());
            }

            if (useRedis && redis != null) {
                redis.OnReceivedPubSubMessage -= SubMessageDelegate;
            }

            // #if UNITY_EDITOR
            //     if (m_CallbackFunction != null) {
            //         EditorApplication.update -= m_CallbackFunction;
            //     }
            // #endif
        }

        //void Awake() => OnEnable();

        public void Dispose()
        {
            // disposed = true;
            // jsEnv?.Dispose();
            if (Application.isEditor) {
                DestroyImmediate(this);
            }
            else {
                Destroy(this);
            }
        }

        public bool FileExists(string filepath)
        {
            result = null;

            // if(!filepath.Contains(".")) {
            //     filepath += ".js";
            // }
            // if(filepath.StartsWith("puerts/")) {
            //     return true;
            // }

            // 不用 lastindexof, 因为有js.txt这种
            var pathWithoutExt = filepath.Substring(0, filepath.IndexOf('.'));
            if (useRedis && redis?.Connect() == true && !string.IsNullOrEmpty(redis.Get($"ts://{pathWithoutExt}"))) {
                result = new TextAsset(redis.Get($"ts://{pathWithoutExt}"));
                return true;
            }

            if (modules.TryGetValue(pathWithoutExt, out result)) {
                return true;
            }

            if (result = Resources.Load<TextAsset>(filepath) ??
                Resources.LoadAll(pathWithoutExt)?.FirstOrDefault() as TextAsset) {
                return true;
            }
#if UNITY_EDITOR
            if (Application.isEditor) {
                if (result = AssetDatabase.LoadAssetAtPath<TextAsset>(root + "/" + filepath)) {
                    return true;
                }

                if (result = AssetDatabase.LoadAssetAtPath<TextAsset>(filepath)) {
                    return true;
                }

                result = AssetDatabase.FindAssets("t:TextAsset", new[] {"Assets", "Packages"})
                    .Select(AssetDatabase.GUIDToAssetPath)
                    .Where(p => p.Substring(0, p.IndexOf('.')).EndsWith(pathWithoutExt))
                    .Select(AssetDatabase.LoadAssetAtPath<TextAsset>).FirstOrDefault(t => t != null);
                return result != null;
            }

#endif

            // if(File.Exists(Path.Combine(Path.GetDirectoryName(Application.dataPath), root, filepath))) {
            //     return true;
            // }
            Debug.LogError($"file not found: {filepath}");
            return false;
        }

        public string ReadFile(string filepath, out string debugpath)
        {
            // #if PUERTS_GENERAL
            //             debugpath = Path.Combine(root, filepath);
            //             return File.ReadAllText(debugpath);
            // #else
            var file = FileExists(filepath) ? result : null;
            var pathWithoutExt = filepath.Substring(0, filepath.IndexOf('.'));

            // UnityEngine.TextAsset file = (UnityEngine.TextAsset)UnityEngine.Resources.Load(filepath);
            debugpath = Path.Combine(root, filepath);
            if (file != null) {
#if UNITY_EDITOR
                var tpath = AssetDatabase.GetAssetPath(file);
                if (!tpath.IsNullOrWhitespace()) {
                    debugpath = $"{Path.GetDirectoryName(Application.dataPath)}/{tpath}";
                }
#endif
                if (modules.ContainsKey(pathWithoutExt)) {
                    debugpath = Path.GetDirectoryName(Application.dataPath) + "/" + paths[pathWithoutExt];
                }

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
                debugpath = debugpath.Replace("/", "\\");
#endif
                return file.text;
            }

            return null;

            // debugpath = System.IO.Path.Combine(root, filepath);
            //
            // return file == null ? null : file.text;

            // #endif
        }

        // public static T Use<T>(JsEnv env, Func<JsEnv, T> func)
        // {
        //
        // }
        public static void Register(int id) { }

        public void Reload()
        {
            // while (PuertsDLL.InspectorTick(jsEnv.isolate)) {
            //      PuertsDLL.DestroyInspector(jsEnv.isolate);
            // }
#if UNITY_EDITOR
            EditorUtility.DisplayDialog("start", "close", "ok", "cancel");
#endif

            //jsEnv.Dispose();
            //JsEnv.RuntimeReload();
            Debug.Log("JsEnv Reload 03");
            m_Env = new JsEnv(this, m_WaitDebug && (Application.isEditor || Debug.isDebugBuild) ? m_DebugPort : -1);
#if UNITY_EDITOR
            EditorUtility.DisplayDialog("end", "close", "ok", "cancel");
#endif
        }

        //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static Env ClearInstance()
        {
            if (Application.isEditor && !Application.isPlaying) {
                DestroyImmediate(m_Instance);
            }
            else {
                Destroy(m_Instance);
            }

            return instance;
        }

        public Env Tick()
        {
            env.Tick();
            return this;
        }

        public async Task OnUpdate()
        {
            if (env?.IsDisposed() == false) {
                //Debug.Log("check debug");
                await env.WaitDebuggerAsync(js => { js.Tick(); }, true);
            }
        }

        public bool Expired(string moduleName) => false;

        public Env AddModule(params TextAsset[] modules)
        {
            modules.ForEach(module => {
                if (module == null || this.modules.ContainsValue(module)) {
                    return;
                }
#if UNITY_EDITOR
                this.modules[module.ResourcePath()] = module;
                return;
#endif
                this.modules[$"{module.GetHashCode()}"] = module;
            });
            return this;
        }

        public Env Require(TextAsset module) => Require<object>(module, null);

        public Env ClearCache()
        {
            env.ClearModuleCache();
            return this;
        }

        public Env Begin()
        {
            Running = true;
            return this;
        }

        public Env End()
        {
            Running = false;
            return this;
        }

        public Env Require<T>(TextAsset module, T bind) where T : class
        {
            AddModule(module);
#if UNITY_EDITOR
            return Load(module?.ResourcePath(), bind);
#endif
            return Load($"{module.GetHashCode()}", bind);
        }

        public Env Load(string moduleName) => Load<object>(moduleName, null);

        public Env Load<T>(string moduleName, T bind) where T : class
        {
            lastModuleName = moduleName;
            if (!string.IsNullOrEmpty(moduleName)) {
                // if (bind != default) {
                //     if (!UsedTypes.Contains(typeof(T))) {
                //         env.UsingAction<Action<T>>();
                //     }
                // }
                lastResult = env.Eval<object>($"global._{moduleName.md5()} = require('{moduleName}')");
                if (bind != default) {
                    env.Eval<Action<T>>($"global._{moduleName.md5()}.init")?.Invoke(bind);
                }
            }

            return this;
        }

        public GeneralGetter Getter;

        [SerializeField]
        AssetReference m_TagManagerAsset;

        public GenericDelegate GetBinding<T>(T component, string method = null) where T : Component
        {
            if (component == null) {
                return null;
            }

            return env.Eval<GenericDelegate>($"globalThis['Components']?.{component.GetType().Name}" +
                (method == null ? null : "?." + method));
        }

        public TResult Call<T, TResult>(string cmd, T param)
        {
            if (cmd == null) {
                return default;
            }

            var ret = env.Eval<Func<T, TResult>>(cmd);
            return ret != null ? ret.Invoke(param) : default;
        }

        public TResult Call<T1, T2, TResult>(string cmd, T1 p1, T2 p2)
        {
            if (cmd == null) {
                return default;
            }

            var ret = env.Eval<Func<T1, T2, TResult>>(cmd);
            return ret != null ? ret.Invoke(p1, p2) : default;
        }

        public Env Call<T>(string cmd, T bind)
        {
            if (!string.IsNullOrEmpty(cmd)) {
                if (bind != null) {
                    lastResult = env.Eval<Func<T, object>>($"{cmd}")?.Invoke(bind);
                }
                else {
                    lastResult = env.Eval<object>($"{cmd}");
                }
            }

            return this;
        }

        public Env Call<T1, T2>(string cmd, T1 a, T2 b)
        {
            if (!string.IsNullOrEmpty(cmd)) {
                lastResult = env.Eval<Func<T1, T2, object>>($"{cmd}")?.Invoke(a, b);
            }

            return this;
        }

        public Env Call(string cmd) => Call<object>(cmd, null);

        public Env Result<T>(Action<T> action)
        {
            action?.Invoke((T) lastResult);
            return this;
        }

        public TResult Result<T, TResult>(Func<T, TResult> func) => func.Invoke((T) lastResult);
        public static Env Create() => useInstanceOnly ? instance : CreateInstance<Env>();
    }
}