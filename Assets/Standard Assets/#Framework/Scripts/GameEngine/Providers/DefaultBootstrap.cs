using GameEngine.Extensions;
using GameEngine.Kernel;
using GameEngine.Utils;
using Sirenix.Utilities;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// using UnityEditor;
// using UnityEditor.SceneManagement;

#if UNITY_EDITOR
#endif

namespace GameEngine.Providers {

public interface IEvent { }

public class DefaultBootstrap : SingletonBaseView<DefaultBootstrap> {

    //static App m_instance;
    public static bool Booted;

    public new static DefaultBootstrap Instance {
        get {
            if (m_Instance == null) {
                LoadMain();

                // m_instance = Core.FindInRoot<App>() ?? Instantiate(Res.Load<App>()) ;
                //
                // if( m_instance == null ) {
                //     Debug.LogError("App Component not found");
                // }
            }

            return m_Instance;
        }
    }

    public static void SendEvent<T1>() where T1 : struct, IEvent
    {
        //Instance.system?.SendEvent<T1>();
    }

    public static void SendEvent(Type type)
    {
        if (typeof(IEvent).IsAssignableFrom(type)) {
            //Instance.system?.SendEvent(type);
        }
    }

    // Runs before a scene gets loaded
    // [ RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad) ]
    public static void LoadMain()
    {
        var app = Resources.Load<DefaultBootstrap>("Prefabs/App");

        var main = app
            ? Instantiate(app)
            : new GameObject("App", typeof(DefaultBootstrap)).GetComponent<DefaultBootstrap>(); //Res.Load<App>()

        m_Instance = main;
        main.gameObject.name = nameof(DefaultBootstrap);

        // DontDestroyOnLoad(main.gameObject);
    }

    protected override void Start()
    {
        if (!Application.isPlaying) {
            return;
        }

        Booted = true;
        base.Start();
    }

    protected override void Awake()
    {
        if (m_Instance == null) {
            m_Instance = this;
        }

        //base.Awake();
        Debug.Log($"App Init: {Core.Version}", gameObject);

        // _gameManager = GetComponentInChildren<GameManager>(true);
        // _uiManager = GetComponentInChildren<UIManager>(true);
        // _audioManager = GetComponentInChildren<AudioManager>(true);
        // _scoreManager = GetComponentInChildren<ScoreManager>(true);
        // _inputManager = GetComponentInChildren<PlayerInputManager>(true);
        // _advertisementManager = GetComponentInChildren<AdvertisementManager>(true);
        // _analyticManager = GetComponentInChildren<AnalyticsManager>(true);
        // _cameraManager = GetComponentInChildren<CameraManager>(true);
        // _gridManager = GetComponentInChildren<GridManager>(true);
        // _spawnManager = GetComponentInChildren<SpawnManager>(true);
        // _colorManager = GetComponentInChildren<ColorManager>(true);

        // if( Application.isPlaying ) {
        //     DontDestroyOnLoad(gameObject);
        // }
    }

    void OnDestroy()
    {
        Core.OnDestroy();
    }

#if UNITY_EDITOR

#if ECS
    [ UnityEditor.MenuItem("Game/Add TBtn to Button") ]
    static void AddTBtnToButton()
    {
        Debug.Log($"Add Count: {Core.AddAuthoring<Button, TBtn>()}");
    }
#endif

    protected override void OnEnable()
    {
        if (Application.isPlaying) {
            base.OnEnable();

            return;
        }

        if (!SceneManager.GetActiveScene().isLoaded) {
            return;
        }

    #if ECS
        CheckRequiredComponentTool.CheckRequired();
    #endif

        Core.GetTypes<ProviderManager>()
            .Where(t => !t.IsAbstract &&
                !t.IsGenericType &&
                !Instance.GetComponentInChildren(t, true) &&
                t.IsPublic &&
                !t.IsStatic())
            .ForEach(t => {
                if (Core.FindObjectOfTypeAll(t) != null) {
                    return;
                }

                var go = GameObject.Find("/" + Instance.gameObject.name + "/" + t.Name) ?? new GameObject(t.Name);

                if (go.RequireComponent(t) != null) {
                    if (go.transform.parent == null) {
                        go.transform.SetParent(Instance.transform);
                    }

                    Debug.Log($"Add Manager: {t.Name}", go);
                    UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
                    Debug.Log("set dirty");
                }
            });
    }
#endif

}

}