using Puerts;
using UnityEditor;
using UnityEngine;

public static class PuertsExtension
{
    public const string kDebugMenuItem = "Puerts/显示测试窗口";
#if UNITY_EDITOR
    /// <summary>
    /// Toggles the menu.
    /// </summary>
    [MenuItem(kDebugMenuItem)]
    static void OnClickMenu()
    {
        // Check/Uncheck menu.
        var isChecked = !Menu.GetChecked(kDebugMenuItem);
        Menu.SetChecked(kDebugMenuItem, isChecked);

        // Save to EditorPrefs.
        EditorPrefs.SetBool(kDebugMenuItem, isChecked);
    }

    [MenuItem(kDebugMenuItem, true)]
    static bool Valid()
    {
        // Check/Uncheck menu from EditorPrefs.
        Menu.SetChecked(kDebugMenuItem, EditorPrefs.GetBool(kDebugMenuItem, false));
        return true;
    }
#endif
    public static void WaitDebuggerTimeout(this JsEnv jsEnv, int timeout = 0)
    {
#if THREAD_SAFE
            lock(jsEnv) {
#endif
        if (!Application.isEditor) return;
        var lastTime = Time.realtimeSinceStartup;
        while (!PuertsDLL.InspectorTick(jsEnv.isolate)) {
#if UNITY_EDITOR
            var useDebug = EditorPrefs.GetBool(kDebugMenuItem, false);
            if(!useDebug) break;
#endif
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