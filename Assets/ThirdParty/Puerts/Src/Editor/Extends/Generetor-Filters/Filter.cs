/*
 * Tencent is pleased to support the open source community by making InjectFix available.
 * Copyright (C) 2019 THL A29 Limited, a Tencent company.  All rights reserved.
 * InjectFix is licensed under the MIT License, except for the third-party components listed in the file 'LICENSE' which may be subject to their corresponding license terms.
 * This file is subject to the terms and conditions defined in file 'LICENSE', which is part of this source code package.
 */

using Puerts.Attributes;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using Attribute = System.Attribute;

//using Engine.Tests;

//using UnityEditor;

//1、配置类必须打[Configure]标签
//2、必须放Editor目录
namespace Puerts {

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
public class PuertsExcludeListAttribute : Attribute { }

[PuertsIgnore]
public static partial class Filter {

    public static void Register(this JsEnv env)
    {
        Filter.GetFilters<RegisterEnvAttribute, Action<JsEnv>>(li => {
            return li.OrderBy(mb => mb.GetCustomAttribute<RegisterEnvAttribute>().Order).ToList();
        }).ForEach(action => action?.Invoke(env));
    }

    [Binding]
    static IEnumerable<Type> Bindings {
        get {
            return new List<Type>() {
                typeof(Array),
                typeof(Debug),
                typeof(UnityAction),
                typeof(UnityAction<GameObject>),
                typeof(UnityAction<string>),
                typeof(UnityAction<int>),
                typeof(UnityAction<Component>),
                typeof(List<List<string>>),

                // typeof(TestClass),
                typeof(Vector3),
                typeof(List<int>),
                typeof(Dictionary<string, List<int>>),

                // typeof(BaseClass),
                // typeof(DerivedClass),
                // typeof(BaseClassExtension),
                // typeof(MyEnum),
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
            };
        }
    }

    [Filter]
    static bool FilterMethod(MemberInfo memberInfo)
    {
        if (memberInfo.DeclaringType.Namespace.IsNullOrWhitespace()) {
            return true;
        }
        if (memberInfo.IsDefined(typeof(ObsoleteAttribute))) {
            return true;
        }
        if (memberInfo.MemberType == MemberTypes.Field && !((FieldInfo)memberInfo).IsPublic) {
            return true;
        }
        if (memberInfo.MemberType == MemberTypes.Property &&
            !((PropertyInfo)memberInfo).GetAccessors().Any(MethodInfo => MethodInfo.IsPublic)) {
            return true;
        }
        if (memberInfo is MethodBase methodBase) {
            if (!methodBase.IsPublic) {
                return true;
            }
            if (methodBase.IsGenericMethod && methodBase.GetGenericArguments().Any(t => !t.IsPublic)) {
                return true;
            }
            if (methodBase.GetParameters().Any(t => !t.ParameterType.IsPublic)) {
                return true;
            }
        }

        // if(delegateHasEditorRef(memberInfo.ReflectedType)) {
        //     return true;
        // }
        return false;
    }

    [TypeFilter]
    static bool TypeFilter(Type type)
    {
        // if (type.IsPublic &&
        //     !type.IsDefined(typeof(ObsoleteAttribute)) &&
        //     !type.Namespace.IsNullOrWhitespace() &&
        //     GetFilters<AssemblyFilterAttribute, string>().Any(s => type.Assembly.GetName().Name == s) ||
        //     GetFilters<IncludeFilterAttribute, string>().Any(s => $"{type.FullName}".Contains(s))) {
        //     if (GetFilters<ExcludeFilterAttribute, string>().All(s => !$"{type.FullName}".Contains(s))
        //
        //         //&& GetFilters<SubTypeFilterAttribute, Type>().All(t => !t.IsAssignableFrom(type))
        //     ) {
        //         return false; // 不排除
        //     }
        // }
        if (type.FullName?.Contains("AnonymousType") == true || type.FullName == null) {
            return true;
        }
        return false; //排除
    }

    [Typing]
    static IEnumerable<Type> CustomBindings {
        get {
            var types = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                where !(assembly.ManifestModule is System.Reflection.Emit.ModuleBuilder) &&
                    GetFilters<AssemblyFilterAttribute, string>().Any(mb => assembly.GetName().Name == mb)
                from type in assembly.GetExportedTypes()
                where type.IsPublic &&
                    type.BaseType != typeof(MulticastDelegate) &&
                    !type.IsInterface &&
                    !type.IsEnum &&
                    !IsExcluded(type)
                select type;
            return types;
        }
    }

