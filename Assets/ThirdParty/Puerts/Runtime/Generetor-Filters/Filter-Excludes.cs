using Puerts.Attributes;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

// using UnityEngine.AddressableAssets;
// using UnityEngine.AddressableAssets.ResourceLocators;
// using UnityEngine.ResourceManagement.Util;
// using UnityEngine.Tilemaps;
// using UnityEngine.Timeline;
// using UnityEngine.UI;
// using UnityEngine.UI.Extensions;

namespace Puerts {

[Configure]
public static partial class Filter {

    [Filter]
    static bool m_FilterReadOnlyProperty(MemberInfo mb)
    {
        if (mb is MethodInfo propertyInfo) {
            if (mb.Name.Contains("editor", StringComparison.OrdinalIgnoreCase)) {
                //Debug.Log($"{mb.DeclaringType.FullName} {mb.Name}" );
                return true;
            }
        }
        if (mb.IsDefined<PuertsIgnoreAttribute>()) {
            return true;
        }

        //var test = new Dictionary<string, GameObject>();
        //test.ForEach(t => )
        return false;
    }

    public static List<List<string>> BlackList => GetFilters<PuertsBlacklistAttribute, List<string>>();

    [Filter]
    static bool m_IsMemberInBlackList(MemberInfo mb)
    {
        //if(isDefined(mb, typeof(BlackListAttribute))) return true;

        // if(mb is FieldInfo && (mb as FieldInfo).FieldType.IsPointer) return true;
        // if(mb is PropertyInfo && (mb as PropertyInfo).PropertyType.IsPointer) return true;

        // foreach(var filter in memberFilters) {
        //     if(filter(mb)) {
        //         return true;
        //     }
        // }

        // if(BlackList == null) {
        //     GetAllBlackList();
        //     Debug.Log($"BlackList: {BlackList.Count}");
        // }
        if (BlackList != null) {
            foreach (var exclude in BlackList) {
                if (exclude.Count == 2 && mb.DeclaringType.FullName == exclude[0] && mb.Name == exclude[1]) {
                    return true;
                }
            }
        }
        if (GetFilters<ExcludeFilterAttribute, string>().Contains($"{mb.DeclaringType?.FullName}.{mb.Name}")) {
            return true;
        }
        if (mb is MethodBase methodBase && m_IsMethodInBlackList(methodBase)) {
            return true;
        }
        if (m_IsObsolete(mb) || m_IsObsolete(mb.DeclaringType)) {
            return true;
        }

        // 有bug, Dicitonay/list的会被过滤所有方法
        // if(mb.DeclaringType is Type declaringType && isNotPublic(declaringType)) {
        //     return true;
        // }

        // if(MethodFilter.Invoke(mb)) {
        //     return true;
        // }
        return false;
    }

    static bool m_IsMethodInBlackList(MethodBase mb)
    {
        //指针目前不支持，先过滤
        // if(mb.GetParameters().Any(pInfo => pInfo.ParameterType.IsPointer)) return true;
        // if(mb is MethodInfo && (mb as MethodInfo).ReturnType.IsPointer) return true;

        // foreach(var filter in memberFilters) {
        //     if(filter(mb)) {
        //         return true;
        //     }
        // }
        foreach (var exclude in BlackList) {
            if (mb.DeclaringType.ToString() == exclude[0] && mb.Name == exclude[1]) {
                // if(exclude.Count == 2) {
                //     return true;
                // }
                var parameters = mb.GetParameters();
                if (parameters.Length != exclude.Count - 2) {
                    continue;
                }
                bool paramsMatch = true;
                for (int i = 0; i < parameters.Length; i++) {
                    if (parameters[i].ParameterType.ToString() != exclude[i + 2]) {
                        paramsMatch = false;
                        break;
                    }
                }
                if (paramsMatch) return true;
            }
        }
        return false;
    }

    // static bool isNotPublic(Type type)
    // {
    //     if(type.IsByRef || type.IsArray) {
    //         return isNotPublic(type.GetElementType());
    //     } else {
    //         if((!type.IsNested && !type.IsPublic) || (type.IsNested && !type.IsNestedPublic)) {
    //             return true;
    //         }
    //         if(type.IsGenericType) {
    //             foreach(var ga in type.GetGenericArguments()) {
    //                 if(isNotPublic(ga)) {
    //                     return true;
    //                 }
    //             }
    //         }
    //         if(type.IsNested) {
    //             var parent = type.DeclaringType;
    //             while(parent != null) {
    //                 if((!parent.IsNested && !parent.IsPublic) ||
    //                     (parent.IsNested && !parent.IsNestedPublic)) {
    //                     return true;
    //                 }
    //                 if(parent.IsNested) {
    //                     parent = parent.DeclaringType;
    //                 } else {
    //                     break;
    //                 }
    //             }
    //         }
    //         return false;
    //     }
    // }

    static bool m_IsObsolete(MemberInfo mb)
    {
        if (mb == null) return false;

        ObsoleteAttribute oa = m_GetCustomAttribute(mb, typeof(ObsoleteAttribute)) as ObsoleteAttribute;
    #if XLUA_GENERAL && !XLUA_ALL_OBSOLETE || XLUA_JUST_EXCLUDE_ERROR
            return oa != null && oa.IsError;
    #else
        return oa != null;
    #endif
    }

    static bool m_IsDefined(MemberInfo test, Type type)
    {
    #if XLUA_GENERAL
            return test.GetCustomAttributes(false).Any(ca => ca.GetType().ToString() == type.ToString());
    #else
        return test.IsDefined(type, false);
    #endif
    }

    static object m_GetCustomAttribute(MemberInfo test, Type type)
    {
    #if XLUA_GENERAL
            return test.GetCustomAttributes(false).FirstOrDefault(ca => ca.GetType().ToString() == type.ToString());
    #else
        return test.GetCustomAttributes(type, false).FirstOrDefault();
    #endif
    }

    static bool m_IsObsolete(Type type)
    {
        if (type == null) return false;

        if (m_IsObsolete(type as MemberInfo)) {
            return true;
        }
        return (type.DeclaringType != null) ? m_IsObsolete(type.DeclaringType) : false;
    }

}

}