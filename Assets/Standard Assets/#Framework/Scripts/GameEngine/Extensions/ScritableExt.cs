using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace GameEngine.Extensions {

public static class ScritableExt {

    public static T Save<T>(this T obj) where T : ScriptableObject
    {
        File.WriteAllText(typeof(T).FullName.md5(),
            Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonUtility.ToJson(obj))));

        return obj;
    }

    public static T Restore<T>(this T obj) where T : ScriptableObject
    {
        JsonUtility.FromJsonOverwrite(
            Encoding.UTF8.GetString(Convert.FromBase64String(File.ReadAllText(typeof(T).FullName.md5()))), obj);

        return obj;
    }
}

}