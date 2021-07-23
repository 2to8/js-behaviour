using GameEngine.Controllers.Commands;
using GameEngine.Kernel.Args;
using GameEngine.Views.Contracts;
using UniRx.Async;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
#endif

namespace GameEngine.Kernel._Appliation
{
    public class GameBootstrapV1 : TManager<GameBootstrapV1>
    {
        [SerializeField]
        Config m_Config;

        //启动入口
        protected override async UniTask Start()
        {
            await base.Start();

            //DontDestroyOnLoad(Root);
            RegisterController<StartUpCmd>(GameEngine.Kernel.Args.Defines.E_StartUp);
            SendEvent(GameEngine.Kernel.Args.Defines.E_StartUp);
        }

        void OnDestroy()
        {
            // Remove the delegate when the object is destroyed
            SceneManager.sceneLoaded -= SceneLoaded;
           // Core.;
        }

        public static void Bootstap()
        {
            Debug.Log("Bootstrap application");
        }

        public static void LoadScene(int level)
        {
            var e = new SceneArgs { sceneIndex = SceneManager.GetActiveScene().buildIndex };
            Instance.SendEvent(GameEngine.Kernel.Args.Defines.E_ExitScene, e);
            SceneManager.LoadScene(level, LoadSceneMode.Single);
        }

        void OnLevelWasLoaded/*_Removed*/(int level)
        {
            var e = new SceneArgs { sceneIndex = level };
            Debug.Log("On LevelWasLoaded");
            SendEvent(GameEngine.Kernel.Args.Defines.E_EnterScene, e);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnRuntimeMethodLoad()
        {
            // Add the delegate to be called when the scene is loaded, between Awake and Start.
            SceneManager.sceneLoaded += SceneLoaded;
        }

        static void SceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (m_Instance != null) {
                m_Instance.OnLevelWasLoaded/*_Removed*/(scene.buildIndex);
            }
            Debug.Log($"Scene{scene.name} has been loaded ({loadSceneMode.ToString()})");
        }
    }
}