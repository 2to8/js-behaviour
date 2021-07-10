using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameEngine.Kernel._Appliation.Views.RaidView {

public class RaidView : Provider<RaidView> {

    [SerializeField]
    AssetReference m_Reference;

    public AssetReference Reference { get => m_Reference; set => m_Reference = value; }

}

}