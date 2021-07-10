using Database;
using GameEngine.Controllers.Contracts;
using GameEngine.Extensions;
using GameEngine.Models.Contracts;
using GameEngine.Utils;
using GameEngine.Views.Contracts;
using Sirenix.Utilities;
using SqlCipher4Unity3D;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using SQLiteNetExtensions.Extensions.TextBlob.Serializers;
using UnityEditor;

// using Unity.Entities;
// using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.SceneManagement;
using Component = UnityEngine.Component;
using Random = System.Random;

public static partial class Core
{
    static string db_suffix => Application.isEditor || Debug.isDebugBuild ? "_dev" : "";
    public static string DB_FILENAME => $"7456765D{db_suffix}";
    public static Common.JSRuntime.Env js => Common.JSRuntime.Env.instance;

    public static string FindRootPath(string dirname)
    {
        // DirectoryInfo info = new DirectoryInfo(Application.dataPath);
        // var dirs = info.EnumerateDirectories();
        var dirs = Directory.EnumerateDirectories(Application.dataPath, "*.*", SearchOption.AllDirectories);
        return dirs.FirstOrDefault(t => Path.GetFileName(t) == dirname)?.dataPathRoot();
    }

    public static T PickRandom<T>(this IEnumerable<T> source) => source.PickRandom(1).Single();
    public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count) => source.Shuffle().Take(count);

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
        return source.OrderBy(x => Guid.NewGuid());
    }

    public static AsyncOperationHandle<IList<IResourceLocation>> LoadByLables(Action<string> action, out bool result,
        params string[] keys)
    {
        var ret = false;
        var handle = Addressables.LoadResourceLocationsAsync(keys, Addressables.MergeMode.Intersection);
        handle.Completed += async op => {
            var key = op.Result.FirstOrDefault()?.PrimaryKey;
            if (ret = op.Status == AsyncOperationStatus.Succeeded && key != null) {
                await Addressables.DownloadDependenciesAsync(key).Task;
                action.Invoke(key);
            }
        };
        result = ret;
        return handle;
    }

    public static string UpdateServerUrl { get; set; } =
        "https://git.lug.ustc.edu.cn/joycraft/brick2020-assets/-/raw/master";

    public static T KeyByValue<T, W>(this Dictionary<T, W> dict, W val)
    {
        T key = default;
        foreach (var pair in dict) {
            if (EqualityComparer<W>.Default.Equals(pair.Value, val)) {
                key = pair.Key;
                break;
            }
        }

        return key;
    }

    static readonly Dictionary<string, ScriptableObject> models = new Dictionary<string, ScriptableObject>(); //名字---模型
    static readonly Dictionary<string, View> views = new Dictionary<string, View>(); //名字---视图
    static readonly Dictionary<string, Type> commandMap = new Dictionary<string, Type>(); //事件名字--控制器类型

    //static IEnumerable<Type> m_ControllerTypes;
    static readonly Dictionary<Type, IEnumerable<Type>> TypesCache = new Dictionary<Type, IEnumerable<Type>>();
    static SQLiteConnection m_DB;
    public static string m_seed;

    //= "A802F98A-A602-4EF6-9AD6-20BEDFB16574";
    public static Random m_random;
    static readonly Dictionary<Type, object> Tables = new Dictionary<Type, object>();
    public static IEnumerable<Type> ControllerTypes => GetTypes<Controller>();
    public static string RandomSeed => m_seed.IsNullOrWhitespace() ? m_seed = NewGuid() : m_seed;

    //public static Config Config => Instance.m_Config;

    public static string Version {
        get {
#if UNITY_EDITOR && false
            var v1 = DateTime.Now.ToString("ddH");
            var n = int.Parse(DateTime.Now.ToString("yy")) - 19;
            var m = DateTime.Now.ToString("MM");
            n += int.Parse(m.Substring(0, 1));
            var p = int.Parse(m.Substring(1) + v1.Substring(0, 1));
            var ver = $"{n}.{p}.{int.Parse(v1.Substring(1))}";
            var gc = new GregorianCalendar();
            var weekOfYear =
 gc.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            var dayOfWeek = (int)DateTime.Now.DayOfWeek;

            if(dayOfWeek == 0) {
                dayOfWeek = 7;
            }

            ver =
 $"{DateTime.Now:yyyy}.{weekOfYear}.{dayOfWeek}{int.Parse(DateTime.Now.ToString("HH"))}";

            if(PlayerSettings.bundleVersion != ver) {
                PlayerSettings.bundleVersion = ver;
            }

            return PlayerSettings.bundleVersion;
#else
            return Application.version;
#endif
        }
        set {
#if UNITY_EDITOR
            if (UnityEditor.PlayerSettings.bundleVersion != value) {
                UnityEditor.PlayerSettings.bundleVersion = value;
            }
#endif
        }
    }

    public static TextAsset UserFile;

    public static SQLiteConnection UserConnection {
        get {
            if (m_DB == null) {
                TypeDescriptor.AddAttributes(typeof((int, int)),
                    new TypeConverterAttribute(typeof(TupleConverter<int, int>)));

                m_DB = new DataService(ref UserFile, (DB_FILENAME + "UserData").md5() + ".bytes").Connection();
                m_DB.Trace = false; // GameCore.Config.Instance.LogSqliteQuery;
                if (m_DB.Trace) {
                    m_DB.Tracer += str => { Debug.Log(str); };
                }
                else {
                    m_DB.Tracer = null;
                }
            }

            return m_DB;
        }
        private set => m_DB = value;
    }

    public static SQLiteConnection Connection {
        get {
            if (m_DB == null) {
                TypeDescriptor.AddAttributes(typeof((int, int)),
                    new TypeConverterAttribute(typeof(TupleConverter<int, int>)));

                var asset = Application.isEditor || Debug.isDebugBuild
                    ? DBManager.instance.devFile
                    : DBManager.instance.file;
#if UNITY_EDITOR
                Debug.Log($"Asset Path: {AssetDatabase.GetAssetPath(asset)}");
                Debug.Log($"DB_FILENAME:{DB_FILENAME}");
#endif
                m_DB = new DataService(ref asset, DB_FILENAME + ".bytes").Connection();
                m_DB.Trace = false; // GameCore.Config.Instance.LogSqliteQuery;
                if (m_DB.Trace) {
                    m_DB.Tracer += str => { Debug.Log(str); };
                }
                else {
                    m_DB.Tracer = null;
                }
            }

            return m_DB;
        }
        private set => m_DB = value;
    }

    // Turn a GUID into a string and strip out the '-' characters.
    public static BigInteger huge => BigInteger.Parse(RandomSeed.Replace("-", ""), NumberStyles.AllowHexSpecifier);

    // 基于GUID的Random Seed
    public static Random Rnd => m_random ?? (m_random = new Random((int) (huge % int.MaxValue)));

    // {
    //     get {
    //         if ( m_ControllerTypes == null ) {
    //             var types = AssemblyUtilities.GetTypes(AssemblyTypeFlags.CustomTypes)
    //                 .Where(t => typeof(Controller).IsAssignableFrom(t));
    //             m_ControllerTypes = types;
    //         }
    //
    //         return m_ControllerTypes;
    //     }
    // }