    static readonly Dictionary<Type, object> Filters_Cache = new Dictionary<Type, object>();
    static bool Debug_DisplayFilterInfo = false;

    public static List<TResult> GetFilters<T, TResult>(Func<List<MemberInfo>, List<MemberInfo>> action = null)
        where T : Attribute
    {
        //var filters = new List<MethodInfo>();
        if (!Filters_Cache.TryGetValue(typeof(T), out var list)) {
            var filters = new List<MemberInfo>();
            IEnumerable<Type> types = new List<Type>();
            try {
                // var abs = AppDomain.CurrentDomain.GetAssemblies();
                // foreach (var assembly in abs) {
                //     //Debug.Log(assembly.GetName().Name);
                //     if (!(assembly.ManifestModule is System.Reflection.Emit.ModuleBuilder)) {
                //         types = assembly.GetTypes().Where(type => type.IsDefined(typeof(ConfigureAttribute), false));
                //     }
                // }
                 types = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                    where !(assembly.ManifestModule is System.Reflection.Emit.ModuleBuilder)
                    from type in assembly.GetTypes()
                    where type.IsDefined(typeof(ConfigureAttribute), false)
                    select type;
                Debug.Log($"assembly types: {types.Count()}");
            } catch (Exception e) {
                Debug.LogException(e);

                //var types = new List<Type>();
            }
            foreach (var type in types) {
                foreach (var it in type.GetMembers(BindingFlags.Static |
                    BindingFlags.Public |
                    BindingFlags.NonPublic |
                    BindingFlags.DeclaredOnly)) {
                    if (it.IsDefined(typeof(T), false)) {
                        filters.Add(it);
                    }
                }
            }
            filters = action?.Invoke(filters) ?? filters;
            if (typeof(MethodBase).IsAssignableFrom(typeof(TResult))) {
                var mbResult = filters.Cast<TResult>().ToList();
                Filters_Cache[typeof(T)] = mbResult;
                return mbResult;
            }
            if (Debug_DisplayFilterInfo) {
                Debug.Log($"{typeof(TResult)} mb count: {filters.Count}");
            }
            var result = filters.SelectMany(mb => {
                    //if (mb is PropertyInfo propertyInfo) {
                    var ret = mb is PropertyInfo propertyInfo ? propertyInfo.GetValue(null, null) :
                        mb is FieldInfo fieldInfo ? fieldInfo.GetValue(null) : null;
                    if (mb is MethodInfo method) {
                        var parameters = method.GetParameters();
                        object[] args = new object[parameters.Length];
                        for (int i = 0; i < args.Length; i++) {
                            args[i] = parameters[i].HasDefaultValue ? parameters[i].DefaultValue : default;
                        }
                        ret = method.Invoke(null, args);
                    }
                    if (ret != null) {
                        if (Debug_DisplayFilterInfo)
                            Debug.Log(
                                $"[{typeof(TResult).GetNiceFullName()}] {mb.DeclaringType.FullName}.{mb.Name} is property: {ret.GetType().GetNiceFullName()}");
                        return (ret is List<TResult> _list) ? _list :
                            ret is TResult _item ? new List<TResult>() { _item } : null; // ret as List<TResult>;}
                    }
                    return null;
                })
                .Where(n => n != null)
                .Distinct()
                .ToList();
            if (Debug_DisplayFilterInfo) Debug.Log($"{typeof(TResult)} count: {result.Count}");
            Filters_Cache[typeof(T)] = result;
            return result;
        }
        return list as List<TResult>;
    }

    public static bool IsExcluded(MemberInfo memberInfo)
    {
        return GetFilters<FilterAttribute, MethodInfo>().Any(mb => (bool)mb.Invoke(null, new object[] { memberInfo }));
    }

    static List<string> m_ExcludesList;

