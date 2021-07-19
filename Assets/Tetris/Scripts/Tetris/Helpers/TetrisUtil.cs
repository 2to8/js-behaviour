using System;
using System.Collections.Generic;
using GameEngine.Extensions;
using System.Linq;
using GameEngine.Models.Contracts;
using ParadoxNotion.Design;
using Puerts;
using Puerts.Extensions;
using UnityEngine;

namespace Tetris
{
    public static class TetrisUtil
    {
        public static void ForEach(this Transform transform, Action<Transform> action)
        {
            foreach (Transform child in transform) action?.Invoke(child);
        }

        public static void ForEach(this GameObject gameObject, Action<Transform> action)
        {
            gameObject.transform.ForEach(action);
        }

        public static void WaitDebuggerTimeout(this JsEnv env, int timeout = 0)
        {
            var endTime = timeout + Time.realtimeSinceStartup;
            while (!env.DebugConnected())
                if (timeout > 0 && Time.realtimeSinceStartup >= endTime) {
#if UNITY_EDITOR
                    var isContinue = UnityEditor.EditorUtility.DisplayDialog("等待调试",
                        $"是否继续等待 {timeout}秒?", "Yes", "No");
                    if (isContinue) {
                        endTime = timeout + Time.realtimeSinceStartup;
                        continue;
                    }
#endif
                    break;
                }
        }

        static GameObject dynamicRoot;
        public static List<KeyCode> KeyUp = new List<KeyCode>();
        public static List<KeyCode> KeyDown = new List<KeyCode>();
        public static List<KeyCode> KeyHold = new List<KeyCode>();
        static GameObject _dynamicRoot;

        public static bool IsKeyDown(this KeyCode[] keyCodes)
        {
            return keyCodes.Any(k => KeyUp.Contains(k) || Input.GetKeyDown(k));
        }

        public static bool IsKeyUp(this KeyCode[] keyCodes)
        {
            return keyCodes.Any(k => KeyDown.Contains(k) || Input.GetKeyUp(k));
        }

        public static bool IsKey(this KeyCode[] keyCodes)
        {
            return keyCodes.Any(k => KeyHold.Contains(k) || Input.GetKey(k));
        }

        public static T setDynamicRoot<T>(this T component, string path = null) where T : Component
        {
            return component?.Of(t => t.gameObject.setDynamicRoot(path));
        }

        public static GameObject DynamicRoot {
            get => _dynamicRoot ??= GameObject.Find("/_Dynamic") ?? new GameObject("_Dynamic");
            set => _dynamicRoot = value;
        }

        public static Transform GetDynamicRoot(this GameObject go, string path = null)
        {
            return DynamicRoot.CreateGameObjectByPath(path).transform;
        }

        public static Transform GetDynamicRoot(this Component go, string path = null)
        {
            return DynamicRoot.CreateGameObjectByPath(path).transform;
        }

        public static Transform GetDynamicRoot(this TableBase go, string path = null)
        {
            return DynamicRoot.CreateGameObjectByPath(path).transform;
        }

        public static Transform GetDynamicRoot(string path = null)
        {
            return DynamicRoot.CreateGameObjectByPath(path).transform;
        }

        public static GameObject setDynamicRoot(this GameObject gameObject, string path = null)
        {
            return gameObject?.Of(go => go.transform.SetParent(DynamicRoot.CreateGameObjectByPath(path).transform));

            // var dest = root.transform;
            //
            // if (category.IsNotNullOrEmpty()) {
            //     Core.CreateGameObjectByPath(category);
            //     dest = GameObject.Find($"/_Dynamic/{category}")?.transform
            //         ?? new GameObject(category).Of(t => t.transform.SetParent(root.transform)).transform;
            // }
            // gameObject.transform.SetParent(dest);
            //
            // return gameObject;
        }

        public static int Float2Int(float val)
        {
            return Mathf.RoundToInt(val);
        }
    }
}