#if UNITY_EDITOR && ECS
    [InitializeOnLoadMethod]
    static void InitWorld()
    {
        if(!Application.isPlaying) {
            DefaultWorldInitialization.DefaultLazyEditModeInitialize();
        }
    }
#endif
    public static IEnumerable<Type> GetTypes<T>() => GetTypes(typeof(T));

    public static IEnumerable<Type> GetTypes(Type type)
    {
        if (!TypesCache.ContainsKey(type)) {
            TypesCache[type] = typeof(Core).Assembly.GetTypes().Where(t1 => type.IsAssignableFrom(t1));
        }

        return TypesCache[type];
    }

    public static TA RandomEnumValue<TA>() where TA : Enum
    {
        var v = Enum.GetValues(typeof(TA));
        return (TA) v.GetValue(Rnd.Next(v.Length));
    }

    public static string NewGuid() => Guid.NewGuid().ToString("N");

    public static List<Component> FindObjectsOfTypeAll(Type type)
    {
        return SceneManager.GetAllScenes().Where(scene => scene.isLoaded).SelectMany(scen => scen.GetRootGameObjects())

            //.GetRootGameObjects()
            .SelectMany(g => g.GetComponentsInChildren(type, true)).ToList();
    }

    public static T FindInRoot<T>() where T : Component => FindInRoot(typeof(T)) as T;

    public static Component FindInRoot(Type type)
    {
        return SceneManager.GetAllScenes().Where(scene => scene.isLoaded).SelectMany(scen => scen.GetRootGameObjects())

            //.GetRootGameObjects()
            .Select(t => t.GetComponent(type)).FirstOrDefault(t => t != null);
    }

    public static List<T> FindObjectsOfTypeAll<T>()
    {
        return SceneManager.GetAllScenes().Where(scene => scene.isLoaded).SelectMany(scen => scen.GetRootGameObjects())

            //.GetRootGameObjects()
            .SelectMany(g => g.GetComponentsInChildren<T>(true)).ToList();
    }

    public static Component FindObjectOfTypeAll(Type type)
    {
        if (!SceneManager.GetActiveScene().isLoaded) {
            return null;
        }

        return SceneManager.GetAllScenes().Where(scene => scene.isLoaded).SelectMany(scen => scen.GetRootGameObjects())

            //.GetRootGameObjects()
            .SelectMany(g => g.GetComponentsInChildren(type, true)).FirstOrDefault();

        //.ToList();
    }

    public static T FindObjectOfTypeAll<T>() where T : Component
    {
        var ret = SceneManager.GetAllScenes()
            .Where(scene => scene.isLoaded /*|| scene == SceneManager.GetActiveScene()*/)
            .SelectMany(scene => scene.GetRootGameObjects())

            //.GetRootGameObjects()
            .SelectMany(g => g.GetComponentsInChildren<T>(true)).FirstOrDefault();
        if (ret == null && SceneManager.GetActiveScene().isLoaded) {
            ret = (GameObject.Find("/" + typeof(T).Name) ?? new GameObject(typeof(T).Name)).RequireComponent<T>();
        }

        return ret;

        //.ToList();
    }

    public static IEnumerable<Type> StateTypes<T>()
    {
        var t1 = ControllerTypes.Where(t =>
            t.BaseType != null && t.BaseType.IsGenericType && t.BaseType.GetGenericArguments().Contains(typeof(T)));
        return ControllerTypes.Where(t =>
            !t1.Contains(t) && t.BaseType != null && t.BaseType.IsGenericType &&
            t.BaseType.GetGenericArguments().Any(t2 => t1.Any(x => x == t2)));
    }

    //注册模型
    public static void RegisterModel(ScriptableObject model)
    {
        models[model.GetType().FullName + ""] = model;
    }

    public static bool isQuit;
    public static bool isBuilding;
    public static Action OnQuit;

    [RuntimeInitializeOnLoadMethod]
    static void RunOnStart()
    {
        Application.quitting += () => {
            isQuit = true;
            OnQuit?.Invoke();
            Debug.Log("Quitting the Player");
            OnDestroy();
        };
#if UNITY_EDITOR
        UnityEditor.EditorApplication.playModeStateChanged += mode => {
            isQuit = false;
            OnQuit = null;
        };
#endif
    }

    public static void OnDestroy()
    {
        /* liteDB */
        //DbModel.WriteToFile();
        if (Connection != null) {
            Connection.Close();
            Connection = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }

    public static DbData<T1> Data<T1>(T1 value) where T1 : DbData<T1>, new()
    {
        var type = typeof(T1);
        if (!Tables.ContainsKey(type)) {
            Tables[type] = new DbData<T1>();
        }

        if (Tables[type] is DbData<T1> data && value != null) {
            JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(value), data);
        }

        return Tables[typeof(T1)] as DbData<T1>;
    }

    //注册视图
    public static void RegisterView(View view)
    {
        // if(views.ContainsKey(view.getName())) {
        //     return;
        // }
        //
        // views[view.getName()] = view;
        // view.RegisterViewEvents(); //注册关心的事件
    }

    //注册控制器
    public static void RegisterController(string eventName, Type controllerType)
    {
        commandMap[eventName] = controllerType;
    }

    public static void RegisterController(object eventName, Type controllerType)
    {
        commandMap[eventName.GetType().FullName + "." + eventName] = controllerType;
    }

    public static void SendEvent(object eventName, params object[] data)
    {
        SendEvent<object>(GetName(eventName), data);
    }

    public static void SendEvent<T>(object eventName, params object[] data)
    {
        SendEvent<T>(GetName(eventName), data);
    }

