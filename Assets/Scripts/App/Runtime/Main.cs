using Base.Runtime;
using Puerts;
using UnityEditor;
using UnityEngine;

namespace App.Runtime
{
    public class Main : Singleton<Main>
    {
        public static bool Inited;

        public static JsEnv JsEnv {
            get {
                if (!Inited) {
                    Inst.Awake();
                }

                return JsEnvRuntime.Inst.JsEnv;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            if (!Inited) {
                Inited = true;
                JsEnvRuntime.Inst.Init();
                JsEnvRuntime.Inst.JsEnv.Eval("var app = require('index').default || require('index');");
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