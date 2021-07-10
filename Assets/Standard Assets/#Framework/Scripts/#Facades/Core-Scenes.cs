#if UNITY_EDITOR
using UnityEditor.Experimental.SceneManagement;
#endif

public static partial class Core
{
    public static bool IsPrefabScene()
    {
#if UNITY_EDITOR
        return PrefabStageUtility.GetCurrentPrefabStage() != null;
#endif
        return false;
    }
}