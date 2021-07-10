using Common.JSRuntime;
using GameEngine.Extensions;
using GameEngine.Kernel;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Channels;
using TeamDev.Redis;
#if UNITY_EDITOR
using UnityEditor.Callbacks;
#endif
using UnityEngine;
using UnityEngine.Rendering;
using Debug = UnityEngine.Debug;

namespace GameEngine.Utils
{
    /*
     * Redis Client Wrapper for Unity.
     * required library [ TeamDev.Redis.dll ].
     * https://www.nuget.org/packages/TeamDev.Redis.Client/
     */

    // [ExecuteAlways]
    public class RedisClient : MonoBehaviour
    {
        public string host = "127.0.0.1";
        public int port = 6379;
        public bool connectOnStart = false;

        public enum CLIENT_TYPE { ANY = 0, READWRITER, PUBLISHER, SUBSCRIBER }

        public CLIENT_TYPE clientType {
            get => clientAs;
            set => clientAs = value;
        }

        // #if UNITY_EDITOR
        //     [DidReloadScripts]
        //     static void didReload()
        //     {
        //         m_Instance = null;
        //     }
        // #endif

        [SerializeField]
        CLIENT_TYPE clientAs = CLIENT_TYPE.ANY;

        RedisDataAccessProvider redis = null;
        bool isConnect = false;
        static RedisClient m_Instance;
        public RedisDataAccessProvider raw => redis;

        public static RedisClient Instance {
            get {
                if (m_Instance == null) {
                    m_Instance = FindObjectOfType<RedisClient>() ?? Resources.FindObjectsOfTypeAll<RedisClient>().FirstOrDefault();

                    if (m_Instance?.gameObject.scene.name == null && m_Instance != null) {
                        m_Instance = Core.Instantiate(m_Instance.gameObject).GetComponent<RedisClient>();
                    }
                    m_Instance?.Connect();
                }
                // if (m_Instance == null) {
                //     m_Instance = FindObjectOfType<RedisClient>();
                //     m_Instance.Connect();
                // }

                return m_Instance;
            }
        }

        public bool IsConnect => isConnect;

    #region Pub/Sub

        // スレッドでのデータ受信
        volatile bool isDataReceived = false;
        string receivedChannel = "";
        string receivedMessage = "";

        // PubSubデータ受信イベント
        public delegate void OnReceivedPubSubMessageDelegate(string channel, string message);

        public event OnReceivedPubSubMessageDelegate OnReceivedPubSubMessage;

        void InvokeOnReceived(string channel, string message)
        {
            OnReceivedPubSubMessage?.Invoke(channel, message);
        }

    #endregion

        void Awake()
        {
            Debug.Log("start redis".ToYellow());

            if (m_Instance == null) {
                m_Instance = this;
            } else if (m_Instance != this) {
                Debug.Log($"redis repeated {gameObject.name}");
                gameObject.DestroySelf();

                return;
            }

            if (Application.isPlaying) {
                DontDestroyOnLoad(gameObject);
            }
        }

        [ButtonGroup("test")]
        void SetIp()
        {
            if (!string.IsNullOrEmpty(Config.instance.AddressableIP)) {
                host = Config.instance.AddressableIP;
                Debug.Log("ok");
            } else {
                Debug.Log("GameCore.Config.AddressableIP == null");
            }
        }

