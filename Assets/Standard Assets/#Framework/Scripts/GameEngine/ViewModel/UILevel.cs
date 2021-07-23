using System.Threading.Tasks;
using GameEngine.Kernel;
using GameEngine.Kernel.Attributes;
using GameEngine.Kernel.Consts;
using UniRx.Async;
using UnityEngine.UI;

namespace GameEngine.ViewModel {

public class UILevel : BaseView<UILevel> {

    public Button addLevelButton;
    public Text numberText;
    public Text prenumberText;

    protected override async Task Awake()
    {
        await base.Awake();
        numberText.text = 0.ToString();
        prenumberText.text = 0.ToString();
        addLevelButton.onClick.AddListener(OnClickAddLevel);
    }

    public void OnClickAddLevel()
    {
        SendEvent<E_AddLevel>();
    }

    [Events(typeof(E_LevelChange))]
    void OnLevelChange(object sender, object data)
    {
        if (data is E_LevelChange e) {
            numberText.text = e.level.ToString();
            prenumberText.text = e.level + "%";
        }
    }

}

}