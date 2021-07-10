using GameEngine.Controllers.Contracts;
using GameEngine.Models;

namespace GameEngine.Controllers.Commands {

public class AddLevelCmd : Controller<AddLevelCmd> {

    public override void Execute(object data)
    {
        GetModel<TestGameTable>().Level += 1;
    }

}

}