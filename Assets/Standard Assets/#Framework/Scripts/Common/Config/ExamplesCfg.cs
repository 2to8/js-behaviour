/*
 * Tencent is pleased to support the open source community by making InjectFix available.
 * Copyright (C) 2019 THL A29 Limited, a Tencent company.  All rights reserved.
 * InjectFix is licensed under the MIT License, except for the third-party components listed in the file 'LICENSE' which may be subject to their corresponding license terms.
 * This file is subject to the terms and conditions defined in file 'LICENSE', which is part of this source code package.
 */

using Puerts;
using Puerts.Attributes;
using PuertsTest;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//1、配置类必须打[Configure]标签
//2、必须放Editor目录
namespace Common.Config {

[Configure, PuertsIgnore]
public class ExamplesCfg {

    // [CodeOutputDirectory]
    // static string GenDir => Application.dataPath + "/Gen/Scripts/";

    [Binding]
    static IEnumerable<Type> Bindings => new List<Type>() {
        typeof(Task),
        typeof(TaskAwaiter),
        typeof(Array),
        typeof(bool),
        typeof(MyCallback),
        typeof(UTF8Encoding),
        typeof(Encoding),
        typeof(Debug),
        typeof(TestClass),
        typeof(Vector3),
        typeof(List<int>),
        typeof(MyCallback),
        typeof(Dictionary<string, List<int>>),
        typeof(BaseClass),
        typeof(DerivedClass),
        typeof(BaseClassExtension),
        typeof(MyEnum),
        typeof(Time),
        typeof(Transform),
        typeof(Component),
        typeof(GameObject),
        typeof(UnityEngine.Object),
        typeof(Delegate),
        typeof(object),
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
    };

    [BlittableCopy]
    static IEnumerable<Type> Blittables => new List<Type>() {
        //打开这个可以优化Vector3的GC，但需要开启unsafe编译
        //typeof(Vector3),
    };

}

}