using GameEngine.Controllers.Contracts;
using GameEngine.Kernel.Args;
using GameEngine.Views;

namespace GameEngine.Controllers.Commands {

public class EnterSceneCmd : Controller<EnterSceneCmd> {

    public override void Execute(object data)
    {
        var e = data as SceneArgs;

        switch (e.sceneIndex) {
            case 0 : // Init
                break;
            case 1 :
                // RegisterView(FindObjectOfType<UILevel>());
                // RegisterView(FindObjectOfType<UITitle>());

                break;
        }
    }

}

}