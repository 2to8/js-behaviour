using GameEngine.Kernel;
using GameEngine.Kernel.Consts;
using GameEngine.Models;

namespace GameEngine.Controllers {

public class AddLevelCommand : Controller<AddLevelCommand> {

    public override void Execute(object data)
    {
        // if (data is E_LevelChange level) {
        //     GetModel<M_Game>().Level += 1;
        // }
    }

}

}