using UnityEngine;

namespace DefaultNamespace
{
    [System.Serializable]
    public class EnvSettings
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        static void Config()
        {
            Debug.unityLogger.logEnabled = Application.isEditor || Debug.isDebugBuild;
        }
    }
}