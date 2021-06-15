using Puerts;
using PuertsStaticWrap;

namespace Base.Runtime
{
    public class JsEnvRuntime : Singleton<JsEnvRuntime>
    {
        public int debugPort = 9229;
        public bool useDebug = true;
        public JsEnv JsEnv { get; private set; }

        public void Init()
        {
            JsEnv = new JsEnv(new JsLoaderRuntime(), debugPort);
            JsEnv.AutoUsing();
            JsEnv.Eval(@"require('sourcemap')");
            if (useDebug && debugPort != -1) {
                JsEnv.WaitDebugger(5);
            }
        }

        public void Shut()
        {
            JsEnv.Dispose();
            JsEnv = null;
        }

        public void Update()
        {
            JsEnv.Tick();
        }
    }
}