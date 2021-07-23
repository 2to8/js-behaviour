using GameEngine.Kernel._Appliation.Views.RaidView.States;
using GameEngine.Kernel.Attributes;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

namespace GameEngine.Kernel._Appliation.Views.RaidView {

[DefaultState(typeof(NodeDefault))]
public class RaidNode : Provider<RaidNode> {

    public bool Active;
    public Image Bg;
    public GameObject Content;
    public GameObject Ratings;
    public List<Image> RatingStars = new List<Image>(3);
    public int Stars;
    public RaidType type = RaidType.None;

    protected override async Task Awake()
    {
        await base.Awake();

        if (Bg == null) {
            Bg = GetComponent<Image>();
        }
    }

}

}