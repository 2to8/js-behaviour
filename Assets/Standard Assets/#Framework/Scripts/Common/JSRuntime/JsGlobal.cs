using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Callbacks;

#endif

namespace Common.JSRuntime {

[ExecuteAlways]
public class JsGlobal : MonoBehaviour {

    static JsGlobal m_Instance;

    //JsCore m_Core;

    // [ShowInInspector]
    public static Env Core => Env.instance;

    public static JsGlobal instance {
        get {
            if (m_Instance == null) {
                m_Instance = FindObjectOfType<JsGlobal>() ??
                    (GameObject.Find($"/{nameof(JsGlobal)}") ?? new GameObject(nameof(JsGlobal)))
                    .AddComponent<JsGlobal>();
            }

            return m_Instance;
        }
        protected set => m_Instance = value;
    }

    void OnEnable()
    {
        // hook up assembly reload handles so we can kill the dash network when unity triggeres a reload. Unity will hang if we dont do this cos it wont be able
        // to shut down the DashNetwork thread.
        //AssemblyReloadEvents.beforeAssemblyReload += OnBeforeAssemblyReload;
        if (m_Instance == null) {
            m_Instance = this;
        }
    }

    void OnDisable()
    {
        //AssemblyReloadEvents.beforeAssemblyReload -= OnBeforeAssemblyReload;
    }

    static bool reloading;

    public void OnBeforeAssemblyReload()
    {
        // before unity tries to reload all the .net code assemblies, kill off the dash network....
        Debug.Log("Before Assembly Reload");
        reloading = true;
    }

    float timer;

    async void Update()
    {
        //     if(Core.jsEnv.isolate == IntPtr.Zero) return;
        //
        // #if UNITY_EDITOR
        //     if (reloading || Application.isEditor && EditorApplication.isCompiling || !Core.jsEnv) return;
        //
        //     timer += Time.deltaTime;
        //     if (!Application.isPlaying && timer >= 2f || Application.isPlaying) {
        //         timer -= 2f;
        //     #endif
        //         if (Core.Running || !Core.jsEnv.isDebugReady) {
        //             await Core.OnUpdate();
        //         }
        //     #if UNITY_EDITOR
        //     }
        // #endif
    }

#if UNITY_EDITOR
    [DidReloadScripts]
    static void afterReloading()
    {
        reloading = false;

        //Puerts.PuertsDLL.CreateInspector(Core.jsEnv.isolate,Core.jsEnv.debugPort);
    }

    [UnityEditor.MenuItem("Puerts/JsGlobal", false, 302)]
    static void JsGlobalStatus()
    {
        var o = instance.gameObject;
        UnityEditor.Selection.activeGameObject = o;
        Debug.Log(instance.name, o);
    }
#endif

}

}