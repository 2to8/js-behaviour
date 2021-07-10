// using App;
// using GameEngine.Kernel;
// using Puerts;
// using System;
// using System.Text.RegularExpressions;
// using UnityEngine;
// using UniWebServer;
//
// namespace Env {
//
// [ RequireComponent(typeof(RedisClient)), ExecuteAlways ]
// public class SearchHelper : MonoBehaviour {
//
//     public static SearchHelper Instance;
//     RedisClient redis => RedisClient.Instance;
//     bool inited;
//     const string CHANNEL = "SEARCH";
//
//     void Awake()
//     {
//         Instance = this;
//         Core.SearchHelper(Search);
//     }
//
//     void Update()
//     {
//         if(!inited && redis.IsConnect) {
//             inited = true;
//             redis.Subscribe(CHANNEL);
//             redis.OnReceivedPubSubMessage -= OnSub;
//             redis.OnReceivedPubSubMessage += OnSub;
//         }
//     }
//
//     void Search(string search)
//     {
//         if(redis.IsConnect) {
//             redis.Publish(CHANNEL, search);
//         }
//     }
//
//     void OnSub(string ch, string message)
//     {
//         if(ch == CHANNEL) {
//             Debug.Log(message);
//             if(redis.IsConnect) {
//                 var jsEnv = new JsEnv(new RedisLoader());
//                 var res = new JsonResponse();
//                 var source = Regex.Replace(message, "^;*(.*)$", "$1");
//                 res["Source"] = source;
//                 try {
//                     Debug.Log(res["Result"] = jsEnv.Eval<object>(source));
//                 } catch(Exception e) {
//                     Debug.LogError(res["Error"] = e);
//                 }
//                 redis.Publish("Response", res);
//             }
//         }
//     }
//
//     void Start() { }
//
// }
//
// }

