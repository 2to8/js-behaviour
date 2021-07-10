
using GameEngine;
using UnityEditor;
using UnityEngine;

namespace Helpers {

public class TestIP {

    [MenuItem("Tests/GetIp")]
    static void GetIp()
    {
        Debug.Log(Config.instance.AddressableIP = IP.GetIP());
        var i = int.TryParse(Config.instance.Changed, out var ret) ? ret += 1 : 0;
        Config.instance.Changed = $"{i}";
        Debug.Log(Config.AssetUrl);

        //Setting.Instance.SaveChange();
        AssetDatabase.OpenAsset(Config.instance);
    }

}

}