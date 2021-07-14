using System;
using Puerts;
using PuertsStaticWrap;
using UnityEngine;

namespace Base.Runtime
{
    public class JsEnvRuntime : Singleton<JsEnvRuntime>
    {
        public int debugPort = 9229;
        public bool useDebug = true;
        JsEnv m_jsEnv;
        public bool inited;

        public JsEnv env {
            get {
                if (m_jsEnv == null || m_jsEnv.isolate == IntPtr.Zero) {
                    Init();
                }

                return m_jsEnv;
            }
            private set => m_jsEnv = value;
        }

        public JsEnv Init(bool force = false)
        {
            inited = false;
            if (m_jsEnv != null && m_jsEnv.isolate != IntPtr.Zero && !force) return m_jsEnv;
            m_jsEnv = new JsEnv(new JsLoaderRuntime(), debugPort);
            m_jsEnv.AutoUsing();
            m_jsEnv.Eval(@"require('sourcemap')");
            if (useDebug && debugPort != -1 && Application.isEditor) {
                m_jsEnv.WaitDebuggerTimeout(5);
            }

            return m_jsEnv;
        }

        public void Shut()
        {
            env.Dispose();
            env = null;
        }

        bool m_Updated;

        void LateUpdate()
        {
            m_Updated = false;
        }

        public void Update()
        {
            if (!m_Updated) {
                m_Updated = true;
                if (env?.IsDisposed() == false) {
                    env?.Tick();
                }
            }
        }
    }
}