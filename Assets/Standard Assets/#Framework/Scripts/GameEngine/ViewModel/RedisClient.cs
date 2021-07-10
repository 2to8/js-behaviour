// using Sirenix.Utilities;
// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using TeamDev.Redis;
// using UnityEngine;
//
// namespace GameCore {
//
// /*
//  * Redis Client Wrapper for Unity.
//  * required library [ TeamDev.Redis.dll ].
//  * https://www.nuget.org/packages/TeamDev.Redis.Client/
//  */
//
// [ExecuteAlways]
// public class RedisClient : MonoBehaviour {
//
//     public string host = "127.0.0.1";
//     public int port = 6379;
//     public bool connectOnStart = false;
//     public enum CLIENT_TYPE { ANY = 0, READWRITER, PUBLISHER, SUBSCRIBER }
//
//     public CLIENT_TYPE clientType {
//         get => clientAs;
//         set => clientAs = value;
//     }
//
//     [ SerializeField ]
//     private CLIENT_TYPE clientAs = CLIENT_TYPE.ANY;
//
//     private RedisDataAccessProvider redis = null;
//     private bool isConnect = false;
//     static RedisClient m_Instance;
//     public RedisDataAccessProvider raw => redis;
//
//     public static RedisClient Instance =>
//         m_Instance ?? (m_Instance = AppCore.FindObjectOfTypeAll<RedisClient>());
//
//     public bool IsConnect => isConnect;
//
// #region Pub/Sub
//
//     // スレッドでのデータ受信
//     private volatile bool isDataReceived = false;
//     private string receivedChannel = "";
//     private string receivedMessage = "";
//
//     // PubSubデータ受信イベント
//     public delegate void OnReceivedPubSubMessageDelegate(string channel, string message);
//     public event OnReceivedPubSubMessageDelegate OnReceivedPubSubMessage;
//
//     private void InvokeOnReceived(string channel, string message)
//     {
//         OnReceivedPubSubMessage?.Invoke(channel, message);
//     }
//
// #endregion
//
//     void Awake()
//     {
//         m_Instance = this;
//     }
//
//     void Start()
//     {
//         //   clientAs = clientType;
//         if(connectOnStart) Connect();
//     }
//
//     void Update()
//     {
//         // 受信イベントをメインスレッドで実行
//         if(isDataReceived) {
//             InvokeOnReceived(receivedChannel, receivedMessage);
//             isDataReceived = false;
//         }
//     }
//
//     void OnApplicationQuit()
//     {
//         Close();
//     }
//
//     // 接続
//     public bool Connect(string host = default)
//     {
//         if(redis == null) {
//             if(!host.IsNullOrWhitespace()) {
//                 this.host = host;
//             }
//
//             isConnect = false;
//
//             try {
//                 redis = new RedisDataAccessProvider();
//                 redis.Configuration.Host = this.host;
//                 redis.Configuration.Port = port;
//                 redis.Connect();
//                 redis.MessageReceived += messageReceived;
//
//                 if(redis == null) return isConnect;
//
//                 Debug.Log($"RedisClient :: connected to [ {this.host} : {port.ToString()} ]");
//                 //Debug.Log(Get("ts://hello.ts"));
//                 isConnect = true;
//             } catch(Exception err) {
//                 Debug.LogError("RedisClient :: " + err.Message);
//             }
//         }
//
//         return isConnect;
//     }
//
//     // 切断
//     public void Close()
//     {
//         if(redis != null) {
//             redis.MessageReceived -= messageReceived;
//             redis.Close();
//             redis.Dispose();
//             redis = null;
//         }
//
//         isConnect = false;
//     }
//
// #region Set/Get string
//
//     public void Set(string key, string value)
//     {
//         if(clientAs == CLIENT_TYPE.READWRITER || clientAs == CLIENT_TYPE.ANY) {
//             if(isConnect && redis != null) {
//                 redis.SendCommand(RedisCommand.SET, key, value);
//                 redis.WaitComplete();
//             }
//         } else
//             Debug.LogWarning("RedisClient :: Set can only client type [ CLIENT_TYPE.READWRITER ]");
//     }
//
//     public void SetAs(CLIENT_TYPE type)
//     {
//         clientAs = type;
//     }
//
//     public string Get(string key)
//     {
//         string ret = string.Empty;
//
//         if(clientAs == CLIENT_TYPE.READWRITER || clientAs == CLIENT_TYPE.ANY) {
//             if(isConnect && redis != null) {
//                 redis.SendCommand(RedisCommand.GET, key);
//                 //redis.WaitComplete();
//                 ret = redis.ReadString();
//             }
//         } else
//             Debug.LogWarning("RedisClient :: Get can only client type [ CLIENT_TYPE.READWRITER ]");
//
//         return ret;
//     }
//
// #endregion
//
//     public void Zadd(string key, float score, string value)
//     {
//         if(clientAs == CLIENT_TYPE.READWRITER || clientAs == CLIENT_TYPE.ANY) {
//             if(isConnect && redis != null) {
//                 redis.SendCommand(RedisCommand.ZADD, key, score.ToString("f6"), value);
//                 redis.WaitComplete();
//             }
//         } else
//             Debug.LogWarning("RedisClient :: ZADD can only client type [ CLIENT_TYPE.READWRITER ]");
//     }
//
//     public string[] keys;
//
//     public void Keys()
//     {
//         redis.SendCommand(RedisCommand.KEYS, "*");
//         keys = redis.ReadMultiString();
//
//         //打印所有的key
//         foreach(var item in keys) {
//             Debug.Log(item);
//         }
//     }
//
// #region Pub/Sub
//
//     List<string> subChannels = new List<string>();
//
//     public void Subscribe(params string[] channels)
//     {
//         if(clientAs == CLIENT_TYPE.SUBSCRIBER || clientAs == CLIENT_TYPE.ANY) {
//             if(isConnect) {
//                 channels.Where(s => !subChannels.Contains(s))
//                     .ToList().ForEach(ch => {
//                         redis.Messaging.Subscribe(ch);
//                         subChannels.Add(ch);
//                     });
//             }
//         } else
//             Debug.LogWarning("RedisClient :: Subscribe can only client type [ CLIENT_TYPE.SUBSCRIBER ]");
//     }
//
//     public void Publish(string channel, string message)
//     {
//         if(clientAs == CLIENT_TYPE.PUBLISHER || clientAs == CLIENT_TYPE.ANY) {
//             if(isConnect && redis != null) StartCoroutine(PublishCoroutine(channel, message));
//         } else
//             Debug.LogWarning("RedisClient :: Publish can only client type [ CLIENT_TYPE.PUBLISHER ]");
//     }
//
//     private IEnumerator PublishCoroutine(string channel, string message)
//     {
//         // 送信
//         redis.Messaging.Publish(channel, message);
//
//         yield return null;
//     }
//
//     private void messageReceived(string channel, string message)
//     {
//         isDataReceived = true;
//         receivedChannel = channel;
//         receivedMessage = message;
//         Debug.Log($"RedisClient :: channel is [ {channel} ] / message is [ {message} ]");
//     }
//
// #endregion
//
// }
//
// }

