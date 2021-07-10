using GameEngine.Kernel;
using GameEngine.Kernel.Consts;
using GameEngine.ViewModel;

namespace GameEngine.Controllers {

public class EnterSceneCommand : Controller<EnterSceneCommand> {

    public override void Execute(object data)
    {
        var e = data as E_EnterScene;

        switch (e.sceneIndex) {
            case 0 : // Init
                break;
            case 1 :
                RegisterView(FindObjectOfType<UILevel>());
                RegisterView(FindObjectOfType<UITitle>());

                break;
        }
    }

}

}