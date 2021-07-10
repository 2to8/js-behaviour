using GameEngine.Kernel._Appliation.Views.MainView.Attrs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameEngine.Kernel._Appliation.Models.User {

[CreateAssetMenu(fileName = "UserProfile", menuName = "App/UserProfile", order = 0)]
public class UserProfile : Model<UserProfile> {

    [HideInInspector, SerializeField]
    string m_Nickname;

    [HideInInspector, SerializeField]
    int m_Tiberium;

    [HideInInspector, SerializeField]
    string m_Uid;

    [HideInInspector, SerializeField]
    string m_UserDesc;

    [ShowInInspector]
    public string Nickname { get => m_Nickname; set => m_Nickname = NicknameAttr.Val(value); }

    [ShowInInspector]
    public string UserDesc { get => m_UserDesc; set => m_UserDesc = UserDescAttr.Val(value); }

    [ShowInInspector]
    public string uid { get => m_Uid; set => m_Uid = value; }

    [ShowInInspector]
    public int Tiberium { get => m_Tiberium; set => m_Tiberium = TiberiumAttr.Val(value); } // 泰晶

}

}