using Newtonsoft.Json;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using Object = UnityEngine.Object;

namespace GameEngine.Extensions {

public static partial class Strings {

    public static string Join<T>(this IEnumerable<T> list, string str = ", ") => string.Join(str, list.Select(t => $"{t}"));

    public static bool IsNotNullOrEmpty(this string value) => !(value == null || value.Length == 0);

    public static bool IsNullOrEmpty(this string value) => value == null || value.Length == 0;

    public static string _TagKey(this string str)
    {
        var opt = RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace;
        str = Regex.Replace($"{str.Trim()}", @"\s+", "_", opt);

        return str.Replace("__", "_").ToCapitalStr();
    }

    //public static string ToString(this byte[] bytes) => System.Text.Encoding.Default.GetString(bytes);

    public static string ToPadSides(this string str, int totalWidth = -1, char paddingChar = '-')
    {
        totalWidth = totalWidth > 0 ? totalWidth : PadLength;
        var padding = totalWidth - str.Length;
        var padLeft = padding / 2 + str.Length;

        return $" {str} ".ToCapitalStr().PadLeft(padLeft, paddingChar).PadRight(totalWidth, paddingChar).ToBold();
    }

    public static string ToCapitalStr(this string str) => Regex.Replace(str, @"\b[a-z]", m => m.Value.ToUpper());

    static int PadLength = 40;

    //
    static string DebugStr(this object str, params object[] extra) => ($"{str} "
        + string.Join(" | ", extra.Select(t => t is Type type ? type.GetNiceFullName() :
            t is string s ? s :
            t is Component component ? component.GetPath() + " -> " + component.GetType().GetNiceFullName() :
            JsonConvert.SerializeObject(t)))).Trim();

    public static string ToGreen(this object str, params object[] extra) =>
        DebugStr($"<color=#21B351>{$"{str}".ToPadSides()}</color>", extra);

    public static string ToYellow(this object str, params object[] extra) =>
        DebugStr($"<color=yellow>{$"{str}".ToPadSides()}</color>", extra);

    public static string ToWhite(this object str, params object[] extra) =>
        DebugStr($"<color=white>{$"{str}".ToPadSides()}</color>", extra);

    public static string ToBlue(this object str, params object[] extra) =>
        DebugStr($"<color=#19A8F3>{$"{str}".ToPadSides()}</color>", extra);

    public static string ToRed(this object str, params object[] extra) =>
        DebugStr($"<color=red>{$"{str}".ToPadSides()}</color>", extra);

    public static string ToBold(this object str, params object[] extra) => DebugStr($"<b>{str}</b>", extra);

    public static byte[] GetBytes(this string str) => Encoding.UTF8.GetBytes(str);

    public static string GetString(this byte[] bytes) => Encoding.UTF8.GetString(bytes);

    public static string Md5(this string str)
    {
        using (var hash = MD5.Create()) {
            return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(str)).Select(x => x.ToString("x2")));
        }
    }

    public static string PadBoth(this string str, int length, string chars = "=")
    {
        var spaces = length - str.Length - 2;
        var padLeft = spaces / 2 + str.Length;

        return $" {str} ".PadLeft(padLeft, chars[0]).PadRight(length, chars[0]);
    }

    public static string Color(this string str, Color color) =>
        "<color=#" + ColorUtility.ToHtmlStringRGB(color) + $">{str}</color>";

    public static string md5(this string observedText)
    {
        return string.Concat(MD5.Create()
            .ComputeHash(Encoding.UTF8.GetBytes(observedText + "405B8654-0BF2-4462-93E6-F77B290F249D"))
            .Select(x => x.ToString("x2")));
    }

    public static string JsPath(this string path, string ext = default, bool fullpath = false)
    {
        path = path.Replace("\\", "/");
        path = Regex.Replace(path, "^[./]+", "");

        if (!ext.IsNullOrWhitespace()) {
            ext = $"{ext}".Contains(".") ? ext : $".{ext}";

            if (!path.Contains(ext)) {
                path += ext;
            }
        }

        return path;
    }

    public static string JsFullPath(this string path)
    {
        if (!path.StartsWith("Assets") && !path.StartsWith("Packages")) {
            path = $"Assets/{path}";
        }

        return Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Application.dataPath) + "", path))
            .Replace("\\", "/");
    }

    public static T HasAsset<T>(this string path) where T : Object
    {
        path = path.JsPath();
        var resPath = path.Split(new[] { "Resources/" }, StringSplitOptions.None).Last();
        var res = Resources.Load<T>(resPath);
    #if UNITY_EDITOR
        if (res == null) {
            res = AssetDatabase.LoadAssetAtPath<T>(!path.Contains("Assets/") && !path.Contains("Packages/")
                ? $"Assets/{path}"
                : path);
        }
    #endif
        if (res == null) {
            var allAssets = Addressables.ResourceLocators.OfType<ResourceLocationMap>()
                .SelectMany(locationMap => locationMap.Locations.Keys.Select(key => key.ToString()));

            if (allAssets.Any(s => s == path)) { }
        }

        return res;
    }

    public static string RemoveAssets(this string path)
    {
        path = path.Replace(Application.dataPath, "Assets");

        return Regex.Replace($"{path}", @"^Assets", "").TrimStart('\\', '/');
    }

    public static string GetObjectPath(this Object obj)
    {
    #if UNITY_EDITOR
        return AssetDatabase.GetAssetPath(obj) ?? string.Empty;
    #endif
        return string.Empty;
    }

    public static string GetGUID(this string path)
    {
    #if UNITY_EDITOR
        return AssetDatabase.AssetPathToGUID(path) ?? string.Empty;
    #endif
        return string.Empty;
    }

    public static string FullPath(this string path)
    {
    #if UNITY_EDITOR
        var root = Path.GetDirectoryName(Application.dataPath);

        if (!File.Exists(path)) {
            if (File.Exists(Path.Combine(root, path))) {
                path = Path.Combine(root, path);
            } else if (File.Exists(Path.Combine(root, "Assets", path))) {
                path = Path.Combine(root, "Assets", path);
            }
        }
    #endif
        return path.Replace('\\', '/');
    }

}

}