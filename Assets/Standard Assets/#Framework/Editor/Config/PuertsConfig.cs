using Puerts;
using Puerts.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine; //using UnityEditor;

namespace Common.Config {

[Configure, PuertsIgnore] 
public partial class PuertsConfig {

#if UNITY_EDITOR

    [UnityEditor.MenuItem("Puerts/Web/Starter Kit")]
    static void OpenStarterKitUrl() => Application.OpenURL("https://github.com/Geequlim/puerts-starter-kit");

    [UnityEditor.MenuItem("Puerts/Web/Puerts")]
    static void OpenPuertsUrl() => Application.OpenURL("https://github.com/Geequlim/puerts-starter-kit");
#endif

    [Typing]
    static IEnumerable<Type> Typings {
        get {
            var types = new List<Type>();

            var namespaces = new HashSet<string>();
            namespaces.Add("tiny");
            namespaces.Add("tiny.utils");
            namespaces.Add("System");
            namespaces.Add("UnityEngine");
            namespaces.Add("UnityEngine.Networking");
            namespaces.Add("UnityEngine.ParticleSystem");
            namespaces.Add("UnityEngine.SceneManagement");
            namespaces.Add("UnityEngine.AI");
            namespaces.Add("UnityEditor");
            namespaces.Add("FairyGUI");
            namespaces.Add("FairyGUI.Utils");
            namespaces.Add("System.IO");
            namespaces.Add("System.Net");
            namespaces.Add("System.Reflection");
            namespaces.Add("FSG.MeshAnimator.ShaderAnimated");

            // TODO: 添加需要导出声明的命名空间

            var ignored = new Dictionary<string, HashSet<string>>();
            var ignored_classes = new HashSet<string>();
            ignored_classes.Add("ContextMenuItemAttribute");
            ignored_classes.Add("HashUnsafeUtilities");
            ignored_classes.Add("SpookyHash");
            ignored_classes.Add("ContextMenuItemAttribute");
            ignored_classes.Add("U");
            ignored.Add("UnityEngine", ignored_classes);
            ignored_classes = new HashSet<string>();
            ignored_classes.Add("ContextMenuItemAttribute");
            ignored.Add("UnityEditor", ignored_classes);

            // TODO: 添加需要忽略导出声明的类型

            var registered = new Dictionary<string, HashSet<string>>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                var name = assembly.GetName().Name;

                foreach (var type in assembly.GetTypes()) {
                    if (!type.IsPublic) {
                        continue;
                    }

                    if (type.Name.Contains("<") || type.Name.Contains("*")) {
                        continue; // 忽略泛型，指针类型
                    }

                    if (type.Namespace == null || type.Name == null) {
                        continue; // 这是啥玩意？
                    }

                    if (registered.ContainsKey(type.Namespace) && registered[type.Namespace].Contains(type.Name)) {
                        continue; // 忽略重复的类
                    }

                    var accept = namespaces.Contains(type.Namespace);

                    if (accept && ignored.ContainsKey(type.Namespace) && ignored[type.Namespace].Contains(type.Name)) {
                        continue;
                    }

                    if (accept) {
                        types.Add(type);

                        if (!registered.ContainsKey(type.Namespace)) {
                            var classes = new HashSet<string>();
                            classes.Add(type.Name);
                            registered.Add(type.Namespace, classes);
                        } else {
                            registered[type.Namespace].Add(type.Name);
                        }
                    }
                }
            }
            types.Add(typeof(Convert));
            types.Add(typeof(System.Text.Encoding));
            types.Add(typeof(Dictionary<string, string>));
            types.Add(typeof(KeyValuePair<string, string>));
            types.Add(typeof(Dictionary<string, string>.Enumerator));

            // TODO: 添加需要导出声明的类型

            return types.Where(t => !Puerts.Filter.IsExcluded(t)).Distinct();

            // Debug.Log($"{ret.Count()}");
            // return types;
        }
    }

