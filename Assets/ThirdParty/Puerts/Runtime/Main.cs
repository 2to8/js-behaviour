using System;
using Base.Runtime;
using Puerts;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace App.Runtime
{
    [RequireComponent(typeof(JsEnvRuntime)), ExecuteAlways, DefaultExecutionOrder(-56000)]
    public class Main : Singleton<Main>
    {
        public static bool m_Inited;
        public static JsEnvRuntime runtime => JsEnvRuntime.instance;

        public static JsEnv js {
            get {
                if (!m_Inited || runtime.env.IsDisposed() != false || runtime.inited == false) {
                    instance.Init(true);
                }

                return runtime.env;
            }
        }

        [Button]
        public void ReloadEnv()
        {
            js.Dispose();
            m_Inited = false;
            //JsEnvRuntime.Inst.Init(true);
            Init();
        }

        protected override void Awake()
        {
            base.Awake();
            Init();
            if (Application.isPlaying) {
                DontDestroyOnLoad(gameObject);
            }
        }

        void Init(bool force = false)
        {
            if (!m_Inited || force) {
                Debug.Log("<color=white>[JsEnv] init</color>");
                m_Inited = true;
                runtime.Init(true);
                runtime.env.Eval("var app = require('index').default || require('index');");
                runtime.env.Eval<Action<JsEnv>>("$InitEnv").Invoke(runtime.env);
                runtime.inited = true;
            }
        }

        private void OnDestroy()
        {
            runtime.Shut();
        }

        private void Update()
        {
            runtime.Update();
        }
    }
}