#if ECS
    public static void SendEvent<T>(string eventName, params object[] data)
    {
        //SendEvent(eventName.GetType().FullName + "." + eventName, data);
        GetTypes<SystemBase>()
            .ForEach(t => {
                //Debug.Log(t.FullName);
                t.GetMethods(BindingFlags.Public |
                        BindingFlags.NonPublic |
                        BindingFlags.FlattenHierarchy |
                        BindingFlags.Instance |
                        BindingFlags.DeclaredOnly |
                        BindingFlags.Static)
                    .Where(mi => mi.GetCustomAttribute<Do.EventAttribute>(true)?.EventName == eventName)
                    .ForEach(t1 => {
                        //Debug.Log($"found: {t.FullName} {t1.GetFullName()}");
                        if(!t1.IsGenericMethod ||
                            t1.IsGenericMethod && t1.GetGenericArguments()[0].BaseType == typeof(T)) {
                            if(data == null ||
                                t1.IsGenericMethod ||
                                data.Select((kt, i) => new { kt, i })
                                    .Any(k => k.i >= t1.GetParameters().Length ||
                                        k.kt.GetType() != t1.GetParameters()[k.i].ParameterType) ==
                                false) {
                                var sys =
 World.DefaultGameObjectInjectionWorld.GetOrCreateSystem(t);
                                var method =
 t1.IsGenericMethod ? t1.MakeGenericMethod(typeof(T)) : t1;
                                method.Invoke(t1.IsStatic ? null : sys, data);
                            }

                            //Debug.Log($"bingo");
                            // } else if ( !t1.IsGenericMethod
                            //     && (data == null || data.GetType() == t1.GetParameters()[0].ParameterType) ) {
                            //     var sys = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem(t);
                            //     t1.Invoke(t1.IsStatic ? null : sys, data);
                            //
                        }
                    });
            });
    }
