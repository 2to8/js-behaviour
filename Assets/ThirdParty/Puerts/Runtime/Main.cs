using System;
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
        public static bool m_Inited;

        public static JsEnv js {
            get {
                if (!m_Inited) {
                    Inst.Init();
                }

                return JsEnvRuntime.Inst.env;
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
        }

        void Init()
        {
            if (!m_Inited) {
                m_Inited = true;
                JsEnvRuntime.Inst.Init(true);
                JsEnvRuntime.Inst.env.Eval("var app = require('index').default || require('index');");
                JsEnvRuntime.Inst.env.Eval<Action<JsEnv>>("$InitEnv").Invoke(JsEnvRuntime.Inst.env);
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