using Extensions;
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

        public JsEnv JsEnv {
            get {
                if (m_jsEnv == null) {
                    Init();
                }

                return m_jsEnv;
            }
            private set => m_jsEnv = value;
        }

        public JsEnv Init()
        {
            if (m_jsEnv != null) return m_jsEnv;
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
            JsEnv.Dispose();
            JsEnv = null;
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
                JsEnv?.Tick();
            }
        }
    }
}