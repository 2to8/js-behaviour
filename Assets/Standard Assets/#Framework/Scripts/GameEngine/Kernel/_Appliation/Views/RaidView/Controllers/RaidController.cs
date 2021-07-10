using GameEngine.Kernel._Appliation.Views.RaidView.Types;
using GameEngine.Kernel.Attributes;
using Sirenix.OdinInspector;
using SQLiteNetExtensions.Attributes;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameEngine.Kernel._Appliation.Views.RaidView.Controllers {

public class RaidController : Controller<RaidController> {

    [SerializeField]
    AssetReference m_Reference;

    [TextBlob(nameof(blobRef))]
    public AssetReference Reference { get => m_Reference; set => m_Reference = value; }

    public string blobRef { get; set; }

    public override void Execute(object data) { }

    [Events(typeof(E_RaidStart))]
    public void DoRaidStart(object sender, object param)
    {
        Debug.Log("Raid Start");
    }

    [Button]
    void TestLoad()
    {
        SaveToFirst();
    }

}

}