    public static bool IsExcluded(Type type)
    {
        //
        // if(GetFilters<TypeFilterAttribute, MethodInfo>().Any(mb => (bool)mb.Invoke(null, new object[] { type }))) {
        //     return true;
        // }
        if (m_ExcludesList == null) {
            m_ExcludesList = GetFilters<ExcludeFilterAttribute, string>();
            if (!m_ExcludesList.Any()) {
                Debug.LogError("exclude not found");
            }
            if (Debug_DisplayFilterInfo) Debug.Log($"excludes: {m_ExcludesList.Count}");
        }
        //Debug.Log(m_ExcludesList.Count());
        var fullName = type.FullName;
        if (fullName == null ||
            type.IsDefined(typeof(ObsoleteAttribute)) ||
            !type.IsPublic ||
            type.Namespace.IsNullOrWhitespace() ||
            type.IsDefined<PuertsIgnoreAttribute>()) {
            return true;
        }
        if (TypeHasEditorRef(type)) {
            return true;
        }
        for (int i = 0; i < m_ExcludesList.Count; i++) {
            //new Regex(exclude[i]).Match(fullName).Success
            if (fullName.Contains(m_ExcludesList[i])) {
                return true;
            }
        }
        return false;
    }

    [Typing]
    static IEnumerable<Type> m_AllTypes {
        get {
            var unityTypes = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                where !(assembly.ManifestModule is System.Reflection.Emit.ModuleBuilder) &&
                    GetFilters<AssemblyFilterAttribute, string>().Any(s => assembly.GetName().Name == s)
                from type in assembly.GetExportedTypes()
                where type.Namespace != null &&
                    (GetFilters<IncludeFilterAttribute, string>()
                            .Any(t => t == type.Namespace || type.Namespace.StartsWith(t + "."))

                        //|| IncluceList().Any( t => $"{type.FullName}".Contains( t ) )
                    ) &&
                    !IsExcluded(type) &&
                    type.BaseType != typeof(MulticastDelegate) &&
                    !type.IsInterface &&
                    !type.IsEnum
                select type);

            // var customTypes = AppDomain.CurrentDomain.GetAssemblies()
            //     .Where(a => GetFilters<AssemblyFilterAttribute, string>().Any(s => a.GetName().Name == s))
            //
            //     //.Where(assembly => assembly != null)
            //     .SelectMany(assembly => assembly.GetExportedTypes(), (assembly, type) => new { assembly, type })
            //     .Where(@t =>
            //         @t.type.Namespace != null &&
            //         @t.type.BaseType != typeof(MulticastDelegate) &&
            //         !@t.type.IsInterface &&
            //         !@t.type.IsEnum &&
            //         !IsExcluded(@t.type))
            //     .Select(@t => @t.type);
            return unityTypes/*.Concat(customTypes)*/.Distinct()/*.Where(t => !t.Namespace.IsNullOrWhitespace())*/;
        }
    }

    [Binding]
    public static List<Type> delegate_types {
        get {
            var lua_call_csharp = m_AllTypes;
            var delegate_types = new List<Type>();
            var flag = BindingFlags.Public |
                BindingFlags.Instance |
                BindingFlags.Static |
                BindingFlags.IgnoreCase |
                BindingFlags.DeclaredOnly;
            foreach (var field in (from type in lua_call_csharp select type).SelectMany(type => type.GetFields(flag))) {
                if (typeof(Delegate).IsAssignableFrom(field.FieldType)) {
                    delegate_types.Add(field.FieldType);
                }
            }
            foreach (var method in (from type in lua_call_csharp select type).SelectMany(type => type.GetMethods(flag))
            ) {
                if (typeof(Delegate).IsAssignableFrom(method.ReturnType)) {
                    delegate_types.Add(method.ReturnType);
                }
                foreach (var param in method.GetParameters()) {
                    var paramType = param.ParameterType.IsByRef
                        ? param.ParameterType.GetElementType()
                        : param.ParameterType;
                    if (typeof(Delegate).IsAssignableFrom(paramType)) {
                        delegate_types.Add(paramType);
                    }
                }
            }
            return delegate_types.Where(t =>
                    !t.Namespace.IsNullOrWhitespace() &&
                    t.FullName != null &&
                    t.BaseType == typeof(MulticastDelegate) &&
                    !HasGenericParameter(t) &&
                    !DelegateHasEditorRef(t) &&
                    !IsExcluded(t))
                .Distinct()
                .ToList();
        }
    }

    public static bool HasGenericParameter(Type type)
    {
        if (type.IsGenericTypeDefinition) return true;
        if (type.IsGenericParameter) return true;

        if (type.IsByRef || type.IsArray) {
            return HasGenericParameter(type.GetElementType());
        }
        if (type.IsGenericType) {
            foreach (var typeArg in type.GetGenericArguments()) {
                if (HasGenericParameter(typeArg)) {
                    return true;
                }
            }
        }
        return false;
    }