        [ButtonGroup("test")]
        void TestJSCode()
        {
            Debug.Log($"jscode length: {Get("ts://app.cjs").Length}".ToGreen());
        }

        void Start()
        {
            //   clientAs = clientType;
            if (connectOnStart && (Debug.isDebugBuild || Application.isEditor)) {
                Debug.Log("connect redis".ToYellow());
                Connect();
                TestJSCode();
            }
        }

        void Update()
        {
            // 受信イベントをメインスレッドで実行
            if (isDataReceived) {
                InvokeOnReceived(receivedChannel, receivedMessage);
                isDataReceived = false;
            }
        }

        void OnApplicationQuit()
        {
            Close();
        }

        // 接続
        public bool Connect(string host = default)
        {
            if (redis == null) {
                if (!host.IsNullOrWhitespace()) {
                    this.host = host;
                }
                isConnect = false;

                try {
                    redis = new RedisDataAccessProvider();
                    redis.Configuration.Host = this.host;
                    redis.Configuration.Port = port;
                    redis.Connect();
                    redis.MessageReceived += messageReceived;

                    if (redis == null) {
                        return isConnect;
                    }

                    Debug.Log($"RedisClient :: connected to [ {this.host} : {port.ToString()} ]");

                    //Debug.Log(Get("ts://hello.ts"));
                    isConnect = true;
                } catch (Exception err) {
                    Debug.LogError("RedisClient :: " + err.Message);
                }
            }

            return isConnect;
        }

        // 切断
        public void Close()
        {
            if (redis != null) {
                redis.MessageReceived -= messageReceived;
                redis.Close();
                redis.Dispose();
                redis = null;
            }
            isConnect = false;
        }

    #region Set/Get string

        public void Set(string key, string value)
        {
            if (clientAs == CLIENT_TYPE.READWRITER || clientAs == CLIENT_TYPE.ANY) {
                if (isConnect && redis != null) {
                    redis.SendCommand(RedisCommand.SET, key, value);
                    redis.WaitComplete();
                }
            } else {
                Debug.LogWarning("RedisClient :: Set can only client type [ CLIENT_TYPE.READWRITER ]");
            }
        }

        public void SetAs(CLIENT_TYPE type)
        {
            clientAs = type;
        }

        public string Get(string key)
        {
            var ret = string.Empty;

            if (clientAs == CLIENT_TYPE.READWRITER || clientAs == CLIENT_TYPE.ANY) {
                if (isConnect && redis != null) {
                    redis.SendCommand(RedisCommand.GET, key);

                    //   redis.WaitComplete();

                    ret = redis.ReadString();
                }
            } else {
                Debug.LogWarning("RedisClient :: Get can only client type [ CLIENT_TYPE.READWRITER ]");
            }

            return ret;
        }

    #endregion

        public void Zadd(string key, float score, string value)
        {
            if (clientAs == CLIENT_TYPE.READWRITER || clientAs == CLIENT_TYPE.ANY) {
                if (isConnect && redis != null) {
                    redis.SendCommand(RedisCommand.ZADD, key, score.ToString("f6"), value);
                    redis.WaitComplete();
                }
            } else {
                Debug.LogWarning("RedisClient :: ZADD can only client type [ CLIENT_TYPE.READWRITER ]");
            }
        }

        public string[] keys;

        public void Keys()
        {
            redis.SendCommand(RedisCommand.KEYS, "*");
            keys = redis.ReadMultiString();

            //打印所有的key
            foreach (var item in keys) {
                Debug.Log(item);
            }
        }

    #region Pub/Sub

        List<string> subChannels = new List<string>();

        public void Subscribe(params string[] channels)
        {
            if (clientAs == CLIENT_TYPE.SUBSCRIBER || clientAs == CLIENT_TYPE.ANY) {
                if (isConnect) {
                    channels.Where(s => !subChannels.Contains(s))
                        .ForEach(ch => {
                            redis.Messaging.Subscribe(ch);
                            subChannels.Add(ch);
                        });
                }
            } else {
                Debug.LogWarning("RedisClient :: Subscribe can only client type [ CLIENT_TYPE.SUBSCRIBER ]");
            }
        }

        public void Publish(string channel, string message)
        {
            if (clientAs == CLIENT_TYPE.PUBLISHER || clientAs == CLIENT_TYPE.ANY) {
                if (isConnect && redis != null) {
                    StartCoroutine(PublishCoroutine(channel, message));
                }
            } else {
                Debug.LogWarning("RedisClient :: Publish can only client type [ CLIENT_TYPE.PUBLISHER ]");
            }
        }

        IEnumerator PublishCoroutine(string channel, string message)
        {
            // 送信
            redis.Messaging.Publish(channel, message);

            yield return null;
        }

        void messageReceived(string channel, string message)
        {
            isDataReceived = true;
            receivedChannel = channel;
            receivedMessage = message;
            Debug.Log($"RedisClient :: channel is [ {channel} ] / message is [ {message} ]");
        }

    #endregion
    }
}