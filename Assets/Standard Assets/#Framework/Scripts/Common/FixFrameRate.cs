using System.Globalization;
using UnityEngine;

namespace Common {

public class FixFrameRate : Object {

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    static void FixRate()
    {
        if (Application.platform == RuntimePlatform.LinuxPlayer || Application.isConsolePlatform) {
            Debug.Log("Run in Linux Server");
            Time.timeScale = 0;
            Application.targetFrameRate = 10;
        } else {
            Application.targetFrameRate = 30;
            PlayerPrefs.SetString("LastRun", Time.timeSinceLevelLoad.ToString(CultureInfo.InvariantCulture));
            PlayerPrefs.Save();
            Debug.Log($"Run as client: {Application.version}");
        }
    }

}

}