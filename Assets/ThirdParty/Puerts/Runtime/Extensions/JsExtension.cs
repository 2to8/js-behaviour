using Puerts.Attributes;
using System;
using System.Linq;
using System.Reflection;

namespace Puerts.Extensions
{
    public static class JsExtension
    {
        static int debugPort;

        public static int DebugPort(this JsEnv env, int port = -1)
        {
            if (port != -1) debugPort = port;
            return debugPort;
        }

        public static bool DebugConnected(this JsEnv env) => PuertsDLL.InspectorTick(env.isolate);
        public static void DisconnectDebug(this JsEnv env) => PuertsDLL.DestroyInspector(env.isolate);
        public static void ConnectDebug(this JsEnv env) => PuertsDLL.CreateInspector(env.isolate, env.DebugPort());


    }
}