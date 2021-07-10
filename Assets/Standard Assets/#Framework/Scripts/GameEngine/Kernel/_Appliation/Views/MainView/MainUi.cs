using GameEngine.Kernel._Appliation.Views.MainView.Attrs;
using UnityEngine;

namespace GameEngine.Kernel._Appliation.Views.MainView {

public class MainUi : Provider<MainUi> {

    [SerializeField]
    LastVersionAttr m_LastVersion;

    [SerializeField]
    TiberiumAttr m_Tiberium;

    [SerializeField]
    VersionAttr m_Version;

}

}