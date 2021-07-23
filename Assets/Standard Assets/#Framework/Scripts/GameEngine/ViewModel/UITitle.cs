using System.Threading.Tasks;
using GameEngine.Kernel;
using GameEngine.Kernel.Attributes;
using GameEngine.Kernel.Consts;
using UniRx.Async;
using UnityEngine.UI;

namespace GameEngine.ViewModel {

public class UITitle : BaseView<UITitle> {

    Text textHead;

    protected override async Task Awake()
    {
        await base.Awake();
        textHead = transform.Find("HeadText").GetComponent<Text>();
    }

    [Events(typeof(E_LevelChange))]
    void OnLevelChange(object sender, object data)
    {
        if (data is E_LevelChange e) {
            textHead.text = e.level.ToString();
        }
    }

}

}