#endif
    public static string GetName(object name) => name == null ? string.Empty : name.GetType().FullName + ":" + name;

    //发送事件
    public static void SendEvent(string eventName, object data = null)
    {
        SendEvent<object>(eventName, data);

        //控制层
        // Type t = null;
        //
        // commandMap.TryGetValue(eventName, out t);
        // if ( t != null ) {
        //     var instance = Activator.CreateInstance(t);
        //     if ( instance is Controller ) {
        //         ((Controller)instance).Execute(data);
        //     }
        // }
        //
        // //视图层
        // foreach ( var v in views.Values ) {
        //     if ( v.attentionEvents.Contains(eventName) ) {
        //         v.HandleEvent(eventName, data);
        //     }
        // }
    }

    //获取模型
    public static T GetModel<T>() where T : DbTable<T>, new()
    {
        foreach (var m in models.Values) {
            if (m is T) {
                return (T) m;
            }
        }

        return null;
    }

    //获取视图
    public static T GetView<T>() where T : View
    {
        foreach (var v in views.Values) {
            if (v is T view) {
                return view;
            }
        }

        return null;
    }

    // //#if ECS
    // public static int AddAuthoring<T1, T2>() where T1 : Component where T2 : IComponentData
    // {
    //     var count = 0;
    //     var btntype = typeof(T2).Assembly.GetType(typeof(T2).FullName + "Authoring");
    //
    //     FindObjectsOfTypeAll<T1>()
    //         .Where(btn => btn.gameObject.GetComponent(btntype) == null)
    //         ?.ForEach(btn => {
    //             var func = btn.gameObject.GetType()
    //                 .GetMethods(BindingFlags.Instance | BindingFlags.Public)
    //                 .FirstOrDefault(tm =>
    //                     tm.Name.Equals("AddComponent", StringComparison.OrdinalIgnoreCase)
    //                     && tm.IsGenericMethod
    //                     && tm.GetGenericArguments()[0].BaseType == typeof(Component));
    //
    //             var fn = func?.MakeGenericMethod(btntype);
    //             count += 1;
    //
    //             if (btn.gameObject.GetComponent(btntype) == null) {
    //                 fn?.Invoke(btn.gameObject, null);
    //             }
    //
    //             // btn.gameObject.AddComponent("TBtn");
    //         });
    //
    //     FindObjectsOfTypeAll<T1>()
    //         .ForEach(btn => {
    //             var cc = btn.gameObject.GetComponent<ConvertToEntity>()
    //                 ?? btn.gameObject.AddComponent<ConvertToEntity>();
    //
    //             // if(cc.ConversionMode == ConvertToEntity.Mode.ConvertAndDestroy) {
    //             //     cc.ConversionMode = ConvertToEntity.Mode.ConvertAndInjectGameObject;
    //             // }
    //         });
    //
    //     return count;
    // }
}