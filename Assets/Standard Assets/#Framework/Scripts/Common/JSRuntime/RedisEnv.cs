using GameEngine.Utils;
using Puerts;
using Sirenix.Utilities;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UniWebServer;

namespace Common.JSRuntime {

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

    public new bool FileExists(string filepath) =>
        !redis.Get($"ts://{Path(filepath)}").IsNullOrWhitespace() || base.FileExists(Path(filepath));

    public new string ReadFile(string filepath, out string debugpath)
    {
        var ret = redis.Get(debugpath = $"ts://{Path(filepath)}");
        Debug.Log($"[file] {debugpath}");

        return ret.IsNullOrWhitespace() ? base.ReadFile(Path(filepath), out debugpath) : ret;
    }

}

public class HttpRedis : MonoBehaviour, IWebResource {

    [SerializeField]
    string path = "/redis";

    const string requestChannel = "Request";
    const string responseChannel = "Response";

    //JsEnv jsEnv;
    //public JsArg[] args;

    //
    RedisClient redis => RedisClient.Instance;

    //public TextAsset html;
    EmbeddedWebServerComponent server;
    public JsonResponse Result = new JsonResponse();

    void Start()
    {
        // jsEnv = new JsEnv(new RedisLoader());

        server = GetComponentInParent<EmbeddedWebServerComponent>() ??
            gameObject.AddComponent<EmbeddedWebServerComponent>();

        server.AddResource(path, this);
    }

    void OnEnable()
    {
        //注册log监听
        Application.logMessageReceived += HangleLog;
    }

    void OnDisable()
    {
        // Remove callback when object goes out of scope
        //当对象超出范围，删除回调。
        Application.logMessageReceived -= HangleLog;
    }

    void HangleLog(string logString, string stackTrace, LogType type)
    {
        switch (type) {
            case LogType.Log :       break;
            case LogType.Error :     break;
            case LogType.Exception : break;
            case LogType.Warning :   break;
        }

        if (redis.IsConnect) {
            var ret = new JsonResponse { ["massage"] = $"[{type}] {logString}", ["trace"] = stackTrace };
            redis.Publish(responseChannel, ret);

            // File.AppendAllText(Application.dataPath + "/../Packages/Hotfix/Logs/Debug.log", ret,
            //     Encoding.UTF8);
        }
    }

    RedisClient.OnReceivedPubSubMessageDelegate SubMessageDelegate;

    public void HandleRequest(Request request, Response response)
    {
        var query = Core.ParseQueryString(request.uri);

        if (query.TryGetValue("host", out var host)) {
            if (!redis.IsConnect && redis.Connect(host)) {
                if (SubMessageDelegate == null) {
                    SubMessageDelegate = (ch, message) => {
                        if (ch == requestChannel) {
                            var res = new JsonResponse();
                            res["Source"] = message;

                            try {
                                //jsEnv = new JsEnv(new RedisLoader());
                                //Debug.Log(res["Result"] = jsEnv.Eval<object>(message));
                            } catch (Exception e) {
                                Debug.LogError(res["Error"] = e.ToString());
                            }

                            redis.Publish(responseChannel, res);
                        }
                    };
                }

                redis.Subscribe(requestChannel);
                redis.OnReceivedPubSubMessage -= SubMessageDelegate;
                redis.OnReceivedPubSubMessage += SubMessageDelegate;
                Result["IsFirst"] = true;
            }

            Result["Connect"] = redis.IsConnect;
        } else if (query.TryGetValue("req", out var req)) {
            if (redis.IsConnect) {
                var res = new JsonResponse { ["Source"] = request.uri };

                try {
                    //Debug.Log(res["Result"] = jsEnv.Eval<object>($"require('{req}')"));
                } catch (Exception e) {
                    Debug.LogError(Result["Error"] = e);
                }

                Result = res;
            } else {
                Result["Error"] = "Redis Not Connected";
            }
        } else {
            Result["Error"] = "Missing Param: host|req";
        }
        redis.Publish(responseChannel, Result);

        response.WriteJson(Result);
    }

}

}