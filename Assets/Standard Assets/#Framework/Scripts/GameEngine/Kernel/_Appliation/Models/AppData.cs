using GameEngine.Kernel._Appliation.Views.MainView.Attrs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameEngine.Kernel._Appliation.Models {

[CreateAssetMenu(fileName = "AppData", menuName = "App/AppData", order = 0)]
public class AppData : Model<AppData> {

    [SerializeField, HideInInspector]
    string m_CurrentVersion;

    [SerializeField, HideInInspector]
    string m_LastVersion;

    [ShowInInspector]
    public string CurrentVersion { get => m_CurrentVersion; set => m_CurrentVersion = VersionAttr.Val(value); }

    [ShowInInspector]
    public string LastVersion { get => m_LastVersion; set => m_LastVersion = LastVersionAttr.Val(value); }

}

}