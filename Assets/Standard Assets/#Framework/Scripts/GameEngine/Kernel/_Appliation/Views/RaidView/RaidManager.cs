using GameEngine.Kernel._Appliation.Models.Levels;
using GameEngine.Kernel._Appliation.Views.RaidView.Controllers;
using GameEngine.Kernel._Appliation.Views.RaidView.Types;
using Sirenix.Utilities;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 1998

namespace GameEngine.Kernel._Appliation.Views.RaidView {

public enum RaidType { None = 0, Normal, Hard, Factory, Hospital, Shop, Bonus, Unkown }

public enum NodeCount { Raid = 21, Full = 70 }

public class RaidManager : Provider<RaidManager> {

    public Image ActiveBg;
    public List<Image> ActiveStars = new List<Image>(3);
    public GameObject Container;
    public Image DisabledBg;

    public List<int> Empties = new List<int> {
        0,
        1,
        5,
        6,
        7,
        13,
        63,
        64,
        65,
        67,
        68,
        69,
    };

    public Dictionary<RaidType, int> FirstLineRandom = new Dictionary<RaidType, int> {
        { RaidType.None, 0 },
        { RaidType.Normal, 0 },
        { RaidType.Hard, 0 },
        { RaidType.Factory, 0 },
        { RaidType.Bonus, 0 },
        { RaidType.Unkown, 0 },
        { RaidType.Hospital, 0 },
        { RaidType.Shop, 0 },
    };

    public int level = 1;
    public LevelData levelData;
    public NodeCount levelType = NodeCount.Raid;

    public Dictionary<RaidType, Image> NodeIcons = new Dictionary<RaidType, Image> {
        { RaidType.None, null },
        { RaidType.Normal, null },
        { RaidType.Hard, null },
        { RaidType.Factory, null },
        { RaidType.Hospital, null },
        { RaidType.Bonus, null },
        { RaidType.Shop, null },
        { RaidType.Unkown, null },
    };

    public Dictionary<RaidType, int> NodeRandom = new Dictionary<RaidType, int> {
        { RaidType.None, 0 },
        { RaidType.Normal, 0 },
        { RaidType.Hard, 0 },
        { RaidType.Factory, 0 },
        { RaidType.Hospital, 0 },
        { RaidType.Bonus, 0 },
        { RaidType.Unkown, 0 },
        { RaidType.Shop, 0 },
    };

    public Image NormalBg;
    public List<Image> NormalStars = new List<Image>(3);
    int[] weight = new int[4] { 50, 25, 15, 10 };

    //返回值可做为奖品类数组下标，和权重比一一对应即可。
    RaidType GetRandomType(Dictionary<RaidType, int> target)
    {
        //int[] array, int _total
        var _total = 0;

        target.ForEach(t => {
            _total += t.Value;
        });

        var rand = Random.Range(1, _total + 1);
        var tmp = 0;

        foreach (var item in target) {
            tmp += item.Value;

            if (rand < tmp) {
                return item.Key;
            }
        }

        return RaidType.None;
    }

    protected override async UniTask Awake()
    {
        // todo: 需要放在 base.awake() 前面
        await base.Awake();
    }

    protected override async UniTask RegisterControllers()
    {
        await base.RegisterControllers();

        if (RaidController.Instance != null) {
            Debug.Log($"{nameof(RaidController)} ready");
        }

        RegisterController(RaidController.Instance);
    }

    protected override async UniTask OnStart()
    {
        Debug.Log("level start", gameObject);

        levelData.LoadWhere(t => t.Level == level, () => {
            levelData.Level = level;
        });

        SendEvent<E_RaidStart>();

        // await Addressables.LoadAssetAsync<GameObject>("test").Task;

        // var table = LevelData.Where(t => t.Level == level).FirstOrDefault();
        // if(level != table.Level) {
        //     table.Level = level;
        //     table.Update();
        // }
        //
        // levelData = table;
    }

}

}