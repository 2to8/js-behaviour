using Base.Runtime;
using Puerts;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace App.Runtime
{
    [RequireComponent(typeof(JsEnvRuntime))]
    public class Main : Singleton<Main>
    {
        public static bool Inited;

        public static JsEnv js {
            get {
                if (!Inited) {
                    Inst.Awake();
                }

                return JsEnvRuntime.Inst.env;
            }
        }

        [Button]
        public void ReloadEnv()
        {
             js.Dispose();
             Inited = false;
             JsEnvRuntime.Inst.Init(true);
        }

        protected override void Awake()
        {
            base.Awake();
            if (!Inited) {
                Inited = true;
                JsEnvRuntime.Inst.Init();
                JsEnvRuntime.Inst.env.Eval("var app = require('index').default || require('index');");
            }
        }

        private void OnDestroy()
        {
            JsEnvRuntime.Inst.Shut();
        }

        private void Update()
        {
            JsEnvRuntime.Inst.Update();
        }
    }
}