    static List<Type> m_EditorTypes = new List<Type>();
    static List<Type> m_NotEditorTypes = new List<Type>();

    public static bool TypeHasEditorRef(Type type)
    {
        if (type == null || m_NotEditorTypes.Contains(type)) {
            return false;
        }
        if (m_EditorTypes.Contains(type)) {
            return true;
        }
        if (type.Namespace != null && (type.Namespace == "UnityEditor" || type.Namespace.StartsWith("UnityEditor."))) {
            if (!m_EditorTypes.Contains(type)) {
                m_EditorTypes.Add(type);
            }
            return true;
        }
        if (type.IsNested) {
            return TypeHasEditorRef(type.DeclaringType);
        }
        if (type.IsByRef || type.IsArray) {
            return TypeHasEditorRef(type.GetElementType());
        }
        if (type.IsGenericType) {
            foreach (var typeArg in type.GetGenericArguments()) {
                if (typeArg.IsNested) {
                    continue;
                }
                if (TypeHasEditorRef(typeArg)) {
                    return true;
                }
            }
        }
        if (type.BaseType != null &&
            !type.BaseType.IsGenericType &&
            !type.BaseType.IsNested &&
            type.BaseType != type.DeclaringType &&
            type.BaseType != type) {
            return TypeHasEditorRef(type.BaseType);
        }
        if (!m_NotEditorTypes.Contains(type)) {
            m_NotEditorTypes.Add(type);
        }
        return false;
    }

    public static bool DelegateHasEditorRef(Type delegateType)
    {
        if (TypeHasEditorRef(delegateType)) return true;

        var method = delegateType.GetMethod("Invoke");
        if (method == null) {
            return false;
        }
        if (TypeHasEditorRef(method.ReturnType)) return true;

        return method.GetParameters().Any(pinfo => TypeHasEditorRef(pinfo.ParameterType));
    }

    // 配置某Assembly下所有涉及到的delegate到CSharpCallLua下，Hotfix下拿不准那些delegate需要适配到lua function可以这么配置
    [Binding]
    static IEnumerable<Type> AllDelegate {
        get {
            BindingFlags flag = BindingFlags.DeclaredOnly |
                BindingFlags.Instance |
                BindingFlags.Static |
                BindingFlags.Public;
            List<Type> allTypes = new List<Type>();
            var allAssemblys = new List<Assembly> {
                //Assembly.Load( "Assembly-CSharp" ), Assembly.Load( "Assembly-CSharp-firstpass" )
            };

            // customAssemblys.AddRange( AssemblyList );
            GetFilters<AssemblyFilterAttribute, string>()
                .Where(s => AppDomain.CurrentDomain.GetAssemblies().Any(a => a.GetName().Name == s))
                .ForEach(t => allAssemblys.Add(Assembly.Load(t)));
            allAssemblys = allAssemblys.Distinct().ToList();
            foreach (var t in (from assembly in allAssemblys from type in assembly.GetTypes() select type)) {
                var p = t;
                while (p != null) {
                    allTypes.Add(p);
                    p = p.BaseType;
                }
            }
            allTypes = allTypes.Where(t =>
                    GetFilters<ExcludeFilterAttribute, string>()
                        .All(s => !(t.FullName != null && t.FullName.Contains(s))))
                .Distinct()
                .ToList();
            var allMethods = from type in allTypes from method in type.GetMethods(flag) select method;
            var returnTypes = from method in allMethods select method.ReturnType;
            var paramTypes = allMethods.SelectMany(m => m.GetParameters())
                .Select(pinfo =>
                    pinfo.ParameterType.IsByRef ? pinfo.ParameterType.GetElementType() : pinfo.ParameterType);
            var fieldTypes = from type in allTypes from field in type.GetFields(flag) select field.FieldType;
            return (returnTypes.Concat(paramTypes).Concat(fieldTypes)).Where(t =>
                    t.BaseType == typeof(MulticastDelegate) && !HasGenericParameter(t) && !DelegateHasEditorRef(t))
                .Distinct()
                .Where(t => !IsExcluded(t) && !t.Namespace.IsNullOrWhitespace() && t.FullName != null);
        }
    }

    [BlittableCopy]
    static IEnumerable<Type> Blittables {
        get {
            return new List<Type>() {
                //打开这个可以优化Vector3的GC，但需要开启unsafe编译
                //typeof(Vector3),
            };
        }
    }

}

}
