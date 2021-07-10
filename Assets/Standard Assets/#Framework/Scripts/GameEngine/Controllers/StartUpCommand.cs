using GameEngine.Kernel;
using GameEngine.Kernel.Consts;
using GameEngine.Models;

namespace GameEngine.Controllers {

public class StartUpCommand : Controller<StartUpCommand> {

    public override void Execute(object data)
    {
        //注册模型
        RegisterModel<M_Game>();

        //注册控制器
        RegisterController<E_EnterScene>(typeof(EnterSceneCommand));
        RegisterController<E_ExitScene>(typeof(ExitSceneCommand));
        RegisterController<E_AddLevel>(typeof(AddLevelCommand));

        //Game.Instance.LoadScene(1);
    }

}

}