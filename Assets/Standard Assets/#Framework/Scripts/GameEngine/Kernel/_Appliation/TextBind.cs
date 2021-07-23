using System.Threading.Tasks;
using UniRx.Async;
using UnityEngine.UI;

namespace GameEngine.Kernel._Appliation {

public class TextBind<T, TV> : BaseView<TextBind<T, TV>> where TV : TextBind<T, TV> {

    public Text ui;
    T value;

    public T Value {
        get => value;
        set {
            this.value = value;

            if (ui == null) {
                ui = GetComponent<Text>();
            }

            if (ui != null) {
                ui.text = $"{value}";
            }
        }
    }

    public static T Val(T p_value)
    {
        if (Instance != null) {
            Instance.Value = p_value;
        }

        return p_value;
    }

    protected override async Task Awake()
    {
        await base.Awake();
        Value = value;
    }

}

}