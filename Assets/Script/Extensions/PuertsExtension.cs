using Puerts;
using UnityEditor;
using UnityEngine;

namespace Extensions
{
    public static class PuertsExtension
    {
        public static void WaitDebuggerTimeout(this JsEnv jsEnv, int timeout = 0)
        {
            
#if THREAD_SAFE
            lock(jsEnv) {
#endif
            if (!Application.isEditor) return; 
            
            var lastTime = Time.realtimeSinceStartup;
            while (!PuertsDLL.InspectorTick(jsEnv.isolate)) {
                if (timeout > 0 && Time.realtimeSinceStartup >= lastTime) {
#if UNITY_EDITOR
                    var check = EditorUtility.DisplayDialog("Debug", $"等待连接调试器: {timeout} 秒", $"等待{timeout}秒", "取消调试");
                    if (check) {
                        lastTime = timeout + Time.realtimeSinceStartup;
                        continue;
                    }
#endif
                    break;
                }
            }
#if THREAD_SAFE
            }
#endif
        }
    }
}