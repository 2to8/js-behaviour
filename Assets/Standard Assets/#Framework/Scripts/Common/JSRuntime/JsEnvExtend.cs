// using Puerts;
//
// namespace Env {
//
// public class JsEnvExtend : JsEnv {
//
//     public int debugPort = 9808;
//     public bool DebugConnected => PuertsDLL.InspectorTick(isolate);
//     public void DisconnectDebug() => PuertsDLL.DestroyInspector(isolate);
//     public void ConnectDebug() => PuertsDLL.CreateInspector(isolate, debugPort);
//     public JsEnvExtend() : this(new DefaultLoader()) { }
//
//     // public static void DisposeAll()
//     // {
//     //     var i = 0;
//     //     foreach(var env in jsEnvs) {
//     //         if(env != null && env.isolate != IntPtr.Zero && PuertsDLL.InspectorTick(env.isolate)) {
//     //             Debug.Log($"dispose: {i += 1}");
//     //             PuertsDLL.DestroyInspector(env.isolate);
//     //         }
//     //     }
//     // }
//
//     // public static JsEngine GegLastConnected()
//     // {
//     //     return jsEnvs.FirstOrDefault(t =>
//     //         t != null && t.isolate != IntPtr.Zero && PuertsDLL.InspectorTick(t.isolate)) as JsEngine;
//     // }
//
//     public JsEnvExtend(ILoader loader, int debugPort = -1) : base(loader, debugPort)
//     {
//         this.debugPort = debugPort;
//     }
//
//     public static JsEnvExtend Create(ILoader loader, int debugPort = -1)
//     {
//         return new JsEnvExtend(loader, debugPort);
//     }
//
// }
//
// }

