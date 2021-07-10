using JetBrains.Annotations;
using Puerts;
using Puerts.Attributes;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI; // using NodeCanvas.Framework;

namespace Common.JSRuntime {

[PuertsIgnore]
public class JsMain : SerializedMonoBehaviour {

    public static JsEnv env => instance.jsEnv;
    Env m_EnvCore;
    JsEnv jsEnv => (m_EnvCore ?? (m_EnvCore = ScriptableObject.CreateInstance<Env>())).env;

    static JsMain m_Instace;

    //Dictionary<GraphOwner, bool> graphs = new Dictionary<GraphOwner, bool>();

    [SerializeField, CanBeNull]
    Toggle DebuggerLoaded;

    public static JsMain instance {
        get {
            if (m_Instace != null) {
                return m_Instace;
            }

            if ((m_Instace = FindObjectOfType<JsMain>()) == null) {
                var prefab = Resources.LoadAll<JsMain>("").First();
            #if UNITY_EDITOR
                if (!Application.isPlaying) {
                    m_Instace = UnityEditor.PrefabUtility.InstantiatePrefab(prefab) as JsMain;
                }
            #endif
                if (m_Instace == null) {
                    m_Instace = Instantiate(prefab);
                }
            }

            return m_Instace;
        }
        private set => m_Instace = value;
    }

    [SerializeField]
    bool AutoStart;

    [SerializeField, ValueDropdown(nameof(m_ListModules))]
    string module = "UIEvent";

    IEnumerable<string> m_ListModules
#if UNITY_EDITOR
        => Resources.LoadAll<TextAsset>("")
            .Select(UnityEditor.AssetDatabase.GetAssetPath)
            .Where(t => new[] { ".js", ".ts" }.Contains(Path.GetExtension(t)))
            .Select(s => s.Split(new[] { "Resources/" }, StringSplitOptions.None)[1])
#endif
    ;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void RuntimeReload()
    {
        // Debug.Log("Reset JsEnv");
        m_Instace = null;
    }

    public Env Create() => ScriptableObject.CreateInstance<Env>();

    // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    // static void RuntimeReload_1()
    // {
    //     Debug.Log("Reset JsEnv");
    //     jsEnv = null;
    // }
    async void Awake()
    {
        if (m_Instace == null) {
            //
            // if (jsEnv == null) {
            //     Debug.Log("JsEnv Reload");
            //     jsEnv = new JsEnv(this, m_WaitDebug && (Application.isEditor || Debug.isDebugBuild) ? m_DebugPort : -1);
            // }

            m_Instace = this;

            if (Application.isPlaying) {
                DontDestroyOnLoad(this);

                // 卡住所有的图, 以便等待jsenv的debugger连接
                // FindObjectsOfType<GraphOwner>()
                //     .ForEach(t => {
                //         graphs[t] = t.enabled;
                //         t.enabled = false;
                //         Debug.Log($"{t.gameObject.name}", t.gameObject);
                //     });
            }

            await jsEnv.WaitDebuggerAsync(e => {
                // debugger连上后再打开图
                // graphs.ForEach(t => t.Key.enabled = t.Value);
                // graphs.Clear();
                e.UsingAction<bool>(); //toggle.onValueChanged用到
                e.UsingAction<Action<JsMain>>();
            });

            //jsEnv.WaitDebugger();
        }
    }

    [Button]
    async void Start()
    {
        if (!AutoStart && Application.isPlaying) {
            return;
        }

        await jsEnv.WaitDebuggerAsync(js => {
            Debug.Log("starting");

            var init = js.Eval<Action<JsMain>>($"require('{module}').init");

            init?.Invoke(this);
        });
    }

    async void Update()
    {
        await jsEnv.WaitDebuggerAsync(js => {
            if (!(DebuggerLoaded is null)) {
                DebuggerLoaded.isOn = js.isDebugReady();
            }

            js.Tick();
        }, true);
    }

}

}