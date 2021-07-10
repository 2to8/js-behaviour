using Sirenix.Utilities;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameEngine.Extensions {

public static partial class Files {

    public static void Rename(this FileInfo fileInfo, string newName)
    {
        if (newName.IsNullOrWhitespace() || fileInfo == null) {
            return;
        }

        Debug.Log($"move to: {Path.Combine(fileInfo.Directory.FullName, newName)}");
        fileInfo.MoveTo(Path.Combine(fileInfo.Directory.FullName, newName));
    #if UNITY_EDITOR
        AssetDatabase.Refresh();
    #endif
    }

    public static string AssetPath(this string path)
    {
    #if UNITY_EDITOR
        if (Guid.TryParse(path, out var guid)) {
            path = AssetDatabase.GUIDToAssetPath(path);
        }
    #endif
        return path.Replace('\\', '/').Replace(Application.dataPath, "Assets").Trim('/');
    }

    public static string PathCombine(this string path, params string[] names)
    {
        names.ForEach(s => path = Path.Combine(path, s));

        return path.Replace("\\", "/");
    }

    public static void Rename(this Object obj, string newName)
    {
        obj?.dataPathRoot().Rename(newName);
    }

    public static void Rename(this string fullpath, string newName)
    {
        if (fullpath.IsNullOrWhitespace()) {
            return;
        }

        if (!File.Exists(fullpath)) {
            fullpath = fullpath.dataPathRoot();
        }

        if (File.Exists(fullpath)) {
            new FileInfo(fullpath).Rename(newName);
        } else {
            Debug.Log($"{fullpath} not exists");
        }
    }

}

}