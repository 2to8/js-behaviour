using GameEngine.Kernel;
using GameEngine.Models.Contracts;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Add an entry to the Assets menu for creating an asset of this type
namespace GameEngine {

[CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(Config))]
public class Config : /*SingletonScriptableObject*/DbTable<Config> {

    // public class URLSetting {
    public int Port = 11220;

    public static string START_UP_URL =>

        // TODO：外网启动地址，这个地址在发布线上游戏时自行部署和设置
        "https://chivas.framework.com/startup";

    public static string SERVER_RESOURCE_URL { get; set; }
    public static string APP_DOWNLOAD_URL { get; set; }
    public static string LOGIN_URL { get; set; }
    public static string REPORT_ERROR_URL { get; set; }
    public static string SERVER_LIST_URL { get; set; }
    public static string NOTICE_URL { get; set; }

    //}

    [SerializeField, HideInInspector]
    string m_AddressableIP;

    [SerializeField, HideInInspector]
    bool m_LogSqliteQuery;

    [OdinSerialize]
    public bool LogSqliteQuery { get => m_LogSqliteQuery; set => logChanged(m_LogSqliteQuery = value); }

    void logChanged(bool value)
    {
        //PlayerPrefs.SetInt("DB_Trace", value ? 1 : 0);
    }

    [OdinSerialize, ShowInInspector, Title("PC端的IP, 从菜单 Tests/GetIP 获取")]
    public string AddressableIP {
        get => m_AddressableIP ?? (m_AddressableIP = GetLocalIPAddress());
        set => SetValue(ref m_AddressableIP, value);
    }

    public static string AssetUrl => $"http://{instance.AddressableIP}:{instance.Port}";

    [SerializeField]
    string m_Version;

    [SerializeField]
    public AssetReference InitScene;

    [OdinSerialize]
    public string Version { get => m_Version; set => SetValue(ref m_Version, value); }

#if UNITY_EDITOR

    //[UnityEditor.Callbacks.DidReloadScripts]
    static void CheckVersion()
    {
        var config = DB.Load<Config>();
        var version = string.Empty;

        if (config.Version != Application.version) {
        #if UNITY_EDITOR
            version = $"before: {Application.version}";

            // if (EditorApplication.isPlayingOrWillChangePlaymode)
            UnityEditor.PlayerSettings.bundleVersion = config.Version;

            //else
        #endif
        }
        Debug.Log($"version: {config.Version} {version}");
    }
#endif

    [SerializeField]
    public string Changed = "0";

    //static GameSetting m_Instance;
    //public static GameSetting Instance => m_Instance ?? (m_Instance = Resources.Load<GameSetting>(nameof(GameSetting)));
    public static bool HasNetwork => System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

    public static string GetLocalIPAddress()
    {
        var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());

        foreach (var ip in host.AddressList) {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) {
                return ip.ToString();
            }
        }

        throw new System.Exception("No network adapters with an IPv4 address in the system!");
    }

}

}