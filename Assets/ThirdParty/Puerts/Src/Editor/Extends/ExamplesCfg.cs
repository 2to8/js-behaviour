/*
 * Tencent is pleased to support the open source community by making InjectFix available.
 * Copyright (C) 2019 THL A29 Limited, a Tencent company.  All rights reserved.
 * InjectFix is licensed under the MIT License, except for the third-party components listed in the file 'LICENSE' which may be subject to their corresponding license terms.
 * This file is subject to the terms and conditions defined in file 'LICENSE', which is part of this source code package.
 */

#if UNITY_EDITOR
using System.Collections.Generic;
using Puerts;
using Puerts.Attributes;
using Puerts.Editor;
using Puerts.Extensions;
using Sirenix.Utilities;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

//1、配置类必须打[Configure]标签
//2、必须放Editor目录
[Configure, PuertsIgnore]
public class ExamplesCfg
{
    [Binding]
    static IEnumerable<Type> Bindings =>
        new List<Type>() {
            typeof(Array),
            typeof(Debug),
            typeof(PuertsTest.TestClass),
            typeof(Vector3),
            typeof(List<int>),
            typeof(Dictionary<string, List<int>>),
            typeof(PuertsTest.BaseClass),
            typeof(PuertsTest.BaseClass1),
            typeof(PuertsTest.DerivedClass),
            typeof(PuertsTest.BaseClassExtension),
            typeof(PuertsTest.MyEnum),
            typeof(Time),
            typeof(Transform),
            typeof(Component),
            typeof(GameObject),
            typeof(UnityEngine.Object),
            typeof(Delegate),
            typeof(System.Object),
            typeof(Type),
            typeof(ParticleSystem),
            typeof(Canvas),
            typeof(RenderMode),
            typeof(Behaviour),
            typeof(MonoBehaviour),
            typeof(UnityEngine.EventSystems.UIBehaviour),
            typeof(UnityEngine.UI.Selectable),
            typeof(UnityEngine.UI.Button),
            typeof(UnityEngine.UI.Button.ButtonClickedEvent),
            typeof(UnityEngine.Events.UnityEvent),
            typeof(UnityEngine.UI.InputField),
            typeof(UnityEngine.UI.Toggle),
            typeof(UnityEngine.UI.Toggle.ToggleEvent),
            typeof(UnityEngine.Events.UnityEvent<bool>),
        }.Distinct();

    [BlittableCopy]
    static IEnumerable<Type> Blittables {
        get {
            return new List<Type>() {
                //打开这个可以优化Vector3的GC，但需要开启unsafe编译
                //typeof(Vector3),
            };
        }
    }

    [Filter]
    static bool FilterMethods(System.Reflection.MemberInfo mb)
    {
        // 排除 MonoBehaviour.runInEditMode, 在 Editor 环境下可用发布后不存在
        if (mb.DeclaringType == typeof(MonoBehaviour) && mb.Name == "runInEditMode") {
            return true;
        }

        return false;
    }

    static List<Type> DelegateTypes = new List<Type>();

    [DidReloadScripts]
    static void clearDelegateTypes() => DelegateTypes.Clear();

    [Filter]
    static bool FilterDelegates(MemberInfo memberInfo)
    {
        if (memberInfo is MethodInfo mi) {
            mi.GetParameters().Where(t => Generator.IsDelegate(t.ParameterType)).Select(pi => pi.ParameterType).ForEach(
                t => {
                    if (!DelegateTypes.Contains(t)) {
                        DelegateTypes.Add(t);
                    }
                });
        }

        var type = memberInfo is FieldInfo fieldInfo ? fieldInfo.FieldType :
            memberInfo is PropertyInfo propertyInfo ? propertyInfo.PropertyType : null;
        if (type != null && Generator.IsDelegate(type) && !DelegateTypes.Contains(type)) {
            DelegateTypes.Add(type);
        }

        return false;
    }

