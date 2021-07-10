#if XLUA
using System;
using System.Threading.Tasks;
using GameKit.Demo;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using XLua;

namespace GameKit.CoreMVC.Kernel
{
    [ CreateAssetMenu(fileName = "LuaProxy", menuName = "[APP]/LuaProxy", order = 0) ]
    public class LuaProxy : SerializedScriptableObject
    {
        [ System.Serializable ]
        public class Injection
        {
            public string name;
            public GameObject value;
        }

        internal static float lastGCTime = 0;
        internal const float GCInterval = 1; //1 second
        public TextAsset luaScript;
        public AssetReference reference;
        public Injection[] injections;
        private Action luaStart;
        private Action luaUpdate;
        private Action luaOnDestroy;
        private LuaEnv luaEnv => GameApp.luaEnv;

        private LuaTable scriptEnv;

        public async void Run(AssetReference assetReference) {
            reference = assetReference;
            await Run();
        }

        public async void Run(string path) {
            luaScript = await Addressables.LoadAssetAsync<TextAsset>(path).Task;
            await Run();
        }

        public async Task<TextAsset> Run() {
            scriptEnv = luaEnv.NewTable();

            if(luaScript == null && reference != null) {
                luaScript = await reference.LoadAssetAsync<TextAsset>().Task;
            }

            // 为每个脚本设置一个独立的环境，可一定程度上防止脚本间全局变量、函数冲突
            LuaTable meta = luaEnv.NewTable();
            meta.Set("__index", luaEnv.Global);
            scriptEnv.SetMetaTable(meta);
            meta.Dispose();

            scriptEnv.Set("self", this);
            foreach(var injection in injections) {
                scriptEnv.Set(injection.name, injection.value);
            }

            if(luaScript != null) {
                luaEnv.DoString(luaScript.text, "LuaTestScript", scriptEnv);
            }

            Action luaAwake = scriptEnv.Get<Action>("awake");
            scriptEnv.Get("start", out luaStart);
            scriptEnv.Get("update", out luaUpdate);
            scriptEnv.Get("ondestroy", out luaOnDestroy);

            luaAwake?.Invoke();

            return null;
        }

        // Use this for initialization
        public void Start() {
            luaStart?.Invoke();
        }

        // Update is called once per frame
        public void Update() {
            luaUpdate?.Invoke();

            if(Time.time - lastGCTime > GCInterval) {
                luaEnv.Tick();
                lastGCTime = Time.time;
            }
        }

        void OnDestroy() {
            luaOnDestroy?.Invoke();

            luaOnDestroy = null;
            luaUpdate = null;
            luaStart = null;
            scriptEnv?.Dispose();
            injections = null;
        }
    }
}
#endif