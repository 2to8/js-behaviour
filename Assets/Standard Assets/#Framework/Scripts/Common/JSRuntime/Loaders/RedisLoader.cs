using GameEngine.Utils;
using Puerts;
using Sirenix.Utilities;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Common.JSRuntime.Loaders {

public class RedisLoader : DefaultLoader {

    static RedisClient redis => RedisClient.Instance;

    public static string Path(string filepath)
    {
        if (!filepath.EndsWith(".js")) {
            filepath += ".js";
        }
        filepath = Regex.Replace(filepath, "^[./]*(.*)$", "$1");

        return filepath;
    }

    public new bool FileExists(string filepath) => !redis.Get($"ts://{Path(filepath)}").IsNullOrWhitespace() ||
        base.FileExists(Path(filepath));

    public new string ReadFile(string filepath, out string debugpath)
    {
        debugpath = $"ts://{Path(filepath)}";

        var ret = redis.Get(debugpath);
        Debug.Log($"[file] {debugpath}");

        var load = base.ReadFile(Path(filepath), out debugpath);

        return ret.IsNullOrWhitespace() ? load : ret;
    }

}

}