    [MenuItem("Puerts/Test UsingAction", false, 301)]
    public static void TestUsingAction()
    {
        int total = 0;
        List<List<Type>> types = new List<List<Type>>();
        List<List<Type>> editor_types = new List<List<Type>>();
        DelegateTypes.ForEach(t => {
            MethodInfo[] mInfos = t.GetMethods(BindingFlags.Public);
            MemberInfo[] mi = t.GetMember("Invoke");
            if (mi.Length > 0) {
                int token = mi[0].MetadataToken;
                MethodBase methodBase = ((MethodInfo) mi[0]);
                ParameterInfo[] parameterInfos = methodBase.GetParameters();
                var t2 = parameterInfos.Select(pi => pi.ParameterType).ToList();
                if (parameterInfos.Any(pp => typeof(ValueType).IsAssignableFrom(pp.ParameterType))

                    // !types.Any(tl => {
                    //     var ret = tl.Count == t2.Count;
                    //     if (ret) {
                    //         tl.ForEach((tp1, ii) => {
                    //             if (tp1 != t2[ii]) {
                    //                 ret = false;
                    //             }
                    //         });
                    //     }
                    //     return ret;
                    // })
                ) {
                    var has = false;
                    if (t2.Any(Filter.TypeHasEditorRef)) {
                        if (has = !editor_types.ContainsList(t2)) {
                            editor_types.Add(t2);
                        }
                    }
                    else if (has = !types.ContainsList(t2)) {
                        types.Add(t2);
                    }

                    if (has) {
                        total += 1;
                        //var pstr = string.Join(", ", parameterInfos.Select(pi => pi.ParameterType.GetFriendlyName()));
                        //Debug.Log($"{t.GetFriendlyName()} => {pstr}");
                    }

                    //MethodBody methodBody = methodBase.GetMethodBody();
                }
            }

            // var returnType = methodInfo.ReturnParameter.ParameterType;
            //
            // var parameterTypes = methodInfo.GetParameters().Select(pi => pi.ParameterType);
        });
        var genStr = $@"
namespace PuertsStaticWrap {{

    public static partial class PuertsHelper {{

        public static (int runtime, int editor) UsingActions(this Puerts.JsEnv jsEnv)
        {{
            /*[BODY]*/
#if UNITY_EDITOR
            /*[EDITOR_BODY]*/
#endif
            return /*[RETURN]*/;
        }}

    }}

    }}
";
        var lines = string.Empty;
        var editorLines = string.Empty;
        types.ForEach(list => {
            if (list.All(tn =>
                !string.IsNullOrEmpty(tn.GetFriendlyName()) && !tn.GetFriendlyName().Contains("<T>") &&
                !tn.GetFriendlyName().Contains("Unity.Entities")) && list.Count <= 4) {
                lines += (string.IsNullOrWhiteSpace(lines) ? "" : "\n" + new String(' ', 12)) +
                    $"jsEnv.UsingAction<{string.Join(", ", list.Select(tn => tn.GetFriendlyName()?.Replace("&", "")))}>();";
            }
        });
        editor_types.ForEach(list => {
            editorLines += (string.IsNullOrWhiteSpace(editorLines) ? "" : "\n" + new String(' ', 12)) +
                $"jsEnv.UsingAction<{string.Join(", ", list.Select(tn => tn.GetFriendlyName()?.Replace("&", "")))}>();";
        });
        File.WriteAllText(Configure.GetCodeOutputDirectory() + "/UsingActions.cs",
            genStr.Replace("/*[BODY]*/", lines.Trim('\n')).Replace("/*[EDITOR_BODY]*/", editorLines.Trim('\n'))
                .Replace("/*[RETURN]*/", $"({types.Count()}, {editor_types.Count()})"));
        Debug.Log($"total: {types.Count()} + {editor_types.Count()} = {total} all: {DelegateTypes.Count()}" +
            $"\n{string.Join("\n", DelegateTypes.Select(t => t.GetFriendlyName()))}");
    }
}

#endif