    [Binding]
    static IEnumerable<Type> Bindings {
        get {
            var types = new List<Type>();
            var namespaces = new HashSet<string>();
            namespaces.Add("tiny");
            namespaces.Add("tiny.utils");
            var ignored = new Dictionary<string, HashSet<string>>();
            var ignored_classes = new HashSet<string>();

            // 忽略 tiny.EditorUtils 类
            ignored_classes = new HashSet<string>();
            ignored_classes.Add("EditorUtils");
            ignored.Add("tiny", ignored_classes);

            // TODO：在此处添加要忽略绑定的类型

            var registered = new Dictionary<string, HashSet<string>>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                var name = assembly.GetName().Name;

                foreach (var type in assembly.GetTypes()) {
                    if (!type.IsPublic) {
                        continue;
                    }

                    if (type.Name.Contains("<") || type.Name.Contains("*")) {
                        continue; // 忽略泛型，指针类型
                    }

                    if (type.Namespace == null || type.Name == null) {
                        continue; // 这是啥玩意？
                    }

                    var accept = namespaces.Contains(type.Namespace);

                    if (accept && ignored.ContainsKey(type.Namespace) && ignored[type.Namespace].Contains(type.Name)) {
                        continue;
                    }

                    if (accept) {
                        types.Add(type);

                        if (!registered.ContainsKey(type.Namespace)) {
                            var classes = new HashSet<string>();
                            classes.Add(type.Name);
                            registered.Add(type.Namespace, classes);
                        } else {
                            registered[type.Namespace].Add(type.Name);
                        }
                    }
                }
            }

            // 绑定 Unity常用类型
            types.Add(typeof(Vector2));
            types.Add(typeof(Vector3));
            types.Add(typeof(Vector4));
            types.Add(typeof(Quaternion));
            types.Add(typeof(Color));
            types.Add(typeof(Rect));
            types.Add(typeof(Bounds));
            types.Add(typeof(Ray));
            types.Add(typeof(RaycastHit));
            types.Add(typeof(Matrix4x4));

            types.Add(typeof(Time));
            types.Add(typeof(Transform));
            types.Add(typeof(UnityEngine.Object));
            types.Add(typeof(GameObject));
            types.Add(typeof(Component));
            types.Add(typeof(Behaviour));
            types.Add(typeof(MonoBehaviour));
            types.Add(typeof(AudioClip));
            types.Add(typeof(ParticleSystem.MainModule));
            types.Add(typeof(AnimationClip));
            types.Add(typeof(Animator));
            types.Add(typeof(AnimationCurve));
            types.Add(typeof(AndroidJNI));
            types.Add(typeof(AndroidJNIHelper));
            types.Add(typeof(Collider));
            types.Add(typeof(Collision));
            types.Add(typeof(Rigidbody));
            types.Add(typeof(Screen));
            types.Add(typeof(Texture));
            types.Add(typeof(TextAsset));
            types.Add(typeof(SystemInfo));
            types.Add(typeof(Input));
            types.Add(typeof(Mathf));

            types.Add(typeof(Camera));
            types.Add(typeof(ParticleSystem));
            types.Add(typeof(AudioSource));
            types.Add(typeof(AudioListener));
            types.Add(typeof(Physics));
            types.Add(typeof(UnityEngine.SceneManagement.Scene));
            types.Add(typeof(UnityEngine.Networking.UnityWebRequest));

            return types.Where(t => !Puerts.Filter.IsExcluded(t)).Distinct();
        }
    }

    [Filter]
    static bool Filter(MemberInfo memberInfo)
    {
        var sig = memberInfo.ToString();

        if (memberInfo.ReflectedType.FullName == "UnityEngine.MonoBehaviour" && memberInfo.Name == "runInEditMode") {
            return true;
        }

        if (memberInfo.ReflectedType.FullName == "UnityEngine.Input" && memberInfo.Name == "IsJoystickPreconfigured") {
            return true;
        }

        if (memberInfo.ReflectedType.FullName == "UnityEngine.Texture" && memberInfo.Name == "imageContentsHash") {
            return true;
        }

        // TODO: 添加要忽略导出的类成员

        if (sig.Contains("*")) {
            return true;
        }

        return false;
    }

    [BlittableCopy]
    static IEnumerable<Type> Blittables => new List<Type>() {
        // 使用 Blittable 优化 struct 的 GC，需要开启unsafe编译生效
        typeof(Vector2),
        typeof(Vector3),
        typeof(Vector4),
        typeof(Quaternion),
        typeof(Color),
        typeof(Rect),
        typeof(Bounds),
        typeof(Ray),
        typeof(RaycastHit),
        typeof(Matrix4x4),
    };

    // [CodeOutputDirectory]
    // static string GenerateDirectory => Path.Combine(Application.dataPath, "Scripts", "Generated") + "/";

}

}